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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
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
            this.button1 = new System.Windows.Forms.Button();
            this.BtnBrowsRule = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "decode definition file|*.ddf";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.Filter = "decode definition file|*.ddf";
            // 
            // column1
            // 
            this.column1.Text = "Ch Name";
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
            this.ListViewDecode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ListViewDecode.MultiSelect = false;
            this.ListViewDecode.Name = "ListViewDecode";
            this.ListViewDecode.Size = new System.Drawing.Size(781, 506);
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
            this.BtnDel.Location = new System.Drawing.Point(141, 626);
            this.BtnDel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnDel.Name = "BtnDel";
            this.BtnDel.Size = new System.Drawing.Size(55, 39);
            this.BtnDel.TabIndex = 2;
            this.BtnDel.Text = "Del";
            this.BtnDel.UseVisualStyleBackColor = true;
            this.BtnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnOK.Location = new System.Drawing.Point(699, 581);
            this.BtnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(67, 39);
            this.BtnOK.TabIndex = 3;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // lablCanId
            // 
            this.lablCanId.AutoSize = true;
            this.lablCanId.Location = new System.Drawing.Point(41, 561);
            this.lablCanId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lablCanId.Name = "lablCanId";
            this.lablCanId.Size = new System.Drawing.Size(55, 15);
            this.lablCanId.TabIndex = 4;
            this.lablCanId.Text = "CAN ID";
            // 
            // lblChName
            // 
            this.lblChName.AutoSize = true;
            this.lblChName.Location = new System.Drawing.Point(31, 529);
            this.lblChName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChName.Name = "lblChName";
            this.lblChName.Size = new System.Drawing.Size(66, 15);
            this.lblChName.TabIndex = 5;
            this.lblChName.Text = "Ch Name";
            // 
            // lblFormula
            // 
            this.lblFormula.AutoSize = true;
            this.lblFormula.Location = new System.Drawing.Point(39, 592);
            this.lblFormula.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFormula.Name = "lblFormula";
            this.lblFormula.Size = new System.Drawing.Size(57, 15);
            this.lblFormula.TabIndex = 6;
            this.lblFormula.Text = "Formula";
            // 
            // TextChName
            // 
            this.TextChName.Location = new System.Drawing.Point(108, 525);
            this.TextChName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextChName.Name = "TextChName";
            this.TextChName.Size = new System.Drawing.Size(296, 22);
            this.TextChName.TabIndex = 7;
            // 
            // TextCanId
            // 
            this.TextCanId.Location = new System.Drawing.Point(108, 558);
            this.TextCanId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextCanId.Name = "TextCanId";
            this.TextCanId.Size = new System.Drawing.Size(55, 22);
            this.TextCanId.TabIndex = 8;
            // 
            // TextFormula
            // 
            this.TextFormula.Location = new System.Drawing.Point(108, 589);
            this.TextFormula.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextFormula.Name = "TextFormula";
            this.TextFormula.Size = new System.Drawing.Size(296, 22);
            this.TextFormula.TabIndex = 9;
            // 
            // BtnEdit
            // 
            this.BtnEdit.Location = new System.Drawing.Point(79, 626);
            this.BtnEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(55, 39);
            this.BtnEdit.TabIndex = 10;
            this.BtnEdit.Text = "Edit";
            this.BtnEdit.UseVisualStyleBackColor = true;
            this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(329, 626);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(55, 39);
            this.BtnSave.TabIndex = 11;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(267, 626);
            this.BtnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(55, 39);
            this.BtnLoad.TabIndex = 12;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(699, 626);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(67, 39);
            this.BtnCancel.TabIndex = 13;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(441, 529);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Ch Color";
            // 
            // TextColor
            // 
            this.TextColor.Location = new System.Drawing.Point(516, 525);
            this.TextColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextColor.Name = "TextColor";
            this.TextColor.Size = new System.Drawing.Size(96, 22);
            this.TextColor.TabIndex = 15;
            this.TextColor.TextChanged += new System.EventHandler(this.TextColor_TextChanged);
            this.TextColor.DoubleClick += new System.EventHandler(this.TextColor_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(477, 561);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "Min";
            // 
            // TextMin
            // 
            this.TextMin.Location = new System.Drawing.Point(516, 558);
            this.TextMin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextMin.Name = "TextMin";
            this.TextMin.Size = new System.Drawing.Size(96, 22);
            this.TextMin.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(473, 592);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 15);
            this.label3.TabIndex = 18;
            this.label3.Text = "Max";
            // 
            // TextMax
            // 
            this.TextMax.Location = new System.Drawing.Point(516, 589);
            this.TextMax.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextMax.Name = "TextMax";
            this.TextMax.Size = new System.Drawing.Size(96, 22);
            this.TextMax.TabIndex = 19;
            // 
            // CheckPreview
            // 
            this.CheckPreview.AutoSize = true;
            this.CheckPreview.Location = new System.Drawing.Point(516, 626);
            this.CheckPreview.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CheckPreview.Name = "CheckPreview";
            this.CheckPreview.Size = new System.Drawing.Size(79, 19);
            this.CheckPreview.TabIndex = 20;
            this.CheckPreview.Text = "Preview";
            this.CheckPreview.UseVisualStyleBackColor = true;
            // 
            // BtnNew
            // 
            this.BtnNew.Location = new System.Drawing.Point(16, 626);
            this.BtnNew.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnNew.Name = "BtnNew";
            this.BtnNew.Size = new System.Drawing.Size(55, 39);
            this.BtnNew.TabIndex = 1;
            this.BtnNew.Text = "Add";
            this.BtnNew.UseVisualStyleBackColor = true;
            this.BtnNew.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // CheckShow
            // 
            this.CheckShow.AutoSize = true;
            this.CheckShow.Location = new System.Drawing.Point(516, 654);
            this.CheckShow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CheckShow.Name = "CheckShow";
            this.CheckShow.Size = new System.Drawing.Size(64, 19);
            this.CheckShow.TabIndex = 21;
            this.CheckShow.Text = "Show";
            this.CheckShow.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(204, 626);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 39);
            this.button1.TabIndex = 22;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // BtnBrowsRule
            // 
            this.BtnBrowsRule.Location = new System.Drawing.Point(413, 589);
            this.BtnBrowsRule.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnBrowsRule.Name = "BtnBrowsRule";
            this.BtnBrowsRule.Size = new System.Drawing.Size(33, 24);
            this.BtnBrowsRule.TabIndex = 23;
            this.BtnBrowsRule.Text = "…";
            this.BtnBrowsRule.UseVisualStyleBackColor = true;
            this.BtnBrowsRule.Click += new System.EventHandler(this.BtnBrowsRule_Click);
            // 
            // FormDecodeOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 678);
            this.Controls.Add(this.BtnBrowsRule);
            this.Controls.Add(this.button1);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormDecodeOption";
            this.Text = "Decode Option";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnBrowsRule;
    }
}