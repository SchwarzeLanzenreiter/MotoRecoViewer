﻿namespace MotoRecoViewer
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAppend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuConvert = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuConvertCANData = new System.Windows.Forms.ToolStripMenuItem();
            this.dashWareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wholeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.telemetryOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wholeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cursorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.cANDecodeSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuMap = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuBingMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGoogleMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenCycleMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenStreetMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWikiMapiaMap = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.PanelMainChart = new System.Windows.Forms.Panel();
            this.PanelSubChart = new System.Windows.Forms.Panel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.GMapControl = new GMap.NET.WindowsForms.GMapControl();
            this.folderBrowsDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip.SuspendLayout();
            this.contextMenuMap.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.ToolStripMenuItemView,
            this.helpHToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1099, 30);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOpen,
            this.MenuAppend,
            this.toolStripMenuItem1,
            this.MenuConvert,
            this.MenuExit});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(63, 26);
            this.MenuFile.Text = "File(&F)";
            // 
            // MenuOpen
            // 
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MenuOpen.Size = new System.Drawing.Size(217, 26);
            this.MenuOpen.Text = "Open(&O)";
            this.MenuOpen.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // MenuAppend
            // 
            this.MenuAppend.Name = "MenuAppend";
            this.MenuAppend.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.MenuAppend.Size = new System.Drawing.Size(217, 26);
            this.MenuAppend.Text = "Append(&A)";
            this.MenuAppend.Click += new System.EventHandler(this.MenuAppend_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(214, 6);
            // 
            // MenuConvert
            // 
            this.MenuConvert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuConvertCANData,
            this.dashWareToolStripMenuItem,
            this.telemetryOverlayToolStripMenuItem});
            this.MenuConvert.Name = "MenuConvert";
            this.MenuConvert.Size = new System.Drawing.Size(217, 26);
            this.MenuConvert.Text = "CSV Export";
            // 
            // MenuConvertCANData
            // 
            this.MenuConvertCANData.Name = "MenuConvertCANData";
            this.MenuConvertCANData.Size = new System.Drawing.Size(206, 26);
            this.MenuConvertCANData.Text = "CAN Data";
            this.MenuConvertCANData.Click += new System.EventHandler(this.MenuConvertAscii_Click);
            // 
            // dashWareToolStripMenuItem
            // 
            this.dashWareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wholeToolStripMenuItem,
            this.cursorToolStripMenuItem,
            this.folderToolStripMenuItem1});
            this.dashWareToolStripMenuItem.Name = "dashWareToolStripMenuItem";
            this.dashWareToolStripMenuItem.Size = new System.Drawing.Size(206, 26);
            this.dashWareToolStripMenuItem.Text = "DashWare";
            // 
            // wholeToolStripMenuItem
            // 
            this.wholeToolStripMenuItem.Name = "wholeToolStripMenuItem";
            this.wholeToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.wholeToolStripMenuItem.Text = "Whole";
            this.wholeToolStripMenuItem.Click += new System.EventHandler(this.wholeToolStripMenuItem_Click);
            // 
            // cursorToolStripMenuItem
            // 
            this.cursorToolStripMenuItem.Name = "cursorToolStripMenuItem";
            this.cursorToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.cursorToolStripMenuItem.Text = "Cursor";
            this.cursorToolStripMenuItem.Click += new System.EventHandler(this.cursorToolStripMenuItem_Click);
            // 
            // folderToolStripMenuItem1
            // 
            this.folderToolStripMenuItem1.Name = "folderToolStripMenuItem1";
            this.folderToolStripMenuItem1.Size = new System.Drawing.Size(135, 26);
            this.folderToolStripMenuItem1.Text = "Folder";
            this.folderToolStripMenuItem1.Click += new System.EventHandler(this.folderToolStripMenuItem1_Click);
            // 
            // telemetryOverlayToolStripMenuItem
            // 
            this.telemetryOverlayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wholeToolStripMenuItem1,
            this.cursorToolStripMenuItem1,
            this.folderToolStripMenuItem});
            this.telemetryOverlayToolStripMenuItem.Name = "telemetryOverlayToolStripMenuItem";
            this.telemetryOverlayToolStripMenuItem.Size = new System.Drawing.Size(206, 26);
            this.telemetryOverlayToolStripMenuItem.Text = "TelemetryOverlay";
            // 
            // wholeToolStripMenuItem1
            // 
            this.wholeToolStripMenuItem1.Name = "wholeToolStripMenuItem1";
            this.wholeToolStripMenuItem1.Size = new System.Drawing.Size(135, 26);
            this.wholeToolStripMenuItem1.Text = "Whole";
            this.wholeToolStripMenuItem1.Click += new System.EventHandler(this.wholeToolStripMenuItem1_Click);
            // 
            // cursorToolStripMenuItem1
            // 
            this.cursorToolStripMenuItem1.Name = "cursorToolStripMenuItem1";
            this.cursorToolStripMenuItem1.Size = new System.Drawing.Size(135, 26);
            this.cursorToolStripMenuItem1.Text = "Cursor";
            this.cursorToolStripMenuItem1.Click += new System.EventHandler(this.cursorToolStripMenuItem1_Click);
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new System.Drawing.Size(135, 26);
            this.folderToolStripMenuItem.Text = "Folder";
            this.folderToolStripMenuItem.Click += new System.EventHandler(this.folderToolStripMenuItem_Click);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(217, 26);
            this.MenuExit.Text = "Exit(&X)";
            this.MenuExit.Click += new System.EventHandler(this.終了ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemView
            // 
            this.ToolStripMenuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cANDecodeSettingToolStripMenuItem,
            this.mapSettingToolStripMenuItem});
            this.ToolStripMenuItemView.Name = "ToolStripMenuItemView";
            this.ToolStripMenuItemView.Size = new System.Drawing.Size(90, 26);
            this.ToolStripMenuItemView.Text = "Option(&O)";
            // 
            // cANDecodeSettingToolStripMenuItem
            // 
            this.cANDecodeSettingToolStripMenuItem.Name = "cANDecodeSettingToolStripMenuItem";
            this.cANDecodeSettingToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.cANDecodeSettingToolStripMenuItem.Text = "Ch Setting";
            this.cANDecodeSettingToolStripMenuItem.Click += new System.EventHandler(this.CANDecodeSettingToolStripMenuItem_Click);
            // 
            // mapSettingToolStripMenuItem
            // 
            this.mapSettingToolStripMenuItem.Name = "mapSettingToolStripMenuItem";
            this.mapSettingToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.mapSettingToolStripMenuItem.Text = "Map Setting";
            this.mapSettingToolStripMenuItem.Click += new System.EventHandler(this.MapSettingToolStripMenuItem_Click);
            // 
            // helpHToolStripMenuItem
            // 
            this.helpHToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.aboutAToolStripMenuItem});
            this.helpHToolStripMenuItem.Name = "helpHToolStripMenuItem";
            this.helpHToolStripMenuItem.Size = new System.Drawing.Size(76, 26);
            this.helpHToolStripMenuItem.Text = "Help(&H)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(185, 26);
            this.toolStripMenuItem2.Text = "MotoReco.net";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2_Click);
            // 
            // aboutAToolStripMenuItem
            // 
            this.aboutAToolStripMenuItem.Name = "aboutAToolStripMenuItem";
            this.aboutAToolStripMenuItem.Size = new System.Drawing.Size(185, 26);
            this.aboutAToolStripMenuItem.Text = "About(&A)";
            this.aboutAToolStripMenuItem.Click += new System.EventHandler(this.AboutAToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.Filter = "datファイル|*.dat";
            // 
            // contextMenuMap
            // 
            this.contextMenuMap.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuMap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBingMap,
            this.menuGoogleMap,
            this.menuOpenCycleMap,
            this.menuOpenStreetMap,
            this.menuWikiMapiaMap});
            this.contextMenuMap.Name = "contextMenuMap";
            this.contextMenuMap.Size = new System.Drawing.Size(192, 124);
            // 
            // menuBingMap
            // 
            this.menuBingMap.Name = "menuBingMap";
            this.menuBingMap.Size = new System.Drawing.Size(191, 24);
            this.menuBingMap.Text = "Bing Map";
            this.menuBingMap.Click += new System.EventHandler(this.MenuBingMap_Click);
            // 
            // menuGoogleMap
            // 
            this.menuGoogleMap.Name = "menuGoogleMap";
            this.menuGoogleMap.Size = new System.Drawing.Size(191, 24);
            this.menuGoogleMap.Text = "Google Map";
            this.menuGoogleMap.Click += new System.EventHandler(this.MenuGoogleMap_Click);
            // 
            // menuOpenCycleMap
            // 
            this.menuOpenCycleMap.Name = "menuOpenCycleMap";
            this.menuOpenCycleMap.Size = new System.Drawing.Size(191, 24);
            this.menuOpenCycleMap.Text = "Open Cycle Map";
            this.menuOpenCycleMap.Click += new System.EventHandler(this.MenuOpenCycleMap_Click);
            // 
            // menuOpenStreetMap
            // 
            this.menuOpenStreetMap.Name = "menuOpenStreetMap";
            this.menuOpenStreetMap.Size = new System.Drawing.Size(191, 24);
            this.menuOpenStreetMap.Text = "Open Street Map";
            this.menuOpenStreetMap.Click += new System.EventHandler(this.MenuOpenStreetMap_Click);
            // 
            // menuWikiMapiaMap
            // 
            this.menuWikiMapiaMap.Name = "menuWikiMapiaMap";
            this.menuWikiMapiaMap.Size = new System.Drawing.Size(191, 24);
            this.menuWikiMapiaMap.Text = "Wiki Mapia Map";
            this.menuWikiMapiaMap.Click += new System.EventHandler(this.MenuWikiMapiaMap_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "CSVファイル|*.csv";
            this.saveFileDialog.OverwritePrompt = false;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 561);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(1099, 28);
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "statusStrip1";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(133, 20);
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 22);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 30);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1099, 531);
            this.splitContainer1.SplitterDistance = 916;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 9;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.PanelMainChart);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.PanelSubChart);
            this.splitContainer2.Size = new System.Drawing.Size(916, 531);
            this.splitContainer2.SplitterDistance = 345;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // PanelMainChart
            // 
            this.PanelMainChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelMainChart.Location = new System.Drawing.Point(0, 0);
            this.PanelMainChart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PanelMainChart.Name = "PanelMainChart";
            this.PanelMainChart.Size = new System.Drawing.Size(914, 343);
            this.PanelMainChart.TabIndex = 1;
            this.PanelMainChart.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelMainChart_Paint);
            this.PanelMainChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelMainChart_MouseDown);
            this.PanelMainChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelMainChart_MouseMove);
            this.PanelMainChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelMainChart_MouseUp);
            this.PanelMainChart.Resize += new System.EventHandler(this.PanelMainChart_Resize);
            // 
            // PanelSubChart
            // 
            this.PanelSubChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelSubChart.Location = new System.Drawing.Point(0, 0);
            this.PanelSubChart.Margin = new System.Windows.Forms.Padding(4);
            this.PanelSubChart.Name = "PanelSubChart";
            this.PanelSubChart.Size = new System.Drawing.Size(914, 179);
            this.PanelSubChart.TabIndex = 1;
            this.PanelSubChart.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelSubChart_Paint);
            this.PanelSubChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelSubChart_MouseDown);
            this.PanelSubChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelSubChart_MouseMove);
            this.PanelSubChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelSubChart_MouseUp);
            this.PanelSubChart.Resize += new System.EventHandler(this.PanelSubChart_Resize);
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.GMapControl);
            this.splitContainer3.Size = new System.Drawing.Size(178, 531);
            this.splitContainer3.SplitterDistance = 459;
            this.splitContainer3.SplitterWidth = 5;
            this.splitContainer3.TabIndex = 2;
            // 
            // GMapControl
            // 
            this.GMapControl.Bearing = 0F;
            this.GMapControl.CanDragMap = true;
            this.GMapControl.ContextMenuStrip = this.contextMenuMap;
            this.GMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GMapControl.EmptyTileColor = System.Drawing.Color.Navy;
            this.GMapControl.GrayScaleMode = false;
            this.GMapControl.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.GMapControl.LevelsKeepInMemory = 5;
            this.GMapControl.Location = new System.Drawing.Point(0, 0);
            this.GMapControl.Margin = new System.Windows.Forms.Padding(4);
            this.GMapControl.MarkersEnabled = true;
            this.GMapControl.MaxZoom = 18;
            this.GMapControl.MinZoom = 3;
            this.GMapControl.MouseWheelZoomEnabled = true;
            this.GMapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.GMapControl.Name = "GMapControl";
            this.GMapControl.NegativeMode = false;
            this.GMapControl.PolygonsEnabled = true;
            this.GMapControl.RetryLoadTile = 0;
            this.GMapControl.RoutesEnabled = true;
            this.GMapControl.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.GMapControl.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.GMapControl.ShowTileGridLines = false;
            this.GMapControl.Size = new System.Drawing.Size(176, 65);
            this.GMapControl.TabIndex = 4;
            this.GMapControl.Zoom = 3D;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 589);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "Moto Reco Viewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.contextMenuMap.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemView;
        private System.Windows.Forms.ToolStripMenuItem cANDecodeSettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuConvert;
        private System.Windows.Forms.ToolStripMenuItem MenuConvertCANData;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem mapSettingToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuMap;
        private System.Windows.Forms.ToolStripMenuItem menuBingMap;
        private System.Windows.Forms.ToolStripMenuItem menuGoogleMap;
        private System.Windows.Forms.ToolStripMenuItem menuOpenCycleMap;
        private System.Windows.Forms.ToolStripMenuItem menuOpenStreetMap;
        private System.Windows.Forms.ToolStripMenuItem menuWikiMapiaMap;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private GMap.NET.WindowsForms.GMapControl GMapControl;
        private System.Windows.Forms.ToolStripMenuItem helpHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem MenuAppend;
        private System.Windows.Forms.Panel PanelMainChart;
        private System.Windows.Forms.Panel PanelSubChart;
        private System.Windows.Forms.ToolStripMenuItem dashWareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wholeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cursorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem telemetryOverlayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wholeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cursorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowsDialog;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem1;
    }
}