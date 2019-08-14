namespace CloverExamplePOS.UIDialogs
{
    partial class ManualCaptureDialog
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
            this.OkBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.ExternalPaymentIdEdit = new System.Windows.Forms.TextBox();
            this.PaymentIdEdit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ExternalPaymentIdCheckbox = new System.Windows.Forms.CheckBox();
            this.LookupButton = new System.Windows.Forms.Button();
            this.AmountEdit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TipAmountEdit = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(196, 213);
            this.OkBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(56, 20);
            this.OkBtn.TabIndex = 0;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(256, 213);
            this.CancelBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(56, 20);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // ExternalPaymentIdEdit
            // 
            this.ExternalPaymentIdEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExternalPaymentIdEdit.Enabled = false;
            this.ExternalPaymentIdEdit.Location = new System.Drawing.Point(6, 26);
            this.ExternalPaymentIdEdit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ExternalPaymentIdEdit.Name = "ExternalPaymentIdEdit";
            this.ExternalPaymentIdEdit.Size = new System.Drawing.Size(245, 20);
            this.ExternalPaymentIdEdit.TabIndex = 3;
            // 
            // PaymentIdEdit
            // 
            this.PaymentIdEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PaymentIdEdit.Location = new System.Drawing.Point(6, 76);
            this.PaymentIdEdit.Margin = new System.Windows.Forms.Padding(2);
            this.PaymentIdEdit.Name = "PaymentIdEdit";
            this.PaymentIdEdit.Size = new System.Drawing.Size(305, 20);
            this.PaymentIdEdit.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Payment ID";
            // 
            // ExternalPaymentIdCheckbox
            // 
            this.ExternalPaymentIdCheckbox.AutoSize = true;
            this.ExternalPaymentIdCheckbox.Location = new System.Drawing.Point(6, 4);
            this.ExternalPaymentIdCheckbox.Name = "ExternalPaymentIdCheckbox";
            this.ExternalPaymentIdCheckbox.Size = new System.Drawing.Size(122, 17);
            this.ExternalPaymentIdCheckbox.TabIndex = 6;
            this.ExternalPaymentIdCheckbox.Text = "External Payment ID";
            this.ExternalPaymentIdCheckbox.UseVisualStyleBackColor = true;
            this.ExternalPaymentIdCheckbox.CheckedChanged += new System.EventHandler(this.ExternalPaymentIdCheckbox_CheckedChanged);
            // 
            // LookupButton
            // 
            this.LookupButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LookupButton.Enabled = false;
            this.LookupButton.Location = new System.Drawing.Point(256, 26);
            this.LookupButton.Margin = new System.Windows.Forms.Padding(2);
            this.LookupButton.Name = "LookupButton";
            this.LookupButton.Size = new System.Drawing.Size(56, 20);
            this.LookupButton.TabIndex = 7;
            this.LookupButton.Text = "Lookup";
            this.LookupButton.UseVisualStyleBackColor = true;
            this.LookupButton.Click += new System.EventHandler(this.LookupButton_Click);
            // 
            // AmountEdit
            // 
            this.AmountEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AmountEdit.Location = new System.Drawing.Point(6, 127);
            this.AmountEdit.Margin = new System.Windows.Forms.Padding(2);
            this.AmountEdit.Name = "AmountEdit";
            this.AmountEdit.Size = new System.Drawing.Size(305, 20);
            this.AmountEdit.TabIndex = 9;
            this.AmountEdit.Text = "2500";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 110);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Amount";
            // 
            // TipAmountEdit
            // 
            this.TipAmountEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TipAmountEdit.Location = new System.Drawing.Point(6, 180);
            this.TipAmountEdit.Margin = new System.Windows.Forms.Padding(2);
            this.TipAmountEdit.Name = "TipAmountEdit";
            this.TipAmountEdit.Size = new System.Drawing.Size(305, 20);
            this.TipAmountEdit.TabIndex = 11;
            this.TipAmountEdit.Text = "500";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 163);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Tip Amount";
            // 
            // ManualCaptureDialog
            // 
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(322, 243);
            this.ControlBox = false;
            this.Controls.Add(this.TipAmountEdit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AmountEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LookupButton);
            this.Controls.Add(this.ExternalPaymentIdCheckbox);
            this.Controls.Add(this.PaymentIdEdit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ExternalPaymentIdEdit);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManualCaptureDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manual Capture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManualCaptureDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TextBox ExternalPaymentIdEdit;
        private System.Windows.Forms.TextBox PaymentIdEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ExternalPaymentIdCheckbox;
        private System.Windows.Forms.Button LookupButton;
        private System.Windows.Forms.TextBox AmountEdit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TipAmountEdit;
        private System.Windows.Forms.Label label3;
    }
}