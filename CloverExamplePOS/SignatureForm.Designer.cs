namespace CloverExamplePOS
{
    partial class SignatureForm
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
            this.RejectButton = new System.Windows.Forms.Button();
            this.AcceptButton = new System.Windows.Forms.Button();
            this.signaturePanel1 = new SignaturePanel();
            this.SuspendLayout();
            // 
            // RejectButton
            // 
            this.RejectButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.RejectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RejectButton.Location = new System.Drawing.Point(214, 354);
            this.RejectButton.Name = "RejectButton";
            this.RejectButton.Size = new System.Drawing.Size(86, 23);
            this.RejectButton.TabIndex = 1;
            this.RejectButton.Text = "Reject";
            this.RejectButton.UseVisualStyleBackColor = false;
            this.RejectButton.Click += new System.EventHandler(this.RejectButton_Click);
            // 
            // AcceptButton
            // 
            this.AcceptButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.AcceptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AcceptButton.Location = new System.Drawing.Point(306, 354);
            this.AcceptButton.Name = "AcceptButton";
            this.AcceptButton.Size = new System.Drawing.Size(86, 23);
            this.AcceptButton.TabIndex = 2;
            this.AcceptButton.Text = "Accept";
            this.AcceptButton.UseVisualStyleBackColor = false;
            this.AcceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // signaturePanel1
            // 
            this.signaturePanel1.BackColor = System.Drawing.Color.White;
            this.signaturePanel1.Location = new System.Drawing.Point(13, 13);
            this.signaturePanel1.Name = "signaturePanel1";
            this.signaturePanel1.Signature = null;
            this.signaturePanel1.Size = new System.Drawing.Size(379, 320);
            this.signaturePanel1.TabIndex = 0;
            this.signaturePanel1.Text = "signaturePanel1";
            // 
            // SignatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 389);
            this.Controls.Add(this.AcceptButton);
            this.Controls.Add(this.RejectButton);
            this.Controls.Add(this.signaturePanel1);
            this.Name = "SignatureForm";
            this.Text = "SignatureForm";
            this.Load += new System.EventHandler(this.SignatureForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SignaturePanel signaturePanel1;
        private System.Windows.Forms.Button RejectButton;
        private new System.Windows.Forms.Button AcceptButton;
    }
}