namespace CloverExamplePOS.UiPages
{
    partial class AboutSdkInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SdkInfo = new System.Windows.Forms.TextBox();
            this.DeviceInfo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(446, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Clover Remote Pay Windows SDK";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "SDK Info";
            // 
            // SdkInfo
            // 
            this.SdkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SdkInfo.BackColor = System.Drawing.SystemColors.Window;
            this.SdkInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SdkInfo.Location = new System.Drawing.Point(20, 101);
            this.SdkInfo.Multiline = true;
            this.SdkInfo.Name = "SdkInfo";
            this.SdkInfo.ReadOnly = true;
            this.SdkInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SdkInfo.Size = new System.Drawing.Size(584, 112);
            this.SdkInfo.TabIndex = 2;
            this.SdkInfo.WordWrap = false;
            // 
            // DeviceInfo
            // 
            this.DeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceInfo.BackColor = System.Drawing.SystemColors.Window;
            this.DeviceInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DeviceInfo.Location = new System.Drawing.Point(17, 236);
            this.DeviceInfo.Multiline = true;
            this.DeviceInfo.Name = "DeviceInfo";
            this.DeviceInfo.ReadOnly = true;
            this.DeviceInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DeviceInfo.Size = new System.Drawing.Size(587, 317);
            this.DeviceInfo.TabIndex = 4;
            this.DeviceInfo.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Connected Device Info";
            // 
            // AboutSdkInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DeviceInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SdkInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AboutSdkInfo";
            this.Size = new System.Drawing.Size(712, 576);
            this.Load += new System.EventHandler(this.AboutSdkInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SdkInfo;
        private System.Windows.Forms.TextBox DeviceInfo;
        private System.Windows.Forms.Label label3;
    }
}
