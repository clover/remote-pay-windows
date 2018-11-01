namespace CloverExamplePOS
{
    partial class LoyaltyApiPage
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
            this.ShowUiButton = new System.Windows.Forms.Button();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.ClearCustomerButton = new System.Windows.Forms.Button();
            this.SetCustomerButton = new System.Windows.Forms.Button();
            this.LoyaltyLog = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ClearLoyaltyLogButton = new System.Windows.Forms.Button();
            this.EndUiButton = new System.Windows.Forms.Button();
            this.PhoneCheck = new System.Windows.Forms.CheckBox();
            this.BarcodeCheck = new System.Windows.Forms.CheckBox();
            this.VasCheck = new System.Windows.Forms.CheckBox();
            this.CustomAccountCheck = new System.Windows.Forms.CheckBox();
            this.SendCustomerProvidedDataButton = new System.Windows.Forms.Button();
            this.SendRegistrationConfigsButton = new System.Windows.Forms.Button();
            this.SendCustomerInfoButton = new System.Windows.Forms.Button();
            this.CustomerNameEdit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ShowUiButton
            // 
            this.ShowUiButton.Location = new System.Drawing.Point(38, 164);
            this.ShowUiButton.Name = "ShowUiButton";
            this.ShowUiButton.Size = new System.Drawing.Size(244, 32);
            this.ShowUiButton.TabIndex = 0;
            this.ShowUiButton.Text = "Show Loyalty Sample UI";
            this.ShowUiButton.UseVisualStyleBackColor = true;
            this.ShowUiButton.Click += new System.EventHandler(this.ShowUiButton_Click);
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(38, 126);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(244, 32);
            this.RegisterButton.TabIndex = 1;
            this.RegisterButton.Text = "Register for Loyalty Messages";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // ClearCustomerButton
            // 
            this.ClearCustomerButton.Location = new System.Drawing.Point(38, 315);
            this.ClearCustomerButton.Name = "ClearCustomerButton";
            this.ClearCustomerButton.Size = new System.Drawing.Size(244, 32);
            this.ClearCustomerButton.TabIndex = 2;
            this.ClearCustomerButton.Text = "Clear Customer";
            this.ClearCustomerButton.UseVisualStyleBackColor = true;
            this.ClearCustomerButton.Click += new System.EventHandler(this.ClearCustomerButton_Click);
            // 
            // SetCustomerButton
            // 
            this.SetCustomerButton.Location = new System.Drawing.Point(38, 277);
            this.SetCustomerButton.Name = "SetCustomerButton";
            this.SetCustomerButton.Size = new System.Drawing.Size(244, 32);
            this.SetCustomerButton.TabIndex = 3;
            this.SetCustomerButton.Text = "Set Customer Info";
            this.SetCustomerButton.UseVisualStyleBackColor = true;
            this.SetCustomerButton.Click += new System.EventHandler(this.SetCustomerButton_Click);
            // 
            // LoyaltyLog
            // 
            this.LoyaltyLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoyaltyLog.BackColor = System.Drawing.SystemColors.Window;
            this.LoyaltyLog.Location = new System.Drawing.Point(326, 35);
            this.LoyaltyLog.Name = "LoyaltyLog";
            this.LoyaltyLog.ReadOnly = true;
            this.LoyaltyLog.Size = new System.Drawing.Size(300, 559);
            this.LoyaltyLog.TabIndex = 4;
            this.LoyaltyLog.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(324, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Loyalty API Message Log";
            // 
            // ClearLoyaltyLogButton
            // 
            this.ClearLoyaltyLogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearLoyaltyLogButton.Location = new System.Drawing.Point(505, 3);
            this.ClearLoyaltyLogButton.Name = "ClearLoyaltyLogButton";
            this.ClearLoyaltyLogButton.Size = new System.Drawing.Size(121, 25);
            this.ClearLoyaltyLogButton.TabIndex = 6;
            this.ClearLoyaltyLogButton.Text = "Clear Log";
            this.ClearLoyaltyLogButton.UseVisualStyleBackColor = true;
            this.ClearLoyaltyLogButton.Click += new System.EventHandler(this.ClearLoyaltyLogButton_Click);
            // 
            // EndUiButton
            // 
            this.EndUiButton.Location = new System.Drawing.Point(38, 380);
            this.EndUiButton.Name = "EndUiButton";
            this.EndUiButton.Size = new System.Drawing.Size(244, 32);
            this.EndUiButton.TabIndex = 7;
            this.EndUiButton.Text = "End Loyalty Sample UI";
            this.EndUiButton.UseVisualStyleBackColor = true;
            this.EndUiButton.Click += new System.EventHandler(this.EndUiButton_Click);
            // 
            // PhoneCheck
            // 
            this.PhoneCheck.AutoSize = true;
            this.PhoneCheck.Checked = true;
            this.PhoneCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PhoneCheck.Location = new System.Drawing.Point(38, 28);
            this.PhoneCheck.Name = "PhoneCheck";
            this.PhoneCheck.Size = new System.Drawing.Size(71, 21);
            this.PhoneCheck.TabIndex = 8;
            this.PhoneCheck.Text = "Phone";
            this.PhoneCheck.UseVisualStyleBackColor = true;
            // 
            // BarcodeCheck
            // 
            this.BarcodeCheck.AutoSize = true;
            this.BarcodeCheck.Checked = true;
            this.BarcodeCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BarcodeCheck.Location = new System.Drawing.Point(38, 55);
            this.BarcodeCheck.Name = "BarcodeCheck";
            this.BarcodeCheck.Size = new System.Drawing.Size(83, 21);
            this.BarcodeCheck.TabIndex = 8;
            this.BarcodeCheck.Text = "Barcode";
            this.BarcodeCheck.UseVisualStyleBackColor = true;
            // 
            // VasCheck
            // 
            this.VasCheck.AutoSize = true;
            this.VasCheck.Location = new System.Drawing.Point(38, 82);
            this.VasCheck.Name = "VasCheck";
            this.VasCheck.Size = new System.Drawing.Size(210, 21);
            this.VasCheck.TabIndex = 9;
            this.VasCheck.Text = "VAS (Apple / Google Phone)";
            this.VasCheck.UseVisualStyleBackColor = true;
            // 
            // CustomAccountCheck
            // 
            this.CustomAccountCheck.AutoSize = true;
            this.CustomAccountCheck.Checked = true;
            this.CustomAccountCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CustomAccountCheck.Location = new System.Drawing.Point(136, 28);
            this.CustomAccountCheck.Name = "CustomAccountCheck";
            this.CustomAccountCheck.Size = new System.Drawing.Size(132, 21);
            this.CustomAccountCheck.TabIndex = 10;
            this.CustomAccountCheck.Text = "Custom Account";
            this.CustomAccountCheck.UseVisualStyleBackColor = true;
            // 
            // SendCustomerProvidedDataButton
            // 
            this.SendCustomerProvidedDataButton.Location = new System.Drawing.Point(38, 486);
            this.SendCustomerProvidedDataButton.Name = "SendCustomerProvidedDataButton";
            this.SendCustomerProvidedDataButton.Size = new System.Drawing.Size(244, 32);
            this.SendCustomerProvidedDataButton.TabIndex = 11;
            this.SendCustomerProvidedDataButton.Text = "Send Customer Provided Data";
            this.SendCustomerProvidedDataButton.UseVisualStyleBackColor = true;
            this.SendCustomerProvidedDataButton.Click += new System.EventHandler(this.SendCustomerProvidedDataButton_Click);
            // 
            // SendRegistrationConfigsButton
            // 
            this.SendRegistrationConfigsButton.Location = new System.Drawing.Point(38, 524);
            this.SendRegistrationConfigsButton.Name = "SendRegistrationConfigsButton";
            this.SendRegistrationConfigsButton.Size = new System.Drawing.Size(244, 32);
            this.SendRegistrationConfigsButton.TabIndex = 11;
            this.SendRegistrationConfigsButton.Text = "Send Registration Configs";
            this.SendRegistrationConfigsButton.UseVisualStyleBackColor = true;
            this.SendRegistrationConfigsButton.Click += new System.EventHandler(this.SendRegistrationConfigsButton_Click);
            // 
            // SendCustomerInfoButton
            // 
            this.SendCustomerInfoButton.Location = new System.Drawing.Point(38, 562);
            this.SendCustomerInfoButton.Name = "SendCustomerInfoButton";
            this.SendCustomerInfoButton.Size = new System.Drawing.Size(244, 32);
            this.SendCustomerInfoButton.TabIndex = 11;
            this.SendCustomerInfoButton.Text = "Send Customer Info";
            this.SendCustomerInfoButton.UseVisualStyleBackColor = true;
            this.SendCustomerInfoButton.Click += new System.EventHandler(this.SendCustomerInfoButton_Click);
            // 
            // CustomerNameEdit
            // 
            this.CustomerNameEdit.Location = new System.Drawing.Point(44, 249);
            this.CustomerNameEdit.Name = "CustomerNameEdit";
            this.CustomerNameEdit.Size = new System.Drawing.Size(224, 22);
            this.CustomerNameEdit.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 229);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Customer Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label3.Location = new System.Drawing.Point(35, 466);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(222, 17);
            this.label3.TabIndex = 14;
            this.label3.Text = "Custom Sample Activity Messages";
            // 
            // LoyaltyApiPage
            // 
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CustomerNameEdit);
            this.Controls.Add(this.SendCustomerInfoButton);
            this.Controls.Add(this.SendRegistrationConfigsButton);
            this.Controls.Add(this.SendCustomerProvidedDataButton);
            this.Controls.Add(this.CustomAccountCheck);
            this.Controls.Add(this.VasCheck);
            this.Controls.Add(this.BarcodeCheck);
            this.Controls.Add(this.PhoneCheck);
            this.Controls.Add(this.EndUiButton);
            this.Controls.Add(this.ClearLoyaltyLogButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoyaltyLog);
            this.Controls.Add(this.SetCustomerButton);
            this.Controls.Add(this.ClearCustomerButton);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.ShowUiButton);
            this.Name = "LoyaltyApiPage";
            this.Size = new System.Drawing.Size(635, 605);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ShowUiButton;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.Button ClearCustomerButton;
        private System.Windows.Forms.Button SetCustomerButton;
        private System.Windows.Forms.RichTextBox LoyaltyLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ClearLoyaltyLogButton;
        private System.Windows.Forms.Button EndUiButton;
        private System.Windows.Forms.CheckBox PhoneCheck;
        private System.Windows.Forms.CheckBox BarcodeCheck;
        private System.Windows.Forms.CheckBox VasCheck;
        private System.Windows.Forms.CheckBox CustomAccountCheck;
        private System.Windows.Forms.Button SendCustomerProvidedDataButton;
        private System.Windows.Forms.Button SendRegistrationConfigsButton;
        private System.Windows.Forms.Button SendCustomerInfoButton;
        private System.Windows.Forms.TextBox CustomerNameEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
