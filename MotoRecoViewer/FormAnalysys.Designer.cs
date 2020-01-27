namespace MotoRecoViewer
{
    partial class FormAnalysys
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
            this.LBRemove = new System.Windows.Forms.ListBox();
            this.LBAdd = new System.Windows.Forms.ListBox();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnRemove = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBLow = new System.Windows.Forms.CheckBox();
            this.CBHigh = new System.Windows.Forms.CheckBox();
            this.CBByte = new System.Windows.Forms.CheckBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnAddAll = new System.Windows.Forms.Button();
            this.BtnRemoveAll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LBRemove
            // 
            this.LBRemove.ForeColor = System.Drawing.SystemColors.WindowText;
            this.LBRemove.FormattingEnabled = true;
            this.LBRemove.ItemHeight = 15;
            this.LBRemove.Location = new System.Drawing.Point(12, 51);
            this.LBRemove.Name = "LBRemove";
            this.LBRemove.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.LBRemove.Size = new System.Drawing.Size(86, 319);
            this.LBRemove.Sorted = true;
            this.LBRemove.TabIndex = 0;
            // 
            // LBAdd
            // 
            this.LBAdd.FormattingEnabled = true;
            this.LBAdd.ItemHeight = 15;
            this.LBAdd.Location = new System.Drawing.Point(230, 51);
            this.LBAdd.Name = "LBAdd";
            this.LBAdd.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.LBAdd.Size = new System.Drawing.Size(86, 319);
            this.LBAdd.Sorted = true;
            this.LBAdd.TabIndex = 1;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(136, 149);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(58, 52);
            this.BtnAdd.TabIndex = 2;
            this.BtnAdd.Text = ">";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnRemove
            // 
            this.BtnRemove.Location = new System.Drawing.Point(136, 207);
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(58, 52);
            this.BtnRemove.TabIndex = 3;
            this.BtnRemove.Text = "<";
            this.BtnRemove.UseVisualStyleBackColor = true;
            this.BtnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBLow);
            this.groupBox1.Controls.Add(this.CBHigh);
            this.groupBox1.Controls.Add(this.CBByte);
            this.groupBox1.Location = new System.Drawing.Point(12, 381);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 145);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Option";
            // 
            // CBLow
            // 
            this.CBLow.AutoSize = true;
            this.CBLow.Checked = true;
            this.CBLow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBLow.Location = new System.Drawing.Point(22, 105);
            this.CBLow.Name = "CBLow";
            this.CBLow.Size = new System.Drawing.Size(108, 19);
            this.CBLow.TabIndex = 2;
            this.CBLow.Text = "Lower 4 bits";
            this.CBLow.UseVisualStyleBackColor = true;
            // 
            // CBHigh
            // 
            this.CBHigh.AutoSize = true;
            this.CBHigh.Checked = true;
            this.CBHigh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBHigh.Location = new System.Drawing.Point(22, 67);
            this.CBHigh.Name = "CBHigh";
            this.CBHigh.Size = new System.Drawing.Size(110, 19);
            this.CBHigh.TabIndex = 1;
            this.CBHigh.Text = "Higher 4 bits";
            this.CBHigh.UseVisualStyleBackColor = true;
            // 
            // CBByte
            // 
            this.CBByte.AutoSize = true;
            this.CBByte.Checked = true;
            this.CBByte.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBByte.Location = new System.Drawing.Point(22, 32);
            this.CBByte.Name = "CBByte";
            this.CBByte.Size = new System.Drawing.Size(59, 19);
            this.CBByte.TabIndex = 0;
            this.CBByte.Text = "Byte";
            this.CBByte.UseVisualStyleBackColor = true;
            // 
            // BtnOK
            // 
            this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnOK.Location = new System.Drawing.Point(75, 547);
            this.BtnOK.Margin = new System.Windows.Forms.Padding(4);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(67, 39);
            this.BtnOK.TabIndex = 5;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(185, 547);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(67, 39);
            this.BtnCancel.TabIndex = 14;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnAddAll
            // 
            this.BtnAddAll.Location = new System.Drawing.Point(136, 91);
            this.BtnAddAll.Name = "BtnAddAll";
            this.BtnAddAll.Size = new System.Drawing.Size(58, 52);
            this.BtnAddAll.TabIndex = 15;
            this.BtnAddAll.Text = ">>";
            this.BtnAddAll.UseVisualStyleBackColor = true;
            this.BtnAddAll.Click += new System.EventHandler(this.BtnAddAll_Click);
            // 
            // BtnRemoveAll
            // 
            this.BtnRemoveAll.Location = new System.Drawing.Point(136, 265);
            this.BtnRemoveAll.Name = "BtnRemoveAll";
            this.BtnRemoveAll.Size = new System.Drawing.Size(58, 52);
            this.BtnRemoveAll.TabIndex = 16;
            this.BtnRemoveAll.Text = "<<";
            this.BtnRemoveAll.UseVisualStyleBackColor = true;
            this.BtnRemoveAll.Click += new System.EventHandler(this.BtnRemoveAll_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Detected ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 18;
            this.label2.Text = "Added ID";
            // 
            // FormAnalysys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 606);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnRemoveAll);
            this.Controls.Add(this.BtnAddAll);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnRemove);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.LBAdd);
            this.Controls.Add(this.LBRemove);
            this.Name = "FormAnalysys";
            this.Text = "FormAnalysys";
            this.Load += new System.EventHandler(this.FormAnalysys_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LBRemove;
        private System.Windows.Forms.ListBox LBAdd;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnRemove;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CBLow;
        private System.Windows.Forms.CheckBox CBHigh;
        private System.Windows.Forms.CheckBox CBByte;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnAddAll;
        private System.Windows.Forms.Button BtnRemoveAll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}