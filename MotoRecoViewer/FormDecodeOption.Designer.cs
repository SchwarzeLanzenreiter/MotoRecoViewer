namespace MotoRecoViewer
{
    partial class FormDecodeOption
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
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenFileDDF = new System.Windows.Forms.OpenFileDialog();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.column1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListViewDecode = new System.Windows.Forms.ListView();
            this.column8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BtnDel = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.lablCanId = new System.Windows.Forms.Label();
            this.lblChName = new System.Windows.Forms.Label();
            this.lblFormula = new System.Windows.Forms.Label();
            this.TextChName = new System.Windows.Forms.TextBox();
            this.TextCanId = new System.Windows.Forms.TextBox();
            this.TextFormula = new System.Windows.Forms.TextBox();
            this.BtnEdit = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TextColor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextMin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextMax = new System.Windows.Forms.TextBox();
            this.CheckPreview = new System.Windows.Forms.CheckBox();
            this.BtnNew = new System.Windows.Forms.Button();
            this.CheckShow = new System.Windows.Forms.CheckBox();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnBrowsRule = new System.Windows.Forms.Button();
            this.BtnPreAna = new System.Windows.Forms.Button();
            this.OpenFileDAT = new System.Windows.Forms.OpenFileDialog();
            this.BtnUp = new System.Windows.Forms.Button();
            this.BtnDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "decode definition file|*.ddf";
            // 
            // OpenFileDDF
            // 
            this.OpenFileDDF.FileName = "openFileDialog1";
            this.OpenFileDDF.Filter = "decode definition file|*.ddf";
            // 
            // column1
            // 
            this.column1.Text = "Ch Name";
            this.column1.Width = 100;
            // 
            // column2
            // 
            this.column2.Text = "CANID";
            this.column2.Width = 71;
            // 
            // column3
            // 
            this.column3.Text = "Formula";
            this.column3.Width = 150;
            // 
            // column4
            // 
            this.column4.Text = "Ch Color";
            // 
            // column5
            // 
            this.column5.Text = "Min";
            // 
            // column6
            // 
            this.column6.Text = "Max";
            // 
            // column7
            // 
            this.column7.Text = "Preview";
            // 
            // ListViewDecode
            // 
            this.ListViewDecode.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column1,
            this.column2,
            this.column3,
            this.column4,
            this.column5,
            this.column6,
            this.column7,
            this.column8});
            this.ListViewDecode.Dock = System.Windows.Forms.DockStyle.Top;
            this.ListViewDecode.FullRowSelect = true;
            this.ListViewDecode.GridLines = true;
            this.ListViewDecode.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListViewDecode.HideSelection = false;
            this.ListViewDecode.Location = new System.Drawing.Point(0, 0);
            this.ListViewDecode.MultiSelect = false;
            this.ListViewDecode.Name = "ListViewDecode";
            this.ListViewDecode.Size = new System.Drawing.Size(626, 406);
            this.ListViewDecode.TabIndex = 0;
            this.ListViewDecode.UseCompatibleStateImageBehavior = false;
            this.ListViewDecode.View = System.Windows.Forms.View.Details;
            this.ListViewDecode.SelectedIndexChanged += new System.EventHandler(this.ListViewDecode_SelectedIndexChanged);
            // 
            // column8
            // 
            this.column8.Text = "Show";
            // 
            // BtnDel
            // 
            this.BtnDel.Location = new System.Drawing.Point(104, 534);
            this.BtnDel.Name = "BtnDel";
            this.BtnDel.Size = new System.Drawing.Size(41, 31);
            this.BtnDel.TabIndex = 2;
            this.BtnDel.Text = "Del";
            this.BtnDel.UseVisualStyleBackColor = true;
            this.BtnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnOK.Location = new System.Drawing.Point(564, 496);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(50, 31);
            this.BtnOK.TabIndex = 3;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // lablCanId
            // 
            this.lablCanId.AutoSize = true;
            this.lablCanId.Location = new System.Drawing.Point(31, 449);
            this.lablCanId.Name = "lablCanId";
            this.lablCanId.Size = new System.Drawing.Size(44, 12);
            this.lablCanId.TabIndex = 4;
            this.lablCanId.Text = "CAN ID";
            // 
            // lblChName
            // 
            this.lblChName.AutoSize = true;
            this.lblChName.Location = new System.Drawing.Point(23, 423);
            this.lblChName.Name = "lblChName";
            this.lblChName.Size = new System.Drawing.Size(52, 12);
            this.lblChName.TabIndex = 5;
            this.lblChName.Text = "Ch Name";
            // 
            // lblFormula
            // 
            this.lblFormula.AutoSize = true;
            this.lblFormula.Location = new System.Drawing.Point(29, 474);
            this.lblFormula.Name = "lblFormula";
            this.lblFormula.Size = new System.Drawing.Size(46, 12);
            this.lblFormula.TabIndex = 6;
            this.lblFormula.Text = "Formula";
            // 
            // TextChName
            // 
            this.TextChName.Location = new System.Drawing.Point(81, 420);
            this.TextChName.Name = "TextChName";
            this.TextChName.Size = new System.Drawing.Size(223, 19);
            this.TextChName.TabIndex = 7;
            // 
            // TextCanId
            // 
            this.TextCanId.Location = new System.Drawing.Point(81, 446);
            this.TextCanId.Name = "TextCanId";
            this.TextCanId.Size = new System.Drawing.Size(42, 19);
            this.TextCanId.TabIndex = 8;
            // 
            // TextFormula
            // 
            this.TextFormula.Location = new System.Drawing.Point(81, 471);
            this.TextFormula.Name = "TextFormula";
            this.TextFormula.Size = new System.Drawing.Size(223, 19);
            this.TextFormula.TabIndex = 9;
            // 
            // BtnEdit
            // 
            this.BtnEdit.Location = new System.Drawing.Point(57, 534);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(41, 31);
            this.BtnEdit.TabIndex = 10;
            this.BtnEdit.Text = "Edit";
            this.BtnEdit.UseVisualStyleBackColor = true;
            this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(387, 534);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(41, 31);
            this.BtnSave.TabIndex = 11;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(339, 534);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(41, 31);
            this.BtnLoad.TabIndex = 12;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(564, 534);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(50, 31);
            this.BtnCancel.TabIndex = 13;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(331, 423);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "Ch Color";
            // 
            // TextColor
            // 
            this.TextColor.Location = new System.Drawing.Point(387, 420);
            this.TextColor.Name = "TextColor";
            this.TextColor.Size = new System.Drawing.Size(73, 19);
            this.TextColor.TabIndex = 15;
            this.TextColor.TextChanged += new System.EventHandler(this.TextColor_TextChanged);
            this.TextColor.DoubleClick += new System.EventHandler(this.TextColor_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(358, 449);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "Min";
            // 
            // TextMin
            // 
            this.TextMin.Location = new System.Drawing.Point(387, 446);
            this.TextMin.Name = "TextMin";
            this.TextMin.Size = new System.Drawing.Size(73, 19);
            this.TextMin.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(355, 474);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "Max";
            // 
            // TextMax
            // 
            this.TextMax.Location = new System.Drawing.Point(387, 471);
            this.TextMax.Name = "TextMax";
            this.TextMax.Size = new System.Drawing.Size(73, 19);
            this.TextMax.TabIndex = 19;
            // 
            // CheckPreview
            // 
            this.CheckPreview.AutoSize = true;
            this.CheckPreview.Location = new System.Drawing.Point(81, 501);
            this.CheckPreview.Name = "CheckPreview";
            this.CheckPreview.Size = new System.Drawing.Size(64, 16);
            this.CheckPreview.TabIndex = 20;
            this.CheckPreview.Text = "Preview";
            this.CheckPreview.UseVisualStyleBackColor = true;
            // 
            // BtnNew
            // 
            this.BtnNew.Location = new System.Drawing.Point(10, 534);
            this.BtnNew.Name = "BtnNew";
            this.BtnNew.Size = new System.Drawing.Size(41, 31);
            this.BtnNew.TabIndex = 1;
            this.BtnNew.Text = "Add";
            this.BtnNew.UseVisualStyleBackColor = true;
            this.BtnNew.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // CheckShow
            // 
            this.CheckShow.AutoSize = true;
            this.CheckShow.Location = new System.Drawing.Point(145, 501);
            this.CheckShow.Name = "CheckShow";
            this.CheckShow.Size = new System.Drawing.Size(51, 16);
            this.CheckShow.TabIndex = 21;
            this.CheckShow.Text = "Show";
            this.CheckShow.UseVisualStyleBackColor = true;
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(434, 534);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(41, 31);
            this.BtnClear.TabIndex = 22;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.Button1_Click);
            // 
            // BtnBrowsRule
            // 
            this.BtnBrowsRule.Location = new System.Drawing.Point(310, 471);
            this.BtnBrowsRule.Name = "BtnBrowsRule";
            this.BtnBrowsRule.Size = new System.Drawing.Size(25, 19);
            this.BtnBrowsRule.TabIndex = 23;
            this.BtnBrowsRule.Text = "…";
            this.BtnBrowsRule.UseVisualStyleBackColor = true;
            this.BtnBrowsRule.Click += new System.EventHandler(this.BtnBrowsRule_Click);
            // 
            // BtnPreAna
            // 
            this.BtnPreAna.Location = new System.Drawing.Point(564, 420);
            this.BtnPreAna.Name = "BtnPreAna";
            this.BtnPreAna.Size = new System.Drawing.Size(50, 31);
            this.BtnPreAna.TabIndex = 24;
            this.BtnPreAna.Text = "Ana";
            this.BtnPreAna.UseVisualStyleBackColor = true;
            this.BtnPreAna.Click += new System.EventHandler(this.BtnPreAna_Click);
            // 
            // OpenFileDAT
            // 
            this.OpenFileDAT.FileName = "openFileDialog1";
            this.OpenFileDAT.Filter = "datファイル|*.dat";
            // 
            // BtnUp
            // 
            this.BtnUp.Location = new System.Drawing.Point(152, 534);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(41, 31);
            this.BtnUp.TabIndex = 25;
            this.BtnUp.Text = "Up";
            this.BtnUp.UseVisualStyleBackColor = true;
            this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // BtnDown
            // 
            this.BtnDown.Location = new System.Drawing.Point(199, 534);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(41, 31);
            this.BtnDown.TabIndex = 26;
            this.BtnDown.Text = "Down";
            this.BtnDown.UseVisualStyleBackColor = true;
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // FormDecodeOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 575);
            this.Controls.Add(this.BtnDown);
            this.Controls.Add(this.BtnUp);
            this.Controls.Add(this.BtnPreAna);
            this.Controls.Add(this.BtnBrowsRule);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.CheckShow);
            this.Controls.Add(this.CheckPreview);
            this.Controls.Add(this.TextMax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextMin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnLoad);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnEdit);
            this.Controls.Add(this.TextFormula);
            this.Controls.Add(this.TextCanId);
            this.Controls.Add(this.TextChName);
            this.Controls.Add(this.lblFormula);
            this.Controls.Add(this.lblChName);
            this.Controls.Add(this.lablCanId);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.BtnDel);
            this.Controls.Add(this.BtnNew);
            this.Controls.Add(this.ListViewDecode);
            this.Name = "FormDecodeOption";
            this.Text = "Decode Option";
            this.Load += new System.EventHandler(this.FormDecodeOption_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog OpenFileDDF;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ColumnHeader column1;
        private System.Windows.Forms.ColumnHeader column2;
        private System.Windows.Forms.ColumnHeader column3;
        private System.Windows.Forms.ColumnHeader column4;
        private System.Windows.Forms.ColumnHeader column5;
        private System.Windows.Forms.ColumnHeader column6;
        private System.Windows.Forms.ColumnHeader column7;
        private System.Windows.Forms.ListView ListViewDecode;
        private System.Windows.Forms.Button BtnDel;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Label lablCanId;
        private System.Windows.Forms.Label lblChName;
        private System.Windows.Forms.Label lblFormula;
        private System.Windows.Forms.TextBox TextChName;
        private System.Windows.Forms.TextBox TextCanId;
        private System.Windows.Forms.TextBox TextFormula;
        private System.Windows.Forms.Button BtnEdit;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextMax;
        private System.Windows.Forms.CheckBox CheckPreview;
        private System.Windows.Forms.Button BtnNew;
        private System.Windows.Forms.CheckBox CheckShow;
        private System.Windows.Forms.ColumnHeader column8;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnBrowsRule;
        private System.Windows.Forms.Button BtnPreAna;
        private System.Windows.Forms.OpenFileDialog OpenFileDAT;
        private System.Windows.Forms.Button BtnUp;
        private System.Windows.Forms.Button BtnDown;
    }
}