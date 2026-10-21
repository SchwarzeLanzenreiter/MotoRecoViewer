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
    /// 地図表示(GMap)の更新
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// 地図を更新表示する
        /// 緯度経度のチャンネル名はLatitudeとLongitude固定とする
        /// 地図位置はカーソル1位置とする
        /// </summary>
        private void UpdateMap()
        {
            //経度緯度情報の存在チェック
            int idx_lat = channelAnalyzer.IndexOfFixedFormula("#GPS_Latitude");
            int idx_lon = channelAnalyzer.IndexOfFixedFormula("#GPS_Longitude");

            //どちらかなければ何もしない
            if ((idx_lat < 0) || (idx_lon < 0))
            {
                return;
            }

            // MainChartの中央に対応するタイムスタンプを計算
            double mainChartCenterTime = subPosTime + (divTime * 20) / 2;

            // MainCharの中央に応じたデータidx取得
            int mainChartCenterIdx = channels[idx_lat].FindLeftIndex(mainChartCenterTime);

            // MainChartの中央に応じた緯度経度取得
            float fl_lon = (float)channels[idx_lon].LogData[mainChartCenterIdx].DataValue;
            float fl_lat = (float)channels[idx_lat].LogData[mainChartCenterIdx].DataValue;

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
            int mainChartLeftIdx = channels[idx_lat].FindLeftIndex(subPosTime);
            int mainChartRightIdx = channels[idx_lat].FindLeftIndex(mainChartRightTime);

            // ルートをMapに追加
            // まず既存ルートをクリア
            GMapOverlayRoute.Routes.Clear();

            // ポイント追加
            List<PointLatLng> points = new List<PointLatLng>();
            for (int i = mainChartLeftIdx; i <= mainChartRightIdx; i++)
            {
                fl_lon = (float)channels[idx_lon].LogData[i].DataValue;
                fl_lat = (float)channels[idx_lat].LogData[i].DataValue;

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
            int idx_lat = channelAnalyzer.IndexOfFixedFormula("#GPS_Latitude");
            int idx_lon = channelAnalyzer.IndexOfFixedFormula("#GPS_Longitude");

            //どちらかなければ何もしない
            if ((idx_lat < 0) || (idx_lon < 0))
            {
                return;
            }

            // MainChartのカーソル位置1に対応するタイムスタンプを計算
            double cur1PosTime = subPosTime + (divTime * 20) / (PanelMainChart.Width - 2 * chartMargin) * mainCur1Pos;

            // カーソル位置左側データidx取得
            int leftIdx = channels[idx_lat].FindLeftIndex(cur1PosTime);
            double leftTime = channels[idx_lat].LogData[leftIdx].DataTime;

            // カーソル位置右側データidx取得
            int rightIdx;
            if (leftIdx == channels[idx_lat].LogData.Count - 1)
            {
                rightIdx = leftIdx;
            } else
            {
                rightIdx = leftIdx + 1;   // ToDo 経度緯度データが、重複してロギングされているため.CanLogger側を治す 
            }
            double rightTime = channels[idx_lat].LogData[rightIdx].DataTime;

            // 緯度経度取得
            double leftLon = channels[idx_lon].LogData[leftIdx].DataValue;
            double leftLat = channels[idx_lat].LogData[leftIdx].DataValue;

            double rightLon = channels[idx_lon].LogData[rightIdx].DataValue;
            double rightLat = channels[idx_lat].LogData[rightIdx].DataValue;

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
            leftIdx = channels[idx_lat].FindLeftIndex(cur2PosTime);
            leftTime = channels[idx_lat].LogData[leftIdx].DataTime;

            // カーソル位置右側データidx取得
            if (leftIdx == channels[idx_lat].LogData.Count - 1)
            {
                rightIdx = leftIdx;
            }
            else
            {
                rightIdx = leftIdx + 1;   // ToDo 経度緯度データが、重複してロギングされているため.CanLogger側を治す 
            }
            rightTime = channels[idx_lat].LogData[rightIdx].DataTime;

            // 緯度経度取得
            leftLon = channels[idx_lon].LogData[leftIdx].DataValue;
            leftLat = channels[idx_lat].LogData[leftIdx].DataValue;

            rightLon = channels[idx_lon].LogData[rightIdx].DataValue;
            rightLat = channels[idx_lat].LogData[rightIdx].DataValue;

            // 経度緯度を前後2点データから線形補間する
            liLat = LinearInterpolation(leftTime, leftLat, rightTime, rightLat, cur2PosTime);
            liLon = LinearInterpolation(leftTime, leftLon, rightTime, rightLon, cur2PosTime);

            //マーカー更新
            GMarkerGoogle marker2 = new GMarkerGoogle(new PointLatLng(liLat, liLon), GMarkerGoogleType.red);
            marker2.ToolTipText = "Cursor2";
            GMapOverlayMarker.Markers.Add(marker2);
        }

    }
}
