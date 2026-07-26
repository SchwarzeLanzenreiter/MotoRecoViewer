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
    /// MainChart / SubChart のDirect2D描画
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// 画面下部のサブChartのグリッドを描画する
        /// </summary>
        private void DrawSubChartGridD2D()
        {
            SolidColorBrush brush = new SolidColorBrush(subCanvas.RenderTarget, SharpDX.Color.Green);

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
                subCanvas.RenderTarget.DrawLine(new Vector2(x, y1), new Vector2(x, y2), brush);
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
                subCanvas.RenderTarget.DrawLine(new Vector2(x1, y), new Vector2(x2, y), brush);
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

            //for (int i = 0; i < channels.Count; i++)
            Parallel.For(0, channels.Count, i =>
            {
                // Subチャートに表示するのは、ChData.ChPreviewがTrueの物のみ
                if (channels[i].ChPreview)
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
                        int targetIdx = channels[i].FindLeftIndex(aryTimeStamp[j]);

                        // 1つ前のインデックスと同じ場合、スキップする
                        if ((targetIdxPrev == targetIdx) || (targetIdx == 0))
                        {
                            continue;
                        }

                        // targetIdx相当分のXとYをラッチ
                        double x = j + chartMargin;

                        // targetIdxのDataValueが、Ch設定のMax-Min幅に対して比率を算出する
                        double y = channels[i].LogData[targetIdx].DataValue;
                        y = (y - channels[i].ChMin) / (channels[i].ChMax - channels[i].ChMin);

                        // 0以下もしくは1以上は0または1に丸める
                        if (y > 1) { y = 1; }
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
                    PathGeometry geo1 = new PathGeometry(subCanvas.Factory);
                    GeometrySink sink1 = geo1.Open();
                    sink1.BeginFigure(points[0], new FigureBegin());
                    sink1.AddLines(points);
                    sink1.EndFigure(new FigureEnd());
                    sink1.Close();

                    //Chごとの色をブラシにセット
                    System.Drawing.Color c;
                    c = System.Drawing.Color.FromArgb(channels[i].ChColor);

                    SharpDX.Mathematics.Interop.RawColor4 rc4;
                    rc4 = ColorToRaw4(c);

                    //ブラシ生成
                    SolidColorBrush brush = new SolidColorBrush(subCanvas.RenderTarget, SharpDX.Color.Red);
                    brush.Color = rc4;

                    //グラフ描画
                    subCanvas.RenderTarget.DrawGeometry(geo1, brush);

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
            SolidColorBrush brush = new SolidColorBrush(subCanvas.RenderTarget, SharpDX.Color.White);

            //rect生成
            SharpDX.Mathematics.Interop.RawRectangleF rect = new SharpDX.Mathematics.Interop.RawRectangleF((float)x1, (float)y1, (float)x2, (float)y2);

            //選択ポジション白枠描画
            subCanvas.RenderTarget.DrawRectangle(rect, brush);

            brush.Dispose();
        }

        /// <summary>
        /// 画面下部のサブChartを描画する
        /// </summary>
        private void DrawSubChartD2D()
        {
            //サブチャートエリアを黒く塗りつぶす
            subCanvas.RenderTarget.Clear(SharpDX.Color.Black);

            DrawSubChartGridD2D();

            //pictureSubのWidth分だけ、channelsからデータをピックアップして描画する
            DrawSubChartDataD2D();

            //サブチャートに現在位置を反転表示で表示
            DrawSubChartPosD2D();
        }


        /// <summary>
        /// 画面上部のメインChartのグリッドを描画する
        /// </summary>
        private void DrawMainChartGridD2D()
        {
            SolidColorBrush brush = new SolidColorBrush(mainCanvas.RenderTarget, SharpDX.Color.Green);

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

                mainCanvas.RenderTarget.DrawLine(new Vector2(x,y1), new Vector2(x,y2),brush);
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

                mainCanvas.RenderTarget.DrawLine(new Vector2(x1, y), new Vector2(x2, y), brush);
            }

            brush.Color = SharpDX.Color.White;

            //カーソル1
            var strokeStyle_dashed1 = new StrokeStyle(mainCanvas.Factory, new StrokeStyleProperties() { DashOffset = 5, DashCap = CapStyle.Square, DashStyle = SharpDX.Direct2D1.DashStyle.DashDot });
            mainCanvas.RenderTarget.DrawLine(new Vector2((float)mainCur1Pos, (float)chartMargin), new Vector2((float)mainCur1Pos, PanelMainChart.Height - (float)chartMargin), brush, 3, strokeStyle_dashed1);

            //カーソル2
            var strokeStyle_dashed2 = new StrokeStyle(mainCanvas.Factory, new StrokeStyleProperties() { DashOffset = 5, DashCap = CapStyle.Square, DashStyle = SharpDX.Direct2D1.DashStyle.DashDotDot });
            mainCanvas.RenderTarget.DrawLine(new Vector2((float)mainCur2Pos, (float)chartMargin), new Vector2((float)mainCur2Pos, PanelMainChart.Height - (float)chartMargin), brush, 3, strokeStyle_dashed2);

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
            mainCanvas.DrawText(str, new Vector2(1.0f, 1.0f));

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

                mainCanvas.DrawText(str, new Vector2((float)x - 5, (float)y2 + 1));
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
            Parallel.For(0, channels.Count, i =>
            //for (int i = 0; i < channels.Count; i++)
            {
                int targetIdxPrev = 0;

                if (channels[i].ChShow)
                {
                    // 座標格納用
                    // データ追加ごとにResizeするとCPUリソース食うので、想定される最大数確保し、最後に縮小する
                    RawVector2[] points = new RawVector2[arySize];
                    
                    int drawCount = 0;

                    //タイムスタンプ最初に対応したChDataインデックス取得
                    int minIdx = channels[i].FindLeftIndex(aryTimeStamp[0]);
                    targetIdxPrev = minIdx - 1;                                          // 1引くのは、初回は描画を絶対したいから
  
                    //タイムスタンプ最後に対応したChDataインデックス取得
                    int maxIdx = channels[i].FindLeftIndex(aryTimeStamp[arySize - 1]);

                    int startIdx, endIdx;

                    startIdx = minIdx;
                    endIdx = maxIdx;

                    //インデックス差分
                    int diffIdx;

                    // MainChart描画ピクセル幅に対してのみ描画処理実施する
                    for (int j = 0; j < arySize; j++)
                    {
                        // targetTimeに対応したタイムスタンプに最も近いChDataのインデックスを取得
                        int targetIdx = channels[i].FindLeftIndex(aryTimeStamp[j], startIdx, endIdx);

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
                                if (aryTimeStamp[j + 1] < channels[i].LogData[startIdx].DataTime)
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
                                if (aryTimeStamp[j + 1] > channels[i].LogData[endIdx].DataTime)
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

                        double y;

                        // Filterなしの場合は生データ参照
                        if (channels[i].ChFilter==false)
                        {
                            y = channels[i].LogData[targetIdx].DataValue;
                        // Filterありの場合Filteredデータ参照
                        } else {
                            y = channels[i].LogData[targetIdx].DataValueFiltered;
                        }

                        // データが表示設定のminとmaxに対する比率を計算
                        y = (y - channels[i].ChMin) / (channels[i].ChMax - channels[i].ChMin);

                        // 比率が0以下もしくは1以上は0または1に丸める
                        if (y > 1) { 
                            y = 1; 
                        }
                        if (y < 0) { y = 0; }

                        // 比率をY軸ピクセルに変換する.その際、グラフ上方が原点になるので、1-yとして計算する。
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
                    PathGeometry geo1 = new PathGeometry(mainCanvas.Factory);
                    GeometrySink sink1 = geo1.Open();
                    sink1.BeginFigure(points[0], new FigureBegin());
                    sink1.AddLines(points);
                    sink1.EndFigure(new FigureEnd());
                    sink1.Close();

                    //Chごとの色をブラシにセット
                    System.Drawing.Color c;
                    c = System.Drawing.Color.FromArgb(channels[i].ChColor);

                    SharpDX.Mathematics.Interop.RawColor4 rc4;
                    rc4 = ColorToRaw4(c);

                    //ブラシ生成
                    SolidColorBrush brush = new SolidColorBrush(mainCanvas.RenderTarget, SharpDX.Color.Red);
                    brush.Color = rc4;

                    var style = new SharpDX.Direct2D1.StrokeStyleProperties();
                    style.LineJoin = SharpDX.Direct2D1.LineJoin.Round;
                    style.StartCap = SharpDX.Direct2D1.CapStyle.Round;
                    style.EndCap = SharpDX.Direct2D1.CapStyle.Round;
                    var stroke = new SharpDX.Direct2D1.StrokeStyle(mainCanvas.Factory, style);

                    //グラフ描画
                    //mainCanvas.RenderTarget.DrawGeometry(geo1, brush, 5f, stroke);
                    mainCanvas.RenderTarget.DrawGeometry(geo1, brush);

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
            mainCanvas.RenderTarget.Clear(SharpDX.Color.Black);

            //Grid描画
            DrawMainChartGridD2D();

            //pictureSubのWidth分だけ、channelsからデータをピックアップして描画する
            DrawMainChartDataD2D();

            //各種情報描画
            DrawMainChartInfoD2D();
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

    }
}
