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
            if (arySize <= 0) { return; }

            //動的数式計算用にDataTableオブジェクト生成
            System.Data.DataTable dt = new System.Data.DataTable();

            //スレッド調停用
            object lockobj = new object();

            //プログレスバー進捗計算用
            var context = SynchronizationContext.Current;
            int counter = 0;
            long div_num = arySize / 100;
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

            //CHUNK_SIZE毎の塊でデコード処理を実施
            Parallel.For(0, numChunk, j =>
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
                

                for (long i = start; i < end; i++)
                {
                    //毎ループメッセージ発行するとクソ遅いので、トータル100回送るようにする。そもそもプログレスバーのWidthも100なので100以上送っても意味なし
                    Interlocked.Increment(ref counter);
                    if (div_num > 0)
                    {
                        if (i % div_num == 0)
                        {
                            context.Post(progress =>
                            {
                                if ((int)progress < this.progressBar.Maximum)
                                {
                                    this.progressBar.Value = (int)progress;
                                    this.statusLabel.Text = string.Format("{0}%", ((int)progress) * 100 / arySize);
                                }
                            }, counter);
                            Application.DoEvents();
                        }
                    }
                    //CanDataのCANIDが、DecodeRuleで一致するかチェック
                    int decodeRuleIdx;
                    decodeRuleIdx = decodeRule.IndexOf(aryCanData[i].id);

                    //もしCANIDが対象外なら即抜ける
                    if (decodeRuleIdx == -1)
                    {
                        // ToDo どう考えても捨てるしかないCANIDは、MotoReco側でフィルターかけるのも有り
                        continue;

                    }
                    
                    //１データ読み込んだらデコード処理にわたす
                    int idxCh;

                    //CAN IDが対象なら、DecodeRuleに従ってデコードする。
                    //なお、DecodeRuleには、同一CANIDのルールは複数存在し得ることに注意。
                    bool running = true;
                    while ((0 <= decodeRuleIdx) & running)
                    {
                        lock (lockobj)
                        {
                            //decodeRuleIdxのChNameが登録済かCheck
                            string chName = decodeRule.GetChName(decodeRuleIdx);

                            //存在しない場合追加する
                            if (!channels.Contains(chName))
                            {
                                int chMax = decodeRule.GetChartMax(decodeRuleIdx);
                                int chMin = decodeRule.GetChartMin(decodeRuleIdx);
                                bool chPrev = decodeRule.GetChartPreview(decodeRuleIdx);
                                int chColor = decodeRule.GetChartColor(decodeRuleIdx);
                                bool chShow = decodeRule.GetChartShow(decodeRuleIdx);
                                bool chUseFilter = decodeRule.GetChartUseFilter(decodeRuleIdx);

                                //ToDO CutOffはDecodeRuleから決めるように後で改良する
                                ChData newData = new ChData(chName, chMin, chMax, chColor, chPrev, chShow, chUseFilter, 0.1, 1.0);

                                channels.Add(newData);
                            }

                            idxCh = channels.IndexOf(chName);
                        }

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
                        //iはdatファイル上のレコード番号。並列処理で積む順序は不定なので、
                        //後段のSortが同一タイムスタンプを一意に並べられるよう記録しておく。
                        lock (lockobj) { channels[idxCh].AddData(second, value, i); }

                        //続きのdecodeRuleを探索
                        if (decodeRuleIdx + 1 < decodeRule.Count)
                        {
                            //次の要素を検索する
                            decodeRuleIdx = decodeRule.IndexOf(aryCanData[i].id, decodeRuleIdx + 1);
                        }
                        else
                        {
                            //最後まで検索したときはループを抜ける
                            running = false;
                        }
                    }
                }
            });

            //スレッド処理した関係でデータがソートできてないのでソートする
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
            context.Post(progress =>
            {
                this.progressBar.Value = 0;
                this.statusLabel.Text ="";
            }, null);
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

               for (int j = targetIdxStart; j <= targetIdxEnd; j++)
               {
                    if (channels[idx].ChFilter == false)
                    {
                        if (channels[idx].LogData[j].DataValue > dataMax) { dataMax = channels[idx].LogData[j].DataValue; }
                        if (channels[idx].LogData[j].DataValue < dataMin) { dataMin = channels[idx].LogData[j].DataValue; }
                    } else
                    {
                        if (channels[idx].LogData[j].DataValue > dataMax) { dataMax = channels[idx].LogData[j].DataValueFiltered; }
                        if (channels[idx].LogData[j].DataValue < dataMin) { dataMin = channels[idx].LogData[j].DataValueFiltered; }
                    }
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
