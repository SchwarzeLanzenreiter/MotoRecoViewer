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
        private BufferedListView ListViewData;

        private const int chartMargin = 10; // グリッドとpictureSubの余白
               
        private List<String> ListChName;
        private List<ChData> ListChData;

        private float startTime;    //CANデータ最初のタイムスタンプ
        private float endTime;      //CANデータ最終データのタイムスタンプ
        private float subPosTime;   //サブチャート選択位置、デフォルト0秒
        private float mainCur1Pos;  //メインチャートカーソル1X位置
        private float mainCur2Pos;  //メインチャートカーソル2X位置
        private float divTime;      //メインチャートの1divisionあたり時間、デフォルト1秒

        private bool IsDragging;                // 現在ドラッグ中かどうか                                 
        private MouseButtons DraggingButton;    // どのボタンが押されているのか(右ボタンで別の処理をしたい時に、区別するため)
        private double DraggingOldX;            // ②呼び出しの直前(前回の②の終了時)のマウスポインタのX座標
        private double DraggingOldY;            // ②呼び出しの直前(同上)のマウスポインタのY座標
        private bool IsReadingCanData;          // 現在CANデータ読み込み中かどうか

        private float prev_lon;                // longitude前回値
        private float prev_lat;                 // latitude前回値

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

            Parallel.For(0, arySize, i =>
            {
                Interlocked.Increment(ref counter);

                //毎ループメッセージ発行するとクソ遅いので、トータル100回送るようにする。そもそもプログレスバーのWidthも100なので100以上送っても意味なし
                if (i % div_num == 0)
                {
                    context.Post(progress =>
                    {
                        this.progressBar.Value = (int)progress;
                        this.statusLabel.Text = string.Format("{0}%", ((int)progress)*100/arySize);
                    }, counter);
                    Application.DoEvents();
                }

                //１データ読み込んだらデコード処理にわたす
                //ToDo DecodeDataはどう考えても遅い。予め決め打ちで関数で計算させるようにしつつ、それで対応できない場合にDecodeDataを使う方法にしたい
                int idxCh;

                //CanDataのCANIDが、DecodeRuleで一致するかチェック
                int decodeRuleIdx;
                decodeRuleIdx = decodeRule.IndexOf(aryCanData[i].id);

                //もしCANIDが対象外なら即抜ける
                if (decodeRuleIdx == -1)
                {
                    // ToDo どう考えても捨てるしかないCANIDは、MotoReco側でフィルターかけるのも有り
                    return;
                }

                //CAN IDが対象なら、DecodeRuleに従ってデコードする。
                //なお、DecodeRuleには、同一CANIDのルールは複数存在し得ることに注意。
                bool running = true;
                while ((0 <= decodeRuleIdx) & running)
                {
                    lock (lockobj)
                    {
                        //foundIndexのChNameが有るかどうかチェック
                        string chName = decodeRule.GetChName(decodeRuleIdx);
                        int chMax = decodeRule.GetChartMax(decodeRuleIdx);
                        int chMin = decodeRule.GetChartMin(decodeRuleIdx);
                        bool chPrev = decodeRule.GetChartPreview(decodeRuleIdx);
                        int chColor = decodeRule.GetChartColor(decodeRuleIdx);
                        bool chShow = decodeRule.GetChartShow(decodeRuleIdx);

                        idxCh = ListChName.IndexOf(chName);

                        //存在しない場合追加する
                        if (idxCh == -1)
                        {

                            ListChName.Add(chName);

                            ChData newData = new ChData(chName, chMin, chMax, chColor, chPrev, chShow);

                            ListChData.Add(newData);

                            //再度idxCh取得
                            idxCh = ListChName.IndexOf(chName);

                        }
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
                    float second = aryCanData[i].timeSec + (float)aryCanData[i].timeMSec / 1000;

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
                    
            });
            
            //開始時間を計算しておく
            startTime = (float)aryCanData[0].timeSec + (float)aryCanData[0].timeMSec / 1000;
            subPosTime = startTime;
            divTime = 1;
                       
            //終了時間を計算しておく
            endTime = (float)aryCanData[arySize - 1].timeSec + (float)aryCanData[arySize - 1].timeMSec / 1000;

            //進捗初期化
            progressBar.Value = 0;
            this.statusLabel.Text = "";

            //スレッド処理した関係でデータがソートできてないのでソートする
            for (int i = 0; i < ListChData.Count; i++)
            {
                ListChData[i].Sort();
            }

            //CANデータ読み取り終了
            IsReadingCanData = false;

            DrawChart();
        }

        private void UpdateListViewData()
        {
            // ListViewDataのValue1を更新

            // MainChartのカーソル位置1に対応するタイムスタンプを計算
            float targetTime = subPosTime + (divTime * 20) / (pictureMain.Width - 2 * chartMargin) * mainCur1Pos;

            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                int idx = ListChName.IndexOf(ListViewData.Items[i].Text);

                //　該当CAN IDが存在しないケースも有りうることに注意
                if (idx < 0) { continue; }

                int targetIdx = ListChData[idx].FindClosestIndex(targetTime);

                ListViewData.Items[i].SubItems[1].Text = ListChData[idx].LogData[targetIdx].DataValue.ToString();
            }

            // MainChartのカーソル位置2に対応するタイムスタンプを計算
            targetTime = subPosTime + (divTime * 20) / (pictureMain.Width - 2 * chartMargin) * mainCur2Pos;

            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                int idx = ListChName.IndexOf(ListViewData.Items[i].Text);

                //　該当CAN IDが存在しないケースも有りうることに注意
                if (idx < 0) { continue; }

                int targetIdx = ListChData[idx].FindClosestIndex(targetTime);

                ListViewData.Items[i].SubItems[2].Text = ListChData[idx].LogData[targetIdx].DataValue.ToString();
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
            // Latitudeがあるかチェック
            int idx_lat = ListChName.IndexOf("Latitude");
            int idx_lon = ListChName.IndexOf("Longitude");

            // LatitudeもLongitudeも存在しない場合、何もしない
            if ((idx_lat < 0) || (idx_lon < 0))
            {
                return;
            }
            
            // MainChartのカーソル位置1に対応するタイムスタンプを計算
            float targetTime = subPosTime + (divTime * 20) / (pictureMain.Width - 2 * chartMargin) * mainCur1Pos;

            // タイムスタンプに応じたデータidx取得
            int targetIdx = ListChData[idx_lat].FindClosestIndex(targetTime);

            // 緯度経度取得
            float fl_lon = (float)ListChData[idx_lon].LogData[targetIdx].DataValue;
            float fl_lat = (float)ListChData[idx_lat].LogData[targetIdx].DataValue;

            // 緯度経度に変化がなければリターンで抜ける
            if ((fl_lon == prev_lon ) && (fl_lat == prev_lat))
            {
                return;
            } else {
                prev_lon = fl_lon;
                prev_lat = fl_lat;
            }

            //地図表示
            GMapControl.Position = new PointLatLng(fl_lat,fl_lon);
        }

        /// <summary>
        /// Chartを描画する
        /// </summary>
        private void DrawChart()
        {
            // ListChNameが0＝CANデータ未読み込み→即抜ける
            if (ListChName.Count < 1) { return; }

            // アプリ下部のサブチャートを描画
            DrawSubChart();

            //　アプリ上部のメインチャートを描画
            DrawMainChart();

            // ListViewData更新
            UpdateListViewData();
        }

        /// <summary>
        /// 画面下部のサブChartのグリッドを描画する
        /// </summary>
        private void DrawSubChartGrid(Graphics g, int margin)
        {
            //まずサブチャートエリアの枠を書く

            //横線
            for (int i = 0; i < 21; i++)
            {
                // 罫線間ピクセルを算出
                float rule = (float)(pictureSub.Width - 2 * margin) / 20;

                // X座標
                float x = margin + i * rule;

                // Y座標
                float y1 = margin;
                float y2 = pictureSub.Height - margin;

                // ToDo グリッドカラーも設定できるようにする
                g.DrawLine(Pens.DarkSeaGreen, x, y1, x, y2);
            }

            //縦線
            // X座標
            for (int i = 0; i < 3; i++)
            {
                // 罫線間ピクセルを算出
                float rule = (float)(pictureSub.Height - 2 * margin) / 2;

                // X座標
                float y = margin + i * rule;

                // Y座標
                float x1 = margin;
                float x2 = pictureSub.Width - margin;

                // ToDo グリッドカラーも設定できるようにする
                g.DrawLine(Pens.DarkSeaGreen, x1, y, x2, y);
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

            //for (int i = 0; i < ListChName.Count; i++)
            Parallel.For(0, ListChData.Count, i =>
            {
                // Subチャートに表示するのは、ChData.ChPreviewがTrueの物のみ
                if (ListChData[i].ChPreview)
                {
                    int targetIdxPrev = 0;
                    int xPrev = chartMargin;
                    double yPrev = pictureSub.Height - chartMargin;

                    // SubChart描画ピクセル幅に対してのみ描画処理実施する
                    for (int j = 0; j <= pictureSub.Width - 2 * chartMargin; j++)
                    {
                        // SubChartのグラフ描画領域のXstart～Xendに対応したタイムスタンプを計算
                        double targetTime = startTime + (endTime - startTime) / (pictureSub.Width - 2 * chartMargin) * j;

                        // targetTimeに対応したタイムスタンプに最も近いChDataのインデックスを取得
                        int targetIdx = ListChData[i].FindClosestIndex(targetTime);


                        // 1つ前のインデックスと同じ場合、スキップする
                        if (targetIdxPrev == targetIdx)
                        {
                            continue;
                        }

                        // targetIdx相当分のXとYをラッチ
                        int x = j + chartMargin;

                        // targetIdxのDataValueが、Ch設定のMax-Min幅に対して何%位置か算出する
                        double y = ListChData[i].LogData[targetIdx].DataValue;
                        y = (y - ListChData[i].ChMin) / (ListChData[i].ChMax - ListChData[i].ChMin);

                        // 0%以下もしくは100以上は0または100に丸める
                        if (y > 100) { y = 1; }
                        if (y < 0) { y = 0; }

                        // %をY軸ピクセルに変換する.その際、グラフ上方が原点になるので、1-yとして計算する。
                        y = (pictureSub.Height - chartMargin * 2) * (1 - y);
                        y = y + chartMargin;

                        // 1つ前のインデックスと異なる場合のみ、ラインを描画する
                        
                        lock (lockobj) {
                            // 描画色
                            p.Color = Color.FromArgb(ListChData[i].ChColor);

                            //　描画
                            g.DrawLine(p, xPrev, (float)yPrev, x, (float)y);
                        }

                        targetIdxPrev = targetIdx;
                        xPrev = x;
                        yPrev = y;
                    }
                }

                idxPreview++;
            });
        }

        /// <summary>
        /// 画面下部のサブChartに選択位置を表示する
        /// </summary>
        private void DrawSubChartPos(Graphics g)
        {
            //選択マーカー幅を計算する
            //データの時間幅に対する、divTime*20の割合から計算できる
            //まずデータの時間幅に対するメインチャート時間幅の割合を計算
            float ratioSelected = (divTime * 20) / (endTime - startTime);

            //ratioSelected上限処理 , データが短くてメインチャート1画面に収まり切る場合に発生
            if (ratioSelected > 1) { ratioSelected = 1; }

            float rectWidth = (pictureSub.Width - 2 * chartMargin) * ratioSelected;

            //rectWidth下限処理
            if (rectWidth < 2){ rectWidth = 2; }

            //XY座標計算
            float x1 = (subPosTime - startTime) / (endTime - startTime);       //ToDo startTimeはMotoRecoでロギング時0のはずなので本来は不要
            x1 = x1 * (pictureSub.Width - 2 * chartMargin)+chartMargin;
            float x2 = x1 + rectWidth;
            float y1 = chartMargin;
            float y2 = pictureSub.Height - chartMargin;

            //rectangleに変換
            Rectangle rect = Rectangle.FromLTRB((int)x1, (int)y1, (int)x2, (int)y2);

            g.DrawRectangle(Pens.White, rect);
        }

        /// <summary>
        /// 画面下部のサブChartを描画する
        /// </summary>
        private void DrawSubChart()
        {
            //Subチャートと同じサイズのBitmapを用意
            Bitmap canvas = new Bitmap(pictureSub.Width, pictureSub.Height);

            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            //サブチャートエリアを黒く塗りつぶす
            g.FillRectangle(Brushes.Black, 0, 0, canvas.Width, canvas.Height);

            //pictureSubのWidth分だけ、ListChDataからデータをピックアップして描画する
            DrawSubChartData(g);

            DrawSubChartGrid(g, chartMargin);

            //サブチャートに現在位置を反転表示で表示
            DrawSubChartPos(g);

            // canvasをpictureboxに転送する
            pictureSub.Image = canvas;
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
                float rule = (float)(pictureMain.Width - 2 * chartMargin) / 20;

                // X座標
                float x = chartMargin + i * rule;

                // Y座標
                float y1 = chartMargin;
                float y2 = pictureMain.Height - chartMargin;

                g.DrawLine(Pens.DarkSeaGreen, x, y1, x, y2);
            }

            //縦線
            // X座標
            for (int i = 0; i < 11; i++)
            {
                // 罫線間ピクセルを算出
                float rule = (float)(pictureMain.Height - 2 * chartMargin) / 10;

                // X座標
                float y = chartMargin + i * rule;

                // Y座標
                float x1 = chartMargin;
                float x2 = pictureMain.Width - chartMargin;

                g.DrawLine(Pens.DarkSeaGreen, x1, y, x2, y);
            }

            Pen curPen = new Pen(Brushes.White,3);

            //カーソル1
            curPen.DashStyle = DashStyle.DashDot;
            g.DrawLine(curPen, mainCur1Pos, chartMargin, mainCur1Pos, pictureMain.Height - chartMargin);

            //カーソル2
            curPen.DashStyle = DashStyle.DashDotDot;
            g.DrawLine(curPen, mainCur2Pos, chartMargin, mainCur2Pos, pictureMain.Height - chartMargin);
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
            {
                int targetIdxPrev = 0;
                int xPrev = chartMargin;
                double yPrev = pictureMain.Height - chartMargin;

                if (ListChData[i].ChShow) {

                    //SubChart左端と右端に対応するListChDataのインデックスを先に求める。
                    //そうしておくことで、中間範囲のListChDataの検索範囲を狭めて高速化できる。
                    int startidx, endidx;
                    startidx = ListChData[i].FindClosestIndex(subPosTime);
                    endidx = ListChData[i].FindClosestIndex(subPosTime + (divTime * 20));


                    // MainChart描画ピクセル幅に対してのみ描画処理実施する
                    for (int j = 0; j <= pictureMain.Width - 2 * chartMargin; j++)
                    {
                        // MainChartのグラフ描画領域のXstart～Xendに対応したタイムスタンプを計算
                        double targetTime = subPosTime + (divTime * 20) / (pictureMain.Width - 2 * chartMargin) * j;

                        // targetTimeに対応したタイムスタンプに最も近いChDataのインデックスを取得
                        int targetIdx = ListChData[i].FindClosestIndex(targetTime,startidx,endidx);

                        // 1つ前のインデックスと同じ場合何もしない
                        if (targetIdxPrev == targetIdx)
                        {
                            continue;
                        }

                        // targetIdx相当分のXとYをラッチ
                        int x = j + chartMargin;

                        // targetIdxのDataValueが、Ch設定のMax-Min幅に対して何%位置か算出する
                        double y = ListChData[i].LogData[targetIdx].DataValue;
                        y = (y- ListChData[i].ChMin) / (ListChData[i].ChMax - ListChData[i].ChMin);

                        // 0%以下もしくは100以上は0または100に丸める
                        if (y > 100) { y = 1; }
                        if (y < 0) { y = 0; }

                        // %をY軸ピクセルに変換する.その際、グラフ上方が原点になるので、1-yとして計算する。
                        y = (pictureMain.Height - chartMargin * 2) * (1 - y);
                        y = y + chartMargin;

                        // 1つ前のインデックスと異なる場合のみ、ラインを描画する
                        lock (lockobj)
                        {
                            p.Color = Color.FromArgb(ListChData[i].ChColor);

                            g.DrawLine(p, xPrev, (float)yPrev, x, (float)y);
                        }

                        targetIdxPrev = targetIdx;
                        xPrev = x;
                        yPrev = y;
                    }
                }
            });
        }

        /// <summary>
        /// 画面上部のメインChartを描画する
        /// </summary>
        private void DrawMainChart()
        {
            //Mainチャートと同じサイズのBitmapを用意
            Bitmap canvas = new Bitmap(pictureMain.Width, pictureMain.Height);

            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            //サブチャートエリアを黒く塗りつぶす
            g.FillRectangle(Brushes.Black, 0, 0, canvas.Width, canvas.Height);

            //pictureSubのWidth分だけ、ListChDataからデータをピックアップして描画する
            DrawMainChartData(g);

            //Grid描画
            DrawMainChartGrid(g);

            // canvasをpictureboxに転送する
            pictureMain.Image = canvas;
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
            string field;

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

                field = "";
                field = field + aryCanData[i].timeSec.ToString() + ",";
                field = field + aryCanData[i].timeMSec.ToString() + ",";
                field = field + aryCanData[i].id.ToString("X3") + ",";
                field = field + aryCanData[i].data[0].ToString("X2") + ",";
                field = field + aryCanData[i].data[1].ToString("X2") + ",";
                field = field + aryCanData[i].data[2].ToString("X2") + ",";
                field = field + aryCanData[i].data[3].ToString("X2") + ",";
                field = field + aryCanData[i].data[4].ToString("X2") + ",";
                field = field + aryCanData[i].data[5].ToString("X2") + ",";
                field = field + aryCanData[i].data[6].ToString("X2") + ",";
                field = field + aryCanData[i].data[7].ToString("X2") + "\r\n";

                //フィールドを書き込む
                sr.Write(field);
            }

            //閉じる
            sr.Close();

            //CANデータ読み取り終了
            IsReadingCanData = false;

            //プログレスバー初期化
            progressBar.Value = 0;
            this.statusLabel.Text = "";
        }

        //==========================
        //
        //  自動生成イベントハンドラ
        //
        //==========================
        public FormMain()
        {
            InitializeComponent();

            // Formコンストラクタで、Private変数のClassをnewしておく
            ListChName = new List<String>();
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
            ListViewData.Columns[0].Width = 100;
            ListViewData.Columns[1].Width = 100;
            ListViewData.Columns[2].Width = 100;
            ListViewData.CheckBoxes = true;
            ListViewData.ItemChecked += ListViewData_ItemChecked;

            // pictureMainにマウスホイールイベント登録
            //ホイールイベントの追加  
            this.pictureMain.MouseWheel
                += new System.Windows.Forms.MouseEventHandler(this.pictureMain_MouseWheel);
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
                newItem.Checked = decodeRule.GetChartShow(i);

                ListViewData.Items.Add(newItem);
            }
        }

         private void pictureSub_MouseDown(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            this.DraggingButton = e.Button;
            this.DraggingOldX = e.X;
            this.DraggingOldY = e.Y;
            switch (this.DraggingButton)
            {
                case MouseButtons.Left:
                    this.IsDragging = true; // ドラッグ中であることを知らせる
                                            // 通常の矢印の代わりをマウスポインタとして表示
                    this.Cursor = Cursors.SizeWE;
                    break;
            }
        }

        private void pictureSub_MouseMove(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            if (this.IsDragging && this.DraggingButton == MouseButtons.Left)
            {
                //クリックした場合、クリックした座標が選択範囲の中心になるように調整する
                Point getXY = e.Location;

                //選択マーカー幅を計算する
                //データの時間幅に対する、divTime*20の割合から計算できる
                //まずデータの時間幅に対するメインチャート時間幅の割合を計算
                float ratioSelected = (divTime * 20) / (endTime - startTime);

                //ratioSelected上限処理 , データが短くてメインチャート1画面に収まり切る場合に発生
                if (ratioSelected > 1) { ratioSelected = 1; }

                float rectWidth = (pictureSub.Width - 2 * chartMargin) * ratioSelected;

                //rectWidth下限処理
                if (rectWidth < 2) { rectWidth = 2; }

                //startPosを算出
                float startPos = getXY.X - rectWidth / 2;

                //startPos上下限処理
                if (startPos < chartMargin) { startPos = chartMargin; }
                if (startPos + rectWidth > pictureSub.Width - 2 * chartMargin) { startPos = pictureSub.Width - chartMargin - rectWidth; }

                //startPosをposTimeに変換する
                //ToDo　末尾の+startTimeは、canloggerでKeyOn検出時にstartTimeは0で記録されているはずなので、不要のはずである
                subPosTime = (endTime - startTime) * ((startPos - chartMargin) / (pictureSub.Width - 2 * chartMargin)) + startTime;

                DrawChart();
            }
        }

        private void pictureSub_MouseUp(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            switch (e.Button)
            {
                case MouseButtons.Left: // 左クリックの時
                    this.IsDragging = false; // ドラッグが終了していることを記録
                    this.Cursor = Cursors.Default; // マウスポインタを通常のものに戻す

                    DrawChart();
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

                DrawChart();
            }
        }
        private void ListViewData_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ListViewData.Items.Count < 1) return;

            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                int idx = ListChName.IndexOf(ListViewData.Items[i].Text);

                if (idx < 0) { break; }

                ListChData[idx].ChShow = ListViewData.Items[i].Checked;
            }

            DrawChart();
        }

        private void PictureMain_MouseDown(object sender, MouseEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            this.DraggingButton = e.Button;
            this.DraggingOldX = e.X;
            this.DraggingOldY = e.Y;
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

                    DrawChart();
                    UpdateMap();
                    break;

                case MouseButtons.Right: // 右クリックの時
                    this.IsDragging = false; // ドラッグが終了していることを記録
                    this.Cursor = Cursors.Default; // マウスポインタを通常のものに戻す

                    DrawChart();
                    UpdateMap();
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
                ListChName.Clear();

                // バイナリファイルからCANデータ抽出する
                ReadCANData(openFileDialog.FileName);

                // 初回描画する
                DrawChart();
                this.Text = openFileDialog.FileName;
            }
        }

        // マウスホイールイベント  
        private void pictureMain_MouseWheel(object sender, MouseEventArgs e)
        {
            // スクロール量
            int delta;
            delta = e.Delta;

            // プラスならdivTimeを2倍にする
            if (delta > 0)
            {
                divTime = divTime * 2;
            } else
            {
                divTime = divTime / 2;
            }


            DrawChart();

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // proxy対応
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;

            // レジストリ情報読み取り
            // Google Map APIがレジストリにあれば、Google Mapを表示
            string KeyName = "MotoRecoViewer";
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + KeyName, true);
            if (rkey != null)
            {
                string strGoogleMapAPIKey = (string)rkey.GetValue("GoogleMapAPI");
                GMapProviders.GoogleMap.ApiKey = strGoogleMapAPIKey;
                GMapControl.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            }
            // Google Map APIがレジストリになければ、Bing Mapを表示
            else
            {
                GMapControl.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            }

            GMaps.Instance.Mode = AccessMode.ServerOnly;
            GMapControl.SetPositionByKeywords("Iwata, Japan");
            GMapControl.Zoom = 13;
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

        private void menuBingMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
        }

        private void menuGoogleMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
        }

        private void menuOpenCycleMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.OpenCycleMapProvider.Instance;
        }

        private void menuOpenStreetMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
        }

        private void menuWikiMapiaMap_Click(object sender, EventArgs e)
        {
            GMapControl.MapProvider = GMap.NET.MapProviders.WikiMapiaMapProvider.Instance;
        }

        private void mapSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMapOption f = new FormMapOption();

            f.ShowDialog(this);
            f.Dispose();
        }
    }
}
