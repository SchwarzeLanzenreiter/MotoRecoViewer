namespace MotoRecoViewer
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LinkLabelGMap = new System.Windows.Forms.LinkLabel();
            this.LinkLabelSharpDX = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 27);
            this.label1.TabIndex = 3;
            this.label1.Text = "MotoReco Viewer";
            this.label1.Click += new System.EventHandler(this.FormAbout_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(245, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ver1.30";
            this.label2.Click += new System.EventHandler(this.FormAbout_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(473, 252);
            this.label3.TabIndex = 5;
            this.label3.Text = resources.GetString("label3.Text");
            this.label3.Click += new System.EventHandler(this.FormAbout_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 352);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(221, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "This software using these libraries,";
            // 
            // LinkLabelGMap
            // 
            this.LinkLabelGMap.AutoSize = true;
            this.LinkLabelGMap.Location = new System.Drawing.Point(15, 364);
            this.LinkLabelGMap.Name = "LinkLabelGMap";
            this.LinkLabelGMap.Size = new System.Drawing.Size(54, 12);
            this.LinkLabelGMap.TabIndex = 7;
            this.LinkLabelGMap.TabStop = true;
            this.LinkLabelGMap.Text = "GMap.Net";
            this.LinkLabelGMap.Click += new System.EventHandler(this.LinkLabelGMap_Click);
            // 
            // LinkLabelSharpDX
            // 
            this.LinkLabelSharpDX.AutoSize = true;
            this.LinkLabelSharpDX.Location = new System.Drawing.Point(15, 376);
            this.LinkLabelSharpDX.Name = "LinkLabelSharpDX";
            this.LinkLabelSharpDX.Size = new System.Drawing.Size(49, 12);
            this.LinkLabelSharpDX.TabIndex = 8;
            this.LinkLabelSharpDX.TabStop = true;
            this.LinkLabelSharpDX.Text = "SharpDX";
            this.LinkLabelSharpDX.Click += new System.EventHandler(this.LinkLabelSharpDX_Click);
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 395);
            this.Controls.Add(this.LinkLabelSharpDX);
            this.Controls.Add(this.LinkLabelGMap);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormAbout";
            this.Text = "FormAbout";
            this.Click += new System.EventHandler(this.FormAbout_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel LinkLabelGMap;
        private System.Windows.Forms.LinkLabel LinkLabelSharpDX;
    }
}