namespace MotoRecoViewer
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
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuConvert = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuConvertAscii = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.cANDecodeSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pictureMain = new System.Windows.Forms.PictureBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureSub = new System.Windows.Forms.PictureBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.GMapControl = new GMap.NET.WindowsForms.GMapControl();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.mapSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuMap = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuBingMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGoogleMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenCycleMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenStreetMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWikiMapiaMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMain)).BeginInit();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.contextMenuMap.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.ToolStripMenuItemView});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1904, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOpen,
            this.toolStripMenuItem1,
            this.MenuConvert,
            this.MenuExit});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(63, 24);
            this.MenuFile.Text = "File(&F)";
            // 
            // MenuOpen
            // 
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MenuOpen.Size = new System.Drawing.Size(224, 26);
            this.MenuOpen.Text = "Open(&O)";
            this.MenuOpen.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(221, 6);
            // 
            // MenuConvert
            // 
            this.MenuConvert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuConvertAscii});
            this.MenuConvert.Name = "MenuConvert";
            this.MenuConvert.Size = new System.Drawing.Size(224, 26);
            this.MenuConvert.Text = "Export";
            // 
            // MenuConvertAscii
            // 
            this.MenuConvertAscii.Name = "MenuConvertAscii";
            this.MenuConvertAscii.Size = new System.Drawing.Size(224, 26);
            this.MenuConvertAscii.Text = "Ascii CSV";
            this.MenuConvertAscii.Click += new System.EventHandler(this.MenuConvertAscii_Click);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(224, 26);
            this.MenuExit.Text = "Exit(&X)";
            this.MenuExit.Click += new System.EventHandler(this.終了ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemView
            // 
            this.ToolStripMenuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cANDecodeSettingToolStripMenuItem,
            this.mapSettingToolStripMenuItem});
            this.ToolStripMenuItemView.Name = "ToolStripMenuItemView";
            this.ToolStripMenuItemView.Size = new System.Drawing.Size(90, 24);
            this.ToolStripMenuItemView.Text = "Option(&O)";
            // 
            // cANDecodeSettingToolStripMenuItem
            // 
            this.cANDecodeSettingToolStripMenuItem.Name = "cANDecodeSettingToolStripMenuItem";
            this.cANDecodeSettingToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.cANDecodeSettingToolStripMenuItem.Text = "Read Setting";
            this.cANDecodeSettingToolStripMenuItem.Click += new System.EventHandler(this.CANDecodeSettingToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.Filter = "datファイル|*.dat";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
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
            this.splitContainer1.Size = new System.Drawing.Size(1904, 728);
            this.splitContainer1.SplitterDistance = 1473;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 5;
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
            this.splitContainer2.Panel1.Controls.Add(this.pictureMain);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.statusStrip);
            this.splitContainer2.Panel2.Controls.Add(this.pictureSub);
            this.splitContainer2.Size = new System.Drawing.Size(1473, 728);
            this.splitContainer2.SplitterDistance = 482;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // pictureMain
            // 
            this.pictureMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureMain.Location = new System.Drawing.Point(0, 0);
            this.pictureMain.Margin = new System.Windows.Forms.Padding(4);
            this.pictureMain.Name = "pictureMain";
            this.pictureMain.Size = new System.Drawing.Size(1471, 480);
            this.pictureMain.TabIndex = 0;
            this.pictureMain.TabStop = false;
            this.pictureMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureMain_MouseDown);
            this.pictureMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureMain_MouseMove);
            this.pictureMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureMain_MouseUp);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 211);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(1471, 28);
            this.statusStrip.TabIndex = 1;
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
            // pictureSub
            // 
            this.pictureSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureSub.Location = new System.Drawing.Point(0, 0);
            this.pictureSub.Margin = new System.Windows.Forms.Padding(4);
            this.pictureSub.Name = "pictureSub";
            this.pictureSub.Size = new System.Drawing.Size(1471, 239);
            this.pictureSub.TabIndex = 0;
            this.pictureSub.TabStop = false;
            this.pictureSub.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureSub_MouseMove);
            this.pictureSub.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureSub_MouseDown);
            this.pictureSub.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureSub_MouseMove);
            this.pictureSub.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureSub_MouseUp);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.GMapControl);
            this.splitContainer3.Size = new System.Drawing.Size(424, 726);
            this.splitContainer3.SplitterDistance = 446;
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
            this.GMapControl.LevelsKeepInMemmory = 5;
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
            this.GMapControl.Size = new System.Drawing.Size(424, 275);
            this.GMapControl.TabIndex = 4;
            this.GMapControl.Zoom = 3D;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "CSVファイル|*.csv";
            // 
            // mapSettingToolStripMenuItem
            // 
            this.mapSettingToolStripMenuItem.Name = "mapSettingToolStripMenuItem";
            this.mapSettingToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.mapSettingToolStripMenuItem.Text = "Map Setting";
            this.mapSettingToolStripMenuItem.Click += new System.EventHandler(this.mapSettingToolStripMenuItem_Click);
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
            this.menuBingMap.Size = new System.Drawing.Size(210, 24);
            this.menuBingMap.Text = "Bing Map";
            this.menuBingMap.Click += new System.EventHandler(this.menuBingMap_Click);
            // 
            // menuGoogleMap
            // 
            this.menuGoogleMap.Name = "menuGoogleMap";
            this.menuGoogleMap.Size = new System.Drawing.Size(210, 24);
            this.menuGoogleMap.Text = "Google Map";
            this.menuGoogleMap.Click += new System.EventHandler(this.menuGoogleMap_Click);
            // 
            // menuOpenCycleMap
            // 
            this.menuOpenCycleMap.Name = "menuOpenCycleMap";
            this.menuOpenCycleMap.Size = new System.Drawing.Size(210, 24);
            this.menuOpenCycleMap.Text = "Open Cycle Map";
            this.menuOpenCycleMap.Click += new System.EventHandler(this.menuOpenCycleMap_Click);
            // 
            // menuOpenStreetMap
            // 
            this.menuOpenStreetMap.Name = "menuOpenStreetMap";
            this.menuOpenStreetMap.Size = new System.Drawing.Size(210, 24);
            this.menuOpenStreetMap.Text = "Open Street Map";
            this.menuOpenStreetMap.Click += new System.EventHandler(this.menuOpenStreetMap_Click);
            // 
            // menuWikiMapiaMap
            // 
            this.menuWikiMapiaMap.Name = "menuWikiMapiaMap";
            this.menuWikiMapiaMap.Size = new System.Drawing.Size(210, 24);
            this.menuWikiMapiaMap.Text = "Wiki Mapia Map";
            this.menuWikiMapiaMap.Click += new System.EventHandler(this.menuWikiMapiaMap_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 756);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "Moto Reco Viewer";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureMain)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSub)).EndInit();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.contextMenuMap.ResumeLayout(false);
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
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox pictureMain;
        private System.Windows.Forms.PictureBox pictureSub;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private GMap.NET.WindowsForms.GMapControl GMapControl;
        private System.Windows.Forms.ToolStripMenuItem MenuConvert;
        private System.Windows.Forms.ToolStripMenuItem MenuConvertAscii;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem mapSettingToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuMap;
        private System.Windows.Forms.ToolStripMenuItem menuBingMap;
        private System.Windows.Forms.ToolStripMenuItem menuGoogleMap;
        private System.Windows.Forms.ToolStripMenuItem menuOpenCycleMap;
        private System.Windows.Forms.ToolStripMenuItem menuOpenStreetMap;
        private System.Windows.Forms.ToolStripMenuItem menuWikiMapiaMap;
    }
}