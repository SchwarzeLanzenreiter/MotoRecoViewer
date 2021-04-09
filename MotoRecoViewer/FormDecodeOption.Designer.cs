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
            this.CheckUseFilter = new System.Windows.Forms.CheckBox();
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
            this.dGVDecodeRule = new System.Windows.Forms.DataGridView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVDecodeRule)).BeginInit();
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
            this.panel1.Controls.Add(this.CheckUseFilter);
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
            this.panel1.Location = new System.Drawing.Point(0, 459);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(904, 191);
            this.panel1.TabIndex = 27;
            // 
            // CheckUseFilter
            // 
            this.CheckUseFilter.AutoSize = true;
            this.CheckUseFilter.Location = new System.Drawing.Point(243, 110);
            this.CheckUseFilter.Margin = new System.Windows.Forms.Padding(4);
            this.CheckUseFilter.Name = "CheckUseFilter";
            this.CheckUseFilter.Size = new System.Drawing.Size(61, 19);
            this.CheckUseFilter.TabIndex = 53;
            this.CheckUseFilter.Text = "Filter";
            this.CheckUseFilter.UseVisualStyleBackColor = true;
            // 
            // BtnPreAna
            // 
            this.BtnPreAna.Location = new System.Drawing.Point(763, 9);
            this.BtnPreAna.Margin = new System.Windows.Forms.Padding(4);
            this.BtnPreAna.Name = "BtnPreAna";
            this.BtnPreAna.Size = new System.Drawing.Size(67, 39);
            this.BtnPreAna.TabIndex = 52;
            this.BtnPreAna.Text = "Ana";
            this.BtnPreAna.UseVisualStyleBackColor = true;
            this.BtnPreAna.Click += new System.EventHandler(this.BtnPreAna_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(580, 144);
            this.BtnClear.Margin = new System.Windows.Forms.Padding(4);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(55, 39);
            this.BtnClear.TabIndex = 51;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // TextMax
            // 
            this.TextMax.Location = new System.Drawing.Point(516, 72);
            this.TextMax.Margin = new System.Windows.Forms.Padding(4);
            this.TextMax.Name = "TextMax";
            this.TextMax.Size = new System.Drawing.Size(96, 22);
            this.TextMax.TabIndex = 50;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(473, 76);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 15);
            this.label3.TabIndex = 49;
            this.label3.Text = "Max";
            // 
            // TextMin
            // 
            this.TextMin.Location = new System.Drawing.Point(516, 41);
            this.TextMin.Margin = new System.Windows.Forms.Padding(4);
            this.TextMin.Name = "TextMin";
            this.TextMin.Size = new System.Drawing.Size(96, 22);
            this.TextMin.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(477, 45);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 47;
            this.label2.Text = "Min";
            // 
            // TextColor
            // 
            this.TextColor.Location = new System.Drawing.Point(516, 9);
            this.TextColor.Margin = new System.Windows.Forms.Padding(4);
            this.TextColor.Name = "TextColor";
            this.TextColor.Size = new System.Drawing.Size(96, 22);
            this.TextColor.TabIndex = 46;
            this.TextColor.DoubleClick += new System.EventHandler(this.TextColor_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(441, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 45;
            this.label1.Text = "Ch Color";
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(763, 144);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(67, 39);
            this.BtnCancel.TabIndex = 44;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(453, 144);
            this.BtnLoad.Margin = new System.Windows.Forms.Padding(4);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(55, 39);
            this.BtnLoad.TabIndex = 43;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(517, 144);
            this.BtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(55, 39);
            this.BtnSave.TabIndex = 42;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnOK.Location = new System.Drawing.Point(763, 100);
            this.BtnOK.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(67, 39);
            this.BtnOK.TabIndex = 41;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnDown
            // 
            this.BtnDown.Location = new System.Drawing.Point(271, 144);
            this.BtnDown.Margin = new System.Windows.Forms.Padding(4);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(55, 39);
            this.BtnDown.TabIndex = 40;
            this.BtnDown.Text = "Down";
            this.BtnDown.UseVisualStyleBackColor = true;
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // BtnUp
            // 
            this.BtnUp.Location = new System.Drawing.Point(208, 144);
            this.BtnUp.Margin = new System.Windows.Forms.Padding(4);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(55, 39);
            this.BtnUp.TabIndex = 39;
            this.BtnUp.Text = "Up";
            this.BtnUp.UseVisualStyleBackColor = true;
            this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // BtnBrowsRule
            // 
            this.BtnBrowsRule.Location = new System.Drawing.Point(387, 72);
            this.BtnBrowsRule.Margin = new System.Windows.Forms.Padding(4);
            this.BtnBrowsRule.Name = "BtnBrowsRule";
            this.BtnBrowsRule.Size = new System.Drawing.Size(33, 24);
            this.BtnBrowsRule.TabIndex = 38;
            this.BtnBrowsRule.Text = "…";
            this.BtnBrowsRule.UseVisualStyleBackColor = true;
            this.BtnBrowsRule.Click += new System.EventHandler(this.BtnBrowsRule_Click);
            // 
            // CheckShow
            // 
            this.CheckShow.AutoSize = true;
            this.CheckShow.Location = new System.Drawing.Point(167, 110);
            this.CheckShow.Margin = new System.Windows.Forms.Padding(4);
            this.CheckShow.Name = "CheckShow";
            this.CheckShow.Size = new System.Drawing.Size(64, 19);
            this.CheckShow.TabIndex = 37;
            this.CheckShow.Text = "Show";
            this.CheckShow.UseVisualStyleBackColor = true;
            // 
            // CheckPreview
            // 
            this.CheckPreview.AutoSize = true;
            this.CheckPreview.Location = new System.Drawing.Point(81, 110);
            this.CheckPreview.Margin = new System.Windows.Forms.Padding(4);
            this.CheckPreview.Name = "CheckPreview";
            this.CheckPreview.Size = new System.Drawing.Size(79, 19);
            this.CheckPreview.TabIndex = 36;
            this.CheckPreview.Text = "Preview";
            this.CheckPreview.UseVisualStyleBackColor = true;
            // 
            // BtnEdit
            // 
            this.BtnEdit.Location = new System.Drawing.Point(81, 144);
            this.BtnEdit.Margin = new System.Windows.Forms.Padding(4);
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(55, 39);
            this.BtnEdit.TabIndex = 35;
            this.BtnEdit.Text = "Edit";
            this.BtnEdit.UseVisualStyleBackColor = true;
            this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // TextFormula
            // 
            this.TextFormula.Location = new System.Drawing.Point(81, 72);
            this.TextFormula.Margin = new System.Windows.Forms.Padding(4);
            this.TextFormula.Name = "TextFormula";
            this.TextFormula.Size = new System.Drawing.Size(296, 22);
            this.TextFormula.TabIndex = 34;
            // 
            // TextCanId
            // 
            this.TextCanId.Location = new System.Drawing.Point(81, 41);
            this.TextCanId.Margin = new System.Windows.Forms.Padding(4);
            this.TextCanId.Name = "TextCanId";
            this.TextCanId.Size = new System.Drawing.Size(55, 22);
            this.TextCanId.TabIndex = 33;
            // 
            // TextChName
            // 
            this.TextChName.Location = new System.Drawing.Point(81, 9);
            this.TextChName.Margin = new System.Windows.Forms.Padding(4);
            this.TextChName.Name = "TextChName";
            this.TextChName.Size = new System.Drawing.Size(296, 22);
            this.TextChName.TabIndex = 32;
            // 
            // lblFormula
            // 
            this.lblFormula.AutoSize = true;
            this.lblFormula.Location = new System.Drawing.Point(12, 76);
            this.lblFormula.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFormula.Name = "lblFormula";
            this.lblFormula.Size = new System.Drawing.Size(57, 15);
            this.lblFormula.TabIndex = 31;
            this.lblFormula.Text = "Formula";
            // 
            // lblChName
            // 
            this.lblChName.AutoSize = true;
            this.lblChName.Location = new System.Drawing.Point(4, 12);
            this.lblChName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChName.Name = "lblChName";
            this.lblChName.Size = new System.Drawing.Size(66, 15);
            this.lblChName.TabIndex = 30;
            this.lblChName.Text = "Ch Name";
            // 
            // lablCanId
            // 
            this.lablCanId.AutoSize = true;
            this.lablCanId.Location = new System.Drawing.Point(15, 45);
            this.lablCanId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lablCanId.Name = "lablCanId";
            this.lablCanId.Size = new System.Drawing.Size(55, 15);
            this.lablCanId.TabIndex = 29;
            this.lablCanId.Text = "CAN ID";
            // 
            // BtnDel
            // 
            this.BtnDel.Location = new System.Drawing.Point(144, 144);
            this.BtnDel.Margin = new System.Windows.Forms.Padding(4);
            this.BtnDel.Name = "BtnDel";
            this.BtnDel.Size = new System.Drawing.Size(55, 39);
            this.BtnDel.TabIndex = 28;
            this.BtnDel.Text = "Del";
            this.BtnDel.UseVisualStyleBackColor = true;
            this.BtnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // BtnNew
            // 
            this.BtnNew.Location = new System.Drawing.Point(19, 144);
            this.BtnNew.Margin = new System.Windows.Forms.Padding(4);
            this.BtnNew.Name = "BtnNew";
            this.BtnNew.Size = new System.Drawing.Size(55, 39);
            this.BtnNew.TabIndex = 27;
            this.BtnNew.Text = "Add";
            this.BtnNew.UseVisualStyleBackColor = true;
            this.BtnNew.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dGVDecodeRule);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(904, 459);
            this.panel2.TabIndex = 28;
            // 
            // dGVDecodeRule
            // 
            this.dGVDecodeRule.AllowUserToAddRows = false;
            this.dGVDecodeRule.AllowUserToDeleteRows = false;
            this.dGVDecodeRule.AllowUserToOrderColumns = true;
            this.dGVDecodeRule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVDecodeRule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9});
            this.dGVDecodeRule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGVDecodeRule.Location = new System.Drawing.Point(0, 0);
            this.dGVDecodeRule.MultiSelect = false;
            this.dGVDecodeRule.Name = "dGVDecodeRule";
            this.dGVDecodeRule.RowHeadersVisible = false;
            this.dGVDecodeRule.RowHeadersWidth = 51;
            this.dGVDecodeRule.RowTemplate.Height = 24;
            this.dGVDecodeRule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVDecodeRule.Size = new System.Drawing.Size(904, 459);
            this.dGVDecodeRule.TabIndex = 0;
            this.dGVDecodeRule.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVDecodeRule_CellValueChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Ch Name";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "CAN ID";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 70;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Formula";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.Width = 125;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Color";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Min";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 60;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Max";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Width = 60;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Preview";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column7.Width = 60;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Show";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column8.Width = 60;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Filter";
            this.Column9.MinimumWidth = 6;
            this.Column9.Name = "Column9";
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column9.Width = 60;
            // 
            // FormDecodeOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 650);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormDecodeOption";
            this.Text = "Decode Option";
            this.Load += new System.EventHandler(this.FormDecodeOption_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVDecodeRule)).EndInit();
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
        private System.Windows.Forms.CheckBox CheckUseFilter;
        public System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.DataGridView dGVDecodeRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column7;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column8;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column9;
    }
}