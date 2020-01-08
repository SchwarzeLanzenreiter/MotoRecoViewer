namespace MotoRecoViewer
{
    partial class FormMapOption
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
            this.label1 = new System.Windows.Forms.Label();
            this.textGoogleAPIKey = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Google Map API Key";
            // 
            // textGoogleAPIKey
            // 
            this.textGoogleAPIKey.Location = new System.Drawing.Point(165, 24);
            this.textGoogleAPIKey.Name = "textGoogleAPIKey";
            this.textGoogleAPIKey.Size = new System.Drawing.Size(382, 22);
            this.textGoogleAPIKey.TabIndex = 2;
            this.textGoogleAPIKey.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // FormMapOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 70);
            this.Controls.Add(this.textGoogleAPIKey);
            this.Controls.Add(this.label1);
            this.Name = "FormMapOption";
            this.Text = "FormMapOption";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMapOption_FormClosed);
            this.Load += new System.EventHandler(this.FormMapOption_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textGoogleAPIKey;
    }
}