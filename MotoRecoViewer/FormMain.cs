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
        private double subPosTimePrev;              //サブチャート選択位置、前回値
        private double mainCur1Pos;                 //メインチャートカーソル1X位置
        private double mainCur1PosPrev;             //メインチャートカーソル1X位置前回値
        private double mainCur2Pos;                 //メインチャートカーソル2X位置
        private double mainCur2PosPrev;             //メインチャートカーソル2X位置前回値
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

        // DirectX関連
        private SharpDX.Direct3D11.Device       dxMainDevice;
        private SharpDX.DXGI.SwapChain          dxMainSwapChain;
        private SharpDX.Direct3D11.Texture2D    dxMainBackBuffer;
        private SharpDX.Direct2D1.Factory       dxMainD2dFactory;
        private SharpDX.Direct2D1.RenderTarget  dxMainRenderTarget2D;
        private SharpDX.DirectWrite.Factory     dxMainFactoryDWrite;
        private TextFormat                      dxMainTextFont;
        private SharpDX.DXGI.Surface            dxMainSurface;

        private SharpDX.Direct3D11.Device       dxSubDevice;
        private SharpDX.DXGI.SwapChain          dxSubSwapChain;
        private SharpDX.Direct3D11.Texture2D    dxSubBackBuffer;
        private SharpDX.Direct2D1.Factory       dxSubD2dFactory;
        private SharpDX.Direct2D1.RenderTarget  dxSubRenderTarget2D;
        private SharpDX.DirectWrite.Factory     dxSubFactoryDWrite;
        private TextFormat                      dxSubTextFont;
        private SharpDX.DXGI.Surface            dxSubSurface;


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

            // この4関数は依存関係なし→並列処理可
            Parallel.Invoke(
                () => CalcGPSDistance(),                    //GPS積算距離を計算
                () => CalcFrSpeedDistance(),                //FrSpeed積算距離を計算
                () => CalcAccumulatedDistCountFr(),         //Fr距離カウンタから積算距離を計算
                () => CalcAccumulatedDistCountRr(),         //Rr距離カウンタから積算距離を計算
                () => CalcAccumulatedFuelCount()            //燃料カウンタから消費燃料を計算
            );

            //燃費を出すには、積算距離と積算燃料が計算できていないといけない
            //燃費を計算
            CalcFuelConsumption();

            //Rangeを出すには、燃費が計算できていないといけない
            //走行可能距離を計算
            CalcRange();
            

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
            int idx_GPSDistance = GetIndexOfFixedFormula("#GPS_Distance");
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
        /// ＃K51_FrSpeed1から#BMW_DistFrSpeed1を計算する
        /// </summary>
        private void CalcFrSpeedDistance()
        {
            //#BMW_FrSpeed1 のChName取得
            int idx_FrSpeed = GetIndexOfFixedFormula("#BMW_FrSpeed1");
            if (idx_FrSpeed < 0) { return; }

            //#BMW_DistFrSpeed1 のChName取得
            int idx_DistFrSpeed = GetIndexOfFixedFormula("#BMW_DistFrSpeed1");
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
        /// ＃K51_DistCountFr から K51_AccumulatedDistCountFrを計算する
        /// </summary>
        private void CalcAccumulatedDistCountFr()
        {
            //#BMW_DistCountFr のChName取得
            int idx_DistCount = GetIndexOfFixedFormula("#BMW_DistCountFr");
            if (idx_DistCount < 0) { return; }

            //#BMW_AccumulatedDistCountFr のChName取得
            int idx_AccumulatedDistCount = GetIndexOfFixedFormula("#BMW_AccumulatedDistCountFr");
            if (idx_AccumulatedDistCount < 0) { return; }

            //#BMW_DistCountの積分計算
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

                // カウンタ1周またはリセット時、負の値になる。
                if (dCounter < 0)
                {
                    // 0で判定すると、EG STOP時もカウントアップしてしまうので、明らかに1周カウンタが回ったときだけ+3951する
                    if (dCounter < -3900)
                    {
                        // DistCountFrは、MAX3951
                        dCounter += 3951;
                    }
                    else
                    {
                        dCounter = 0;
                    }
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
        /// ＃K51_DistCountRr から K51_AccumulatedDistCountRr　を計算する
        /// </summary>
        private void CalcAccumulatedDistCountRr()
        {
            //#BMW_DistCountRr のChName取得
            int idx_DistCount = GetIndexOfFixedFormula("#BMW_DistCountRr");
            if (idx_DistCount < 0) { return; }

            //#BMW_AccumulatedDistCountRr のChName取得
            int idx_AccumulatedDistCount = GetIndexOfFixedFormula("#BMW_AccumulatedDistCountRr");
            if (idx_AccumulatedDistCount < 0) { return; }

            //#BMW_DistCountの積分計算
            TVData tvData;
            double AccumulatedCounter = 0.0;

            tvData = ListChData[idx_AccumulatedDistCount].LogData[0];
            tvData.DataValue = AccumulatedCounter;
            ListChData[idx_AccumulatedDistCount].LogData[0] = tvData;

            for (int i = 1; i < ListChData[idx_DistCount].Count; i++)
            {
                // DistCount unit:?
                double dCounter = ListChData[idx_DistCount].LogData[i].DataValue - ListChData[idx_DistCount].LogData[i - 1].DataValue;

                //ToDo File Append時、工夫が必要

                // カウンタ1周またはリセット時、負の値になる。
                if (dCounter < 0)
                {
                    // 0で判定すると、EG STOP時もカウントアップしてしまうので、明らかに1周カウンタが回ったときだけ+255する
                    if (dCounter < -250)
                    {
                        // DistCountFrは、MAX255
                        dCounter += 255;
                    }
                    else
                    {
                        dCounter = 0;
                    }
                }

                AccumulatedCounter += dCounter;

                tvData = ListChData[idx_AccumulatedDistCount].LogData[i];

                // 積算カウンタ
                // カウンタ値に0.004掛けると、大体Fr車速を積分した積算距離に等しくなっている。
                // Rrは加速中Fr車速よりSlipするため早くなるので、Fr車速の積分よりは多少距離が伸びる
                tvData.DataValue = AccumulatedCounter * 0.004;
                ListChData[idx_AccumulatedDistCount].LogData[i] = tvData;
            }
        }

        /// <summary>
        /// #BMW_FuelCount から #BMW_AccumulatedFuelCount
        /// </summary>
        private void CalcAccumulatedFuelCount()
        {
            //#BMW_FuelCount のChName取得
            int idx_FuelCount = GetIndexOfFixedFormula("#BMW_FuelCount");
            if (idx_FuelCount < 0) { return; }

            //#BMW_AccumulatedFuelCount のChName取得
            int idx_AccumulatedFuelCount = GetIndexOfFixedFormula("#BMW_AccumulatedFuelCount");
            if (idx_AccumulatedFuelCount < 0) { return; }

            //#BMW_DistCountの積分計算
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

                // EGリセット時又はカウンタ1周すると負の値になる
                if (dCounter < 0)
                {
                    // 0で判定すると、EG STOP時もカウントアップしてしまうので、明らかに1周カウンタが回ったときだけ+256*256する
                    if (dCounter < -65000)
                    {
                        dCounter += 256 * 256;
                    } else {
                        dCounter = 0;
                    }
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
            //#BMW_AccumulatedFuelCount のChName取得
            int idx_AccumulatedFuelCount = GetIndexOfFixedFormula("#BMW_AccumulatedFuelCount");
            if (idx_AccumulatedFuelCount < 0) { return; }

            //#BMW_AccumulatedDistCountFr のChName取得
            int idx_AccumulatedDistCount = GetIndexOfFixedFormula("#BMW_AccumulatedDistCountFr");
            if (idx_AccumulatedDistCount < 0) { return; }

            //#BMW_FuelConsumption のChName取得
            int idx_FuelConsumption = GetIndexOfFixedFormula("#BMW_FuelConsumption");
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

            //#BMW_FuelLevel のChName取得
            int idx_FuelLevel = GetIndexOfFixedFormula("#BMW_FuelLevel");
            if (idx_FuelLevel < 0) { return; }

            //#BMW_FuelConsumption のChName取得
            int idx_FuelConsumption = GetIndexOfFixedFormula("#BMW_FuelConsumption");
            if (idx_FuelConsumption < 0) { return; }

            //#BMW_Range のChName取得
            int idx_Range = GetIndexOfFixedFormula("#BMW_Range");
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
                if (!DicChName.ContainsKey(ListViewData.Items[i].Text))
                {
                    continue;
                }

                int idx = DicChName[ListViewData.Items[i].Text];
                int targetIdx = ListChData[idx].FindLeftIndex(targetTime1);

                ListViewData.Items[i].SubItems[1].Text = ListChData[idx].LogData[targetIdx].DataValue.ToString();
            }

            // MainChartのカーソル位置2に対応するタイムスタンプを計算
            // mainCur1Posは、PanelMainChart上の絶対的なX座標の為、グラフ描画領域幅に対するポジションに変換する
            double ratioMainCurPos2 = (mainCur2Pos - chartMargin) / (PanelMainChart.Width - 2 * chartMargin);
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

            //ListViewData更新再開
            ListViewData.EndUpdate();
        }

        /// <summary>
        /// 地図を更新表示する
        /// 緯度経度のチャンネル名はLatitudeとLongitude固定とする
        /// 地図位置はカーソル1位置とする
        /// </summary>
        private void UpdateMap()
        {
            //経度緯度情報の存在チェック
            int idx_lat = GetIndexOfFixedFormula("#GPS_Latitude");
            int idx_lon = GetIndexOfFixedFormula("#GPS_Longitude");

            //どちらかなければ何もしない
            if ((idx_lat < 0) || (idx_lat < 0))
            {
                return;
            }

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
                Stroke = new Pen(System.Drawing.Color.Red, 3)
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
            //経度緯度情報の存在チェック
            int idx_lat = GetIndexOfFixedFormula("#GPS_Latitude");
            int idx_lon = GetIndexOfFixedFormula("#GPS_Longitude");

            //どちらかなければ何もしない
            if ((idx_lat < 0) || (idx_lat < 0))
            {
                return;
            }

            // MainChartのカーソル位置1に対応するタイムスタンプを計算
            double cur1PosTime = subPosTime + (divTime * 20) / (PanelMainChart.Width - 2 * chartMargin) * mainCur1Pos;

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
            marker.ToolTipText = "Cursor1";
            GMapOverlayMarker.Markers.Add(marker);

            //カーソル2位置マーカー更新
            // MainChartのカーソル位置2に対応するタイムスタンプを計算
            double cur2PosTime = subPosTime + (divTime * 20) / (PanelMainChart.Width - 2 * chartMargin) * mainCur2Pos;

            // カーソル位置左側データidx取得
            leftIdx = ListChData[idx_lat].FindLeftIndex(cur2PosTime);
            leftTime = ListChData[idx_lat].LogData[leftIdx].DataTime;

            // カーソル位置右側データidx取得
            if (leftIdx == ListChData[idx_lat].LogData.Count - 1)
            {
                rightIdx = leftIdx;
            }
            else
            {
                rightIdx = leftIdx + 1;   // ToDo 経度緯度データが、重複してロギングされているため.CanLogger側を治す 
            }
            rightTime = ListChData[idx_lat].LogData[rightIdx].DataTime;

            // 緯度経度取得
            leftLon = ListChData[idx_lon].LogData[leftIdx].DataValue;
            leftLat = ListChData[idx_lat].LogData[leftIdx].DataValue;

            rightLon = ListChData[idx_lon].LogData[rightIdx].DataValue;
            rightLat = ListChData[idx_lat].LogData[rightIdx].DataValue;

            // 経度緯度を前後2点データから線形補間する
            liLat = LinearInterpolation(leftTime, leftLat, rightTime, rightLat, cur2PosTime);
            liLon = LinearInterpolation(leftTime, leftLon, rightTime, rightLon, cur2PosTime);

            //マーカー更新
            GMarkerGoogle marker2 = new GMarkerGoogle(new PointLatLng(liLat, liLon), GMarkerGoogleType.red);
            marker2.ToolTipText = "Cursor2";
            GMapOverlayMarker.Markers.Add(marker2);
        }

        /// <summary>
        /// 画面下部のサブChartのグリッドを描画する
        /// </summary>
        private void DrawSubChartGridD2D()
        {
            SolidColorBrush brush = new SolidColorBrush(dxSubRenderTarget2D, SharpDX.Color.Green);

            //サブチャートエリアの枠を書く

            //横線
            for (int i = 0; i < 21; i++)
            {
                // 罫線間ピクセルを算出
                float rule = (float)(PanelSubChart.Width - 2f * chartMargin) / 20f;

                // X座標
                float x = (float)chartMargin + i * rule;

                // Y座標
                float y1 = (float)chartMargin;
                float y2 = (float)PanelSubChart.Height - (float)chartMargin;

                // グリッドカラーも設定できるようにする
                dxSubRenderTarget2D.DrawLine(new Vector2(x, y1), new Vector2(x, y2), brush);
            }

            //縦線
            // X座標
            for (int i = 0; i < 3; i++)
            {
                // 罫線間ピクセルを算出
                float rule = (float)(PanelSubChart.Height - 2f * chartMargin) / 2f;

                // X座標
                float y = (float)chartMargin + i * rule;

                // Y座標
                float x1 = (float)chartMargin;
                float x2 = (float)PanelSubChart.Width - (float)chartMargin;

                // ToDo グリッドカラーも設定できるようにする
                dxSubRenderTarget2D.DrawLine(new Vector2(x1, y), new Vector2(x2, y), brush);
            }
        }

        /// <summary>
        /// 画面下部のサブChartの描画する
        /// </summary>
        private void DrawSubChartDataD2D()
        {
            // ListChNameを調べてプレビュー表示TrueのものだけSubChartに表示する
            int idxPreview = 0;

            //描画ピクセル数（幅）計算
            int arySize = PanelSubChart.Width - 2 * (int)chartMargin +1;

            //描画Point用配列に対応したタイムスタンプ保持用配列
            double[] aryTimeStamp = new double[arySize];

            //タイムスタンプ計算
            for (int i = 0; i < arySize; i++)
            {
                aryTimeStamp[i] = startTime + (endTime - startTime) / (PanelSubChart.Width - 2d * chartMargin) * i;
            }

            //for (int i = 0; i < ListChData.Count; i++)
            Parallel.For(0, ListChData.Count, i =>
            {
                // Subチャートに表示するのは、ChData.ChPreviewがTrueの物のみ
                if (ListChData[i].ChPreview)
                {
                    // 座標格納用
                    // データ追加ごとにResizeするとCPUリソース食うので、想定される最大数確保し、最後に縮小する

                    RawVector2[] points = new RawVector2[arySize];

                    int drawCount = 0;
                    double targetIdxPrev = 0d;

                    // SubChart描画ピクセル幅に対してのみ描画処理実施する
                    for (int j = 0; j <= PanelSubChart.Width - 2 * chartMargin; j++)
                    {
                        // タイムスタンプに最も近いChDataのインデックスを取得
                        int targetIdx = ListChData[i].FindLeftIndex(aryTimeStamp[j]);

                        // 1つ前のインデックスと同じ場合、スキップする
                        if ((targetIdxPrev == targetIdx) || (targetIdx == 0))
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
                        y = (PanelSubChart.Height - chartMargin * 2d) * (1d - y);
                        y += chartMargin;

                        //座標格納
                        points[drawCount].X = (int)x;
                        points[drawCount].Y = (int)y;

                        //描画座標数インクリメント
                        drawCount++;

                        targetIdxPrev = targetIdx;
                    }

                    //配列要素数を実際のデータカウント数に調整する
                    Array.Resize(ref points, drawCount);

                    //描画ポイントを多角形として扱うため、ジオメトリーにセットする
                    PathGeometry geo1 = new PathGeometry(dxSubD2dFactory);
                    GeometrySink sink1 = geo1.Open();
                    sink1.BeginFigure(points[0], new FigureBegin());
                    sink1.AddLines(points);
                    sink1.EndFigure(new FigureEnd());
                    sink1.Close();

                    //Chごとの色をブラシにセット
                    System.Drawing.Color c;
                    c = System.Drawing.Color.FromArgb(ListChData[i].ChColor);

                    SharpDX.Mathematics.Interop.RawColor4 rc4;
                    rc4 = ColorToRaw4(c);

                    //ブラシ生成
                    SolidColorBrush brush = new SolidColorBrush(dxSubRenderTarget2D, SharpDX.Color.Red);
                    brush.Color = rc4;

                    //グラフ描画
                    dxSubRenderTarget2D.DrawGeometry(geo1, brush);

                    //ブラシ破棄
                    brush.Dispose();
                    geo1.Dispose();
                    sink1.Dispose();
                }

                idxPreview++;
            });
            //}
        }

        /// <summary>
        /// 画面下部のサブChartに選択位置を表示する
        /// </summary>
        private void DrawSubChartPosD2D()
        {
            //選択マーカー幅を計算する
            //データの時間幅に対する、divTime*20の割合から計算できる
            //まずデータの時間幅に対するメインチャート時間幅の割合を計算
            double ratioSelected = (divTime * 20) / (endTime - startTime);

            //ratioSelected上限処理 , データが短くてメインチャート1画面に収まり切る場合に発生
            if (ratioSelected > 1) { ratioSelected = 1; }

            double rectWidth = (PanelSubChart.Width - 2 * chartMargin) * ratioSelected;

            //rectWidth下限処理
            if (rectWidth < 2) { rectWidth = 2; }

            //XY座標計算
            double x1 = (subPosTime - startTime) / (endTime - startTime);       //ToDo startTimeはMotoRecoでロギング時0のはずなので本来は不要
            x1 = x1 * (PanelSubChart.Width - 2 * chartMargin) + chartMargin;
            double x2 = x1 + rectWidth;
            double y1 = chartMargin;
            double y2 = PanelSubChart.Height - chartMargin;

            //ブラシ生成
            SolidColorBrush brush = new SolidColorBrush(dxSubRenderTarget2D, SharpDX.Color.White);

            //rect生成
            SharpDX.Mathematics.Interop.RawRectangleF rect = new SharpDX.Mathematics.Interop.RawRectangleF((float)x1, (float)y1, (float)x2, (float)y2);

            //選択ポジション白枠描画
            dxSubRenderTarget2D.DrawRectangle(rect, brush);

            brush.Dispose();
        }

        /// <summary>
        /// 画面下部のサブChartを描画する
        /// </summary>
        private void DrawSubChartD2D()
        {
            //サブチャートエリアを黒く塗りつぶす
            dxSubRenderTarget2D.Clear(SharpDX.Color.Black);

            DrawSubChartGridD2D();

            //pictureSubのWidth分だけ、ListChDataからデータをピックアップして描画する
            DrawSubChartDataD2D();

            //サブチャートに現在位置を反転表示で表示
            DrawSubChartPosD2D();
        }


        /// <summary>
        /// 画面上部のメインChartのグリッドを描画する
        /// </summary>
        private void DrawMainChartGridD2D()
        {
            SolidColorBrush brush = new SolidColorBrush(dxMainRenderTarget2D, SharpDX.Color.Green);

            //メインチャートエリアの枠を書く

            //横線
            for (int i = 0; i < 21; i++)
            {
                // 罫線間ピクセルを算出
                float rule = ((float)PanelMainChart.Width - 2f * (float)chartMargin) / 20f;

                // X座標
                float x = (float)chartMargin + i * rule;

                // Y座標
                float y1 = (float)chartMargin;
                float y2 = (float)PanelMainChart.Height - (float)chartMargin;

                dxMainRenderTarget2D.DrawLine(new Vector2(x,y1), new Vector2(x,y2),brush);
            }

            //縦線
            // X座標
            for (int i = 0; i < 11; i++)
            {
                // 罫線間ピクセルを算出
                float rule = ((float)PanelMainChart.Height - 2f * (float)chartMargin) / 10f;

                // X座標
                float y = (float)chartMargin + i * rule;

                // Y座標
                float x1 = (float)chartMargin;
                float x2 = (float)PanelMainChart.Width - (float)chartMargin;

                dxMainRenderTarget2D.DrawLine(new Vector2(x1, y), new Vector2(x2, y), brush);
            }

            brush.Color = SharpDX.Color.White;

            //カーソル1
            var strokeStyle_dashed1 = new StrokeStyle(dxMainD2dFactory, new StrokeStyleProperties() { DashOffset = 5, DashCap = CapStyle.Square, DashStyle = SharpDX.Direct2D1.DashStyle.DashDot });
            dxMainRenderTarget2D.DrawLine(new Vector2((float)mainCur1Pos, (float)chartMargin), new Vector2((float)mainCur1Pos, PanelMainChart.Height - (float)chartMargin), brush, 3, strokeStyle_dashed1);

            //カーソル2
            var strokeStyle_dashed2 = new StrokeStyle(dxMainD2dFactory, new StrokeStyleProperties() { DashOffset = 5, DashCap = CapStyle.Square, DashStyle = SharpDX.Direct2D1.DashStyle.DashDotDot });
            dxMainRenderTarget2D.DrawLine(new Vector2((float)mainCur2Pos, (float)chartMargin), new Vector2((float)mainCur2Pos, PanelMainChart.Height - (float)chartMargin), brush, 3, strokeStyle_dashed2);

            brush.Dispose();
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
        private void DrawMainChartInfoD2D()
        {

            //TimeDivを表示する
            string str = "1Div:" + divTime.ToString() + "sec";
            DrawText(str, new Vector2(1.0f, 1.0f));

            // divTimeの小数点以下桁数を計算
            int pres = GetPrecision(divTime);

            // 元のCANDATAがせいぜい10ms周期なので、2桁までとする
            if (pres > 2) { pres = 2; }

            //時刻を下部に表示する
            //右端の罫線はタイムスタンプ見切れるので表示しない
            for (int i = 0; i < 20; i++)
            {
                // 罫線間ピクセルを算出
                double rule = (PanelMainChart.Width - 2d * chartMargin) / 20d;

                // X座標
                double x = chartMargin + i * rule;

                // Y座標
                double y2 = PanelMainChart.Height - chartMargin;

                // 各罫線に対応するタイムスタンプを計算
                double targetTime = subPosTime + (divTime * i);

                str = targetTime.ToString("F" + pres.ToString());

                DrawText(str, new Vector2((float)x - 5, (float)y2 + 1));
            }
        }

        /// <summary>
        /// ColorをRawColor4に変換する
        /// </summary>
        SharpDX.Mathematics.Interop.RawColor4 ColorToRaw4(System.Drawing.Color color)
        {
            const float n = 255f;
            return new SharpDX.Mathematics.Interop.RawColor4(color.R / n, color.G / n, color.B / n, color.A / n);
        }

        /// <summary>
        /// 画面上部のメインChartの描画する
        /// </summary>
        private void DrawMainChartDataD2D()
        {
            //描画Point用配列数確保
            int arySize = PanelMainChart.Width - 2 * (int)chartMargin + 1;

            //描画Point用配列に対応したタイムスタンプ保持用配列
            double[] aryTimeStamp = new double[arySize];

            //タイムスタンプ計算
            for (int i = 0; i < arySize; i++)
            {
                aryTimeStamp[i] = subPosTime + (divTime * 20d) / (double)arySize * i;
            }

            // ListChNameの項目数すべて描画する
            Parallel.For(0, ListChData.Count, i =>
            //for (int i = 0; i < ListChData.Count; i++)
            {
                int targetIdxPrev = 0;

                if (ListChData[i].ChShow)
                {
                    // 座標格納用
                    // データ追加ごとにResizeするとCPUリソース食うので、想定される最大数確保し、最後に縮小する
                    RawVector2[] points = new RawVector2[arySize];
                    
                    int drawCount = 0;

                    //タイムスタンプ最初に対応したChDataインデックス取得
                    int minIdx = ListChData[i].FindLeftIndex(aryTimeStamp[0]);
                    targetIdxPrev = minIdx - 1;                                          // 1引くのは、初回は描画を絶対したいから
  
                    //タイムスタンプ最後に対応したChDataインデックス取得
                    int maxIdx = ListChData[i].FindLeftIndex(aryTimeStamp[arySize - 1]);

                    int startIdx, endIdx;

                    startIdx = minIdx;
                    endIdx = maxIdx;

                    //インデックス差分
                    int diffIdx;

                    // MainChart描画ピクセル幅に対してのみ描画処理実施する
                    for (int j = 0; j < arySize; j++)
                    {
                        // targetTimeに対応したタイムスタンプに最も近いChDataのインデックスを取得
                        int targetIdx = ListChData[i].FindLeftIndex(aryTimeStamp[j], startIdx, endIdx);

                        if (targetIdx != 0)
                        {
                            //　前回indexからの差分
                            diffIdx = targetIdx - targetIdxPrev;

                            if (diffIdx < 0)
                            {
                                continue;
                            }

                            // 1つ前のインデックスと同じ場合 または targetIdx が 0の場合何もしない
                            // ただし右端については前回Indexと同じでも描画させる
                            if ((targetIdxPrev == targetIdx) && (j != arySize - 1))
                            {
                                continue;
                            }

                            // 次のサーチ時は、今回データより後ろなのでスタート位置を変更する
                            startIdx = targetIdx + diffIdx - 4;

                            // startIdx MAXガード
                            if (startIdx > maxIdx)
                            {
                                startIdx = maxIdx;
                            }

                            // startIdx MINガード
                            if (startIdx < 0)
                            {
                                startIdx = 0;
                            }

                            // 次のタイムスタンプの時刻が次の検索スタートの時刻より小さい場合は調整する
                            if (j != arySize - 1)
                            {
                                if (aryTimeStamp[j + 1] < ListChData[i].LogData[startIdx].DataTime)
                                {
                                    startIdx = targetIdx + 1;
                                }
                            }

                            // 次のサーチ時のendIdxは、diffIdx+4とする。diffIdxは安定しているため、+4以内で十分な精度がある
                            endIdx = targetIdx + (int)(diffIdx + 4);

                            // endIdx MAXガード
                            if (endIdx > maxIdx) { endIdx = maxIdx; }

                            // 次のタイムスタンプの時刻が次の検索終了の時刻より大きい場合は調整する
                            if (j != arySize - 1)
                            {
                                if (aryTimeStamp[j + 1] > ListChData[i].LogData[endIdx].DataTime)
                                {
                                    endIdx = maxIdx;
                                }
                            }
                        }
                        else
                        {
                            //targetIdx = 0ならcontinue
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
                        y = (PanelMainChart.Height - chartMargin * 2) * (1 - y);
                        y += chartMargin;

                        //座標格納
                        points[drawCount].X = (float)x;
                        points[drawCount].Y = (float)y;

                        //描画座標数インクリメント
                        drawCount++;

                        targetIdxPrev = targetIdx;
                    }

                    //配列要素数を実際のデータカウント数に調整する
                    Array.Resize(ref points, drawCount);

                    //drawCountが0の場合は何もしない
                    if (drawCount == 0) { return; }

                    //描画ポイントを多角形として扱うため、ジオメトリーにセットする
                    PathGeometry geo1 = new PathGeometry(dxMainD2dFactory);
                    GeometrySink sink1 = geo1.Open();
                    sink1.BeginFigure(points[0], new FigureBegin());
                    sink1.AddLines(points);
                    sink1.EndFigure(new FigureEnd());
                    sink1.Close();

                    //Chごとの色をブラシにセット
                    System.Drawing.Color c;
                    c = System.Drawing.Color.FromArgb(ListChData[i].ChColor);

                    SharpDX.Mathematics.Interop.RawColor4 rc4;
                    rc4 = ColorToRaw4(c);

                    //ブラシ生成
                    SolidColorBrush brush = new SolidColorBrush(dxMainRenderTarget2D, SharpDX.Color.Red);
                    brush.Color = rc4;

                    //グラフ描画
                    dxMainRenderTarget2D.DrawGeometry(geo1, brush);

                    //ブラシ破棄
                    brush.Dispose();
                    geo1.Dispose();
                    sink1.Dispose();
                }
            });
            //}
        }

        /// <summary>
        /// 画面上部のメインChartを描画する
        /// </summary>
        private void DrawMainChartD2D()
        {
            //メインチャートエリアを黒く塗りつぶす
            dxMainRenderTarget2D.Clear(SharpDX.Color.Black);

            //Grid描画
            DrawMainChartGridD2D();

            //pictureSubのWidth分だけ、ListChDataからデータをピックアップして描画する
            DrawMainChartDataD2D();

            //各種情報描画
            DrawMainChartInfoD2D();
        }

        /// <summary>
        /// DatファイルをAsciiのCSVに変換する
        /// CSVのフォーマットは、"秒","ミリ秒","CAN ID(HEX)","Data1のHEX","Data2のHEX","Data3のHEX","Data4のHEX","Data5のHEX","Data6のHEX","Data7のHEX","Data8のHEX"
        /// </summary>
        private void ConvertCANData(string SrcFileName, string DstFileName)
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
        /// 画面に表示中のデコード済データをCSVに変換する
        /// CSVのフォーマットは、"秒","ミリ秒","ch1 物理値","ch2物理値"...とする
        /// <param name="DstFileName">エクスポートファイル名</param>
        /// <param name="Mode">0:ファイル全体 1:カーソル間</param>
        /// </summary>
        private void ConvertDecodeData(string DstFileName, int Mode)
        {
            const int MODE_ALL = 0;
            const int MODE_CURSOR = 1;

            //開始時間と終了時間を取得
            double exportStartTime;
            double exportEndTime;

            if (Mode == MODE_CURSOR)
            {
                // MainChartのカーソル位置1に対応するタイムスタンプを計算
                // mainCur1Posは、PanelMainChart上の絶対的なX座標の為、グラフ描画領域幅に対するポジションに変換する
                double ratioMainCurPos1 = (mainCur1Pos - chartMargin) / (PanelMainChart.Width - 2 * chartMargin);
                exportStartTime = subPosTime + (divTime * 20) * ratioMainCurPos1;

                double ratioMainCurPos2 = (mainCur2Pos - chartMargin) / (PanelMainChart.Width - 2 * chartMargin);
                exportEndTime = subPosTime + (divTime * 20) * ratioMainCurPos2;
            }
            else
            {
                //デフォルトファイル全体
                exportStartTime = startTime;
                exportEndTime = endTime;
            }

            //プログレスバー計算用
            long div_num = (long)(exportEndTime - exportStartTime);
            int counter = 0;
            progressBar.Value = 0;
            progressBar.Maximum = (int)((exportEndTime - exportStartTime)*100);

            //CSVファイルに書き込むときに使うEncoding
            System.Text.Encoding enc =
                System.Text.Encoding.GetEncoding("Shift_JIS");

            //書き込むファイルを開く
            System.IO.StreamWriter sr =
                new System.IO.StreamWriter(DstFileName, false, enc);

            //レコードを書き込む
            // ToDo fieldはStringBuilderに置き換えること
            System.Text.StringBuilder field = new System.Text.StringBuilder("");

            // CSVのヘッダー行を書き込む
            field.Append("MotoReco Logger File\r\n");
            field.Append("Time" + ",");
            for (int i=0; i < ListChData.Count - 1 ; i++)
            {
                field.Append(ListChData[i].ChName + ",");
            }
            field.Append(ListChData[ListChData.Count - 1].ChName  + "\r\n");

            //フィールドを書き込む
            sr.Write(field);

            //field初期化
            field.Clear();

            // CSVはタイムスタンプごとにデータを保管して出力したいのでタイムスタンプでループする
            // とりあえず10msごとのタイムスタンプにする
            double timeStamp;

            timeStamp = exportStartTime;

            while (timeStamp < exportEndTime)
            {
                //毎ループメッセージ発行するとクソ遅いので、トータル100回送るようにする。
                if ( (int)(timeStamp*100) % div_num == 0)
                {
                    progressBar.Value = (int)counter;
                    this.statusLabel.Text = string.Format("{0}%", ((int)counter) * 100 / progressBar.Maximum);
                    Application.DoEvents();
                }
                counter++;

                field.Append(timeStamp.ToString("F2") + ",");
                for (int i=0; i< ListChData.Count-1; i++)
                {
                    // タイムスタンプに最も近いChDataのインデックスを取得
                    int targetIdx = ListChData[i].FindLeftIndex(timeStamp);
                    double value = ListChData[i].LogData[targetIdx].DataValue;
                    field.Append(value.ToString() + ",");
                }

                int targetIdx2 = ListChData[ListChData.Count-1].FindLeftIndex(timeStamp);
                double value2 = ListChData[ListChData.Count-1].LogData[targetIdx2].DataValue;
                field.Append(value2.ToString() + "\r\n");

                //フィールドを書き込む
                sr.Write(field);

                //field初期化
                field.Clear();

                // 次のタイムスタンプ
                timeStamp += 0.01;   // とりあえず10ms固定
            }  

            //閉じる
            sr.Close();

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
            System.Drawing.Point getXY = e.Location;

            //選択マーカー幅を計算する
            //データの時間幅に対する、divTime*20の割合から計算できる
            //まずデータの時間幅に対するメインチャート時間幅の割合を計算
            double ratioSelected = (divTime * 20) / (endTime - startTime);

            //ratioSelected上限処理 , データが短くてメインチャート1画面に収まり切る場合に発生
            if (ratioSelected > 1) { ratioSelected = 1; }

            double rectWidth = (PanelSubChart.Width - 2 * chartMargin) * ratioSelected;

            //rectWidth下限処理
            if (rectWidth < 2) { rectWidth = 2; }

            //startPosを算出
            double startPos = getXY.X - rectWidth / 2;

            //startPos上下限処理
            if (startPos < chartMargin) { startPos = chartMargin; }
            if (startPos + rectWidth > PanelSubChart.Width - 2 * chartMargin) { startPos = PanelSubChart.Width - chartMargin - rectWidth; }

            //startPosをposTimeに変換する
            subPosTime = (endTime - startTime) * ((startPos - chartMargin) / (PanelSubChart.Width - 2 * chartMargin)) + startTime;

            return subPosTime;
        }

        /// <summary>
        /// DirectX初期化
        /// </summary>
        private void InitializeMainDirextX()
        {
            //バックバッファの作成
            dxMainBackBuffer = Texture2D.FromSwapChain<Texture2D>(dxMainSwapChain, 0);

            //D2Dレンダーターゲットの作成
            dxMainD2dFactory = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.MultiThreaded);　　　　　　　　　　　　//ToDo これで良いのか？
            dxMainSurface = dxMainBackBuffer.QueryInterface<Surface>();
            SharpDX.Direct2D1.PixelFormat pixelFormat = new PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied);
            SharpDX.Direct2D1.RenderTargetProperties targetProp = new RenderTargetProperties(pixelFormat);
            dxMainRenderTarget2D = new RenderTarget(dxMainD2dFactory, dxMainSurface, targetProp);
            dxMainRenderTarget2D.AntialiasMode = AntialiasMode.Aliased;
            dxMainRenderTarget2D.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;

            //DirectWriteオブジェクトを生成するために必要なファクトリオブジェクトを生成
            dxMainFactoryDWrite = new SharpDX.DirectWrite.Factory();

            //D2Dフォント作成
            dxMainTextFont = new TextFormat(dxMainFactoryDWrite, "MS UI Gothic", 9.0f)
            {
                // レイアウトに沿った文字の左右配置
                // ※読み方向軸に沿った段落テキストの相対的な配置を指定します
                TextAlignment = TextAlignment.Leading,
                // レイアウトに沿った文字の上下配置
                // ※相対フロー方向軸に沿った段落テキストの配置を指定します
                ParagraphAlignment = ParagraphAlignment.Near,
            };
        }

        /// <summary>
        /// DirectX初期化
        /// </summary>
        private void InitializeSubDirextX()
        {
            //バックバッファの作成
            dxSubBackBuffer = Texture2D.FromSwapChain<Texture2D>(dxSubSwapChain, 0);

            //D2Dレンダーターゲットの作成
            dxSubD2dFactory = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.MultiThreaded);　　　　　　　　　　　　//ToDo これで良いのか？
            dxSubSurface = dxSubBackBuffer.QueryInterface<Surface>();
            SharpDX.Direct2D1.PixelFormat pixelFormat = new PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied);
            SharpDX.Direct2D1.RenderTargetProperties targetProp = new RenderTargetProperties(pixelFormat);
            dxSubRenderTarget2D = new RenderTarget(dxSubD2dFactory, dxSubSurface, targetProp);
            dxSubRenderTarget2D.AntialiasMode = AntialiasMode.Aliased;
            dxSubRenderTarget2D.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;

            //DirectWriteオブジェクトを生成するために必要なファクトリオブジェクトを生成
            dxSubFactoryDWrite = new SharpDX.DirectWrite.Factory();

            //D2Dフォント作成
            dxSubTextFont = new TextFormat(dxSubFactoryDWrite, "MS UI Gothic", 9.0f)
            {
                // レイアウトに沿った文字の左右配置
                // ※読み方向軸に沿った段落テキストの相対的な配置を指定します
                TextAlignment = TextAlignment.Leading,
                // レイアウトに沿った文字の上下配置
                // ※相対フロー方向軸に沿った段落テキストの配置を指定します
                ParagraphAlignment = ParagraphAlignment.Near,
            };
        }

        /// <summary>
        /// DirectXをリサイズする
        /// </summary>
        private void ResizeDirectX()
        {
            dxMainDevice.ImmediateContext.ClearState();
            dxSubDevice.ImmediateContext.ClearState();

            //ResizeBufferする前に、backBuffer含めすべての関係するリソースを開放する必要がある。
            dxMainRenderTarget2D.Dispose();
            dxMainBackBuffer.Dispose();
            dxMainD2dFactory.Dispose();
            dxMainFactoryDWrite.Dispose();
            dxMainTextFont.Dispose();
            dxMainSurface.Dispose();

            dxSubRenderTarget2D.Dispose();
            dxSubBackBuffer.Dispose();
            dxSubD2dFactory.Dispose();
            dxSubFactoryDWrite.Dispose();
            dxSubTextFont.Dispose();
            dxSubSurface.Dispose();

            //SwapChainのりサイズ
            dxMainSwapChain.ResizeBuffers(2, 0, 0, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);
            dxSubSwapChain.ResizeBuffers(2, 0, 0, Format.R8G8B8A8_UNorm, SwapChainFlags.AllowModeSwitch);

            //再度初期化
            InitializeMainDirextX();
            InitializeSubDirextX();
        }

        /// <summary>
        /// 文字を画面上に描画
        /// </summary>
        private void DrawText(string text, Vector2 pos)
        {
            if (dxMainRenderTarget2D != null)
            {
                // 文字を描画する領域
                // ※「改行の目安」や「文字のAlignment」などで使用される
                float maxWidth = 1000.0f;
                float maxHeight = 1000.0f;

                // 文字描画
                dxMainRenderTarget2D.DrawText(text, dxMainTextFont, 
                                              new SharpDX.Mathematics.Interop.RawRectangleF(pos.X, pos.Y, pos.X + maxWidth, pos.Y + maxHeight),
                                              new SolidColorBrush(dxMainRenderTarget2D, SharpDX.Color.White));
            }
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

            // PanelMainChartとpictureSubにマウスホイールイベント登録
            //ホイールイベントの追加  
            this.PanelMainChart.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.PanelMainChart_MouseWheel);

            this.PanelSubChart.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.PanelSubChart_MouseWheel);

            // DirectX初期化
            //SwapChain description
            SharpDX.DXGI.SwapChainDescription descMain = new SharpDX.DXGI.SwapChainDescription()
            {
                BufferCount = 2,                                                                 //ToDo これで良いのか？
                ModeDescription = new SharpDX.DXGI.ModeDescription(0, 0, new SharpDX.DXGI.Rational(60, 1), SharpDX.DXGI.Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = PanelMainChart.Handle,                                           //パネルに描画する
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
                Usage = SharpDX.DXGI.Usage.RenderTargetOutput
            };

            //D3Dデバイスとスワップチェーンの作成
            SharpDX.Direct3D11.Device.CreateWithSwapChain(SharpDX.Direct3D.DriverType.Hardware
                , DeviceCreationFlags.BgraSupport, descMain
                , out dxMainDevice, out dxMainSwapChain);

            //SwapChain description
            SharpDX.DXGI.SwapChainDescription descSub = new SharpDX.DXGI.SwapChainDescription()
            {
                BufferCount = 2,                                                                 //ToDo これで良いのか？
                ModeDescription = new SharpDX.DXGI.ModeDescription(0, 0, new SharpDX.DXGI.Rational(60, 1), SharpDX.DXGI.Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = PanelSubChart.Handle,                                           //パネルに描画する
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
                Usage = SharpDX.DXGI.Usage.RenderTargetOutput
            };

            //D3Dデバイスとスワップチェーンの作成
            SharpDX.Direct3D11.Device.CreateWithSwapChain(SharpDX.Direct3D.DriverType.Hardware
                , DeviceCreationFlags.BgraSupport, descSub
                , out dxSubDevice, out dxSubSwapChain);

            InitializeMainDirextX();
            InitializeSubDirextX();

            this.Disposed += (sender, e) => DisposeDevice();

        }

        public void DisposeDevice()
        {
            dxMainRenderTarget2D.Dispose();
            dxMainD2dFactory.Dispose();
            dxMainBackBuffer.Dispose();
            dxMainSwapChain.Dispose();
            dxMainDevice.Dispose();
            dxMainFactoryDWrite.Dispose();
            dxMainTextFont.Dispose();

            dxSubRenderTarget2D.Dispose();
            dxSubD2dFactory.Dispose();
            dxSubBackBuffer.Dispose();
            dxSubSwapChain.Dispose();
            dxSubDevice.Dispose();
            dxSubFactoryDWrite.Dispose();
            dxSubTextFont.Dispose();
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
            PanelMainChart.Refresh();
            PanelSubChart.Refresh();
        }

        private void ListViewData_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ListViewData.Items.Count < 1) return;

            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                if (!DicChName.ContainsKey(ListViewData.Items[i].Text)) {
                    continue; 
                }

                int idx = DicChName[ListViewData.Items[i].Text];
                ListChData[idx].ChShow = ListViewData.Items[i].Checked;
            }

            PanelMainChart.Refresh();
            PanelSubChart.Refresh();
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
                PanelMainChart.Refresh();
                PanelSubChart.Refresh();
                UpdateMap();
                UpdateMapMarker();

                this.Text = openFileDialog.FileName;
            }
        }

        // マウスホイールイベント  
        // ホイール単独　→　divTimeづつ移動
        // ctrl + ホイール　→　2倍づつ拡大縮小
        private void PanelMainChart_MouseWheel(object sender, MouseEventArgs e)
        {
            // スクロール量
            int delta;
            delta = e.Delta;

            // ctrl + wheelなら、2倍づつ拡大縮小
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
                    //subPosTimeを前回値にコピーする
                    subPosTimePrev = subPosTime;

                    subPosTime -= divTime;
                }
                else
                {
                    //subPosTimeを前回値にコピーする
                    subPosTimePrev = subPosTime;

                    subPosTime += divTime;
                }
            }

            PanelMainChart.Refresh();
            PanelSubChart.Refresh();
            UpdateMap();
            UpdateMapMarker();
        }

        // マウスホイールイベント  
        // ホイール単独　→　divTimeづつ移動
        // ctrl + ホイール　→　2倍づつ拡大縮小
        private void PanelSubChart_MouseWheel(object sender, MouseEventArgs e)
        {
            // スクロール量
            int delta;
            delta = e.Delta;

            // ctrl + wheelなら、2倍づつ拡大縮小
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
                    //subPosTimeを前回値にコピーする
                    subPosTimePrev = subPosTime;

                    subPosTime -= divTime;
                }
                else
                {
                    //subPosTimeを前回値にコピーする
                    subPosTimePrev = subPosTime;

                    subPosTime += divTime;
                }
            }

            PanelMainChart.Refresh();
            PanelSubChart.Refresh();
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
                ConvertCANData(currentDatFile, saveFileDialog.FileName);
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
                PanelMainChart.Refresh();
                PanelSubChart.Refresh();
                UpdateMap();
                UpdateMapMarker();

                this.Text = openFileDialog.FileName;
            }
        }

        private void MenuConvertDecodeDataWhole_Click(object sender, EventArgs e)
        {
            // ファイルがOpenされていない
            if (currentDatFile == "")
            {
                return;
            }

            // ファイルがOpenされたがデコード条件が空
            if (decodeRule.Count == 0)
            {
                return;
            }

            // ファイルがOpenされたがデコードされたデータがない
            if (ListChData.Count == 0)
            { 
                return;
            }

            // 現在保持している表示中のデコード済データをCSVエクスポートする
            // 名前をつけて保存
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ConvertDecodeData(saveFileDialog.FileName, 0);
            }
        }

        private void MenuConvertDecodeDataCursor_Click(object sender, EventArgs e)
        {
            // ファイルがOpenされていない
            if (currentDatFile == "")
            {
                return;
            }

            // ファイルがOpenされたがデコード条件が空
            if (decodeRule.Count == 0)
            {
                return;
            }

            // ファイルがOpenされたがデコードされたデータがない
            if (ListChData.Count == 0)
            {
                return;
            }

            // 現在保持している表示中のデコード済データをCSVエクスポートする
            // 名前をつけて保存
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ConvertDecodeData(saveFileDialog.FileName,1);
            }
        }

        private void PanelMainChart_Paint(object sender, PaintEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            // ListChNameがNULL→即抜ける
            if (DicChName == null) { return; }

            // ListChNameが0＝CANデータ未読み込み→即抜ける
            if (DicChName.Count < 1) { return; }

            dxMainRenderTarget2D.BeginDraw();
            //=======================================================================================================================
            // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓　　　　BeginDrawとEndDrawの間で描画すること　↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            // ======================================================================================================================

            DrawMainChartD2D();

            //=======================================================================================================================
            // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑　　　　BeginDrawとEndDrawの間で描画すること　↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            // ======================================================================================================================
            dxMainRenderTarget2D.EndDraw();
            dxMainSwapChain.Present(0, PresentFlags.None);

            UpdateListViewData();
        }

        private void PanelMainChart_MouseDown(object sender, MouseEventArgs e)
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

        private void PanelMainChart_MouseMove(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            //クリックした場合、クリックした座標が選択範囲の中心になるように調整する
            System.Drawing.Point getXY = e.Location;

            if (this.IsDragging)
            {

                if (this.DraggingButton == MouseButtons.Left)
                {
                    mainCur1PosPrev = mainCur1Pos;

                    //左クリックの場合、Cur1Posを更新
                    mainCur1Pos = getXY.X;
                }

                if (this.DraggingButton == MouseButtons.Right)
                {
                    mainCur2PosPrev = mainCur2Pos;

                    //右クリックの場合、Cur2Posを更新
                    mainCur2Pos = getXY.X;
                }

                PanelMainChart.Refresh();
                PanelSubChart.Refresh();
                UpdateMapMarker();
            }
        }

        private void PanelMainChart_MouseUp(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            switch (e.Button)
            {
                case MouseButtons.Left: // 左クリックの時
                    IsDragging = false; // ドラッグが終了していることを記録
                    this.Cursor = Cursors.Default; // マウスポインタを通常のものに戻す
                    mainCur1PosPrev = mainCur1Pos;
                    mainCur2PosPrev = mainCur2Pos;

                    PanelMainChart.Refresh();
                    PanelSubChart.Refresh();
                    UpdateMapMarker();
                    break;

                case MouseButtons.Right: // 右クリックの時
                    IsDragging = false; // ドラッグが終了していることを記録
                    this.Cursor = Cursors.Default; // マウスポインタを通常のものに戻す
                    mainCur1PosPrev = mainCur1Pos;
                    mainCur2PosPrev = mainCur2Pos;

                    PanelMainChart.Refresh();
                    PanelSubChart.Refresh();
                    UpdateMapMarker();
                    break;
            }
            this.DraggingButton = 0;
        }

        private void PanelMainChart_Resize(object sender, EventArgs e)
        {
            if (dxMainDevice == null)
            {
                return;
            }
            
            ResizeDirectX();
        }

        private void PanelSubChart_Paint(object sender, PaintEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            // ListChNameがNULL→即抜ける
            if (DicChName == null) { return; }

            // ListChNameが0＝CANデータ未読み込み→即抜ける
            if (DicChName.Count < 1) { return; }
            
            dxSubRenderTarget2D.BeginDraw();
            //=======================================================================================================================
            // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓　　　　BeginDrawとEndDrawの間で描画すること　↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            // ======================================================================================================================

            DrawSubChartD2D();

            //=======================================================================================================================
            // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑　　　　BeginDrawとEndDrawの間で描画すること　↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            // ======================================================================================================================
            dxSubRenderTarget2D.EndDraw();
            dxSubSwapChain.Present(0, PresentFlags.None);

        }

        private void PanelSubChart_Resize(object sender, EventArgs e)
        {
            if (dxMainDevice == null)
            {
                return;
            }

            ResizeDirectX();
        }

        private void PanelSubChart_MouseDown(object sender, MouseEventArgs e)
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

        private void PanelSubChart_MouseMove(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            if (this.IsDragging && this.DraggingButton == MouseButtons.Left)
            {
                //subPosTimeを前回値にコピーする
                subPosTimePrev = subPosTime;

                //PictureSub上のマウス位置から、subPosTimeを算出する
                subPosTime = MousePointToSubPosTime(e);

                //DrawChart();
                PanelMainChart.Refresh();
                PanelSubChart.Refresh();
            }
        }

        private void PanelSubChart_MouseUp(object sender, MouseEventArgs e)
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

                    //subPosTimeを前回値にコピーして一致させておく
                    subPosTimePrev = subPosTime;

                    PanelMainChart.Refresh();
                    PanelSubChart.Refresh();
                    UpdateMapMarker();
                    UpdateMap();

                    break;
            }
            this.DraggingButton = 0;
        }
    }
}
