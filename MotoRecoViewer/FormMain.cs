﻿// MIT License
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
using GMap.NET;
using GMap.NET.MapProviders;
using System.Net;
using Microsoft.Win32;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace MotoRecoViewer
{
    public partial class FormMain : Form
    {
        // ダブルバッファ仕様のListView
        class BufferedListView : System.Windows.Forms.ListView
        {
            protected override bool DoubleBuffered { get { return true; } set { } }
        }

        //==========================
        //
        //  internal変数
        //
        //==========================
        internal DecodeRule decodeRule;

        internal struct CanData
        {
            public uint timeSec;
            public ushort timeMSec;
            public ushort id;
            public byte[] data;
        }

        //==========================
        //
        //  Private変数
        //
        //==========================
        private BufferedListView ListViewData;      // ダブルバッファのListView

        private const double chartMargin = 15;      // グリッドとpictureSubの余白
               
        private Dictionary<String, int> DicChName;  // ListChDataに格納しているChData.Nameと、ListChData上のインデックスを保持
        private List<ChData> ListChData;            // ChDataのリスト

        private double startTime;                   //CANデータ最初のタイムスタンプ
        private double endTime;                     //CANデータ最終データのタイムスタンプ
        private double subPosTime;                  //サブチャート選択位置、デフォルト0秒
        private double mainCur1Pos;                 //メインチャートカーソル1X位置
        private double mainCur2Pos;                 //メインチャートカーソル2X位置
        private double divTime;                     //メインチャートの1divisionあたり時間、デフォルト1秒
        private double initTimeOffset;              //データ読み込み時の先頭データ時間オフセット

        private bool IsDragging;                    // 現在ドラッグ中かどうか                                 
        private MouseButtons DraggingButton;        // どのボタンが押されているのか(右ボタンで別の処理をしたい時に、区別するため)
        private bool IsReadingCanData;              // 現在CANデータ読み込み中かどうか

        private float prev_lon;                     // longitude前回値
        private float prev_lat;                     // latitude前回値
        private GMapOverlay GMapOverlayMarker;      // GMapに表示するマーカーレイヤー
        private GMapOverlay GMapOverlayRoute;       // GMapに表示するルート

        private string currentDatFile = "";

        //==========================
        //
        //  Private関数
        //
        //==========================

        /// <summary>
        /// OpenFileDialogueで開いたデータをバイナリー読込する
        /// </summary>
        private void ReadCANData(string FileName)
        {
            //CANデータ読み取り中フラグ
            IsReadingCanData = true;

            //ファイルサイズを調べる
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            long fileSize = fs.Length;

            //CANDataは1データ16バイトなので、用意する配列は　( ファイルサイズ / 16 - 1)
            long arySize = fileSize / 16;

            //CANDataが壊れていると、16で割り切れないかもしれないのでチェックしておき、あまりが出たら読み込む個数を1減らす
            long checkSize = fileSize % 16;

            if (checkSize != 0)
            {
                arySize--;
            }

            //バイナリーリーダー生成
            BinaryReader reader = new BinaryReader(fs);

            //canDataの配列にファイル内容コピーする
            //本当はメモリにマルッとコピーしたくないが、後でParallel.Forで処理する都合で配列に確保する。
            CanData[] aryCanData = new CanData[arySize];

            for (int i = 0; i < arySize; i++)
            {
                aryCanData[i].timeSec = reader.ReadUInt32();
                aryCanData[i].timeMSec = reader.ReadUInt16();
                aryCanData[i].id = reader.ReadUInt16();
                aryCanData[i].data = new byte[8];

                for (int j = 0; j < 8; j++)
                {
                    aryCanData[i].data[j] = reader.ReadByte();
                }
            }

            reader.Close();

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
                    if (i % div_num == 0)
                    {
                        context.Post(progress =>
                        {
                            this.progressBar.Value = (int)progress;
                            this.statusLabel.Text = string.Format("{0}%", ((int)progress) * 100 / arySize);
                        }, counter);
                        Application.DoEvents();
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
                            if (!DicChName.ContainsKey(chName))
                            {
                                int chMax = decodeRule.GetChartMax(decodeRuleIdx);
                                int chMin = decodeRule.GetChartMin(decodeRuleIdx);
                                bool chPrev = decodeRule.GetChartPreview(decodeRuleIdx);
                                int chColor = decodeRule.GetChartColor(decodeRuleIdx);
                                bool chShow = decodeRule.GetChartShow(decodeRuleIdx);

                                //DicChNameには、key:chName value:ListChDataのインデックスを登録
                                DicChName.Add(chName, ListChData.Count);

                                ChData newData = new ChData(chName, chMin, chMax, chColor, chPrev, chShow);

                                ListChData.Add(newData);
                            }

                            idxCh = DicChName[chName];
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
                        lock (lockobj) { ListChData[idxCh].AddData(second, value); }

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
            Parallel.For(0, ListChData.Count, i =>
            {
                ListChData[i].Sort();
            });

            //ToDo この下のCalcXXもAsyncで並列処理できる
            {
                //GPS積算距離を計算
                CalcGPSDistance();

                //FrSpeed積算距離を計算
                CalcFrSpeedDistance();

                //距離カウンタから積算距離を計算
                CalcAccumulatedDistCountFr();

                //燃料カウンタから消費燃料を計算
                CalcAccumulatedFuelCount();

                //燃費を計算
                CalcFuelConsumption();

                //走行可能距離を計算
                CalcRange();
            }

            //開始時間を計算しておく
            if (startTime == 0.0)
            {
                startTime = aryCanData[0].timeSec + aryCanData[0].timeMSec / 1000;
            }
            subPosTime = startTime;
            divTime = 1;
                       
            //終了時間を計算しておく
            endTime = aryCanData[arySize - 1].timeSec + aryCanData[arySize - 1].timeMSec / 1000 + endTime;

            //CANデータ読み取り終了
            IsReadingCanData = false;

            //進捗初期化
            context.Post(progress =>
            {
                this.progressBar.Value = 0;
                this.statusLabel.Text ="";
            }, null);
        }

        private int GetIndexOfFixedFormula(string FixedFormula)
        {
            //FixedFormulaのChName取得
            int i = decodeRule.FormulaIndexOf(FixedFormula);

            //DecodeRuleに定義がない
            if (i == -1)
            {
                return -1;
            }

            string chNameDistCount = decodeRule.GetChName(i);

            //　該当CAN IDが存在しないケースも有りうることに注意
            // 例えば、実際データ読み込んだらDecodeRuleのデータがなかった場合
            if (!DicChName.ContainsKey(chNameDistCount))
            {
                return -1;
            }
            return DicChName[chNameDistCount];
        }

        /// <summary>
        /// ＃GPS_Speedから#GPS_Distanceを計算する
        /// </summary>
        private void CalcGPSDistance() {
            //#GPS_Speed のindex取得
            int idx_GPSSpeed = GetIndexOfFixedFormula("#GPS_Speed");
            if (idx_GPSSpeed < 0) { return; }

            //#GPS_Distanc のChName取得
            int idx_GPSDistance = GetIndexOfFixedFormula("#GPS_Distanc");
            if (idx_GPSDistance < 0) { return; }


            //#GPS_Distanceの積分計算
            TVData tvData;

            tvData = ListChData[idx_GPSDistance].LogData[0];
            tvData.DataValue = 0.0;
            ListChData[idx_GPSDistance].LogData[0] = tvData;

            for (int i=1; i<ListChData[idx_GPSSpeed].Count; i++)
            { 
                // GPSSpeed unit:km/h
                double dSpeed = ListChData[idx_GPSSpeed].LogData[i].DataValue;

                // time diff unit:sec
                double timeDiff = ListChData[idx_GPSSpeed].LogData[i].DataTime - ListChData[idx_GPSSpeed].LogData[i-1].DataTime;

                tvData = ListChData[idx_GPSDistance].LogData[i];

                // 積算距離は km で考える
                tvData.DataValue = ListChData[idx_GPSDistance].LogData[i - 1].DataValue + (dSpeed / 3600) * timeDiff;
                ListChData[idx_GPSDistance].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// ＃K51_FrSpeed1から#K51_DistFrSpeed1を計算する
        /// </summary>
        private void CalcFrSpeedDistance()
        {
            //#K51_FrSpeed1 のChName取得
            int idx_FrSpeed = GetIndexOfFixedFormula("#K51_FrSpeed1");
            if (idx_FrSpeed < 0) { return; }

            //#K51_DistFrSpeed1 のChName取得
            int idx_DistFrSpeed = GetIndexOfFixedFormula("#K51_DistFrSpeed1");
            if (idx_DistFrSpeed < 0) { return; }

            //#GPS_Distanceの積分計算
            TVData tvData;

            tvData = ListChData[idx_DistFrSpeed].LogData[0];
            tvData.DataValue = 0.0;
            ListChData[idx_DistFrSpeed].LogData[0] = tvData;

            for (int i = 1; i < ListChData[idx_FrSpeed].Count; i++)
            {
                // GPSSpeed unit:km/h
                double dSpeed = ListChData[idx_FrSpeed].LogData[i].DataValue;

                // time diff unit:sec
                double timeDiff = ListChData[idx_FrSpeed].LogData[i].DataTime - ListChData[idx_FrSpeed].LogData[i - 1].DataTime;

                tvData = ListChData[idx_DistFrSpeed].LogData[i];

                // 積算距離は km で考える
                tvData.DataValue = ListChData[idx_DistFrSpeed].LogData[i - 1].DataValue + (dSpeed / 3600) * timeDiff;
                ListChData[idx_DistFrSpeed].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// ＃K51_DistCount から #K51_AccumulatedDistCountを計算する
        /// </summary>
        private void CalcAccumulatedDistCountFr()
        {
            //#K51_DistCountFr のChName取得
            int idx_DistCount = GetIndexOfFixedFormula("#K51_DistCountFr");
            if (idx_DistCount < 0) { return; }

            //#K51_AccumulatedDistCountFr のChName取得
            int idx_AccumulatedDistCount = GetIndexOfFixedFormula("#K51_AccumulatedDistCountFr");
            if (idx_AccumulatedDistCount < 0) { return; }

            //#K51_DistCountの積分計算
            TVData tvData;
            double AccumulatedCounter = 0.0;

            tvData = ListChData[idx_AccumulatedDistCount].LogData[0];
            tvData.DataValue = AccumulatedCounter;
            ListChData[idx_AccumulatedDistCount].LogData[0] = tvData;

            for (int i = 1; i < ListChData[idx_DistCount].Count; i++)
            {
                // DistCount unit:?
                double dCounter = ListChData[idx_DistCount].LogData[i].DataValue - ListChData[idx_DistCount].LogData[i-1].DataValue;

                //ToDo File Append時、工夫が必要

                // カウンタ1周すると負の値になるのでその場合は253*16足す
                // 0で判定すると、EG STOP時もカウントアップしてしまうので、明らかに1周カウンタが回ったときだけ+3951する
                if (dCounter < -3900)
                {
                    // DistCountFrは、MAX3951
                    dCounter += 3951;
                }

                AccumulatedCounter += dCounter;

                tvData = ListChData[idx_AccumulatedDistCount].LogData[i];

                // 積算カウンタ
                // カウンタ値に0.000255掛けると、ちょうどFr車速を積分した積算距離に等しくなっている
                tvData.DataValue = AccumulatedCounter * 0.000255;
                ListChData[idx_AccumulatedDistCount].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// ＃K51_FuelCount から #K51_AccumulatedFuelCount
        /// </summary>
        private void CalcAccumulatedFuelCount()
        {
            //#K51_FuelCount のChName取得
            int idx_FuelCount = GetIndexOfFixedFormula("#K51_FuelCount");
            if (idx_FuelCount < 0) { return; }

            //#K51_AccumulatedFuelCount のChName取得
            int idx_AccumulatedFuelCount = GetIndexOfFixedFormula("#K51_AccumulatedFuelCount");
            if (idx_AccumulatedFuelCount < 0) { return; }

            //#K51_DistCountの積分計算
            TVData tvData;
            double AccumulatedCounter = 0.0;

            tvData = ListChData[idx_AccumulatedFuelCount].LogData[0];
            tvData.DataValue = AccumulatedCounter;
            ListChData[idx_AccumulatedFuelCount].LogData[0] = tvData;

            for (int i = 1; i < ListChData[idx_FuelCount].Count; i++)
            {
                // DistCount unit:?
                double dCounter = ListChData[idx_FuelCount].LogData[i].DataValue - ListChData[idx_FuelCount].LogData[i - 1].DataValue;

                //ToDo File Append時、工夫が必要

                // カウンタ1周すると負の値になるのでその場合は256*256足す
                // 0で判定すると、EG STOP時もカウントアップしてしまうので、明らかに1周カウンタが回ったときだけ+256*256する
                if (dCounter < -65000)
                {
                    dCounter += 256*256;
                }

                AccumulatedCounter += dCounter;

                tvData = ListChData[idx_AccumulatedFuelCount].LogData[i];

                // 積算カウンタ
                // 燃料カウンタは1000000で割るとちょうど良さそう
                tvData.DataValue = AccumulatedCounter / 1000000;
                ListChData[idx_AccumulatedFuelCount].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// 燃料カウンタと距離カウンタから、燃費を計算する
        /// 燃費カウンタデータは、2BCと同じデータタイミングとする
        /// </summary>
        private void CalcFuelConsumption()
        {
            //#K51_AccumulatedFuelCount のChName取得
            int idx_AccumulatedFuelCount = GetIndexOfFixedFormula("#K51_AccumulatedFuelCount");
            if (idx_AccumulatedFuelCount < 0) { return; }

            //#K51_AccumulatedDistCountFr のChName取得
            int idx_AccumulatedDistCount = GetIndexOfFixedFormula("#K51_AccumulatedDistCountFr");
            if (idx_AccumulatedDistCount < 0) { return; }

            //#K51_FuelConsumption のChName取得
            int idx_FuelConsumption = GetIndexOfFixedFormula("#K51_FuelConsumption");
            if (idx_FuelConsumption < 0) { return; }

            //燃費計算
            TVData tvData;
            
            for (int i = 0; i < ListChData[idx_FuelConsumption].Count; i++)
            {
                //積算燃料量取得
                double time = ListChData[idx_AccumulatedFuelCount].LogData[i].DataTime;
                double fuel = ListChData[idx_AccumulatedFuelCount].LogData[i].DataValue;

                //積算燃料取得時間に対応した、積算距離を取得
                int idx_dist = ListChData[idx_AccumulatedDistCount].FindLeftIndex(time);
                double dist = ListChData[idx_AccumulatedDistCount].LogData[idx_dist].DataValue;

                tvData = ListChData[idx_FuelConsumption].LogData[i];

                // 燃費
                if (fuel != 0) {
                    tvData.DataValue = dist  / fuel ;
                } else
                {
                    tvData.DataValue = 0;
                }
                ListChData[idx_FuelConsumption].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// 燃費とFuelLevelから走行可能距離を計算する
        /// 2BCと同じデータタイミングとする
        /// </summary>
        private void CalcRange()
        {
            const double K51_TANK_CAPA = 30d;

            //#K51_FuelLevel のChName取得
            int idx_FuelLevel = GetIndexOfFixedFormula("#K51_FuelLevel");
            if (idx_FuelLevel < 0) { return; }

            //#K51_FuelConsumption のChName取得
            int idx_FuelConsumption = GetIndexOfFixedFormula("#K51_FuelConsumption");
            if (idx_FuelConsumption < 0) { return; }

            //#K51_Range のChName取得
            int idx_Range = GetIndexOfFixedFormula("#K51_Range");
            if (idx_Range < 0) { return; }

            //Range計算
            TVData tvData;

            for (int i = 0; i < ListChData[idx_Range].Count; i++)
            {
                //燃費取得
                double time = ListChData[idx_FuelConsumption].LogData[i].DataTime;
                double fuelConsump = ListChData[idx_FuelConsumption].LogData[i].DataValue;

                //燃費計算時点のFuelLevel取得
                int idx_dist = ListChData[idx_FuelLevel].FindLeftIndex(time);
                double fuelLevel = ListChData[idx_FuelLevel].LogData[idx_dist].DataValue;

                tvData = ListChData[idx_Range].LogData[i];

                // 燃費
                if (fuelConsump != 0)
                {
                    tvData.DataValue = K51_TANK_CAPA * (fuelLevel / 100d) * fuelConsump;
                }
                else
                {
                    tvData.DataValue = 0;
                }
                ListChData[idx_Range].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// pictureMain上のカーソル位置のデータをListViewに反映する
        /// </summary>
        private void UpdateListViewData()
        {
            // ListViewDataのValue1を更新
            // MainChartのカーソル位置1に対応するタイムスタンプを計算
            // mainCur1Posは、PictureMain上の絶対的なX座標の為、グラフ描画領域幅に対するポジションに変換する
            double ratioMainCurPos1 = (mainCur1Pos - chartMargin) / (PictureMain.Width - 2 * chartMargin) ;
            double targetTime1 = subPosTime + (divTime * 20) * ratioMainCurPos1;
            
            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                //　該当CAN IDが存在しないケースも有りうることに注意
                // 例えば、実際データ読み込んだらDecodeRuleのデータがなかった場合
                if (!DicChName.ContainsKey(ListViewData.Items[i].Text))
                {
                    continue;
                }

                int idx = DicChName[ListViewData.Items[i].Text];
                int targetIdx = ListChData[idx].FindLeftIndex(targetTime1);

                ListViewData.Items[i].SubItems[1].Text = ListChData[idx].LogData[targetIdx].DataValue.ToString();
            }

            // MainChartのカーソル位置2に対応するタイムスタンプを計算
            // mainCur1Posは、PictureMain上の絶対的なX座標の為、グラフ描画領域幅に対するポジションに変換する
            double ratioMainCurPos2 = (mainCur2Pos - chartMargin) / (PictureMain.Width - 2 * chartMargin);
            double targetTime2 = subPosTime + (divTime * 20) * ratioMainCurPos2;

            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                //　該当CAN IDが存在しないケースも有りうることに注意
                // 例えば、実際データ読み込んだらDecodeRuleのデータがなかった場合
                if (!DicChName.ContainsKey(ListViewData.Items[i].Text))
                {
                    continue;
                }

                int idx = DicChName[ListViewData.Items[i].Text];
                int targetIdx = ListChData[idx].FindLeftIndex(targetTime2);

                ListViewData.Items[i].SubItems[2].Text = ListChData[idx].LogData[targetIdx].DataValue.ToString();
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
                if (!DicChName.ContainsKey(decodeRule.GetChName(i)))
                {
                    //Parallel.Forの中では、continueはreturnとなる
                    return;
                }

               int idx = DicChName[decodeRule.GetChName(i)];
               int targetIdxStart = ListChData[idx].FindLeftIndex(targetTime1);
               int targetIdxEnd = ListChData[idx].FindLeftIndex(targetTime2);

               double dataMax = double.MinValue;
               double dataMin = double.MaxValue;

               for (int j = targetIdxStart; j <= targetIdxEnd; j++)
               {
                   if (ListChData[idx].LogData[j].DataValue > dataMax) { dataMax = ListChData[idx].LogData[j].DataValue; }
                   if (ListChData[idx].LogData[j].DataValue < dataMin) { dataMin = ListChData[idx].LogData[j].DataValue; }
               }

               double diff = dataMax - dataMin;
               ListDiff[i]= diff;
            });

            //ListViewDataにDiff計算結果を反映
            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                //　該当CAN IDが存在しないケースも有りうることに注意
                // 例えば、実際データ読み込んだらDecodeRuleのデータがなかった場合
                if (!DicChName.ContainsKey(ListViewData.Items[i].Text))
                {
                    continue;
                }

                ListViewData.Items[i].SubItems[3].Text = ListDiff[i].ToString();
            }

            // ListView描画
            ListViewData.Update();
        }

        /// <summary>
        /// 地図を更新表示する
        /// 緯度経度のチャンネル名はLatitudeとLongitude固定とする
        /// 地図位置はカーソル1位置とする
        /// </summary>
        private void UpdateMap()
        {
            // LatitudeもLongitudeも存在しない場合、何もしない
            if (!DicChName.ContainsKey("Latitude") || (!DicChName.ContainsKey("Longitude")))
            {
                return;
            }

            int idx_lat = DicChName["Latitude"];
            int idx_lon = DicChName["Longitude"];

            // MainChartの中央に対応するタイムスタンプを計算
            double mainChartCenterTime = subPosTime + (divTime * 20) / 2;

            // MainCharの中央に応じたデータidx取得
            int mainChartCenterIdx = ListChData[idx_lat].FindLeftIndex(mainChartCenterTime);

            // MainChartの中央に応じた緯度経度取得
            float fl_lon = (float)ListChData[idx_lon].LogData[mainChartCenterIdx].DataValue;
            float fl_lat = (float)ListChData[idx_lat].LogData[mainChartCenterIdx].DataValue;

            // 緯度経度に変化がなければリターンで抜ける
            if ((fl_lon == prev_lon) && (fl_lat == prev_lat))
            {
                return;
            } else {
                prev_lon = fl_lon;
                prev_lat = fl_lat;
            }

            //地図中心を、MainChartの中央市に合わせる
            GMapControl.Position = new PointLatLng(fl_lat, fl_lon);

            //現在MainChartに表示されているデータのルートをMapに追加する
            // MainChartの両端に対応するタイムスタンプを計算
            // 左端は、subPosTimeそのもの
            double mainChartRightTime = subPosTime + (divTime * 20);

            // MainChartの両端に対応するデータidx取得。緯度経度はデータ数同等なので、longitudeで代表する
            int mainChartLeftIdx = ListChData[idx_lat].FindLeftIndex(subPosTime);
            int mainChartRightIdx = ListChData[idx_lat].FindLeftIndex(mainChartRightTime);

            // ルートをMapに追加
            // まず既存ルートをクリア
            GMapOverlayRoute.Routes.Clear();

            // ポイント追加
            List<PointLatLng> points = new List<PointLatLng>();
            for (int i = mainChartLeftIdx; i <= mainChartRightIdx; i++)
            {
                fl_lon = (float)ListChData[idx_lon].LogData[i].DataValue;
                fl_lat = (float)ListChData[idx_lat].LogData[i].DataValue;

                points.Add(new PointLatLng(fl_lat, fl_lon));
            }

            GMapRoute route = new GMapRoute(points, "MainChartRoute")
            {
                Stroke = new Pen(Color.Red, 3)
            };
            GMapOverlayRoute.Routes.Add(route);
        }

        /// <summary>
        /// 一般的な線形補間式
        /// 座標(x0,y0)(x1,y1)の2点が既知の場合に、新たな座標xに対応するyを返す
        /// <param name = "x0" > 既知のx1</ param >
        /// <param name = "y0" > 既知のx1</ param >
        /// <param name = "x1" > 既知のx1</ param >
        /// <param name = "y1" > 既知のx1</ param >
        /// <param name = "x" > 新しく求めたいx</ param >
        /// </summary>
        private double LinearInterpolation(double x0, double y0, double x1, double y1, double x)
        {
            return y0 + (y1 - y0) * (x - x0) / (x1 - x0);
        }

        /// <summary>
        /// 地図上のマーカーのみ位置更新する
        /// 緯度経度のチャンネル名はLatitudeとLongitude固定とする
        /// マーカー位置はカーソル1位置とする
        /// </summary>
        private void UpdateMapMarker()
        {
            // LatitudeもLongitudeも存在しない場合、何もしない
            if ( !DicChName.ContainsKey("Latitude") || ( !DicChName.ContainsKey("Longitude")))
            {
                return;
            }

            int idx_lat = DicChName["Latitude"];
            int idx_lon = DicChName["Longitude"];

            // MainChartのカーソル位置1に対応するタイムスタンプを計算
            double cur1PosTime = subPosTime + (divTime * 20) / (PictureMain.Width - 2 * chartMargin) * mainCur1Pos;

            // カーソル位置左側データidx取得
            int leftIdx = ListChData[idx_lat].FindLeftIndex(cur1PosTime);
            double leftTime = ListChData[idx_lat].LogData[leftIdx].DataTime;

            // カーソル位置右側データidx取得
            int rightIdx;
            if (leftIdx == ListChData[idx_lat].LogData.Count - 1)
            {
                rightIdx = leftIdx;
            } else
            {
                rightIdx = leftIdx + 1;   // ToDo 経度緯度データが、重複してロギングされているため.CanLogger側を治す 
            }
            double rightTime = ListChData[idx_lat].LogData[rightIdx].DataTime;

            // 緯度経度取得
            double leftLon = ListChData[idx_lon].LogData[leftIdx].DataValue;
            double leftLat = ListChData[idx_lat].LogData[leftIdx].DataValue;

            double rightLon = ListChData[idx_lon].LogData[rightIdx].DataValue;
            double rightLat = ListChData[idx_lat].LogData[rightIdx].DataValue;

            // 経度緯度を前後2点データから線形補間する
            double liLat = LinearInterpolation(leftTime, leftLat, rightTime, rightLat, cur1PosTime);
            double liLon = LinearInterpolation(leftTime, leftLon, rightTime, rightLon, cur1PosTime);

            //マーカー更新
            GMapOverlayMarker.Markers.Clear();
            GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(liLat, liLon), GMarkerGoogleType.green);
            GMapOverlayMarker.Markers.Add(marker);
        }

        /// <summary>
        /// Chartを描画する
        /// </summary>
        private void DrawChartMain(Graphics g)
        {
            // ListChNameがNULL→即抜ける
            if (DicChName == null) { return; }

            // ListChNameが0＝CANデータ未読み込み→即抜ける
            if (DicChName.Count < 1) { return; }

            //　アプリ上部のメインチャートを描画
            DrawMainChart(g);

            // ListViewData更新
            UpdateListViewData();
        }

        /// <summary>
        /// Chartを描画する
        /// </summary>
        private void DrawChartSub(Graphics g)
        {
            // ListChNameがNULL→即抜ける
            if (DicChName == null) { return; }

            // ListChNameが0＝CANデータ未読み込み→即抜ける
            if (DicChName.Count < 1) { return; }

            // アプリ下部のサブチャートを描画
            DrawSubChart(g);
        }

        /// <summary>
        /// 画面下部のサブChartのグリッドを描画する
        /// </summary>
        private void DrawSubChartGrid(Graphics g)
        {
            //まずサブチャートエリアの枠を書く

            //横線
            for (int i = 0; i < 21; i++)
            {
                // 罫線間ピクセルを算出
                double rule = (double)(PictureSub.Width - 2d * chartMargin) / 20d;

                // X座標
                double x = chartMargin + i * rule;

                // Y座標
                double y1 = chartMargin;
                double y2 = PictureSub.Height - chartMargin;

                // ToDo グリッドカラーも設定できるようにする
                g.DrawLine(Pens.DarkSeaGreen, (float)x, (float)y1, (float)x, (float)y2);
            }

            //縦線
            // X座標
            for (int i = 0; i < 3; i++)
            {
                // 罫線間ピクセルを算出
                double rule = (PictureSub.Height - 2d * chartMargin) / 2d;

                // X座標
                double y = chartMargin + i * rule;

                // Y座標
                double x1 = chartMargin;
                double x2 = PictureSub.Width - chartMargin;

                // ToDo グリッドカラーも設定できるようにする
                g.DrawLine(Pens.DarkSeaGreen, (float)x1, (float)y, (float)x2, (float)y);
            }
        }

        /// <summary>
        /// 画面下部のサブChartの描画する
        /// </summary>
        private void DrawSubChartData(Graphics g)
        {
            //スレッド調停用
            object lockobj = new object();

            // ListChNameを調べてプレビュー表示TrueのものだけSubChartに表示する
            int idxPreview = 0;

            Pen p = new Pen(Brushes.White);

            for (int i = 0; i < ListChData.Count; i++)
            //Parallel.For(0, ListChData.Count, i =>
            {
                // Subチャートに表示するのは、ChData.ChPreviewがTrueの物のみ
                if (ListChData[i].ChPreview)
                {
                    double targetIdxPrev = 0d;
                    double xPrev = chartMargin;
                    double yPrev = PictureSub.Height - chartMargin;

                    // SubChart描画ピクセル幅に対してのみ描画処理実施する
                    for (int j = 0; j <= PictureSub.Width - 2 * chartMargin; j++)
                    {
                        // SubChartのグラフ描画領域のXstart～Xendに対応したタイムスタンプを計算
                        double targetTime = startTime + (endTime - startTime) / (PictureSub.Width - 2d * chartMargin) * j;

                        // targetTimeに対応したタイムスタンプに最も近いChDataのインデックスを取得
                        int targetIdx = ListChData[i].FindLeftIndex(targetTime);


                        // 1つ前のインデックスと同じ場合、スキップする
                        if (targetIdxPrev == targetIdx)
                        {
                            continue;
                        }

                        // targetIdx相当分のXとYをラッチ
                        double x = j + chartMargin;

                        // targetIdxのDataValueが、Ch設定のMax-Min幅に対して何%位置か算出する
                        double y = ListChData[i].LogData[targetIdx].DataValue;
                        y = (y - ListChData[i].ChMin) / (ListChData[i].ChMax - ListChData[i].ChMin);

                        // 0%以下もしくは100以上は0または100に丸める
                        if (y > 100) { y = 1; }
                        if (y < 0) { y = 0; }

                        // %をY軸ピクセルに変換する.その際、グラフ上方が原点になるので、1-yとして計算する。
                        y = (PictureSub.Height - chartMargin * 2d) * (1d - y);
                        y += chartMargin;

                        // 1つ前のインデックスと異なる場合のみ、ラインを描画する

                        lock (lockobj)
                        {
                            // 描画色
                            p.Color = Color.FromArgb(ListChData[i].ChColor);

                            //　描画
                            g.DrawLine(p, (float)xPrev, (float)yPrev, (float)x, (float)y);
                        }

                        targetIdxPrev = targetIdx;
                        xPrev = x;
                        yPrev = y;
                    }
                }

                idxPreview++;
             //});
            }
        }

        /// <summary>
        /// 画面下部のサブChartに選択位置を表示する
        /// </summary>
        private void DrawSubChartPos(Graphics g)
        {
            //選択マーカー幅を計算する
            //データの時間幅に対する、divTime*20の割合から計算できる
            //まずデータの時間幅に対するメインチャート時間幅の割合を計算
            double ratioSelected = (divTime * 20) / (endTime - startTime);

            //ratioSelected上限処理 , データが短くてメインチャート1画面に収まり切る場合に発生
            if (ratioSelected > 1) { ratioSelected = 1; }

            double rectWidth = (PictureSub.Width - 2 * chartMargin) * ratioSelected;

            //rectWidth下限処理
            if (rectWidth < 2){ rectWidth = 2; }

            //XY座標計算
            double x1 = (subPosTime - startTime) / (endTime - startTime);       //ToDo startTimeはMotoRecoでロギング時0のはずなので本来は不要
            x1 = x1 * (PictureSub.Width - 2 * chartMargin)+chartMargin;
            double x2 = x1 + rectWidth;
            double y1 = chartMargin;
            double y2 = PictureSub.Height - chartMargin;

            //rectangleに変換
            Rectangle rect = Rectangle.FromLTRB((int)x1, (int)y1, (int)x2, (int)y2);

            g.DrawRectangle(Pens.White, rect);
        }

        /// <summary>
        /// 画面下部のサブChartを描画する
        /// </summary>
        private void DrawSubChart(Graphics g)
        {
            //サブチャートエリアを黒く塗りつぶす
            g.FillRectangle(Brushes.Black, 0, 0, PictureSub.Width, PictureSub.Height);

            //pictureSubのWidth分だけ、ListChDataからデータをピックアップして描画する
            DrawSubChartData(g);

            DrawSubChartGrid(g);

            //サブチャートに現在位置を反転表示で表示
            DrawSubChartPos(g);
        }


        /// <summary>
        /// 画面上部のメインChartのグリッドを描画する
        /// </summary>
        private void DrawMainChartGrid(Graphics g)
        {
            //まずメインチャートエリアの枠を書く

            //横線
            for (int i = 0; i < 21; i++)
            {
                // 罫線間ピクセルを算出
                double rule = (PictureMain.Width - 2d * chartMargin) / 20d;

                // X座標
                double x = chartMargin + i * rule;

                // Y座標
                double y1 = chartMargin;
                double y2 = PictureMain.Height - chartMargin;

                g.DrawLine(Pens.DarkSeaGreen, (float)x, (float)y1, (float)x, (float)y2);
            }

            //縦線
            // X座標
            for (int i = 0; i < 11; i++)
            {
                // 罫線間ピクセルを算出
                double rule = (PictureMain.Height - 2d * chartMargin) / 10d;

                // X座標
                double y = chartMargin + i * rule;

                // Y座標
                double x1 = chartMargin;
                double x2 = PictureMain.Width - chartMargin;

                g.DrawLine(Pens.DarkSeaGreen, (float)x1, (float)y, (float)x2, (float)y);
            }

            Pen curPen = new Pen(Brushes.White,3);

            //カーソル1
            curPen.DashStyle = DashStyle.DashDot;
            g.DrawLine(curPen, (float)mainCur1Pos, (float)chartMargin, (float)mainCur1Pos, PictureMain.Height - (float)chartMargin);

            //カーソル2
            curPen.DashStyle = DashStyle.DashDotDot;
            g.DrawLine(curPen, (float)mainCur2Pos, (float)chartMargin, (float)mainCur2Pos, PictureMain.Height - (float)chartMargin);
        }

        /// <summary>
        /// 小数点以下の桁数を取得
        /// </summary>
        private int GetPrecision(double d)
        {
            string str = d.ToString().TrimEnd('0');

            int idx = str.IndexOf('.');
            if (idx == -1)
                return 0;

            return str.Substring(idx + 1).Length;
        }

        /// <summary>
        /// 画面上部のメインChartにタイムスタンプ等の情報を描画する
        /// </summary>
        private void DrawMainChartInfo(Graphics g)
        {
            //フォントオブジェクトの作成
            Font fnt = new Font("MS UI Gothic", 9);

            //TimeDivを表示する
            string str = "1Div:"+divTime.ToString()+"sec";
            g.DrawString(str, fnt, Brushes.DarkSeaGreen, 1, 1);

            // divTimeの小数点以下桁数を計算
            int pres = GetPrecision(divTime);

            // 元のCANDATAがせいぜい10ms周期なので、2桁までとする
            if (pres > 2) { pres = 2; }

            //時刻を下部に表示する
            //右端の罫線はタイムスタンプ見切れるので表示しない
            for (int i = 0; i < 20; i++)
            {
                // 罫線間ピクセルを算出
                double rule = (PictureMain.Width - 2d * chartMargin) / 20d;

                // X座標
                double x = chartMargin + i * rule;

                // Y座標
                double y2 = PictureMain.Height - chartMargin;

                // 各罫線に対応するタイムスタンプを計算
                double targetTime = subPosTime + (divTime * i);

      

                str = targetTime.ToString("F" + pres.ToString());

                g.DrawString(str, fnt, Brushes.DarkSeaGreen, (float)x-5, (float)y2+1);
            }
        }

        /// <summary>
        /// 画面上部のメインChartの描画する
        /// </summary>
        private void DrawMainChartData(Graphics g)
        {
            //スレッド調停用
            object lockobj = new object();

            Pen p = new Pen(Brushes.White);

            // ListChNameの項目数すべて描画する
            Parallel.For(0, ListChData.Count, i =>
            //for (int i = 0; i < ListChData.Count; i++)
            {
                int targetIdxPrev = 0;
                double xPrev = chartMargin;
                double yPrev = PictureMain.Height - chartMargin;

                if (ListChData[i].ChShow)
                {
                    // MainChart描画ピクセル幅に対してのみ描画処理実施する
                    for (int j = 0; j <= PictureMain.Width - 2 * chartMargin; j++)
                    {
                        // MainChartのグラフ描画領域のXstart～Xendに対応したタイムスタンプを計算
                        double targetTime = subPosTime + (divTime * 20d) / (PictureMain.Width - 2d * chartMargin) * j;

                        // targetTimeに対応したタイムスタンプに最も近いChDataのインデックスを取得
                        //int targetIdx = ListChData[i].FindLeftIndex(targetTime, startidx, endidx);

                        int targetIdx = ListChData[i].FindLeftIndex(targetTime);
                        // 1つ前のインデックスと同じ場合何もしない
                        if (targetIdxPrev == targetIdx)
                        {
                            continue;
                        }

                        // targetIdx相当分のXとYをラッチ
                        double x = chartMargin + j;

                        // targetIdxのDataValueが、Ch設定のMax-Min幅に対して何%位置か算出する
                        double y = ListChData[i].LogData[targetIdx].DataValue;
                        y = (y - ListChData[i].ChMin) / (ListChData[i].ChMax - ListChData[i].ChMin);

                        // 0%以下もしくは100以上は0または100に丸める
                        if (y > 100) { y = 1; }
                        if (y < 0) { y = 0; }

                        // %をY軸ピクセルに変換する.その際、グラフ上方が原点になるので、1-yとして計算する。
                        y = (PictureMain.Height - chartMargin * 2) * (1 - y);
                        y += chartMargin;

                        // 1つ前のインデックスと異なる場合のみ、ラインを描画する
                        lock (lockobj)
                        {
                            p.Color = Color.FromArgb(ListChData[i].ChColor);

                            g.DrawLine(p, (float)xPrev, (float)yPrev, (float)x, (float)y);
                        }

                        targetIdxPrev = targetIdx;
                        xPrev = x;
                        yPrev = y;
                    }
                }
            });
            //}
        }

        /// <summary>
        /// 画面上部のメインChartを描画する
        /// </summary>
        private void DrawMainChart(Graphics g)
        {
            //サブチャートエリアを黒く塗りつぶす
            g.FillRectangle(Brushes.Black, 0, 0, PictureMain.Width, PictureMain.Height);

            //pictureSubのWidth分だけ、ListChDataからデータをピックアップして描画する
            DrawMainChartData(g);

            //Grid描画
            DrawMainChartGrid(g);

            //各種情報描画
            DrawMainChartInfo(g);
        }

        /// <summary>
        /// DatファイルをAsciiのCSVに変換する
        /// CSVのフォーマットは、"秒","ミリ秒","CAN ID(HEX)","Data1のHEX","Data2のHEX","Data3のHEX","Data4のHEX","Data5のHEX","Data6のHEX","Data7のHEX","Data8のHEX"
        /// </summary>
        private void ConvertToAsciiCSV(string SrcFileName, string DstFileName)
        {
            //CANデータ読み取り中フラグ
            IsReadingCanData = true;

            //ファイルサイズを調べる
            FileStream fs = new FileStream(SrcFileName, FileMode.Open, FileAccess.Read);
            long fileSize = fs.Length;

            //CANDataは1データ16バイトなので、用意する配列は　( ファイルサイズ / 16 - 1)
            long arySize = fileSize / 16;

            //CANDataが壊れていると、16で割り切れないかもしれないのでチェックしておき、あまりが出たら読み込む個数を1減らす
            long checkSize = fileSize % 16;

            if (checkSize != 0)
            {
                arySize--;
            }

            //バイナリーリーダー生成
            BinaryReader reader = new BinaryReader(fs);

            //canDataの配列にファイル内容コピーする
            CanData[] aryCanData = new CanData[arySize];

            //プログレスバー計算用
            long div_num = arySize / 100;
            int counter = 0;
            progressBar.Value = 0;
            progressBar.Maximum = (int)arySize*2;  // 1回目ループで50%, 2回目ループで100%にしたいので、2倍の設定をしておく

            for (int i = 0; i < arySize; i++)
            {
                //毎ループメッセージ発行するとクソ遅いので、トータル50回送るようにする。
                if (i % div_num == 0)
                {
                    progressBar.Value = (int)counter;
                    this.statusLabel.Text = string.Format("{0}%", ((int)counter) * 100 / arySize /2 );
                    Application.DoEvents();
                }
                counter++;

                aryCanData[i].timeSec = reader.ReadUInt32();
                aryCanData[i].timeMSec = reader.ReadUInt16();
                aryCanData[i].id = reader.ReadUInt16();
                aryCanData[i].data = new byte[8];

                for (int j = 0; j < 8; j++)
                {
                    aryCanData[i].data[j] = reader.ReadByte();
                }
            }

            reader.Close();

            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
                new System.IO.StreamWriter(DstFileName, false, enc);

            //レコードを書き込む
            // ToDo fieldはStringBuilderに置き換えること
            System.Text.StringBuilder field = new System.Text.StringBuilder("");

            for (int i = 0; i < arySize; i++)
            {
                //毎ループメッセージ発行するとクソ遅いので、トータル50回送るようにする。
                if (i % div_num == 0)
                {
                    progressBar.Value = (int)counter;
                    this.statusLabel.Text = string.Format("{0}%", ((int)counter) * 100 / arySize / 2);
                    Application.DoEvents();
                }
                counter++;

                field.Append(aryCanData[i].timeSec.ToString() + ",");
                field.Append(aryCanData[i].timeMSec.ToString() + ",");
                field.Append(aryCanData[i].id.ToString("X3") + ",");
                field.Append(aryCanData[i].data[0].ToString("X2") + ",");
                field.Append(aryCanData[i].data[1].ToString("X2") + ",");
                field.Append(aryCanData[i].data[2].ToString("X2") + ",");
                field.Append(aryCanData[i].data[3].ToString("X2") + ",");
                field.Append(aryCanData[i].data[4].ToString("X2") + ",");
                field.Append(aryCanData[i].data[5].ToString("X2") + ",");
                field.Append(aryCanData[i].data[6].ToString("X2") + ",");
                field.Append(aryCanData[i].data[7].ToString("X2") + "\r\n");

                //フィールドを書き込む
                sr.Write(field);

                //field初期化
                field.Clear();
            }

            //閉じる
            sr.Close();

            //CANデータ読み取り終了
            IsReadingCanData = false;

            //プログレスバー初期化
            progressBar.Value = 0;
            this.statusLabel.Text = "";
        }

        /// <summary>
        /// マウスポインタ位置をデータ現在位置時間に変換する
        /// </summary>
        double MousePointToSubPosTime(MouseEventArgs e)
        {
            //クリックした場合、クリックした座標が選択範囲の中心になるように調整する
            Point getXY = e.Location;

            //選択マーカー幅を計算する
            //データの時間幅に対する、divTime*20の割合から計算できる
            //まずデータの時間幅に対するメインチャート時間幅の割合を計算
            double ratioSelected = (divTime * 20) / (endTime - startTime);

            //ratioSelected上限処理 , データが短くてメインチャート1画面に収まり切る場合に発生
            if (ratioSelected > 1) { ratioSelected = 1; }

            double rectWidth = (PictureSub.Width - 2 * chartMargin) * ratioSelected;

            //rectWidth下限処理
            if (rectWidth < 2) { rectWidth = 2; }

            //startPosを算出
            double startPos = getXY.X - rectWidth / 2;

            //startPos上下限処理
            if (startPos < chartMargin) { startPos = chartMargin; }
            if (startPos + rectWidth > PictureSub.Width - 2 * chartMargin) { startPos = PictureSub.Width - chartMargin - rectWidth; }

            //startPosをposTimeに変換する
            //ToDo　末尾の+startTimeは、canloggerでKeyOn検出時にstartTimeは0で記録されているはずなので、不要のはずである
            subPosTime = (endTime - startTime) * ((startPos - chartMargin) / (PictureSub.Width - 2 * chartMargin)) + startTime;

            return subPosTime;
        }

        //=================================================================================================================================
        //
        //  自動生成イベントハンドラ
        //
        //=================================================================================================================================
        public FormMain()
        {
            InitializeComponent();

            // Formコンストラクタで、Private変数のClassをnewしておく
            DicChName = new Dictionary<String, int>();
            ListChData = new List<ChData>();
            decodeRule = new DecodeRule();
            IsReadingCanData = false;

            // フォームのDoubleBufferedもtrueにする
            this.DoubleBuffered = true;

            // BufferedListViewの生成と設定
            ListViewData = new BufferedListView();
            splitContainer3.Panel1.Controls.Add(this.ListViewData);
            ListViewData.Dock = DockStyle.Fill;
            ListViewData.View = View.Details;
            ListViewData.Columns.Add("Ch Name");
            ListViewData.Columns.Add("CurSor1");
            ListViewData.Columns.Add("CurSor2");
            ListViewData.Columns.Add("Max-Min");
            ListViewData.Columns[0].Width = 100;
            ListViewData.Columns[1].Width = 100;
            ListViewData.Columns[2].Width = 100;
            ListViewData.Columns[3].Width = 100;
            ListViewData.CheckBoxes = true;
            ListViewData.ItemChecked += ListViewData_ItemChecked;

            // pictureMainとpictureSubにマウスホイールイベント登録
            //ホイールイベントの追加  
            this.PictureMain.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.PictureMain_MouseWheel);

            this.PictureSub.MouseWheel
               += new System.Windows.Forms.MouseEventHandler(this.PictureSub_MouseWheel);

        }

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void CANDecodeSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDecodeOption f = new FormDecodeOption();

            f.ShowDialog(this);
            f.Dispose();  

            //一時的にListViewDataのClickイベントを削除
            //そうしないと、下でListViewItem追加のたびにClickイベントが起きてしまう為
            ListViewData.ItemChecked -= ListViewData_ItemChecked;

            //一時的に描画停止
            ListViewData.BeginUpdate();

            // ListViewDataをクリア
            ListViewData.Items.Clear();

            // DecodeRuleが設定されているので、その情報をもとにMainWindowのListViewを更新する
            for (int i=0; i<decodeRule.Count; i++)
            {
                ListViewItem newItem = new ListViewItem
                {
                    UseItemStyleForSubItems = false
                };

                newItem.SubItems[0].Text = decodeRule.GetChName(i);
                newItem.SubItems.Add("");
                newItem.SubItems.Add("");
                newItem.SubItems.Add("");
                newItem.Checked = decodeRule.GetChartShow(i);

                ListViewData.Items.Add(newItem);
            }

            //ListViewDataのClickイベント復活
            ListViewData.ItemChecked += ListViewData_ItemChecked;

            //描画再開
            ListViewData.EndUpdate();

            // データロードなければ何もしない
            if (ListChData.Count == 0)
            {
                return;
            }

            // もしデータがロード済だった場合は、ListChDataのチャンネル情報をアップデートする
            for (int i=0; i < decodeRule.Count; i++)
            {
                int idx;
                string chName;

                chName = decodeRule.GetChName(i);

                // decodeRule.ChName[i]と同じChNameのListChDataのidxを取得
                if (!DicChName.ContainsKey(chName))
                {
                    //decodeRuleのChName[i]がDicChNameになければ何もしない
                    continue;
                }

                idx = DicChName[chName];

                ListChData[idx].ChColor = decodeRule.GetChartColor(i);
                ListChData[idx].ChMax = decodeRule.GetChartMax(i);
                ListChData[idx].ChMin = decodeRule.GetChartMin(i);
                ListChData[idx].ChPreview = decodeRule.GetChartPreview(i);
                ListChData[idx].ChShow = decodeRule.GetChartShow(i);
            }

            // もしListChDataには存在するが、decodeRuleに存在しない場合(一度データをロードした後、あるChを削除した場合)、
            // ListChDataからデータを削除する。
            int j = 0;
            while (j < ListChData.Count)
            {
                int idx = decodeRule.ChNameIndexOf(ListChData[j].ChName);

                if (idx == -1)
                {
                    ListChData[j].Clear();
                    ListChData.RemoveAt(j);
                } else
                {
                    j++;
                }
            }

            // もし直前でListChDataがRemoveAtされる場合、DicChNameのKeyとValueの整合が崩れるため、DicChNameを再生成する
            DicChName.Clear();

            for (int i = 0; i<ListChData.Count; i++)
            {
                DicChName.Add(ListChData[i].ChName, i);
            }

            //　グラフ再描画
            PictureMain.Refresh();

            PictureSub.Refresh();
        }

         private void PictureSub_MouseDown(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            this.DraggingButton = e.Button;
            switch (this.DraggingButton)
            {
                case MouseButtons.Left:
                    this.IsDragging = true; // ドラッグ中であることを知らせる
                                            // 通常の矢印の代わりをマウスポインタとして表示
                    this.Cursor = Cursors.SizeWE;
                    break;
            }
        }

        private void PictureSub_MouseMove(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            if (this.IsDragging && this.DraggingButton == MouseButtons.Left)
            {
                //PictureSub上のマウス位置から、subPosTimeを算出する
                subPosTime = MousePointToSubPosTime(e);

                //DrawChart();
                PictureMain.Refresh();
                PictureSub.Refresh();
            }
        }

        private void PictureSub_MouseUp(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            switch (e.Button)
            {
                case MouseButtons.Left: // 左クリックの時
                    this.IsDragging = false; // ドラッグが終了していることを記録
                    this.Cursor = Cursors.Default; // マウスポインタを通常のものに戻す

                    //PictureSub上のマウス位置から、subPosTimeを算出する
                    subPosTime = MousePointToSubPosTime(e);

                    PictureMain.Refresh();
                    PictureSub.Refresh();
                    UpdateMapMarker();
                    UpdateMap();

                    break;
            }
            this.DraggingButton = 0;
        }

        private void PictureMain_MouseMove(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            //クリックした場合、クリックした座標が選択範囲の中心になるように調整する
            Point getXY = e.Location;

            if (this.IsDragging)
            {

                if (this.DraggingButton == MouseButtons.Left)
                {
                    //左クリックの場合、Cur1Posを更新
                    mainCur1Pos = getXY.X;
                }

                if (this.DraggingButton == MouseButtons.Right)
                {
                    //右クリックの場合、Cur2Posを更新
                    mainCur2Pos = getXY.X;
                }

                //DrawChart();
                PictureMain.Refresh();
                PictureSub.Refresh();
                UpdateMapMarker();
            }
        }
        private void ListViewData_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ListViewData.Items.Count < 1) return;

            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                if (!DicChName.ContainsKey(ListViewData.Items[i].Text)) {
                    break; 
                }

                int idx = DicChName[ListViewData.Items[i].Text];
                ListChData[idx].ChShow = ListViewData.Items[i].Checked;
            }

            //DrawChart();
            PictureMain.Refresh();
            PictureSub.Refresh();
        }

        private void PictureMain_MouseDown(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            this.DraggingButton = e.Button;
            switch (this.DraggingButton)
            {
                case MouseButtons.Left:
                    this.IsDragging = true; // ドラッグ中であることを知らせる
                                            // 通常の矢印の代わりをマウスポインタとして表示
                    this.Cursor = Cursors.SizeWE;
                    break;

                case MouseButtons.Right:
                    this.IsDragging = true; // ドラッグ中であることを知らせる
                                            // 通常の矢印の代わりをマウスポインタとして表示
                    this.Cursor = Cursors.SizeWE;
                    break;
            }
        }

        private void PictureMain_MouseUp(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            switch (e.Button)
            {
                case MouseButtons.Left: // 左クリックの時
                    this.IsDragging = false; // ドラッグが終了していることを記録
                    this.Cursor = Cursors.Default; // マウスポインタを通常のものに戻す

                    //DrawChart();
                    PictureMain.Refresh();
                    PictureSub.Refresh();
                    UpdateMapMarker();
                    break;

                case MouseButtons.Right: // 右クリックの時
                    this.IsDragging = false; // ドラッグが終了していることを記録
                    this.Cursor = Cursors.Default; // マウスポインタを通常のものに戻す

                    //DrawChart();
                    PictureMain.Refresh();
                    PictureSub.Refresh();
                    UpdateMapMarker();
                    break;
            }
            this.DraggingButton = 0;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //選択したファイル名を保持する
                currentDatFile = openFileDialog.FileName;

                //まずChDataをクリアする
                for (int i = 0; i < ListChData.Count - 1; i++)
                {
                    ListChData[i].Clear();
                }

                ListChData.Clear();
                DicChName.Clear();
                initTimeOffset = 0.0;
                startTime = 0.0;
                endTime = 0.0;

                // バイナリファイルからCANデータ抽出する
                ReadCANData(openFileDialog.FileName);

                // 初回描画する
                //DrawChart();
                PictureMain.Refresh();
                PictureSub.Refresh();
                UpdateMap();
                UpdateMapMarker();

                this.Text = openFileDialog.FileName;
            }
        }

        // マウスホイールイベント  
        // ホイール単独　→　divTimeづつ移動
        // ctrl + ホイール　→　2倍づつ拡大縮小
        private void PictureMain_MouseWheel(object sender, MouseEventArgs e)
        {
            // スクロール量
            int delta;
            delta = e.Delta;

            // ctrl + wheelなら、2倍づつ拡大縮小
            if((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                // GMapの拡大縮小に合わせて変更。プラスならdivTimeを0.5倍にする。
                // ホイールを奥に回す→拡大　手前に回す→縮小
                if (delta > 0)
                {
                    divTime /= 2;
                }
                else
                {
                    divTime *= 2;
                }
            }

            // wheelのみなら、divTime移動
            if ((Control.ModifierKeys & Keys.Control) == Keys.None)
            {
                // ホイールを奥に回す過去に1divtime移動　手前に回す→未来に1divtime移動
                if (delta > 0)
                {
                    subPosTime -= divTime;
                }
                else
                {
                    subPosTime += divTime;
                }
            }


            PictureMain.Refresh();
            PictureSub.Refresh();
            UpdateMap();
            UpdateMapMarker();
        }

        // マウスホイールイベント  
        // PictureSubでホイール時は、データをdivTimeづつ左右に送る
        private void PictureSub_MouseWheel(object sender, MouseEventArgs e)
        {
            // スクロール量
            int delta;
            delta = e.Delta;

            // shift + wheelなら、拡大縮小
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                // GMapの拡大縮小に合わせて変更。プラスならdivTimeを0.5倍にする。
                // ホイールを奥に回す→拡大　手前に回す→縮小
                if (delta > 0)
                {
                    divTime /= 2;
                }
                else
                {
                    divTime *= 2;
                }
            }

            // wheelのみなら、divTime移動
            if ((Control.ModifierKeys & Keys.Control) == Keys.None)
            {
                // ホイールを奥に回す過去に1divtime移動　手前に回す→未来に1divtime移動
                if (delta > 0)
                {
                    subPosTime -= divTime;
                }
                else
                {
                    subPosTime += divTime;
                }
            }

            PictureMain.Refresh();
            PictureSub.Refresh();
            UpdateMap();
            UpdateMapMarker();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // GMap proxy対応
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;

            // Google Map APIが入力済なら、Google Mapを表示
            string strGoogleMapAPIKey = Properties.Settings.Default.GoogleAPI;

            if (strGoogleMapAPIKey != "")
            {
                GMapProviders.GoogleMap.ApiKey = strGoogleMapAPIKey;
                GMapControl.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            }
            // Google Map APIがレジストリになければ、Bing Mapを表示
            else
            {
                GMapControl.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            }

            GMaps.Instance.Mode = AccessMode.ServerOnly;
            GMapControl.SetPositionByKeywords("Tokyo, Japan");

            //地図中心を、先回最終位置に合わせる
            double lat, lon;
            lat = Properties.Settings.Default.FormMainLat;
            lon = Properties.Settings.Default.FormMainLon;
            GMapControl.Position = new PointLatLng(lat, lon);
            GMapControl.Zoom = 13;

            //マーカー用オーバーレイ生成
            GMapOverlayMarker = new GMapOverlay("marker");
            GMapOverlayRoute = new GMapOverlay("route");
            GMapControl.Overlays.Add(GMapOverlayMarker);
            GMapControl.Overlays.Add(GMapOverlayRoute);

            // Form位置ロード
            this.Location = Properties.Settings.Default.FormMainLocation;
            this.Size = Properties.Settings.Default.FormMainSize;
            this.splitContainer1.SplitterDistance = Properties.Settings.Default.FormMainSplit1;
            this.splitContainer2.SplitterDistance = Properties.Settings.Default.FormMainSplit2;
            this.splitContainer3.SplitterDistance = Properties.Settings.Default.FormMainSplit3;
        }

        private void MenuConvertAscii_Click(object sender, EventArgs e)
        {
            // ファイルが未選択ならOpenDialogを表示してファイル選択する
            if (currentDatFile == "")
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                currentDatFile = openFileDialog.FileName;
            }

            // 名前をつけて保存
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ConvertToAsciiCSV(currentDatFile, saveFileDialog.FileName);
            }
        }

        private void MenuBingMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
        }

        private void MenuGoogleMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
        }

        private void MenuOpenCycleMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.OpenCycleMapProvider.Instance;
        }

        private void MenuOpenStreetMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
        }

        private void MenuWikiMapiaMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.WikiMapiaMapProvider.Instance;
        }

        private void MapSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // GoogleApi入力フォーム表示
            FormMapOption f = new FormMapOption();

            f.ShowDialog(this);
            f.Dispose();
        }

        private void PictureMain_Paint(object sender, PaintEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            // ListChNameがNULL→即抜ける
            if (DicChName == null) { return; }

            // ListChNameが0＝CANデータ未読み込み→即抜ける
            if (DicChName.Count < 1) { return; }

            //　アプリ上部のメインチャートを描画
            DrawChartMain(e.Graphics);
        }

        private void PictureSub_Paint(object sender, PaintEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            // ListChNameがNULL→即抜ける
            if (DicChName == null) { return; }

            // ListChNameが0＝CANデータ未読み込み→即抜ける
            if (DicChName.Count < 1) { return; }

            //　アプリ上部のメインチャートを描画
            DrawChartSub(e.Graphics);
        }

        private void AboutAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout f = new FormAbout();

            f.ShowDialog(this);
            f.Dispose();
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //ブラウザで開く
            System.Diagnostics.Process.Start("https://motoreco.net");
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Form位置セーブ
            Properties.Settings.Default.FormMainLocation = this.Location;
            Properties.Settings.Default.FormMainSize = this.Size;
            Properties.Settings.Default.FormMainSplit1 = this.splitContainer1.SplitterDistance;
            Properties.Settings.Default.FormMainSplit2 = this.splitContainer2.SplitterDistance;
            Properties.Settings.Default.FormMainSplit3 = this.splitContainer3.SplitterDistance;

            // 地図中心セーブ
            Properties.Settings.Default.FormMainLat = GMapControl.Position.Lat;
            Properties.Settings.Default.FormMainLon = GMapControl.Position.Lng;

            Properties.Settings.Default.Save();
        }

        private void MenuAppend_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //選択したファイル名を保持する
                currentDatFile = openFileDialog.FileName;

                initTimeOffset = endTime;

                // バイナリファイルからCANデータ抽出する
                ReadCANData(openFileDialog.FileName);

                // 初回描画する
                //DrawChart();
                PictureMain.Refresh();
                PictureSub.Refresh();
                UpdateMap();
                UpdateMapMarker();

                this.Text = openFileDialog.FileName;
            }
        }
    }
}
