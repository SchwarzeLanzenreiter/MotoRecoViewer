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
            this.PictureMain = new System.Windows.Forms.PictureBox();
            this.PictureSub = new System.Windows.Forms.PictureBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.GMapControl = new GMap.NET.WindowsForms.GMapControl();
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
            ((System.ComponentModel.ISupportInitialize)(this.PictureMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureSub)).BeginInit();
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
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1259, 28);
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
            this.MenuConvertAscii.Size = new System.Drawing.Size(153, 26);
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
            this.cANDecodeSettingToolStripMenuItem.Text = "Ch Setting";
            this.cANDecodeSettingToolStripMenuItem.Click += new System.EventHandler(this.CANDecodeSettingToolStripMenuItem_Click);
            // 
            // mapSettingToolStripMenuItem
            // 
            this.mapSettingToolStripMenuItem.Name = "mapSettingToolStripMenuItem";
            this.mapSettingToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.mapSettingToolStripMenuItem.Text = "Map Setting";
            this.mapSettingToolStripMenuItem.Click += new System.EventHandler(this.MapSettingToolStripMenuItem_Click);
            // 
            // helpHToolStripMenuItem
            // 
            this.helpHToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.aboutAToolStripMenuItem});
            this.helpHToolStripMenuItem.Name = "helpHToolStripMenuItem";
            this.helpHToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.helpHToolStripMenuItem.Text = "Help(&H)";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(224, 26);
            this.toolStripMenuItem2.Text = "MotoReco Web";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.ToolStripMenuItem2_Click);
            // 
            // aboutAToolStripMenuItem
            // 
            this.aboutAToolStripMenuItem.Name = "aboutAToolStripMenuItem";
            this.aboutAToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
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
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 527);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(1259, 28);
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
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(1259, 499);
            this.splitContainer1.SplitterDistance = 878;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 9;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.PictureMain);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.PictureSub);
            this.splitContainer2.Size = new System.Drawing.Size(878, 499);
            this.splitContainer2.SplitterDistance = 327;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // PictureMain
            // 
            this.PictureMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureMain.Location = new System.Drawing.Point(0, 0);
            this.PictureMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PictureMain.Name = "PictureMain";
            this.PictureMain.Size = new System.Drawing.Size(876, 325);
            this.PictureMain.TabIndex = 0;
            this.PictureMain.TabStop = false;
            this.PictureMain.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureMain_Paint);
            this.PictureMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureMain_MouseDown);
            this.PictureMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureMain_MouseMove);
            this.PictureMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureMain_MouseUp);
            // 
            // PictureSub
            // 
            this.PictureSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureSub.Location = new System.Drawing.Point(0, 0);
            this.PictureSub.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PictureSub.Name = "PictureSub";
            this.PictureSub.Size = new System.Drawing.Size(876, 165);
            this.PictureSub.TabIndex = 0;
            this.PictureSub.TabStop = false;
            this.PictureSub.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureSub_Paint);
            this.PictureSub.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureSub_MouseDown);
            this.PictureSub.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureSub_MouseMove);
            this.PictureSub.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureSub_MouseUp);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.GMapControl);
            this.splitContainer3.Size = new System.Drawing.Size(374, 497);
            this.splitContainer3.SplitterDistance = 302;
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
            this.GMapControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.GMapControl.Size = new System.Drawing.Size(374, 190);
            this.GMapControl.TabIndex = 4;
            this.GMapControl.Zoom = 3D;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1259, 555);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormMain";
            this.Text = "Moto Reco Viewer";
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
            ((System.ComponentModel.ISupportInitialize)(this.PictureMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureSub)).EndInit();
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
        private System.Windows.Forms.ToolStripMenuItem MenuConvertAscii;
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
        private System.Windows.Forms.PictureBox PictureMain;
        private System.Windows.Forms.PictureBox PictureSub;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private GMap.NET.WindowsForms.GMapControl GMapControl;
        private System.Windows.Forms.ToolStripMenuItem helpHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}