namespace CloverExamplePOS.UIDialogs
{
    partial class RefundAmountDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.RefundAmountEdit = new System.Windows.Forms.TextBox();
            this.FullRefundCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(261, 98);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 25);
            this.OkBtn.TabIndex = 0;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(342, 98);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 25);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Refund Amount";
            // 
            // RefundAmountEdit
            // 
            this.RefundAmountEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RefundAmountEdit.Location = new System.Drawing.Point(12, 29);
            this.RefundAmountEdit.Name = "RefundAmountEdit";
            this.RefundAmountEdit.Size = new System.Drawing.Size(405, 22);
            this.RefundAmountEdit.TabIndex = 3;
            // 
            // FullRefundCheck
            // 
            this.FullRefundCheck.AutoSize = true;
            this.FullRefundCheck.Location = new System.Drawing.Point(15, 57);
            this.FullRefundCheck.Name = "FullRefundCheck";
            this.FullRefundCheck.Size = new System.Drawing.Size(149, 21);
            this.FullRefundCheck.TabIndex = 4;
            this.FullRefundCheck.Text = "Refund full amount";
            this.FullRefundCheck.UseVisualStyleBackColor = true;
            this.FullRefundCheck.CheckedChanged += new System.EventHandler(this.FullRefundCheck_CheckedChanged);
            // 
            // RefundAmountDialog
            // 
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(429, 135);
            this.ControlBox = false;
            this.Controls.Add(this.FullRefundCheck);
            this.Controls.Add(this.RefundAmountEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RefundAmountDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Refund Payment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox RefundAmountEdit;
        private System.Windows.Forms.CheckBox FullRefundCheck;
    }
}