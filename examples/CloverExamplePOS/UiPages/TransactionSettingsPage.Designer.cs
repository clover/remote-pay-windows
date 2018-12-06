namespace CloverExamplePOS.UiPages
{
    partial class TransactionSettingsPage
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
            this.RegionalExtraParametersLabel = new System.Windows.Forms.Label();
            this.RegionalExtraParametersEditGrid = new System.Windows.Forms.DataGridView();
            this.tipAmount = new CloverExamplePOS.CurrencyTextBox();
            this.labelTipAmount = new System.Windows.Forms.Label();
            this.ContactlessCheckbox = new System.Windows.Forms.CheckBox();
            this.CardNotPresentCheckbox = new System.Windows.Forms.CheckBox();
            this.signatureThreshold = new CloverExamplePOS.CurrencyTextBox();
            this.labelSignatureThreshold = new System.Windows.Forms.Label();
            this.ChipCheckbox = new System.Windows.Forms.CheckBox();
            this.MagStripeCheckbox = new System.Windows.Forms.CheckBox();
            this.ManualEntryCheckbox = new System.Windows.Forms.CheckBox();
            this.label52 = new System.Windows.Forms.Label();
            this.offlineNo = new System.Windows.Forms.RadioButton();
            this.offlineYes = new System.Windows.Forms.RadioButton();
            this.offlineDefault = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.approveOfflineNo = new System.Windows.Forms.RadioButton();
            this.approveOfflineYes = new System.Windows.Forms.RadioButton();
            this.approveOfflineDefault = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.tipModeNone = new System.Windows.Forms.RadioButton();
            this.tipModeOnScreen = new System.Windows.Forms.RadioButton();
            this.tipModeProvided = new System.Windows.Forms.RadioButton();
            this.tipModeDefault = new System.Windows.Forms.RadioButton();
            this.labelTipMode = new System.Windows.Forms.Label();
            this.signatureNone = new System.Windows.Forms.RadioButton();
            this.signatureOnPaper = new System.Windows.Forms.RadioButton();
            this.signatureOnScreen = new System.Windows.Forms.RadioButton();
            this.signatureDefault = new System.Windows.Forms.RadioButton();
            this.label84 = new System.Windows.Forms.Label();
            this.forceOfflineNo = new System.Windows.Forms.RadioButton();
            this.forceOfflineYes = new System.Windows.Forms.RadioButton();
            this.forceOfflineDefault = new System.Windows.Forms.RadioButton();
            this.labelForceOffline = new System.Windows.Forms.Label();
            this.automaticPaymentConfirmationCB = new System.Windows.Forms.CheckBox();
            this.automaticSignatureConfirmationCB = new System.Windows.Forms.CheckBox();
            this.disableDuplicateCheckingCB = new System.Windows.Forms.CheckBox();
            this.disableReceiptOptionsCB = new System.Windows.Forms.CheckBox();
            this.disablePrintingCB = new System.Windows.Forms.CheckBox();
            this.DisableRestartTransactionOnFailure = new System.Windows.Forms.CheckBox();
            this.DisableCashBack = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.RegionalExtraParametersEditGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // RegionalExtraParametersLabel
            // 
            this.RegionalExtraParametersLabel.AutoSize = true;
            this.RegionalExtraParametersLabel.Location = new System.Drawing.Point(725, 9);
            this.RegionalExtraParametersLabel.Name = "RegionalExtraParametersLabel";
            this.RegionalExtraParametersLabel.Size = new System.Drawing.Size(177, 17);
            this.RegionalExtraParametersLabel.TabIndex = 39;
            this.RegionalExtraParametersLabel.Text = "Regional Extra Parameters";
            // 
            // RegionalExtraParametersEditGrid
            // 
            this.RegionalExtraParametersEditGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RegionalExtraParametersEditGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.RegionalExtraParametersEditGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RegionalExtraParametersEditGrid.Location = new System.Drawing.Point(728, 36);
            this.RegionalExtraParametersEditGrid.Name = "RegionalExtraParametersEditGrid";
            this.RegionalExtraParametersEditGrid.RowTemplate.Height = 24;
            this.RegionalExtraParametersEditGrid.Size = new System.Drawing.Size(467, 159);
            this.RegionalExtraParametersEditGrid.TabIndex = 40;
            // 
            // tipAmount
            // 
            this.tipAmount.Location = new System.Drawing.Point(163, 6);
            this.tipAmount.Margin = new System.Windows.Forms.Padding(4);
            this.tipAmount.Name = "tipAmount";
            this.tipAmount.Size = new System.Drawing.Size(191, 22);
            this.tipAmount.TabIndex = 1;
            // 
            // labelTipAmount
            // 
            this.labelTipAmount.AutoSize = true;
            this.labelTipAmount.Location = new System.Drawing.Point(10, 9);
            this.labelTipAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTipAmount.Name = "labelTipAmount";
            this.labelTipAmount.Size = new System.Drawing.Size(88, 17);
            this.labelTipAmount.TabIndex = 0;
            this.labelTipAmount.Text = "Tip Amount: ";
            // 
            // ContactlessCheckbox
            // 
            this.ContactlessCheckbox.AutoSize = true;
            this.ContactlessCheckbox.Checked = true;
            this.ContactlessCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ContactlessCheckbox.Location = new System.Drawing.Point(510, 68);
            this.ContactlessCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.ContactlessCheckbox.Name = "ContactlessCheckbox";
            this.ContactlessCheckbox.Size = new System.Drawing.Size(103, 21);
            this.ContactlessCheckbox.TabIndex = 8;
            this.ContactlessCheckbox.Tag = "";
            this.ContactlessCheckbox.Text = "Contactless";
            this.ContactlessCheckbox.UseVisualStyleBackColor = true;
            // 
            // CardNotPresentCheckbox
            // 
            this.CardNotPresentCheckbox.AutoSize = true;
            this.CardNotPresentCheckbox.Location = new System.Drawing.Point(254, 97);
            this.CardNotPresentCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.CardNotPresentCheckbox.Name = "CardNotPresentCheckbox";
            this.CardNotPresentCheckbox.Size = new System.Drawing.Size(361, 21);
            this.CardNotPresentCheckbox.TabIndex = 9;
            this.CardNotPresentCheckbox.Text = "Card Not Present (only applies to Manual entry type)";
            this.CardNotPresentCheckbox.UseVisualStyleBackColor = true;
            // 
            // signatureThreshold
            // 
            this.signatureThreshold.Location = new System.Drawing.Point(163, 36);
            this.signatureThreshold.Margin = new System.Windows.Forms.Padding(4);
            this.signatureThreshold.Name = "signatureThreshold";
            this.signatureThreshold.Size = new System.Drawing.Size(191, 22);
            this.signatureThreshold.TabIndex = 3;
            // 
            // labelSignatureThreshold
            // 
            this.labelSignatureThreshold.AutoSize = true;
            this.labelSignatureThreshold.Location = new System.Drawing.Point(10, 39);
            this.labelSignatureThreshold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSignatureThreshold.Name = "labelSignatureThreshold";
            this.labelSignatureThreshold.Size = new System.Drawing.Size(145, 17);
            this.labelSignatureThreshold.TabIndex = 2;
            this.labelSignatureThreshold.Text = "Signature Threshold: ";
            // 
            // ChipCheckbox
            // 
            this.ChipCheckbox.AutoSize = true;
            this.ChipCheckbox.Checked = true;
            this.ChipCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChipCheckbox.Location = new System.Drawing.Point(444, 68);
            this.ChipCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.ChipCheckbox.Name = "ChipCheckbox";
            this.ChipCheckbox.Size = new System.Drawing.Size(58, 21);
            this.ChipCheckbox.TabIndex = 7;
            this.ChipCheckbox.Text = "Chip";
            this.ChipCheckbox.UseVisualStyleBackColor = true;
            // 
            // MagStripeCheckbox
            // 
            this.MagStripeCheckbox.AutoSize = true;
            this.MagStripeCheckbox.Checked = true;
            this.MagStripeCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MagStripeCheckbox.Location = new System.Drawing.Point(338, 67);
            this.MagStripeCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.MagStripeCheckbox.Name = "MagStripeCheckbox";
            this.MagStripeCheckbox.Size = new System.Drawing.Size(98, 21);
            this.MagStripeCheckbox.TabIndex = 6;
            this.MagStripeCheckbox.Text = "Mag Stripe";
            this.MagStripeCheckbox.UseVisualStyleBackColor = true;
            // 
            // ManualEntryCheckbox
            // 
            this.ManualEntryCheckbox.AutoSize = true;
            this.ManualEntryCheckbox.Location = new System.Drawing.Point(254, 68);
            this.ManualEntryCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.ManualEntryCheckbox.Name = "ManualEntryCheckbox";
            this.ManualEntryCheckbox.Size = new System.Drawing.Size(76, 21);
            this.ManualEntryCheckbox.TabIndex = 5;
            this.ManualEntryCheckbox.Text = "Manual";
            this.ManualEntryCheckbox.UseVisualStyleBackColor = true;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(10, 68);
            this.label52.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(229, 17);
            this.label52.TabIndex = 4;
            this.label52.Text = "Card Entry Methods (Sale && Auth): ";
            // 
            // offlineNo
            // 
            this.offlineNo.AutoSize = true;
            this.offlineNo.Location = new System.Drawing.Point(147, 1);
            this.offlineNo.Margin = new System.Windows.Forms.Padding(4);
            this.offlineNo.Name = "offlineNo";
            this.offlineNo.Size = new System.Drawing.Size(47, 21);
            this.offlineNo.TabIndex = 13;
            this.offlineNo.Text = "No";
            this.offlineNo.UseVisualStyleBackColor = true;
            // 
            // offlineYes
            // 
            this.offlineYes.AutoSize = true;
            this.offlineYes.Location = new System.Drawing.Point(83, 1);
            this.offlineYes.Margin = new System.Windows.Forms.Padding(4);
            this.offlineYes.Name = "offlineYes";
            this.offlineYes.Size = new System.Drawing.Size(53, 21);
            this.offlineYes.TabIndex = 12;
            this.offlineYes.Text = "Yes";
            this.offlineYes.UseVisualStyleBackColor = true;
            // 
            // offlineDefault
            // 
            this.offlineDefault.AutoSize = true;
            this.offlineDefault.Checked = true;
            this.offlineDefault.Location = new System.Drawing.Point(1, 1);
            this.offlineDefault.Margin = new System.Windows.Forms.Padding(4);
            this.offlineDefault.Name = "offlineDefault";
            this.offlineDefault.Size = new System.Drawing.Size(74, 21);
            this.offlineDefault.TabIndex = 11;
            this.offlineDefault.TabStop = true;
            this.offlineDefault.Text = "Default";
            this.offlineDefault.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 147);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 17);
            this.label13.TabIndex = 10;
            this.label13.Text = "Allow Offline Payment";
            // 
            // approveOfflineNo
            // 
            this.approveOfflineNo.AutoSize = true;
            this.approveOfflineNo.Location = new System.Drawing.Point(147, 1);
            this.approveOfflineNo.Margin = new System.Windows.Forms.Padding(4);
            this.approveOfflineNo.Name = "approveOfflineNo";
            this.approveOfflineNo.Size = new System.Drawing.Size(47, 21);
            this.approveOfflineNo.TabIndex = 17;
            this.approveOfflineNo.Text = "No";
            this.approveOfflineNo.UseVisualStyleBackColor = true;
            // 
            // approveOfflineYes
            // 
            this.approveOfflineYes.AutoSize = true;
            this.approveOfflineYes.Location = new System.Drawing.Point(83, 1);
            this.approveOfflineYes.Margin = new System.Windows.Forms.Padding(4);
            this.approveOfflineYes.Name = "approveOfflineYes";
            this.approveOfflineYes.Size = new System.Drawing.Size(53, 21);
            this.approveOfflineYes.TabIndex = 16;
            this.approveOfflineYes.Text = "Yes";
            this.approveOfflineYes.UseVisualStyleBackColor = true;
            // 
            // approveOfflineDefault
            // 
            this.approveOfflineDefault.AutoSize = true;
            this.approveOfflineDefault.Checked = true;
            this.approveOfflineDefault.Location = new System.Drawing.Point(1, 1);
            this.approveOfflineDefault.Margin = new System.Windows.Forms.Padding(4);
            this.approveOfflineDefault.Name = "approveOfflineDefault";
            this.approveOfflineDefault.Size = new System.Drawing.Size(74, 21);
            this.approveOfflineDefault.TabIndex = 15;
            this.approveOfflineDefault.TabStop = true;
            this.approveOfflineDefault.Text = "Default";
            this.approveOfflineDefault.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 176);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(236, 17);
            this.label14.TabIndex = 14;
            this.label14.Text = "Accept Offline Payment W/O Prompt";
            // 
            // tipModeNone
            // 
            this.tipModeNone.AutoSize = true;
            this.tipModeNone.Location = new System.Drawing.Point(289, 1);
            this.tipModeNone.Margin = new System.Windows.Forms.Padding(4);
            this.tipModeNone.Name = "tipModeNone";
            this.tipModeNone.Size = new System.Drawing.Size(63, 21);
            this.tipModeNone.TabIndex = 22;
            this.tipModeNone.Text = "None";
            this.tipModeNone.UseVisualStyleBackColor = true;
            // 
            // tipModeOnScreen
            // 
            this.tipModeOnScreen.AutoSize = true;
            this.tipModeOnScreen.Location = new System.Drawing.Point(188, 1);
            this.tipModeOnScreen.Margin = new System.Windows.Forms.Padding(4);
            this.tipModeOnScreen.Name = "tipModeOnScreen";
            this.tipModeOnScreen.Size = new System.Drawing.Size(97, 21);
            this.tipModeOnScreen.TabIndex = 21;
            this.tipModeOnScreen.Text = "On Screen";
            this.tipModeOnScreen.UseVisualStyleBackColor = true;
            // 
            // tipModeProvided
            // 
            this.tipModeProvided.AutoSize = true;
            this.tipModeProvided.Location = new System.Drawing.Point(83, 1);
            this.tipModeProvided.Margin = new System.Windows.Forms.Padding(4);
            this.tipModeProvided.Name = "tipModeProvided";
            this.tipModeProvided.Size = new System.Drawing.Size(85, 21);
            this.tipModeProvided.TabIndex = 20;
            this.tipModeProvided.Text = "Provided";
            this.tipModeProvided.UseVisualStyleBackColor = true;
            // 
            // tipModeDefault
            // 
            this.tipModeDefault.AutoSize = true;
            this.tipModeDefault.Checked = true;
            this.tipModeDefault.Location = new System.Drawing.Point(1, 1);
            this.tipModeDefault.Margin = new System.Windows.Forms.Padding(4);
            this.tipModeDefault.Name = "tipModeDefault";
            this.tipModeDefault.Size = new System.Drawing.Size(74, 21);
            this.tipModeDefault.TabIndex = 19;
            this.tipModeDefault.TabStop = true;
            this.tipModeDefault.Text = "Default";
            this.tipModeDefault.UseVisualStyleBackColor = true;
            // 
            // labelTipMode
            // 
            this.labelTipMode.Location = new System.Drawing.Point(10, 228);
            this.labelTipMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTipMode.Name = "labelTipMode";
            this.labelTipMode.Size = new System.Drawing.Size(133, 16);
            this.labelTipMode.TabIndex = 18;
            this.labelTipMode.Text = "Tip Mode";
            // 
            // signatureNone
            // 
            this.signatureNone.AutoSize = true;
            this.signatureNone.Location = new System.Drawing.Point(289, 1);
            this.signatureNone.Margin = new System.Windows.Forms.Padding(4);
            this.signatureNone.Name = "signatureNone";
            this.signatureNone.Size = new System.Drawing.Size(63, 21);
            this.signatureNone.TabIndex = 27;
            this.signatureNone.Text = "None";
            this.signatureNone.UseVisualStyleBackColor = true;
            // 
            // signatureOnPaper
            // 
            this.signatureOnPaper.AutoSize = true;
            this.signatureOnPaper.Location = new System.Drawing.Point(188, 1);
            this.signatureOnPaper.Margin = new System.Windows.Forms.Padding(4);
            this.signatureOnPaper.Name = "signatureOnPaper";
            this.signatureOnPaper.Size = new System.Drawing.Size(90, 21);
            this.signatureOnPaper.TabIndex = 26;
            this.signatureOnPaper.Text = "On Paper";
            this.signatureOnPaper.UseVisualStyleBackColor = true;
            // 
            // signatureOnScreen
            // 
            this.signatureOnScreen.AutoSize = true;
            this.signatureOnScreen.Location = new System.Drawing.Point(83, 1);
            this.signatureOnScreen.Margin = new System.Windows.Forms.Padding(4);
            this.signatureOnScreen.Name = "signatureOnScreen";
            this.signatureOnScreen.Size = new System.Drawing.Size(97, 21);
            this.signatureOnScreen.TabIndex = 25;
            this.signatureOnScreen.TabStop = true;
            this.signatureOnScreen.Text = "On Screen";
            this.signatureOnScreen.UseVisualStyleBackColor = true;
            // 
            // signatureDefault
            // 
            this.signatureDefault.AutoSize = true;
            this.signatureDefault.Checked = true;
            this.signatureDefault.Location = new System.Drawing.Point(1, 1);
            this.signatureDefault.Margin = new System.Windows.Forms.Padding(4);
            this.signatureDefault.Name = "signatureDefault";
            this.signatureDefault.Size = new System.Drawing.Size(74, 21);
            this.signatureDefault.TabIndex = 24;
            this.signatureDefault.TabStop = true;
            this.signatureDefault.Text = "Default";
            this.signatureDefault.UseVisualStyleBackColor = true;
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Location = new System.Drawing.Point(10, 257);
            this.label84.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(127, 17);
            this.label84.TabIndex = 23;
            this.label84.Text = "Signature Location";
            // 
            // forceOfflineNo
            // 
            this.forceOfflineNo.AutoSize = true;
            this.forceOfflineNo.Location = new System.Drawing.Point(147, 1);
            this.forceOfflineNo.Margin = new System.Windows.Forms.Padding(4);
            this.forceOfflineNo.Name = "forceOfflineNo";
            this.forceOfflineNo.Size = new System.Drawing.Size(47, 21);
            this.forceOfflineNo.TabIndex = 31;
            this.forceOfflineNo.Text = "No";
            this.forceOfflineNo.UseVisualStyleBackColor = true;
            // 
            // forceOfflineYes
            // 
            this.forceOfflineYes.AutoSize = true;
            this.forceOfflineYes.Location = new System.Drawing.Point(83, 1);
            this.forceOfflineYes.Margin = new System.Windows.Forms.Padding(4);
            this.forceOfflineYes.Name = "forceOfflineYes";
            this.forceOfflineYes.Size = new System.Drawing.Size(53, 21);
            this.forceOfflineYes.TabIndex = 30;
            this.forceOfflineYes.Text = "Yes";
            this.forceOfflineYes.UseVisualStyleBackColor = true;
            // 
            // forceOfflineDefault
            // 
            this.forceOfflineDefault.AutoSize = true;
            this.forceOfflineDefault.Checked = true;
            this.forceOfflineDefault.Location = new System.Drawing.Point(1, 1);
            this.forceOfflineDefault.Margin = new System.Windows.Forms.Padding(4);
            this.forceOfflineDefault.Name = "forceOfflineDefault";
            this.forceOfflineDefault.Size = new System.Drawing.Size(74, 21);
            this.forceOfflineDefault.TabIndex = 29;
            this.forceOfflineDefault.TabStop = true;
            this.forceOfflineDefault.Text = "Default";
            this.forceOfflineDefault.UseVisualStyleBackColor = true;
            // 
            // labelForceOffline
            // 
            this.labelForceOffline.AutoSize = true;
            this.labelForceOffline.Location = new System.Drawing.Point(10, 284);
            this.labelForceOffline.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelForceOffline.Name = "labelForceOffline";
            this.labelForceOffline.Size = new System.Drawing.Size(148, 17);
            this.labelForceOffline.TabIndex = 28;
            this.labelForceOffline.Text = "Force Offline Payment";
            // 
            // automaticPaymentConfirmationCB
            // 
            this.automaticPaymentConfirmationCB.AutoSize = true;
            this.automaticPaymentConfirmationCB.Location = new System.Drawing.Point(13, 507);
            this.automaticPaymentConfirmationCB.Margin = new System.Windows.Forms.Padding(4);
            this.automaticPaymentConfirmationCB.Name = "automaticPaymentConfirmationCB";
            this.automaticPaymentConfirmationCB.Size = new System.Drawing.Size(293, 21);
            this.automaticPaymentConfirmationCB.TabIndex = 38;
            this.automaticPaymentConfirmationCB.Text = "Automatically Accept Payment Challenges";
            this.automaticPaymentConfirmationCB.UseVisualStyleBackColor = true;
            // 
            // automaticSignatureConfirmationCB
            // 
            this.automaticSignatureConfirmationCB.AutoSize = true;
            this.automaticSignatureConfirmationCB.Location = new System.Drawing.Point(13, 478);
            this.automaticSignatureConfirmationCB.Margin = new System.Windows.Forms.Padding(4);
            this.automaticSignatureConfirmationCB.Name = "automaticSignatureConfirmationCB";
            this.automaticSignatureConfirmationCB.Size = new System.Drawing.Size(225, 21);
            this.automaticSignatureConfirmationCB.TabIndex = 37;
            this.automaticSignatureConfirmationCB.Text = "Automatically Accept Signature";
            this.automaticSignatureConfirmationCB.UseVisualStyleBackColor = true;
            // 
            // disableDuplicateCheckingCB
            // 
            this.disableDuplicateCheckingCB.AutoSize = true;
            this.disableDuplicateCheckingCB.Location = new System.Drawing.Point(13, 449);
            this.disableDuplicateCheckingCB.Margin = new System.Windows.Forms.Padding(4);
            this.disableDuplicateCheckingCB.Name = "disableDuplicateCheckingCB";
            this.disableDuplicateCheckingCB.Size = new System.Drawing.Size(202, 21);
            this.disableDuplicateCheckingCB.TabIndex = 36;
            this.disableDuplicateCheckingCB.Text = "Disable Duplicate Checking";
            this.disableDuplicateCheckingCB.UseVisualStyleBackColor = true;
            // 
            // disableReceiptOptionsCB
            // 
            this.disableReceiptOptionsCB.AutoSize = true;
            this.disableReceiptOptionsCB.Location = new System.Drawing.Point(13, 420);
            this.disableReceiptOptionsCB.Margin = new System.Windows.Forms.Padding(4);
            this.disableReceiptOptionsCB.Name = "disableReceiptOptionsCB";
            this.disableReceiptOptionsCB.Size = new System.Drawing.Size(182, 21);
            this.disableReceiptOptionsCB.TabIndex = 35;
            this.disableReceiptOptionsCB.Text = "Disable Receipt Options";
            this.disableReceiptOptionsCB.UseVisualStyleBackColor = true;
            // 
            // disablePrintingCB
            // 
            this.disablePrintingCB.AutoSize = true;
            this.disablePrintingCB.Location = new System.Drawing.Point(13, 391);
            this.disablePrintingCB.Margin = new System.Windows.Forms.Padding(4);
            this.disablePrintingCB.Name = "disablePrintingCB";
            this.disablePrintingCB.Size = new System.Drawing.Size(129, 21);
            this.disablePrintingCB.TabIndex = 34;
            this.disablePrintingCB.Text = "Disable Printing";
            this.disablePrintingCB.UseVisualStyleBackColor = true;
            // 
            // DisableRestartTransactionOnFailure
            // 
            this.DisableRestartTransactionOnFailure.AutoSize = true;
            this.DisableRestartTransactionOnFailure.Location = new System.Drawing.Point(13, 362);
            this.DisableRestartTransactionOnFailure.Margin = new System.Windows.Forms.Padding(4);
            this.DisableRestartTransactionOnFailure.Name = "DisableRestartTransactionOnFailure";
            this.DisableRestartTransactionOnFailure.Size = new System.Drawing.Size(289, 21);
            this.DisableRestartTransactionOnFailure.TabIndex = 33;
            this.DisableRestartTransactionOnFailure.Text = "Disable Restart of Transaction on Failure";
            this.DisableRestartTransactionOnFailure.UseVisualStyleBackColor = true;
            // 
            // DisableCashBack
            // 
            this.DisableCashBack.AutoSize = true;
            this.DisableCashBack.Location = new System.Drawing.Point(13, 333);
            this.DisableCashBack.Margin = new System.Windows.Forms.Padding(4);
            this.DisableCashBack.Name = "DisableCashBack";
            this.DisableCashBack.Size = new System.Drawing.Size(144, 21);
            this.DisableCashBack.TabIndex = 32;
            this.DisableCashBack.Text = "Disable CashBack";
            this.DisableCashBack.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.offlineDefault);
            this.panel1.Controls.Add(this.offlineYes);
            this.panel1.Controls.Add(this.offlineNo);
            this.panel1.Location = new System.Drawing.Point(253, 144);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 24);
            this.panel1.TabIndex = 41;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.approveOfflineDefault);
            this.panel2.Controls.Add(this.approveOfflineYes);
            this.panel2.Controls.Add(this.approveOfflineNo);
            this.panel2.Location = new System.Drawing.Point(253, 173);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(216, 23);
            this.panel2.TabIndex = 42;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tipModeDefault);
            this.panel3.Controls.Add(this.tipModeProvided);
            this.panel3.Controls.Add(this.tipModeOnScreen);
            this.panel3.Controls.Add(this.tipModeNone);
            this.panel3.Location = new System.Drawing.Point(164, 225);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(383, 23);
            this.panel3.TabIndex = 43;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.signatureDefault);
            this.panel4.Controls.Add(this.signatureOnScreen);
            this.panel4.Controls.Add(this.signatureOnPaper);
            this.panel4.Controls.Add(this.signatureNone);
            this.panel4.Location = new System.Drawing.Point(164, 254);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(383, 24);
            this.panel4.TabIndex = 44;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.forceOfflineDefault);
            this.panel5.Controls.Add(this.forceOfflineYes);
            this.panel5.Controls.Add(this.forceOfflineNo);
            this.panel5.Location = new System.Drawing.Point(164, 281);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(227, 25);
            this.panel5.TabIndex = 45;
            // 
            // TransactionSettingsPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.RegionalExtraParametersLabel);
            this.Controls.Add(this.RegionalExtraParametersEditGrid);
            this.Controls.Add(this.tipAmount);
            this.Controls.Add(this.labelTipAmount);
            this.Controls.Add(this.ContactlessCheckbox);
            this.Controls.Add(this.CardNotPresentCheckbox);
            this.Controls.Add(this.signatureThreshold);
            this.Controls.Add(this.labelSignatureThreshold);
            this.Controls.Add(this.ChipCheckbox);
            this.Controls.Add(this.MagStripeCheckbox);
            this.Controls.Add(this.ManualEntryCheckbox);
            this.Controls.Add(this.label52);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.labelTipMode);
            this.Controls.Add(this.label84);
            this.Controls.Add(this.labelForceOffline);
            this.Controls.Add(this.automaticPaymentConfirmationCB);
            this.Controls.Add(this.automaticSignatureConfirmationCB);
            this.Controls.Add(this.disableDuplicateCheckingCB);
            this.Controls.Add(this.disableReceiptOptionsCB);
            this.Controls.Add(this.disablePrintingCB);
            this.Controls.Add(this.DisableRestartTransactionOnFailure);
            this.Controls.Add(this.DisableCashBack);
            this.MinimumSize = new System.Drawing.Size(1216, 574);
            this.Name = "TransactionSettingsPage";
            this.Size = new System.Drawing.Size(1216, 574);
            ((System.ComponentModel.ISupportInitialize)(this.RegionalExtraParametersEditGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RegionalExtraParametersLabel;
        private System.Windows.Forms.DataGridView RegionalExtraParametersEditGrid;
        private CurrencyTextBox tipAmount;
        private System.Windows.Forms.Label labelTipAmount;
        private System.Windows.Forms.CheckBox ContactlessCheckbox;
        private System.Windows.Forms.CheckBox CardNotPresentCheckbox;
        private CurrencyTextBox signatureThreshold;
        private System.Windows.Forms.Label labelSignatureThreshold;
        private System.Windows.Forms.CheckBox ChipCheckbox;
        private System.Windows.Forms.CheckBox MagStripeCheckbox;
        private System.Windows.Forms.CheckBox ManualEntryCheckbox;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.RadioButton offlineNo;
        private System.Windows.Forms.RadioButton offlineYes;
        private System.Windows.Forms.RadioButton offlineDefault;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton approveOfflineNo;
        private System.Windows.Forms.RadioButton approveOfflineYes;
        private System.Windows.Forms.RadioButton approveOfflineDefault;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton tipModeNone;
        private System.Windows.Forms.RadioButton tipModeOnScreen;
        private System.Windows.Forms.RadioButton tipModeProvided;
        private System.Windows.Forms.RadioButton tipModeDefault;
        private System.Windows.Forms.Label labelTipMode;
        private System.Windows.Forms.RadioButton signatureNone;
        private System.Windows.Forms.RadioButton signatureOnPaper;
        private System.Windows.Forms.RadioButton signatureOnScreen;
        private System.Windows.Forms.RadioButton signatureDefault;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.RadioButton forceOfflineNo;
        private System.Windows.Forms.RadioButton forceOfflineYes;
        private System.Windows.Forms.RadioButton forceOfflineDefault;
        private System.Windows.Forms.Label labelForceOffline;
        private System.Windows.Forms.CheckBox automaticPaymentConfirmationCB;
        private System.Windows.Forms.CheckBox automaticSignatureConfirmationCB;
        private System.Windows.Forms.CheckBox disableDuplicateCheckingCB;
        private System.Windows.Forms.CheckBox disableReceiptOptionsCB;
        private System.Windows.Forms.CheckBox disablePrintingCB;
        private System.Windows.Forms.CheckBox DisableRestartTransactionOnFailure;
        private System.Windows.Forms.CheckBox DisableCashBack;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
    }
}
