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
    /// メニュー・マウス・フォームのイベントハンドラ
    /// </summary>
    public partial class FormMain : Form
    {
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
                newItem.BackColor = System.Drawing.Color.FromArgb(decodeRule.GetChartColor(i));

                ListViewData.Items.Add(newItem);
            }

            //ListViewDataのClickイベント復活
            ListViewData.ItemChecked += ListViewData_ItemChecked;

            //描画再開
            ListViewData.EndUpdate();

            // データロードなければ何もしない
            if (channels.Count == 0)
            {
                return;
            }

            // もしデータがロード済だった場合は、channelsのチャンネル情報をアップデートする
            for (int i=0; i < decodeRule.Count; i++)
            {
                int idx;
                string chName;

                chName = decodeRule.GetChName(i);

                // decodeRule.ChName[i]と同じChNameのchannelsのidxを取得
                if (!channels.Contains(chName))
                {
                    //decodeRuleのChName[i]がchannelsになければ何もしない
                    continue;
                }

                idx = channels.IndexOf(chName);

                channels[idx].ChColor = decodeRule.GetChartColor(i);
                channels[idx].ChMax = decodeRule.GetChartMax(i);
                channels[idx].ChMin = decodeRule.GetChartMin(i);
                channels[idx].ChPreview = decodeRule.GetChartPreview(i);
                channels[idx].ChShow = decodeRule.GetChartShow(i);
                channels[idx].ChFilter = decodeRule.GetChartUseFilter(i);
                channels[idx].ChFilterCutOff = decodeRule.GetChartCutOff(i);
            }

            // もしchannelsには存在するが、decodeRuleに存在しない場合(一度データをロードした後、あるChを削除した場合)、
            // channelsからデータを削除する。
            // 削除に伴うChName→インデックスの貼り直しはChannelStoreが行う。
            channels.RemoveAllExcept(ch => decodeRule.ChNameIndexOf(ch.ChName) != -1);

            //フィルタ処理は各ch毎なので並列化可能
            Parallel.For(0, channels.Count, i =>
            {
                channels[i].FilterData();
            });

            //　グラフ再描画
            PanelMainChart.Refresh();
            PanelSubChart.Refresh();
        }

        private void ListViewData_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ListViewData.Items.Count < 1) return;

            for (int i = 0; i < ListViewData.Items.Count; i++)
            {
                if (!channels.Contains(ListViewData.Items[i].Text)) {
                    continue; 
                }

                int idx = channels.IndexOf(ListViewData.Items[i].Text);
                channels[idx].ChShow = ListViewData.Items[i].Checked;
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
                channels.Clear();
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

                // ListViewDataの表示/非表示を反映
                ListViewData_ItemChecked(sender,null);
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

            try
            {
                // Form位置ロード
                System.Drawing.Point backLocation = this.Location;

                this.Location = Properties.Settings.Default.FormMainLocation;
                this.Size = Properties.Settings.Default.FormMainSize;

                //デスクトップのワークエリアとウィンドウの矩形が重なっていなかったら位置を戻す
                if (System.Windows.Forms.Screen.GetWorkingArea(this).IntersectsWith(this.Bounds) == false)
                {
                    this.Location = backLocation;
                }

 
                this.splitContainer1.SplitterDistance = Properties.Settings.Default.FormMainSplit1;
                this.splitContainer2.SplitterDistance = Properties.Settings.Default.FormMainSplit2;
                this.splitContainer3.SplitterDistance = Properties.Settings.Default.FormMainSplit3;
            }
            catch { 
            }

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

        }

        private void MenuConvertDecodeDataCursor_Click(object sender, EventArgs e)
        {

        }

        private void PanelMainChart_Paint(object sender, PaintEventArgs e)
        {
            // CANデータ読み込み中→即抜ける
            if (IsReadingCanData) { return; }

            // ListChNameがNULL→即抜ける
            if (channels == null) { return; }

            // ListChNameが0＝CANデータ未読み込み→即抜ける
            if (channels.Count < 1) { return; }

            mainCanvas.RenderTarget.BeginDraw();
            //=======================================================================================================================
            // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓　　　　BeginDrawとEndDrawの間で描画すること　↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            // ======================================================================================================================

            DrawMainChartD2D();

            //=======================================================================================================================
            // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑　　　　BeginDrawとEndDrawの間で描画すること　↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            // ======================================================================================================================
            mainCanvas.RenderTarget.EndDraw();
            mainCanvas.Present();

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
            if (mainCanvas == null)
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
            if (channels == null) { return; }

            // ListChNameが0＝CANデータ未読み込み→即抜ける
            if (channels.Count < 1) { return; }
            
            subCanvas.RenderTarget.BeginDraw();
            //=======================================================================================================================
            // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓　　　　BeginDrawとEndDrawの間で描画すること　↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            // ======================================================================================================================

            DrawSubChartD2D();

            //=======================================================================================================================
            // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑　　　　BeginDrawとEndDrawの間で描画すること　↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            // ======================================================================================================================
            subCanvas.RenderTarget.EndDraw();
            subCanvas.Present();

        }

        private void PanelSubChart_Resize(object sender, EventArgs e)
        {
            if (mainCanvas == null)
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

        private void wholeToolStripMenuItem_Click(object sender, EventArgs e)
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
            if (channels.Count == 0)
            {
                return;
            }

            // 現在保持している表示中のデコード済データをCSVエクスポートする
            // 名前をつけて保存
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ConvertDecodeData(saveFileDialog.FileName, MODE_ALL, TYPE_DASHWARE);
            }
        }

        private void cursorToolStripMenuItem_Click(object sender, EventArgs e)
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
            if (channels.Count == 0)
            {
                return;
            }

            // 現在保持している表示中のデコード済データをCSVエクスポートする
            // 名前をつけて保存
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ConvertDecodeData(saveFileDialog.FileName, MODE_CURSOR,TYPE_DASHWARE);
            }
        }

        private void wholeToolStripMenuItem1_Click(object sender, EventArgs e)
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
            if (channels.Count == 0)
            {
                return;
            }

            // 現在保持している表示中のデコード済データをCSVエクスポートする
            // 名前をつけて保存
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ConvertDecodeData(saveFileDialog.FileName, MODE_ALL, TYPE_TELEMETRYOVERLAY);
            }
        }

        private void cursorToolStripMenuItem1_Click(object sender, EventArgs e)
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
            if (channels.Count == 0)
            {
                return;
            }

            // 現在保持している表示中のデコード済データをCSVエクスポートする
            // 名前をつけて保存
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ConvertDecodeData(saveFileDialog.FileName, MODE_CURSOR,TYPE_TELEMETRYOVERLAY);
            }
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CSVConvertFolder(TYPE_TELEMETRYOVERLAY);
        }

        private void folderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CSVConvertFolder(TYPE_DASHWARE);
        }
    }
}
