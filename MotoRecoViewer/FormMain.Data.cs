// MIT License
// 
// Copyright (c) 2019 Schwarze Lanzenreiter
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using GMap.NET;
using GMap.NET.MapProviders;
using System.Net;
using Microsoft.Win32;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.ComponentModel.DataAnnotations;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace MotoRecoViewer
{
    /// <summary>
    /// CANデータの読み込みと、カーソル位置のデータ一覧表示
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// OpenFileDialogueで開いたデータをバイナリー読込する
        /// </summary>
        private void ReadCANData(string FileName)
        {
            //CANデータ読み取り中フラグ
            IsReadingCanData = true;

            //datファイルを全件読み込む
            CanData[] aryCanData = CanDataFile.ReadAll(FileName, null);
            long arySize = aryCanData.Length;
            //ゼロ割防止
            //読み取り中フラグを下ろさずに抜けると、以降チャートが描画されなくなる
            if (arySize <= 0)
            {
                IsReadingCanData = false;
                return;
            }

            //動的数式計算用にDataTableオブジェクト生成
            System.Data.DataTable dt = new System.Data.DataTable();

            //CAN ID→該当ルールの索引を先に作る。1件ごとにDecodeRule.IndexOfを呼ぶと線形探索になる為
            DecodeRuleIndex ruleIndex = new DecodeRuleIndex(decodeRule);

            //実際にデータが来るチャンネルを先に作っておく。
            //こうしておくとデコードループ内でチャンネル生成の排他が要らなくなる
            int[] chIdxByRule = PrepareChannels(aryCanData);

            //プログレスバー進捗計算用
            //ReadCANDataはUIスレッドから呼ばれるので、進捗はこのスレッドから直接反映してよい
            progressBar.Value = 0;
            progressBar.Maximum = (int)arySize;

            //arySizeを1024個づつ処理したい。1024の根拠はあまりないけど…
            //Parallel.Forで並列化しているが、1 CanData毎に並列化するのは効率が悪く、あるまとまり毎に並列化するほうが良さそうな為
            const int CHUNK_SIZE = 1024;
            long numChunk =  arySize / CHUNK_SIZE;
            long extra = arySize % CHUNK_SIZE;

            //もしarySizeが1024で割り切れない場合は、1回多く処理する
            if (extra != 0)
            {
                numChunk++;
            } else
            {
                //割り切れた場合は、extraを1024に伸ばす
                extra = CHUNK_SIZE;
            }

            //デコード結果は一旦チャンク毎のバッファに積み、後でチャンク順にマージする。
            //こうすると共有ロックが要らなくなり、かつマージ結果がファイル順で決定的になる。
            //全チャンク分のバッファを同時に抱えるとログデータ2個分のメモリを使うので、
            //batchChunks個ずつ処理してはマージする。
            //進捗表示もこのバッチ単位で行う。全体で100回程度になるようバッチ幅を決めるが、
            //1バッチが小さすぎると並列化の効率が落ちるので下限を設ける。
            long batchChunks = Math.Max(64, numChunk / 100);
            int chCount = channels.Count;
            List<TVData>[][] chunkBuf = new List<TVData>[batchChunks][];

            for (long batchStart = 0; batchStart < numChunk; batchStart += batchChunks)
            {
                long batchEnd = Math.Min(batchStart + batchChunks, numChunk);

                //進捗表示。バッチ単位なのでデコードループ自体には何の負荷もかからない
                progressBar.Value = (int)Math.Min(batchStart * CHUNK_SIZE, arySize);
                statusLabel.Text = string.Format("{0}%", progressBar.Value * 100L / arySize);
                Application.DoEvents();

                //CHUNK_SIZE毎の塊でデコード処理を実施
                Parallel.For(batchStart, batchEnd, j =>
                {
                    long start, end;
                    start = j * CHUNK_SIZE;

                    if (j < numChunk - 1)
                    {
                        end = start + CHUNK_SIZE;
                    }
                    else
                    {
                        end = start + extra;
                    }

                    //このチャンクのデコード結果。添字はチャンネル番号
                    List<TVData>[] local = new List<TVData>[chCount];

                    for (long i = start; i < end; i++)
                    {
                        //CanDataのCANIDに該当するDecodeRuleを引く。
                        //なお、DecodeRuleには、同一CANIDのルールは複数存在し得ることに注意。
                        //該当が無ければ空配列が返るので、そのまま次のデータへ進む。
                        // ToDo どう考えても捨てるしかないCANIDは、MotoReco側でフィルターかけるのも有り
                        int[] ruleIdxList = ruleIndex[aryCanData[i].id];

                        for (int ri = 0; ri < ruleIdxList.Length; ri++)
                        {
                            int decodeRuleIdx = ruleIdxList[ri];

                            //チャンネルは事前生成済みなので排他不要
                            int idxCh = chIdxByRule[decodeRuleIdx];

                            double value;

                            //計算式が固定式なら（DecodeRuleの1文字目が#なら、固定式とする）
                            if (decodeRule.GetDecodeRule(decodeRuleIdx)[0] == '#')
                            {
                                value = decodeRule.FixedFormula(decodeRule.GetDecodeRule(decodeRuleIdx), aryCanData[i]);
                            }
                            //計算式がユーザー定義なら
                            else
                            {
                                //計算式
                                string exp = decodeRule.DecodeFormula(decodeRule.GetDecodeRule(decodeRuleIdx), aryCanData[i]);

                                //式を計算する
                                value = System.Convert.ToDouble(dt.Compute(exp, ""));

                            }
                            //時間計算
                            // timeMSecの/1000dは、RAM値→物理値変換
                            // initTimeOffsetには、Menu→File→Append時に直前データの最終タイムスタンプが入ってくる
                            double second = aryCanData[i].timeSec + aryCanData[i].timeMSec / 1000d + initTimeOffset;

                            //データ追加
                            //iはdatファイル上のレコード番号。後段のSortが同一タイムスタンプを
                            //一意に並べられるよう記録しておく。
                            if (local[idxCh] == null) { local[idxCh] = new List<TVData>(); }

                            local[idxCh].Add(new TVData
                            {
                                DataTime = second,
                                DataValue = value,
                                DataValueFiltered = 0.0,
                                Sequence = i
                            });
                        }
                    }

                    chunkBuf[j - batchStart] = local;
                });

                //チャンク順にマージする
                for (long j = batchStart; j < batchEnd; j++)
                {
                    List<TVData>[] local = chunkBuf[j - batchStart];

                    for (int c = 0; c < chCount; c++)
                    {
                        if (local[c] != null) { channels[c].LogData.AddRange(local[c]); }
                    }

                    chunkBuf[j - batchStart] = null;
                }
            }

            //マージはチャンク順なのでファイル順に並んでいるが、datファイル自体の
            //タイムスタンプが単調とは限らないのでソートしておく
            //ソートは各ch毎なので並列化可能
            Parallel.For(0, channels.Count, i =>
            {
                channels[i].Sort();
            });

            //フィルタ処理は各ch毎なので並列化可能
            Parallel.For(0, channels.Count, i =>
            {
                channels[i].FilterData();
            });

            //積算距離・消費燃料・燃費・走行可能距離といった派生チャンネルを計算
            channelAnalyzer.CalcDerivedChannels();

            //ChNameでソート
            //ここでソートしておかないと、CANデータ読み込みのたびにChNameとIndexが変化する為、CSVエクスポート時、毎回列の位置が毎回変わってしまう。
            channels.SortByName();

            //開始時間を計算しておく
            if (startTime == 0.0)
            {
                startTime = aryCanData[0].timeSec + aryCanData[0].timeMSec / 1000d;
            }
            subPosTime = startTime;
            divTime = 1;
                       
            //終了時間を計算しておく
            endTime = aryCanData[arySize - 1].timeSec + aryCanData[arySize - 1].timeMSec / 1000d + endTime;

            //CANデータ読み取り終了
            IsReadingCanData = false;

            //進捗初期化
            progressBar.Value = 0;
            statusLabel.Text = "";
        }


        /// <summary>
        /// 読み込んだCANデータに実際に出現するCAN IDを調べ、対応するチャンネルを生成する。
        /// デコードループに入る前に済ませておくことで、ループ内でチャンネル生成の排他が不要になる。
        /// </summary>
        /// <param name="aryCanData">読み込み済みのCANデータ</param>
        /// <returns>DecodeRuleのインデックス→チャンネル番号の対応表。データが来ないルールは-1。</returns>
        private int[] PrepareChannels(CanData[] aryCanData)
        {
            //ファイル内に実在するCAN IDを調べる。CAN IDはushortなので添字表で足りる
            bool[] idPresent = new bool[ushort.MaxValue + 1];

            for (long i = 0; i < aryCanData.LongLength; i++)
            {
                idPresent[aryCanData[i].id] = true;
            }

            int[] chIdxByRule = new int[decodeRule.Count];

            for (int i = 0; i < decodeRule.Count; i++)
            {
                //該当CAN IDがファイルに無いルールは、チャンネルを作らない。
                //データが1件も来ないチャンネルを作らないという元の挙動に合わせる為。
                if (!idPresent[decodeRule.GetCANID(i)])
                {
                    chIdxByRule[i] = -1;
                    continue;
                }

                string chName = decodeRule.GetChName(i);

                if (!channels.Contains(chName))
                {
                    //ToDO CutOffはDecodeRuleから決めるように後で改良する
                    channels.Add(new ChData(chName,
                                            decodeRule.GetChartMin(i),
                                            decodeRule.GetChartMax(i),
                                            decodeRule.GetChartColor(i),
                                            decodeRule.GetChartPreview(i),
                                            decodeRule.GetChartShow(i),
                                            decodeRule.GetChartUseFilter(i),
                                            0.1,
                                            1.0));
                }

                chIdxByRule[i] = channels.IndexOf(chName);
            }

            return chIdxByRule;
        }

        /// <summary>
        /// PanelMainChart上のカーソル位置のデータをListViewに反映する
        /// </summary>
        private void UpdateListViewData()
        {
            // subPosTimeとsubPosTimePrevの差がdivTime以下のときだけ更新する
            if (Math.Abs(subPosTime - subPosTimePrev) > divTime)
            {
                return;
            }

            // mainCur1PosとmainCur1PosPrevの差がdivTime以下のときだけ更新する
            if (Math.Abs(mainCur1Pos - mainCur1PosPrev) > divTime)
            {
                return;
            }

            // mainCur2PosとmainCur2PosPrevの差がdivTime以下のときだけ更新する
            if (Math.Abs(mainCur2Pos - mainCur2PosPrev) > divTime)
            {
                return;
            }

            // ListViewDataのValue1を更新
            // MainChartのカーソル位置1に対応するタイムスタンプを計算
            // mainCur1Posは、PanelMainChart上の絶対的なX座標の為、グラフ描画領域幅に対するポジションに変換する
            double ratioMainCurPos1 = (mainCur1Pos - chartMargin) / (PanelMainChart.Width - 2 * chartMargin) ;
            double targetTime1 = subPosTime + (divTime * 20) * ratioMainCurPos1;

            // ListViewData更新停止
            ListViewData.BeginUpdate();

            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                //　該当CAN IDが存在しないケースも有りうることに注意
                // 例えば、実際データ読み込んだらDecodeRuleのデータがなかった場合
                if (!channels.Contains(ListViewData.Items[i].Text))
                {
                    continue;
                }

                int idx = channels.IndexOf(ListViewData.Items[i].Text);
                int targetIdx = channels[idx].FindLeftIndex(targetTime1);

                if (channels[idx].ChFilter == false)
                {
                    ListViewData.Items[i].SubItems[1].Text = channels[idx].LogData[targetIdx].DataValue.ToString();
                } else
                {
                    ListViewData.Items[i].SubItems[1].Text = channels[idx].LogData[targetIdx].DataValueFiltered.ToString();
                }
            }

            // MainChartのカーソル位置2に対応するタイムスタンプを計算
            // mainCur1Posは、PanelMainChart上の絶対的なX座標の為、グラフ描画領域幅に対するポジションに変換する
            double ratioMainCurPos2 = (mainCur2Pos - chartMargin) / (PanelMainChart.Width - 2 * chartMargin);
            double targetTime2 = subPosTime + (divTime * 20) * ratioMainCurPos2;

            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                //　該当CAN IDが存在しないケースも有りうることに注意
                // 例えば、実際データ読み込んだらDecodeRuleのデータがなかった場合
                if (!channels.Contains(ListViewData.Items[i].Text))
                {
                    continue;
                }

                int idx = channels.IndexOf(ListViewData.Items[i].Text);
                int targetIdx = channels[idx].FindLeftIndex(targetTime2);

                if (channels[idx].ChFilter == false)
                {
                    ListViewData.Items[i].SubItems[2].Text = channels[idx].LogData[targetIdx].DataValue.ToString();
                }
                else
                {
                    ListViewData.Items[i].SubItems[2].Text = channels[idx].LogData[targetIdx].DataValueFiltered.ToString();
                }
            }

            // MainChartのカーソル1とカーソル2の間のデータでのMAX-MINを計算する

            //diff格納用List
            var ListDiff = new List<double>();

            //0で初期化
            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                ListDiff.Add(0.0);
            }

            //並列処理でMAX,MIN,DIFFを算出
            //ここはListViewDataでチャンネル名にアクセスできない（∵別スレッドから画面部品にはアクセスできない）
            //ので、ListViewDataの元になるdecodeRuleから判断する
            Parallel.For(0, decodeRule.Count, i =>
            {
                //　該当CAN IDが存在しないケースも有りうることに注意
                // 例えば、実際データ読み込んだらDecodeRuleのデータがなかった場合
                if (!channels.Contains(decodeRule.GetChName(i)))
                {
                    //Parallel.Forの中では、continueはreturnとなる
                    return;
                }

               int idx = channels.IndexOf(decodeRule.GetChName(i));
               int targetIdxStart = channels[idx].FindLeftIndex(targetTime1);
               int targetIdxEnd = channels[idx].FindLeftIndex(targetTime2);

               double dataMax = double.MinValue;
               double dataMin = double.MaxValue;

               //フィルタ有効chはフィルタ後の値で比較する。
               //以前は比較にDataValue、代入にDataValueFilteredを使っていてMax-Minが狂っていた。
               bool useFiltered = channels[idx].ChFilter;

               for (int j = targetIdxStart; j <= targetIdxEnd; j++)
               {
                    double value = useFiltered ? channels[idx].LogData[j].DataValueFiltered
                                               : channels[idx].LogData[j].DataValue;

                    if (value > dataMax) { dataMax = value; }
                    if (value < dataMin) { dataMin = value; }
               }

               double diff = dataMax - dataMin;
               ListDiff[i]= diff;
            });

            //ListViewDataにDiff計算結果を反映
            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                //　該当CAN IDが存在しないケースも有りうることに注意
                // 例えば、実際データ読み込んだらDecodeRuleのデータがなかった場合
                if (!channels.Contains(ListViewData.Items[i].Text))
                {
                    continue;
                }

                ListViewData.Items[i].SubItems[3].Text = ListDiff[i].ToString();
            }

            //ListViewData更新再開
            ListViewData.EndUpdate();
        }

    }
}
