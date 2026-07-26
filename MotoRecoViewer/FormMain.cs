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
    /// メインウィンドウ。フィールド定義と、生成/破棄まわりを持つ。
    /// 処理の本体は責務ごとに以下のpartialへ分割している。
    ///   FormMain.Data.cs   … CANデータの読み込みとデータ一覧表示
    ///   FormMain.Chart.cs  … MainChart / SubChart のDirect2D描画
    ///   FormMain.Map.cs    … 地図表示の更新
    ///   FormMain.Export.cs … CSVエクスポート
    ///   FormMain.Events.cs … イベントハンドラ
    /// </summary>
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

        //==========================
        //
        //  Private変数
        //
        //==========================
        private BufferedListView ListViewData;      // ダブルバッファのListView

        private const double chartMargin = 15;      // グリッドとpictureSubの余白
               
        private ChannelStore channels;              // 読み込み済みチャンネルの集合
        private ChannelAnalyzer channelAnalyzer;    // 派生チャンネルの計算と固定式チャンネルの検索

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
        private D2DCanvas mainCanvas;               // PanelMainChartへの描画先
        private D2DCanvas subCanvas;                // PanelSubChartへの描画先

        private const int MODE_ALL = 0;
        private const int MODE_CURSOR = 1;

        private const int TYPE_DASHWARE = 0;
        private const int TYPE_TELEMETRYOVERLAY = 1;

        //==========================
        //
        //  生成と破棄
        //
        //==========================

        /// <summary>
        /// DirectXをリサイズする
        /// 片方のパネルのリサイズは他方のレイアウトにも影響するため、常に両方まとめてリサイズする
        /// </summary>
        private void ResizeDirectX()
        {
            mainCanvas.Resize();
            subCanvas.Resize();
        }

        public FormMain()
        {
            InitializeComponent();

            // Formコンストラクタで、Private変数のClassをnewしておく
            channels = new ChannelStore();
            decodeRule = new DecodeRule();
            channelAnalyzer = new ChannelAnalyzer(channels, decodeRule);
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
            mainCanvas = new D2DCanvas(PanelMainChart.Handle);
            subCanvas = new D2DCanvas(PanelSubChart.Handle);

            this.Disposed += (sender, e) => DisposeDevice();

        }

        public void DisposeDevice()
        {
            mainCanvas.Dispose();
            subCanvas.Dispose();
        }

    }
}
