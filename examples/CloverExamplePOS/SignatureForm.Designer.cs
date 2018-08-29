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
            this.panel1 = new System.Windows.Forms.Panel();
            this.signaturePanel1 = new CloverExamplePOS.SignaturePanel();
            this.AcceptButton = new System.Windows.Forms.Button();
            this.RejectButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.signaturePanel1);
            this.panel1.Controls.Add(this.AcceptButton);
            this.panel1.Controls.Add(this.RejectButton);
            this.panel1.Location = new System.Drawing.Point(99, 75);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(559, 481);
            this.panel1.TabIndex = 3;
            // 
            // signaturePanel1
            // 
            this.signaturePanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.signaturePanel1.BackColor = System.Drawing.Color.White;
            this.signaturePanel1.Location = new System.Drawing.Point(16, 15);
            this.signaturePanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.signaturePanel1.Name = "signaturePanel1";
            this.signaturePanel1.Signature = null;
            this.signaturePanel1.Size = new System.Drawing.Size(528, 395);
            this.signaturePanel1.TabIndex = 0;
            this.signaturePanel1.Text = "signaturePanel1";
            // 
            // AcceptButton
            // 
            this.AcceptButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.AcceptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AcceptButton.Location = new System.Drawing.Point(295, 430);
            this.AcceptButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AcceptButton.Name = "AcceptButton";
            this.AcceptButton.Size = new System.Drawing.Size(115, 28);
            this.AcceptButton.TabIndex = 2;
            this.AcceptButton.Text = "Accept";
            this.AcceptButton.UseVisualStyleBackColor = false;
            this.AcceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // RejectButton
            // 
            this.RejectButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.RejectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RejectButton.Location = new System.Drawing.Point(417, 430);
            this.RejectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RejectButton.Name = "RejectButton";
            this.RejectButton.Size = new System.Drawing.Size(115, 28);
            this.RejectButton.TabIndex = 1;
            this.RejectButton.Text = "Reject";
            this.RejectButton.UseVisualStyleBackColor = false;
            this.RejectButton.Click += new System.EventHandler(this.RejectButton_Click);
            // 
            // SignatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 668);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SignatureForm";
            this.Text = "SignatureForm";
            this.Load += new System.EventHandler(this.SignatureForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SignaturePanel signaturePanel1;
        private System.Windows.Forms.Button RejectButton;
        private new System.Windows.Forms.Button AcceptButton;
        private System.Windows.Forms.Panel panel1;
    }
}