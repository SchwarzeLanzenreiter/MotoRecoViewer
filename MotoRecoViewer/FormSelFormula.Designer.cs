namespace MotoRecoViewer
{
    partial class FormSelFormula
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("K51");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("GPS");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Data");
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "RPM",
            "engine RPM",
            "#K51_RPM"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Throttel Valve Position",
            "Throttel Valve Position",
            "#K51_ThrottelValvePosition"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Throttel Position",
            "Throttel Position",
            "#K51_ThrottelPosition"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Throttel Grip Position",
            "Throttel Grip Position",
            "#K51_ThrottelGripPosition"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "FrBrake1"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "FrBrake1"),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "#K51_FrBrake1", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.HotTrack, new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128))))}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "FrBrake2",
            "FrBrake2",
            "#K51_FrBrake2"}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "RrBrake1",
            "RrBrake1",
            "#K51_RrBrake1"}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "RrBrake2",
            "RrBrake2",
            "#K51_RrBrake2"}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "FrStroke",
            "FrStroke",
            "#K51_FrStroke"}, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "RrStroke",
            "RrStroke",
            "#K51_RrStroke"}, -1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "LeanAngle",
            "LeanAngle",
            "#K51_LeanAngle"}, -1);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "FrSpeed1",
            "Front Wheel Speed",
            "#K51_FrSpeed1"}, -1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "FrSpeed2",
            "Front Wheel Speed2",
            "#K51_FrSpeed2"}, -1);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] {
            "RrSpeed",
            "Rear Wheel Speed",
            "#K51_RrSpeed"}, -1);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] {
            "SlipRate",
            "Front/Rear Slip Rate",
            "#K51_SlipRate"}, -1);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] {
            "Latitude",
            "Latitude",
            "#GPS_Latitude"}, -1);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] {
            "Longitude",
            "Longitude",
            "#GPS_Longitude"}, -1);
            System.Windows.Forms.ListViewItem listViewItem18 = new System.Windows.Forms.ListViewItem(new string[] {
            "GPS Speed",
            "Speed from GPS",
            "#GPS_Speed"}, -1);
            System.Windows.Forms.ListViewItem listViewItem19 = new System.Windows.Forms.ListViewItem(new string[] {
            "GPS Altitude",
            "Altitude from GPS",
            "#GPS_Altitude"}, -1);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.TreeViewBike = new System.Windows.Forms.TreeView();
            this.ListViewItem = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.TreeViewBike);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ListViewItem);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 264;
            this.splitContainer1.TabIndex = 0;
            // 
            // TreeViewBike
            // 
            this.TreeViewBike.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeViewBike.Location = new System.Drawing.Point(0, 0);
            this.TreeViewBike.Name = "TreeViewBike";
            treeNode1.Name = "TreeNodeK51";
            treeNode1.Text = "K51";
            treeNode2.Name = "TreeNodeGPS";
            treeNode2.Text = "GPS";
            treeNode3.Name = "TreeNodeData";
            treeNode3.Text = "Data";
            this.TreeViewBike.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.TreeViewBike.Size = new System.Drawing.Size(264, 450);
            this.TreeViewBike.TabIndex = 0;
            this.TreeViewBike.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewBike_AfterSelect);
            // 
            // ListViewItem
            // 
            this.ListViewItem.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.ListViewItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewItem.FullRowSelect = true;
            this.ListViewItem.HideSelection = false;
            this.ListViewItem.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13,
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17,
            listViewItem18,
            listViewItem19});
            this.ListViewItem.Location = new System.Drawing.Point(0, 0);
            this.ListViewItem.Name = "ListViewItem";
            this.ListViewItem.Size = new System.Drawing.Size(532, 450);
            this.ListViewItem.TabIndex = 0;
            this.ListViewItem.UseCompatibleStateImageBehavior = false;
            this.ListViewItem.View = System.Windows.Forms.View.Details;
            this.ListViewItem.SelectedIndexChanged += new System.EventHandler(this.ListViewItem_DoubleClick);
            this.ListViewItem.DoubleClick += new System.EventHandler(this.ListViewItem_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Ch Name";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "CAN ID";
            this.columnHeader2.Width = 50;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Define";
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Color";
            this.columnHeader4.Width = 50;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Max";
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Min";
            this.columnHeader6.Width = 50;
            // 
            // FormSelFormula
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FormSelFormula";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView TreeViewBike;
        private System.Windows.Forms.ListView ListViewItem;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}