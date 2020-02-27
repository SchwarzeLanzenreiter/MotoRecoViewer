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
            this.OpenFileDAT = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnPreAna = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.TextMax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextMin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextColor = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnLoad = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnDown = new System.Windows.Forms.Button();
            this.BtnUp = new System.Windows.Forms.Button();
            this.BtnBrowsRule = new System.Windows.Forms.Button();
            this.CheckShow = new System.Windows.Forms.CheckBox();
            this.CheckPreview = new System.Windows.Forms.CheckBox();
            this.BtnEdit = new System.Windows.Forms.Button();
            this.TextFormula = new System.Windows.Forms.TextBox();
            this.TextCanId = new System.Windows.Forms.TextBox();
            this.TextChName = new System.Windows.Forms.TextBox();
            this.lblFormula = new System.Windows.Forms.Label();
            this.lblChName = new System.Windows.Forms.Label();
            this.lablCanId = new System.Windows.Forms.Label();
            this.BtnDel = new System.Windows.Forms.Button();
            this.BtnNew = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ListViewDecode = new System.Windows.Forms.ListView();
            this.column1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
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
            // OpenFileDAT
            // 
            this.OpenFileDAT.FileName = "openFileDialog1";
            this.OpenFileDAT.Filter = "datファイル|*.dat";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnPreAna);
            this.panel1.Controls.Add(this.BtnClear);
            this.panel1.Controls.Add(this.TextMax);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.TextMin);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.TextColor);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.BtnCancel);
            this.panel1.Controls.Add(this.BtnLoad);
            this.panel1.Controls.Add(this.BtnSave);
            this.panel1.Controls.Add(this.BtnOK);
            this.panel1.Controls.Add(this.BtnDown);
            this.panel1.Controls.Add(this.BtnUp);
            this.panel1.Controls.Add(this.BtnBrowsRule);
            this.panel1.Controls.Add(this.CheckShow);
            this.panel1.Controls.Add(this.CheckPreview);
            this.panel1.Controls.Add(this.BtnEdit);
            this.panel1.Controls.Add(this.TextFormula);
            this.panel1.Controls.Add(this.TextCanId);
            this.panel1.Controls.Add(this.TextChName);
            this.panel1.Controls.Add(this.lblFormula);
            this.panel1.Controls.Add(this.lblChName);
            this.panel1.Controls.Add(this.lablCanId);
            this.panel1.Controls.Add(this.BtnDel);
            this.panel1.Controls.Add(this.BtnNew);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 367);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(629, 153);
            this.panel1.TabIndex = 27;
            // 
            // BtnPreAna
            // 
            this.BtnPreAna.Location = new System.Drawing.Point(572, 7);
            this.BtnPreAna.Name = "BtnPreAna";
            this.BtnPreAna.Size = new System.Drawing.Size(50, 31);
            this.BtnPreAna.TabIndex = 52;
            this.BtnPreAna.Text = "Ana";
            this.BtnPreAna.UseVisualStyleBackColor = true;
            this.BtnPreAna.Click += new System.EventHandler(this.BtnPreAna_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(435, 115);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(41, 31);
            this.BtnClear.TabIndex = 51;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // TextMax
            // 
            this.TextMax.Location = new System.Drawing.Point(387, 58);
            this.TextMax.Name = "TextMax";
            this.TextMax.Size = new System.Drawing.Size(73, 19);
            this.TextMax.TabIndex = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(355, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 12);
            this.label3.TabIndex = 49;
            this.label3.Text = "Max";
            // 
            // TextMin
            // 
            this.TextMin.Location = new System.Drawing.Point(387, 33);
            this.TextMin.Name = "TextMin";
            this.TextMin.Size = new System.Drawing.Size(73, 19);
            this.TextMin.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(358, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 47;
            this.label2.Text = "Min";
            // 
            // TextColor
            // 
            this.TextColor.Location = new System.Drawing.Point(387, 7);
            this.TextColor.Name = "TextColor";
            this.TextColor.Size = new System.Drawing.Size(73, 19);
            this.TextColor.TabIndex = 46;
            this.TextColor.DoubleClick += new System.EventHandler(this.TextColor_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(331, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 12);
            this.label1.TabIndex = 45;
            this.label1.Text = "Ch Color";
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(572, 115);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(50, 31);
            this.BtnCancel.TabIndex = 44;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;

            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(340, 115);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(41, 31);
            this.BtnLoad.TabIndex = 43;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(388, 115);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(41, 31);
            this.BtnSave.TabIndex = 42;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnOK.Location = new System.Drawing.Point(572, 80);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(50, 31);
            this.BtnOK.TabIndex = 41;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnDown
            // 
            this.BtnDown.Location = new System.Drawing.Point(203, 115);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(41, 31);
            this.BtnDown.TabIndex = 40;
            this.BtnDown.Text = "Down";
            this.BtnDown.UseVisualStyleBackColor = true;
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // BtnUp
            // 
            this.BtnUp.Location = new System.Drawing.Point(156, 115);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(41, 31);
            this.BtnUp.TabIndex = 39;
            this.BtnUp.Text = "Up";
            this.BtnUp.UseVisualStyleBackColor = true;
            this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // BtnBrowsRule
            // 
            this.BtnBrowsRule.Location = new System.Drawing.Point(290, 58);
            this.BtnBrowsRule.Name = "BtnBrowsRule";
            this.BtnBrowsRule.Size = new System.Drawing.Size(25, 19);
            this.BtnBrowsRule.TabIndex = 38;
            this.BtnBrowsRule.Text = "…";
            this.BtnBrowsRule.UseVisualStyleBackColor = true;
            this.BtnBrowsRule.Click += new System.EventHandler(this.BtnBrowsRule_Click);
            // 
            // CheckShow
            // 
            this.CheckShow.AutoSize = true;
            this.CheckShow.Location = new System.Drawing.Point(125, 88);
            this.CheckShow.Name = "CheckShow";
            this.CheckShow.Size = new System.Drawing.Size(51, 16);
            this.CheckShow.TabIndex = 37;
            this.CheckShow.Text = "Show";
            this.CheckShow.UseVisualStyleBackColor = true;
            // 
            // CheckPreview
            // 
            this.CheckPreview.AutoSize = true;
            this.CheckPreview.Location = new System.Drawing.Point(61, 88);
            this.CheckPreview.Name = "CheckPreview";
            this.CheckPreview.Size = new System.Drawing.Size(64, 16);
            this.CheckPreview.TabIndex = 36;
            this.CheckPreview.Text = "Preview";
            this.CheckPreview.UseVisualStyleBackColor = true;
            // 
            // BtnEdit
            // 
            this.BtnEdit.Location = new System.Drawing.Point(61, 115);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(41, 31);
            this.BtnEdit.TabIndex = 35;
            this.BtnEdit.Text = "Edit";
            this.BtnEdit.UseVisualStyleBackColor = true;
            this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // TextFormula
            // 
            this.TextFormula.Location = new System.Drawing.Point(61, 58);
            this.TextFormula.Name = "TextFormula";
            this.TextFormula.Size = new System.Drawing.Size(223, 19);
            this.TextFormula.TabIndex = 34;
            // 
            // TextCanId
            // 
            this.TextCanId.Location = new System.Drawing.Point(61, 33);
            this.TextCanId.Name = "TextCanId";
            this.TextCanId.Size = new System.Drawing.Size(42, 19);
            this.TextCanId.TabIndex = 33;
            // 
            // TextChName
            // 
            this.TextChName.Location = new System.Drawing.Point(61, 7);
            this.TextChName.Name = "TextChName";
            this.TextChName.Size = new System.Drawing.Size(223, 19);
            this.TextChName.TabIndex = 32;
            // 
            // lblFormula
            // 
            this.lblFormula.AutoSize = true;
            this.lblFormula.Location = new System.Drawing.Point(9, 61);
            this.lblFormula.Name = "lblFormula";
            this.lblFormula.Size = new System.Drawing.Size(46, 12);
            this.lblFormula.TabIndex = 31;
            this.lblFormula.Text = "Formula";
            // 
            // lblChName
            // 
            this.lblChName.AutoSize = true;
            this.lblChName.Location = new System.Drawing.Point(3, 10);
            this.lblChName.Name = "lblChName";
            this.lblChName.Size = new System.Drawing.Size(52, 12);
            this.lblChName.TabIndex = 30;
            this.lblChName.Text = "Ch Name";
            // 
            // lablCanId
            // 
            this.lablCanId.AutoSize = true;
            this.lablCanId.Location = new System.Drawing.Point(11, 36);
            this.lablCanId.Name = "lablCanId";
            this.lablCanId.Size = new System.Drawing.Size(44, 12);
            this.lablCanId.TabIndex = 29;
            this.lablCanId.Text = "CAN ID";
            // 
            // BtnDel
            // 
            this.BtnDel.Location = new System.Drawing.Point(108, 115);
            this.BtnDel.Name = "BtnDel";
            this.BtnDel.Size = new System.Drawing.Size(41, 31);
            this.BtnDel.TabIndex = 28;
            this.BtnDel.Text = "Del";
            this.BtnDel.UseVisualStyleBackColor = true;
            this.BtnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // BtnNew
            // 
            this.BtnNew.Location = new System.Drawing.Point(14, 115);
            this.BtnNew.Name = "BtnNew";
            this.BtnNew.Size = new System.Drawing.Size(41, 31);
            this.BtnNew.TabIndex = 27;
            this.BtnNew.Text = "Add";
            this.BtnNew.UseVisualStyleBackColor = true;
            this.BtnNew.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ListViewDecode);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(629, 367);
            this.panel2.TabIndex = 28;
            // 
            // ListViewDecode
            // 
            this.ListViewDecode.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.ListViewDecode.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column1,
            this.column2,
            this.column3,
            this.column4,
            this.column5,
            this.column6,
            this.column7,
            this.column8});
            this.ListViewDecode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewDecode.FullRowSelect = true;
            this.ListViewDecode.GridLines = true;
            this.ListViewDecode.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListViewDecode.HideSelection = false;
            this.ListViewDecode.Location = new System.Drawing.Point(0, 0);
            this.ListViewDecode.MultiSelect = false;
            this.ListViewDecode.Name = "ListViewDecode";
            this.ListViewDecode.Size = new System.Drawing.Size(629, 367);
            this.ListViewDecode.TabIndex = 1;
            this.ListViewDecode.UseCompatibleStateImageBehavior = false;
            this.ListViewDecode.View = System.Windows.Forms.View.Details;
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
            // column8
            // 
            this.column8.Text = "Show";
            // 
            // FormDecodeOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 520);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormDecodeOption";
            this.Text = "Decode Option";
            this.Load += new System.EventHandler(this.FormDecodeOption_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog OpenFileDDF;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.OpenFileDialog OpenFileDAT;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnPreAna;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.TextBox TextMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextMin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnDown;
        private System.Windows.Forms.Button BtnUp;
        private System.Windows.Forms.Button BtnBrowsRule;
        private System.Windows.Forms.CheckBox CheckShow;
        private System.Windows.Forms.CheckBox CheckPreview;
        private System.Windows.Forms.Button BtnEdit;
        private System.Windows.Forms.TextBox TextFormula;
        private System.Windows.Forms.TextBox TextCanId;
        private System.Windows.Forms.TextBox TextChName;
        private System.Windows.Forms.Label lblFormula;
        private System.Windows.Forms.Label lblChName;
        private System.Windows.Forms.Label lablCanId;
        private System.Windows.Forms.Button BtnDel;
        private System.Windows.Forms.Button BtnNew;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView ListViewDecode;
        private System.Windows.Forms.ColumnHeader column1;
        private System.Windows.Forms.ColumnHeader column2;
        private System.Windows.Forms.ColumnHeader column3;
        private System.Windows.Forms.ColumnHeader column4;
        private System.Windows.Forms.ColumnHeader column5;
        private System.Windows.Forms.ColumnHeader column6;
        private System.Windows.Forms.ColumnHeader column7;
        private System.Windows.Forms.ColumnHeader column8;
    }
}