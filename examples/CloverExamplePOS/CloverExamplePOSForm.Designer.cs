using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace CloverExamplePOS
{
    partial class CloverExamplePOSForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CloverExamplePOSForm));
            this.ConnectStatusLabel = new System.Windows.Forms.Label();
            this.DeviceMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.TestDeviceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloverMiniUSBMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WebSocketMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoteRESTServiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.currentOrder = new System.Windows.Forms.Label();
            this.OrderItems = new System.Windows.Forms.ListView();
            this.Quantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Item = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Price = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DiscountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.DiscountLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TotalAmount = new System.Windows.Forms.Label();
            this.TaxAmount = new System.Windows.Forms.Label();
            this.SubTotal = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.AuthButton = new DropDownButton();
            this.SaleButton = new DropDownButton();
            this.newOrderBtn = new System.Windows.Forms.Button();
            this.RegisterTabs = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.StoreItems = new System.Windows.Forms.FlowLayoutPanel();
            this.StoreDiscounts = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.SelectedItemPanel = new System.Windows.Forms.Panel();
            this.DiscountButton = new System.Windows.Forms.Button();
            this.ItemNameLabel = new System.Windows.Forms.Label();
            this.DoneEditingLineItemButton = new System.Windows.Forms.Button();
            this.RemoveItemButton = new System.Windows.Forms.Button();
            this.ItemQuantityTextbox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.IncrementQuantityButton = new System.Windows.Forms.Button();
            this.DecrementQuantityButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelForceOffline = new System.Windows.Forms.TableLayoutPanel();
            this.OrdersListView = new System.Windows.Forms.ListView();
            this.idHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.totalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dateHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.subTotalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.taxHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OpenOrder_Button = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.OrderDetailsListView = new System.Windows.Forms.ListView();
            this.quantityHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.itemHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.itemPriceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OrderPaymentsView = new System.Windows.Forms.ListView();
            this.PayStatusHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.amountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tipPaymentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.paymentTotalHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.externalID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.CloseoutButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.DeviceStatusButton = new DropDownButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.TipAdjustButton = new System.Windows.Forms.Button();
            this.RefundPaymentButton = new System.Windows.Forms.Button();
            this.VoidButton = new System.Windows.Forms.Button();
            this.ShowReceiptButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.TransactionsListView = new System.Windows.Forms.ListView();
            this.TransAmountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TransDateHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TransLast4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.ManualRefundButton = new System.Windows.Forms.Button();
            this.RefundAmount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Cards = new System.Windows.Forms.TabPage();
            this.ratingsListView = new System.Windows.Forms.ListView();
            this.cardsListView = new System.Windows.Forms.ListView();
            this.CardName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.First6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Last4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Exp_Month = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Exp_Year = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Token = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VaultCardBtn = new System.Windows.Forms.Button();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.PreAuthListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PreAuthButton = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new CustomTableLayoutPanel();
            this.tableLayoutPanel18 = new CustomTableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.DisplayMessageTextbox = new System.Windows.Forms.TextBox();
            this.DisplayMessageButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.PrintTextBox = new System.Windows.Forms.TextBox();
            this.PrintTextButton = new DropDownButton();
            this.tableLayoutPanel96 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.BrowseImageButton = new System.Windows.Forms.Button();
            this.PrintURLTextBox = new System.Windows.Forms.TextBox();
            this.PrintImageButton = new DropDownButton();
            this.PrintImage = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.ShowWelcomeButton = new System.Windows.Forms.Button();
            this.ShowThankYouButton = new System.Windows.Forms.Button();
            this.OpenCashDrawerButton = new DropDownButton();
            this.RetrievePrintJobStatusButton = new System.Windows.Forms.Button();
            this.CardDataButton = new System.Windows.Forms.Button();
            this.labelTS = new System.Windows.Forms.Label();
            this.labelTipAmount = new System.Windows.Forms.Label();
            this.tipAmount = new CurrencyTextBox();
            this.labelSignatureThreshold = new System.Windows.Forms.Label();
            this.signatureThreshold = new CurrencyTextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label52 = new System.Windows.Forms.Label();
            this.ManualEntryCheckbox = new System.Windows.Forms.CheckBox();
            this.MagStripeCheckbox = new System.Windows.Forms.CheckBox();
            this.ChipCheckbox = new System.Windows.Forms.CheckBox();
            this.ContactlessCheckbox = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanelCNP = new System.Windows.Forms.FlowLayoutPanel();
            this.labelCNP = new System.Windows.Forms.Label();
            this.CardNotPresentCheckbox = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanelForceOffline = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.labelForceOffline = new System.Windows.Forms.Label();
            this.offlineDefault = new System.Windows.Forms.RadioButton();
            this.offlineYes = new System.Windows.Forms.RadioButton();
            this.offlineNo = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.approveOfflineDefault = new System.Windows.Forms.RadioButton();
            this.approveOfflineYes = new System.Windows.Forms.RadioButton();
            this.approveOfflineNo = new System.Windows.Forms.RadioButton();
            this.forceOfflineDefault = new System.Windows.Forms.RadioButton();
            this.forceOfflineYes = new System.Windows.Forms.RadioButton();
            this.forceOfflineNo = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanelTipMode = new System.Windows.Forms.FlowLayoutPanel();
            this.labelTipMode = new System.Windows.Forms.Label();
            this.tipModeDefault = new System.Windows.Forms.RadioButton();
            this.tipModeProvided = new System.Windows.Forms.RadioButton();
            this.tipModeOnScreen = new System.Windows.Forms.RadioButton();
            this.tipModeNone = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel94 = new System.Windows.Forms.FlowLayoutPanel();
            this.label84 = new System.Windows.Forms.Label();
            this.signatureDefault = new System.Windows.Forms.RadioButton();
            this.signatureOnScreen = new System.Windows.Forms.RadioButton();
            this.signatureOnPaper = new System.Windows.Forms.RadioButton();
            this.signatureNone = new System.Windows.Forms.RadioButton();
            this.DisableCashBack = new System.Windows.Forms.CheckBox();
            this.DisableRestartTransactionOnFailure = new System.Windows.Forms.CheckBox();
            this.disablePrintingCB = new System.Windows.Forms.CheckBox();
            this.disableReceiptOptionsCB = new System.Windows.Forms.CheckBox();
            this.disableDuplicateCheckingCB = new System.Windows.Forms.CheckBox();
            this.automaticSignatureConfirmationCB = new System.Windows.Forms.CheckBox();
            this.automaticPaymentConfirmationCB = new System.Windows.Forms.CheckBox();
            this.RetrievePaymentLabel = new System.Windows.Forms.Label();
            this.RetrievePaymentButton = new System.Windows.Forms.Button();
            this.RetrievePaymentText = new System.Windows.Forms.TextBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.pendingPaymentListView = new System.Windows.Forms.ListView();
            this.paymentIdHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.paymentAmountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.refreshPendingPayments = new System.Windows.Forms.Button();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.SendMessageBtn = new System.Windows.Forms.Button();
            this.StartCustomActivityBtn = new System.Windows.Forms.Button();
            this.nonBlockingCB = new System.Windows.Forms.CheckBox();

            this.customActivityAction = new System.Windows.Forms.ComboBox();
            ComboboxItem[] itemList = new ComboboxItem[6];
            ComboboxItem item0 = new ComboboxItem();
            item0.Text = "BasicExample";
            item0.Value = "com.clover.cfp.examples.BasicExample";
            itemList[0] = item0;
            ComboboxItem item1 = new ComboboxItem();
            item1.Text = "BasicConversationalExample";
            item1.Value = "com.clover.cfp.examples.BasicConversationalExample";
            itemList[1] = item1;
            ComboboxItem item2 = new ComboboxItem();
            item2.Text = "WebViewExample";
            item2.Value = "com.clover.cfp.examples.WebViewExample";
            itemList[2] = item2;
            ComboboxItem item3 = new ComboboxItem();
            item3.Text = "CarouselExample";
            item3.Value = "com.clover.cfp.examples.CarouselExample";
            itemList[3] = item3;
            ComboboxItem item4 = new ComboboxItem();
            item4.Text = "RatingsExample";
            item4.Value = "com.clover.cfp.examples.RatingsExample";
            itemList[4] = item4;
            ComboboxItem item5 = new ComboboxItem();
            item5.Text = "NFCExample";
            item5.Value = "com.clover.cfp.examples.NFCExample";
            itemList[5] = item5;
            this.customActivityAction.Items.AddRange(itemList);

            this.customActivityPayload = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.DeviceCurrentStatus = new System.Windows.Forms.Label();
            this.UIStateButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelTipAmount = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelSignatureThreshold = new System.Windows.Forms.TableLayoutPanel();
            this.tipModeOnPaper = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanelSigLoc = new System.Windows.Forms.FlowLayoutPanel();
            this.labelSigLoc = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.RegisterTabs.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.SelectedItemPanel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.Cards.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.tableLayoutPanel96.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PrintImage)).BeginInit();
            this.flowLayoutPanel5.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanelCNP.SuspendLayout();
            this.flowLayoutPanelForceOffline.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.flowLayoutPanelTipMode.SuspendLayout();
            this.flowLayoutPanel94.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConnectStatusLabel
            // 
            this.ConnectStatusLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ConnectStatusLabel.Location = new System.Drawing.Point(63, 0);
            this.ConnectStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ConnectStatusLabel.Name = "ConnectStatusLabel";
            this.ConnectStatusLabel.Size = new System.Drawing.Size(133, 62);
            this.ConnectStatusLabel.TabIndex = 17;
            this.ConnectStatusLabel.Text = "Not Connected";
            this.ConnectStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DeviceMenu
            // 
            this.DeviceMenu.Name = "DeviceMenu";
            this.DeviceMenu.Size = new System.Drawing.Size(32, 19);
            // 
            // TestDeviceMenuItem
            // 
            this.TestDeviceMenuItem.Name = "TestDeviceMenuItem";
            this.TestDeviceMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // CloverMiniUSBMenuItem
            // 
            this.CloverMiniUSBMenuItem.Name = "CloverMiniUSBMenuItem";
            this.CloverMiniUSBMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // WebSocketMenuItem
            // 
            this.WebSocketMenuItem.Name = "WebSocketMenuItem";
            this.WebSocketMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // RemoteRESTServiceMenuItem
            // 
            this.RemoteRESTServiceMenuItem.Name = "RemoteRESTServiceMenuItem";
            this.RemoteRESTServiceMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.TabControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1212, 734);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // TabControl
            // 
            this.TabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Controls.Add(this.tabPage3);
            this.TabControl.Controls.Add(this.Cards);
            this.TabControl.Controls.Add(this.tabPage7);
            this.TabControl.Controls.Add(this.tabPage4);
            this.TabControl.Controls.Add(this.tabPage8);
            this.TabControl.Controls.Add(this.tabPage9);
            this.TabControl.Controls.Add(this.tabPage10);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 5);
            this.TabControl.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1212, 667);
            this.TabControl.TabIndex = 13;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer3);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1204, 635);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Register";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(4, 4);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tableLayoutPanel9);
            this.splitContainer3.Panel1MinSize = 250;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer3.Panel2.Controls.Add(this.RegisterTabs);
            this.splitContainer3.Size = new System.Drawing.Size(1196, 627);
            this.splitContainer3.SplitterDistance = 335;
            this.splitContainer3.SplitterWidth = 5;
            this.splitContainer3.TabIndex = 19;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 1;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Controls.Add(this.flowLayoutPanel2, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.OrderItems, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel10, 0, 2);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel11, 0, 3);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 4;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(335, 627);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.currentOrder);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(4, 4);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(327, 29);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 20);
            this.label3.TabIndex = 24;
            this.label3.Text = "Current Order :";
            // 
            // currentOrder
            // 
            this.currentOrder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.currentOrder.AutoSize = true;
            this.currentOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentOrder.Location = new System.Drawing.Point(150, 0);
            this.currentOrder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currentOrder.Name = "currentOrder";
            this.currentOrder.Size = new System.Drawing.Size(18, 20);
            this.currentOrder.TabIndex = 25;
            this.currentOrder.Text = "0";
            this.currentOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OrderItems
            // 
            this.OrderItems.AutoArrange = false;
            this.OrderItems.BackColor = System.Drawing.SystemColors.Control;
            this.OrderItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OrderItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Quantity,
            this.Item,
            this.Price,
            this.DiscountHeader});
            this.OrderItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OrderItems.FullRowSelect = true;
            this.OrderItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.OrderItems.Location = new System.Drawing.Point(4, 41);
            this.OrderItems.Margin = new System.Windows.Forms.Padding(4);
            this.OrderItems.MultiSelect = false;
            this.OrderItems.Name = "OrderItems";
            this.OrderItems.Size = new System.Drawing.Size(327, 392);
            this.OrderItems.TabIndex = 16;
            this.OrderItems.UseCompatibleStateImageBehavior = false;
            this.OrderItems.View = System.Windows.Forms.View.Details;
            this.OrderItems.SelectedIndexChanged += new System.EventHandler(this.OrderItems_SelectedIndexChanged);
            this.OrderItems.Click += new System.EventHandler(this.OrderItems_SelectedIndexChanged);
            // 
            // Quantity
            // 
            this.Quantity.Width = 15;
            // 
            // Item
            // 
            this.Item.Text = "Item";
            this.Item.Width = 100;
            // 
            // Price
            // 
            this.Price.Text = "Price";
            this.Price.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.DiscountLabel, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel10.Controls.Add(this.TotalAmount, 1, 3);
            this.tableLayoutPanel10.Controls.Add(this.TaxAmount, 1, 2);
            this.tableLayoutPanel10.Controls.Add(this.SubTotal, 1, 1);
            this.tableLayoutPanel10.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(4, 441);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 4;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(327, 127);
            this.tableLayoutPanel10.TabIndex = 17;
            // 
            // DiscountLabel
            // 
            this.DiscountLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DiscountLabel.ForeColor = System.Drawing.Color.ForestGreen;
            this.DiscountLabel.Location = new System.Drawing.Point(75, 9);
            this.DiscountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DiscountLabel.Name = "DiscountLabel";
            this.DiscountLabel.Size = new System.Drawing.Size(248, 16);
            this.DiscountLabel.TabIndex = 28;
            this.DiscountLabel.Text = "None";
            this.DiscountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 98);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.label1.Size = new System.Drawing.Size(40, 29);
            this.label1.TabIndex = 18;
            this.label1.Text = "Total";
            // 
            // TotalAmount
            // 
            this.TotalAmount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalAmount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TotalAmount.Location = new System.Drawing.Point(75, 81);
            this.TotalAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TotalAmount.Name = "TotalAmount";
            this.TotalAmount.Size = new System.Drawing.Size(248, 46);
            this.TotalAmount.TabIndex = 20;
            this.TotalAmount.Text = "$0.00";
            this.TotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TaxAmount
            // 
            this.TaxAmount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TaxAmount.Location = new System.Drawing.Point(75, 55);
            this.TaxAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TaxAmount.Name = "TaxAmount";
            this.TaxAmount.Size = new System.Drawing.Size(248, 20);
            this.TaxAmount.TabIndex = 21;
            this.TaxAmount.Text = "$0.00";
            this.TaxAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SubTotal
            // 
            this.SubTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SubTotal.Location = new System.Drawing.Point(75, 34);
            this.SubTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SubTotal.Name = "SubTotal";
            this.SubTotal.Size = new System.Drawing.Size(248, 16);
            this.SubTotal.TabIndex = 22;
            this.SubTotal.Text = "$0.00";
            this.SubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 33);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 17);
            this.label6.TabIndex = 23;
            this.label6.Text = "Subtotal";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 8);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 17);
            this.label10.TabIndex = 27;
            this.label10.Text = "Discount";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 17);
            this.label2.TabIndex = 19;
            this.label2.Text = "Tax";
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 3;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel11.Controls.Add(this.AuthButton, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.SaleButton, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.newOrderBtn, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(4, 576);
            this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(327, 47);
            this.tableLayoutPanel11.TabIndex = 18;
            // 
            // AuthButton
            // 
            this.AuthButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.AuthButton.Click = ((System.Collections.Generic.List<System.EventHandler>)(resources.GetObject("AuthButton.Click")));
            this.AuthButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AuthButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AuthButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AuthButton.Location = new System.Drawing.Point(216, 4);
            this.AuthButton.Margin = new System.Windows.Forms.Padding(4);
            this.AuthButton.Name = "AuthButton";
            this.AuthButton.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.AuthButton.Size = new System.Drawing.Size(107, 39);
            this.AuthButton.TabIndex = 27;
            this.AuthButton.Text = "Auth";
            this.AuthButton.UseVisualStyleBackColor = false;
            // 
            // SaleButton
            // 
            this.SaleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.SaleButton.Click = ((System.Collections.Generic.List<System.EventHandler>)(resources.GetObject("SaleButton.Click")));
            this.SaleButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaleButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaleButton.Location = new System.Drawing.Point(102, 4);
            this.SaleButton.Margin = new System.Windows.Forms.Padding(4);
            this.SaleButton.Name = "SaleButton";
            this.SaleButton.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.SaleButton.Size = new System.Drawing.Size(106, 39);
            this.SaleButton.TabIndex = 17;
            this.SaleButton.Text = "Sale";
            this.SaleButton.UseVisualStyleBackColor = false;
            // 
            // newOrderBtn
            // 
            this.newOrderBtn.BackColor = System.Drawing.Color.White;
            this.newOrderBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newOrderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newOrderBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newOrderBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.newOrderBtn.Location = new System.Drawing.Point(4, 4);
            this.newOrderBtn.Margin = new System.Windows.Forms.Padding(4);
            this.newOrderBtn.Name = "newOrderBtn";
            this.newOrderBtn.Size = new System.Drawing.Size(90, 39);
            this.newOrderBtn.TabIndex = 26;
            this.newOrderBtn.Text = "New";
            this.newOrderBtn.UseVisualStyleBackColor = false;
            this.newOrderBtn.Click += new System.EventHandler(this.NewOrder_Click);
            // 
            // RegisterTabs
            // 
            this.RegisterTabs.Controls.Add(this.tabPage5);
            this.RegisterTabs.Controls.Add(this.tabPage6);
            this.RegisterTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RegisterTabs.Location = new System.Drawing.Point(0, 0);
            this.RegisterTabs.Margin = new System.Windows.Forms.Padding(0);
            this.RegisterTabs.Name = "RegisterTabs";
            this.RegisterTabs.SelectedIndex = 0;
            this.RegisterTabs.Size = new System.Drawing.Size(856, 627);
            this.RegisterTabs.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tableLayoutPanel12);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage5.Size = new System.Drawing.Size(848, 598);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 1;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Controls.Add(this.StoreItems, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.StoreDiscounts, 0, 1);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 2;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(840, 590);
            this.tableLayoutPanel12.TabIndex = 0;
            // 
            // StoreItems
            // 
            this.StoreItems.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.StoreItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StoreItems.Location = new System.Drawing.Point(4, 0);
            this.StoreItems.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StoreItems.Name = "StoreItems";
            this.StoreItems.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.StoreItems.Size = new System.Drawing.Size(832, 498);
            this.StoreItems.TabIndex = 7;
            // 
            // StoreDiscounts
            // 
            this.StoreDiscounts.BackColor = System.Drawing.Color.White;
            this.StoreDiscounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StoreDiscounts.Location = new System.Drawing.Point(4, 498);
            this.StoreDiscounts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StoreDiscounts.Name = "StoreDiscounts";
            this.StoreDiscounts.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.StoreDiscounts.Size = new System.Drawing.Size(832, 92);
            this.StoreDiscounts.TabIndex = 18;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.tableLayoutPanel13);
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage6.Size = new System.Drawing.Size(848, 598);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 1;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Controls.Add(this.SelectedItemPanel, 0, 0);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(840, 590);
            this.tableLayoutPanel13.TabIndex = 0;
            // 
            // SelectedItemPanel
            // 
            this.SelectedItemPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SelectedItemPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.SelectedItemPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SelectedItemPanel.Controls.Add(this.DiscountButton);
            this.SelectedItemPanel.Controls.Add(this.ItemNameLabel);
            this.SelectedItemPanel.Controls.Add(this.DoneEditingLineItemButton);
            this.SelectedItemPanel.Controls.Add(this.RemoveItemButton);
            this.SelectedItemPanel.Controls.Add(this.ItemQuantityTextbox);
            this.SelectedItemPanel.Controls.Add(this.label7);
            this.SelectedItemPanel.Controls.Add(this.IncrementQuantityButton);
            this.SelectedItemPanel.Controls.Add(this.DecrementQuantityButton);
            this.SelectedItemPanel.Location = new System.Drawing.Point(79, 24);
            this.SelectedItemPanel.Margin = new System.Windows.Forms.Padding(4);
            this.SelectedItemPanel.Name = "SelectedItemPanel";
            this.SelectedItemPanel.Size = new System.Drawing.Size(681, 541);
            this.SelectedItemPanel.TabIndex = 18;
            // 
            // DiscountButton
            // 
            this.DiscountButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DiscountButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DiscountButton.Location = new System.Drawing.Point(393, 175);
            this.DiscountButton.Margin = new System.Windows.Forms.Padding(4);
            this.DiscountButton.Name = "DiscountButton";
            this.DiscountButton.Size = new System.Drawing.Size(175, 39);
            this.DiscountButton.TabIndex = 7;
            this.DiscountButton.Text = "10% Discount";
            this.DiscountButton.UseVisualStyleBackColor = true;
            this.DiscountButton.Click += new System.EventHandler(this.DiscountButton_Click);
            // 
            // ItemNameLabel
            // 
            this.ItemNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.ItemNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemNameLabel.ForeColor = System.Drawing.Color.Green;
            this.ItemNameLabel.Location = new System.Drawing.Point(0, 0);
            this.ItemNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ItemNameLabel.Name = "ItemNameLabel";
            this.ItemNameLabel.Padding = new System.Windows.Forms.Padding(13, 12, 0, 0);
            this.ItemNameLabel.Size = new System.Drawing.Size(677, 50);
            this.ItemNameLabel.TabIndex = 6;
            this.ItemNameLabel.Text = "Item Name";
            // 
            // DoneEditingLineItemButton
            // 
            this.DoneEditingLineItemButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DoneEditingLineItemButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoneEditingLineItemButton.Location = new System.Drawing.Point(201, 489);
            this.DoneEditingLineItemButton.Margin = new System.Windows.Forms.Padding(4);
            this.DoneEditingLineItemButton.Name = "DoneEditingLineItemButton";
            this.DoneEditingLineItemButton.Size = new System.Drawing.Size(367, 39);
            this.DoneEditingLineItemButton.TabIndex = 5;
            this.DoneEditingLineItemButton.Text = "Done";
            this.DoneEditingLineItemButton.UseVisualStyleBackColor = true;
            this.DoneEditingLineItemButton.Click += new System.EventHandler(this.DoneEditingLineItem_Click);
            // 
            // RemoveItemButton
            // 
            this.RemoveItemButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveItemButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveItemButton.Location = new System.Drawing.Point(201, 175);
            this.RemoveItemButton.Margin = new System.Windows.Forms.Padding(4);
            this.RemoveItemButton.Name = "RemoveItemButton";
            this.RemoveItemButton.Size = new System.Drawing.Size(184, 39);
            this.RemoveItemButton.TabIndex = 4;
            this.RemoveItemButton.Text = "Remove Item";
            this.RemoveItemButton.UseVisualStyleBackColor = true;
            this.RemoveItemButton.Click += new System.EventHandler(this.RemoveItemButton_Click);
            // 
            // ItemQuantityTextbox
            // 
            this.ItemQuantityTextbox.Enabled = false;
            this.ItemQuantityTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemQuantityTextbox.Location = new System.Drawing.Point(359, 105);
            this.ItemQuantityTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.ItemQuantityTextbox.Name = "ItemQuantityTextbox";
            this.ItemQuantityTextbox.Size = new System.Drawing.Size(56, 37);
            this.ItemQuantityTextbox.TabIndex = 3;
            this.ItemQuantityTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(357, 76);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Quantity";
            // 
            // IncrementQuantityButton
            // 
            this.IncrementQuantityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IncrementQuantityButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncrementQuantityButton.Location = new System.Drawing.Point(468, 85);
            this.IncrementQuantityButton.Margin = new System.Windows.Forms.Padding(4);
            this.IncrementQuantityButton.Name = "IncrementQuantityButton";
            this.IncrementQuantityButton.Size = new System.Drawing.Size(100, 58);
            this.IncrementQuantityButton.TabIndex = 1;
            this.IncrementQuantityButton.Text = "+";
            this.IncrementQuantityButton.UseVisualStyleBackColor = true;
            this.IncrementQuantityButton.Click += new System.EventHandler(this.IncrementQuantityButton_Click);
            // 
            // DecrementQuantityButton
            // 
            this.DecrementQuantityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DecrementQuantityButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DecrementQuantityButton.Location = new System.Drawing.Point(201, 85);
            this.DecrementQuantityButton.Margin = new System.Windows.Forms.Padding(4);
            this.DecrementQuantityButton.Name = "DecrementQuantityButton";
            this.DecrementQuantityButton.Size = new System.Drawing.Size(100, 58);
            this.DecrementQuantityButton.TabIndex = 0;
            this.DecrementQuantityButton.Text = "-";
            this.DecrementQuantityButton.UseVisualStyleBackColor = true;
            this.DecrementQuantityButton.Click += new System.EventHandler(this.DecrementQuantityButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1204, 635);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Orders";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1196, 627);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel17);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1188, 539);
            this.splitContainer1.SplitterDistance = 352;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel17
            // 
            this.tableLayoutPanel17.ColumnCount = 1;
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel17.Controls.Add(this.OrdersListView, 0, 0);
            this.tableLayoutPanel17.Controls.Add(this.OpenOrder_Button, 0, 1);
            this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel17.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel17.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel17.Name = "tableLayoutPanel17";
            this.tableLayoutPanel17.RowCount = 2;
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel17.Size = new System.Drawing.Size(1188, 352);
            this.tableLayoutPanel17.TabIndex = 21;
            // 
            // OrdersListView
            // 
            this.OrdersListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OrdersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.idHeader,
            this.totalHeader,
            this.dateHeader,
            this.statusHeader,
            this.subTotalHeader,
            this.taxHeader});
            this.OrdersListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OrdersListView.FullRowSelect = true;
            this.OrdersListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.OrdersListView.Location = new System.Drawing.Point(4, 4);
            this.OrdersListView.Margin = new System.Windows.Forms.Padding(4);
            this.OrdersListView.MultiSelect = false;
            this.OrdersListView.Name = "OrdersListView";
            this.OrdersListView.Size = new System.Drawing.Size(1180, 278);
            this.OrdersListView.TabIndex = 20;
            this.OrdersListView.UseCompatibleStateImageBehavior = false;
            this.OrdersListView.View = System.Windows.Forms.View.Details;
            this.OrdersListView.SelectedIndexChanged += new System.EventHandler(this.OrdersListView_SelectedIndexChanged);
            // 
            // idHeader
            // 
            this.idHeader.Text = "ID";
            // 
            // totalHeader
            // 
            this.totalHeader.Text = "Total";
            this.totalHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.totalHeader.Width = 124;
            // 
            // dateHeader
            // 
            this.dateHeader.Text = "Date";
            this.dateHeader.Width = 148;
            // 
            // statusHeader
            // 
            this.statusHeader.Text = "Status";
            this.statusHeader.Width = 95;
            // 
            // subTotalHeader
            // 
            this.subTotalHeader.Text = "SubTotal";
            this.subTotalHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // taxHeader
            // 
            this.taxHeader.Text = "Tax";
            this.taxHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // OpenOrder_Button
            // 
            this.OpenOrder_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenOrder_Button.BackColor = System.Drawing.Color.White;
            this.OpenOrder_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenOrder_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenOrder_Button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.OpenOrder_Button.Location = new System.Drawing.Point(1083, 290);
            this.OpenOrder_Button.Margin = new System.Windows.Forms.Padding(4);
            this.OpenOrder_Button.Name = "OpenOrder_Button";
            this.OpenOrder_Button.Size = new System.Drawing.Size(101, 58);
            this.OpenOrder_Button.TabIndex = 32;
            this.OpenOrder_Button.Text = "Edit Order";
            this.OpenOrder_Button.UseVisualStyleBackColor = false;
            this.OpenOrder_Button.Click += new System.EventHandler(this.OpenOrder_Button_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.OrderDetailsListView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.OrderPaymentsView);
            this.splitContainer2.Size = new System.Drawing.Size(1188, 182);
            this.splitContainer2.SplitterDistance = 533;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // OrderDetailsListView
            // 
            this.OrderDetailsListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OrderDetailsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.quantityHeader,
            this.itemHeader,
            this.itemPriceHeader});
            this.OrderDetailsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OrderDetailsListView.FullRowSelect = true;
            this.OrderDetailsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.OrderDetailsListView.Location = new System.Drawing.Point(0, 0);
            this.OrderDetailsListView.Margin = new System.Windows.Forms.Padding(4);
            this.OrderDetailsListView.MultiSelect = false;
            this.OrderDetailsListView.Name = "OrderDetailsListView";
            this.OrderDetailsListView.Size = new System.Drawing.Size(533, 182);
            this.OrderDetailsListView.TabIndex = 21;
            this.OrderDetailsListView.UseCompatibleStateImageBehavior = false;
            this.OrderDetailsListView.View = System.Windows.Forms.View.Details;
            // 
            // quantityHeader
            // 
            this.quantityHeader.Text = "Quantity";
            // 
            // itemHeader
            // 
            this.itemHeader.Text = "Item";
            this.itemHeader.Width = 179;
            // 
            // itemPriceHeader
            // 
            this.itemPriceHeader.Text = "Price";
            this.itemPriceHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // OrderPaymentsView
            // 
            this.OrderPaymentsView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OrderPaymentsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PayStatusHeader,
            this.amountHeader,
            this.tipPaymentHeader,
            this.paymentTotalHeader1,
            this.externalID});
            this.OrderPaymentsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OrderPaymentsView.FullRowSelect = true;
            this.OrderPaymentsView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.OrderPaymentsView.Location = new System.Drawing.Point(0, 0);
            this.OrderPaymentsView.Margin = new System.Windows.Forms.Padding(4);
            this.OrderPaymentsView.MultiSelect = false;
            this.OrderPaymentsView.Name = "OrderPaymentsView";
            this.OrderPaymentsView.Size = new System.Drawing.Size(650, 182);
            this.OrderPaymentsView.TabIndex = 23;
            this.OrderPaymentsView.UseCompatibleStateImageBehavior = false;
            this.OrderPaymentsView.View = System.Windows.Forms.View.Details;
            this.OrderPaymentsView.Click += new System.EventHandler(this.OrderPaymentsView_SelectedIndexChanged);
            // 
            // PayStatusHeader
            // 
            this.PayStatusHeader.Text = "Status";
            // 
            // amountHeader
            // 
            this.amountHeader.Text = "Order Amount";
            this.amountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.amountHeader.Width = 150;
            // 
            // tipPaymentHeader
            // 
            this.tipPaymentHeader.Text = "Tip";
            this.tipPaymentHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // paymentTotalHeader1
            // 
            this.paymentTotalHeader1.Text = "Total";
            this.paymentTotalHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // externalID
            // 
            this.externalID.Text = "External ID";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(4, 551);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1188, 72);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.CloseoutButton);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(4, 4);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(586, 64);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // CloseoutButton
            // 
            this.CloseoutButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CloseoutButton.BackColor = System.Drawing.Color.White;
            this.CloseoutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseoutButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseoutButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CloseoutButton.Location = new System.Drawing.Point(4, 4);
            this.CloseoutButton.Margin = new System.Windows.Forms.Padding(4);
            this.CloseoutButton.Name = "CloseoutButton";
            this.CloseoutButton.Size = new System.Drawing.Size(95, 62);
            this.CloseoutButton.TabIndex = 27;
            this.CloseoutButton.Text = "Closeout";
            this.CloseoutButton.UseVisualStyleBackColor = false;
            this.CloseoutButton.Click += new System.EventHandler(this.CloseoutButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ResetButton.BackColor = System.Drawing.Color.White;
            this.ResetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ResetButton.Location = new System.Drawing.Point(107, 4);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(4);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(95, 62);
            this.ResetButton.TabIndex = 27;
            this.ResetButton.Text = "Reset Device";
            this.ResetButton.UseVisualStyleBackColor = false;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
           

            // 
            // DeviceStatusButton
            // 
            this.DeviceStatusButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DeviceStatusButton.BackColor = System.Drawing.Color.White;
            this.DeviceStatusButton.Click = ((System.Collections.Generic.List<System.EventHandler>)(resources.GetObject("DeviceStatusBtn.Click")));
            this.DeviceStatusButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeviceStatusButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceStatusButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.DeviceStatusButton.Location = new System.Drawing.Point(210, 4);
            this.DeviceStatusButton.Margin = new System.Windows.Forms.Padding(4);
            this.DeviceStatusButton.Name = "DeviceStatusButton";
            this.DeviceStatusButton.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.DeviceStatusButton.Size = new System.Drawing.Size(112, 62);
            this.DeviceStatusButton.TabIndex = 28;
            this.DeviceStatusButton.Text = "Device Status";
            this.DeviceStatusButton.UseVisualStyleBackColor = false;

            // 
            // PrintTextButton
            // 
            this.PrintTextButton.BackColor = System.Drawing.Color.White;
            this.PrintTextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrintTextButton.Location = new System.Drawing.Point(483, 72);
            this.PrintTextButton.Margin = new System.Windows.Forms.Padding(4);
            this.PrintTextButton.Name = "PrintTextButton";
            this.PrintTextButton.Size = new System.Drawing.Size(100, 60);
            this.PrintTextButton.TabIndex = 18;
            this.PrintTextButton.Text = "Print";
            this.PrintTextButton.UseVisualStyleBackColor = false;

            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.Controls.Add(this.TipAdjustButton, 2, 0);
            this.tableLayoutPanel7.Controls.Add(this.RefundPaymentButton, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.VoidButton, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.ShowReceiptButton, 3, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(598, 4);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(586, 64);
            this.tableLayoutPanel7.TabIndex = 1;
            // 
            // TipAdjustButton
            // 
            this.TipAdjustButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TipAdjustButton.BackColor = System.Drawing.Color.White;
            this.TipAdjustButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TipAdjustButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TipAdjustButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TipAdjustButton.Location = new System.Drawing.Point(368, 4);
            this.TipAdjustButton.Margin = new System.Windows.Forms.Padding(4);
            this.TipAdjustButton.Name = "TipAdjustButton";
            this.TipAdjustButton.Size = new System.Drawing.Size(105, 56);
            this.TipAdjustButton.TabIndex = 32;
            this.TipAdjustButton.Text = "Tip Adj.";
            this.TipAdjustButton.UseVisualStyleBackColor = false;
            this.TipAdjustButton.Click += new System.EventHandler(this.TipAdjustButton_Click);
            // 
            // RefundPaymentButton
            // 
            this.RefundPaymentButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RefundPaymentButton.BackColor = System.Drawing.Color.White;
            this.RefundPaymentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RefundPaymentButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefundPaymentButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.RefundPaymentButton.Location = new System.Drawing.Point(255, 4);
            this.RefundPaymentButton.Margin = new System.Windows.Forms.Padding(4);
            this.RefundPaymentButton.Name = "RefundPaymentButton";
            this.RefundPaymentButton.Size = new System.Drawing.Size(105, 56);
            this.RefundPaymentButton.TabIndex = 30;
            this.RefundPaymentButton.Text = "Refund";
            this.RefundPaymentButton.UseVisualStyleBackColor = false;
            this.RefundPaymentButton.Click += new System.EventHandler(this.RefundPaymentButton_Click);
            // 
            // VoidButton
            // 
            this.VoidButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.VoidButton.BackColor = System.Drawing.Color.White;
            this.VoidButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VoidButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VoidButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.VoidButton.Location = new System.Drawing.Point(146, 4);
            this.VoidButton.Margin = new System.Windows.Forms.Padding(4);
            this.VoidButton.Name = "VoidButton";
            this.VoidButton.Size = new System.Drawing.Size(101, 56);
            this.VoidButton.TabIndex = 31;
            this.VoidButton.Text = "Void";
            this.VoidButton.UseVisualStyleBackColor = false;
            this.VoidButton.Click += new System.EventHandler(this.VoidButton_Click);
            // 
            // ShowReceiptButton
            // 
            this.ShowReceiptButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ShowReceiptButton.BackColor = System.Drawing.Color.White;
            this.ShowReceiptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowReceiptButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowReceiptButton.Location = new System.Drawing.Point(481, 4);
            this.ShowReceiptButton.Margin = new System.Windows.Forms.Padding(4);
            this.ShowReceiptButton.Name = "ShowReceiptButton";
            this.ShowReceiptButton.Size = new System.Drawing.Size(101, 56);
            this.ShowReceiptButton.TabIndex = 28;
            this.ShowReceiptButton.Text = "Receipt Opt";
            this.ShowReceiptButton.UseVisualStyleBackColor = false;
            this.ShowReceiptButton.Click += new System.EventHandler(this.ShowReceiptButton_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel5);
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage3.Size = new System.Drawing.Size(1204, 635);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Refund";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.TransactionsListView, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1196, 627);
            this.tableLayoutPanel5.TabIndex = 11;
            // 
            // TransactionsListView
            // 
            this.TransactionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TransAmountHeader,
            this.TransDateHeader,
            this.TransLast4});
            this.TransactionsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TransactionsListView.FullRowSelect = true;
            this.TransactionsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.TransactionsListView.Location = new System.Drawing.Point(4, 84);
            this.TransactionsListView.Margin = new System.Windows.Forms.Padding(4);
            this.TransactionsListView.Name = "TransactionsListView";
            this.TransactionsListView.Size = new System.Drawing.Size(1188, 459);
            this.TransactionsListView.TabIndex = 10;
            this.TransactionsListView.UseCompatibleStateImageBehavior = false;
            this.TransactionsListView.View = System.Windows.Forms.View.Details;
            this.TransactionsListView.Click += new System.EventHandler(this.TransactionsListView_SelectedIndexChanged);
            // 
            // TransAmountHeader
            // 
            this.TransAmountHeader.Text = "Amount";
            // 
            // TransDateHeader
            // 
            this.TransDateHeader.Text = "Date";
            this.TransDateHeader.Width = 150;
            // 
            // TransLast4
            // 
            this.TransLast4.Text = "Last 4";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.ManualRefundButton, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.RefundAmount, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1188, 72);
            this.tableLayoutPanel6.TabIndex = 11;
            // 
            // ManualRefundButton
            // 
            this.ManualRefundButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ManualRefundButton.BackColor = System.Drawing.Color.White;
            this.ManualRefundButton.Enabled = false;
            this.ManualRefundButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ManualRefundButton.Location = new System.Drawing.Point(237, 6);
            this.ManualRefundButton.Margin = new System.Windows.Forms.Padding(4);
            this.ManualRefundButton.Name = "ManualRefundButton";
            this.ManualRefundButton.Size = new System.Drawing.Size(88, 60);
            this.ManualRefundButton.TabIndex = 16;
            this.ManualRefundButton.Text = "Refund";
            this.ManualRefundButton.UseVisualStyleBackColor = false;
            this.ManualRefundButton.Click += new System.EventHandler(this.ManualRefundButton_Click);
            // 
            // RefundAmount
            // 
            this.RefundAmount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RefundAmount.Location = new System.Drawing.Point(118, 25);
            this.RefundAmount.Margin = new System.Windows.Forms.Padding(4);
            this.RefundAmount.Name = "RefundAmount";
            this.RefundAmount.Size = new System.Drawing.Size(111, 22);
            this.RefundAmount.TabIndex = 15;
            this.RefundAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RefundAmount_KeyPress);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 27);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Refund Amount";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Cards
            // 
            this.Cards.Controls.Add(this.cardsListView);
            this.Cards.Controls.Add(this.VaultCardBtn);
            this.Cards.Location = new System.Drawing.Point(4, 28);
            this.Cards.Margin = new System.Windows.Forms.Padding(4);
            this.Cards.Name = "Cards";
            this.Cards.Size = new System.Drawing.Size(1204, 635);
            this.Cards.TabIndex = 4;
            this.Cards.Text = "Cards";
            this.Cards.UseVisualStyleBackColor = true;
            // 
            // cardsListView
            // 
            this.cardsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cardsListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CardName,
            this.First6,
            this.Last4,
            this.Exp_Month,
            this.Exp_Year,
            this.Token});
            this.cardsListView.FullRowSelect = true;
            this.cardsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.cardsListView.Location = new System.Drawing.Point(12, 5);
            this.cardsListView.Margin = new System.Windows.Forms.Padding(4);
            this.cardsListView.Name = "cardsListView";
            this.cardsListView.Size = new System.Drawing.Size(1178, 555);
            this.cardsListView.TabIndex = 37;
            this.cardsListView.UseCompatibleStateImageBehavior = false;
            this.cardsListView.View = System.Windows.Forms.View.Details;
            // 
            // CardName
            // 
            this.CardName.Text = "Name";
            this.CardName.Width = 135;
            // 
            // First6
            // 
            this.First6.Text = "First6";
            this.First6.Width = 84;
            // 
            // Last4
            // 
            this.Last4.Text = "Last4";
            // 
            // Exp_Month
            // 
            this.Exp_Month.Text = "Exp. Month";
            // 
            // Exp_Year
            // 
            this.Exp_Year.Text = "Exp. Year";
            // 
            // Token
            // 
            this.Token.Text = "Token";
            this.Token.Width = 147;
            // 
            // VaultCardBtn
            // 
            this.VaultCardBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VaultCardBtn.BackColor = System.Drawing.Color.White;
            this.VaultCardBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VaultCardBtn.Location = new System.Drawing.Point(1091, 567);
            this.VaultCardBtn.Margin = new System.Windows.Forms.Padding(4);
            this.VaultCardBtn.Name = "VaultCardBtn";
            this.VaultCardBtn.Size = new System.Drawing.Size(100, 60);
            this.VaultCardBtn.TabIndex = 36;
            this.VaultCardBtn.Text = "Vault Card";
            this.VaultCardBtn.UseVisualStyleBackColor = false;
            this.VaultCardBtn.Click += new System.EventHandler(this.VaultCardBtn_Click);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.PreAuthListView);
            this.tabPage7.Controls.Add(this.PreAuthButton);
            this.tabPage7.Location = new System.Drawing.Point(4, 28);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage7.Size = new System.Drawing.Size(1204, 635);
            this.tabPage7.TabIndex = 5;
            this.tabPage7.Text = "Pre-Auths";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // PreAuthListView
            // 
            this.PreAuthListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PreAuthListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.PreAuthListView.FullRowSelect = true;
            this.PreAuthListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.PreAuthListView.Location = new System.Drawing.Point(8, 7);
            this.PreAuthListView.Margin = new System.Windows.Forms.Padding(4);
            this.PreAuthListView.MultiSelect = false;
            this.PreAuthListView.Name = "PreAuthListView";
            this.PreAuthListView.Size = new System.Drawing.Size(1182, 548);
            this.PreAuthListView.TabIndex = 38;
            this.PreAuthListView.UseCompatibleStateImageBehavior = false;
            this.PreAuthListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Status";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Authorization Amount";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 150;
            // 
            // PreAuthButton
            // 
            this.PreAuthButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PreAuthButton.BackColor = System.Drawing.Color.White;
            this.PreAuthButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PreAuthButton.Location = new System.Drawing.Point(1091, 564);
            this.PreAuthButton.Margin = new System.Windows.Forms.Padding(4);
            this.PreAuthButton.Name = "PreAuthButton";
            this.PreAuthButton.Size = new System.Drawing.Size(100, 60);
            this.PreAuthButton.TabIndex = 37;
            this.PreAuthButton.Text = "Pre-Auth Card";
            this.PreAuthButton.UseVisualStyleBackColor = false;
            this.PreAuthButton.Click += new System.EventHandler(this.PreAuthButton_Click);
            // 
            // tabPage4
            // 
            //this.tabPage4.AutoScroll = true;
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Location = new System.Drawing.Point(4, 28);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage4.Size = new System.Drawing.Size(1204, 635);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Miscellaneous";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            //this.tableLayoutPanel8.AutoScroll = true;
            //this.tableLayoutPanel8.AutoScrollMargin = new System.Drawing.Size(895, 500);
            this.tableLayoutPanel8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel8.ColumnCount = 6;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.DisplayMessageTextbox, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.DisplayMessageButton, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.PrintTextBox, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.PrintTextButton, 2, 1);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel96, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel16, 1, 2);
            this.tableLayoutPanel8.Controls.Add(this.PrintImageButton, 2, 2);
            this.tableLayoutPanel8.Controls.Add(this.PrintImage, 3, 2);
            this.tableLayoutPanel8.Controls.Add(this.flowLayoutPanel5, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.RetrievePaymentLabel, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.RetrievePaymentButton, 2, 3);
            this.tableLayoutPanel8.Controls.Add(this.RetrievePaymentText, 1, 3);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 21;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.Size = new System.Drawing.Size(1112, 892);
            this.tableLayoutPanel8.TabIndex = 0;




            // 
            // tabPage10
            // 
            //this.tabPage10.AutoScroll = true;
            this.tabPage10.Controls.Add(this.tableLayoutPanel18);
            this.tabPage10.Location = new System.Drawing.Point(4, 37);
            this.tabPage10.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tabPage10.Size = new System.Drawing.Size(1810, 1001);
            this.tabPage10.TabIndex = 3;
            this.tabPage10.Text = "Transaction Overrides";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel18
            // 
            //this.tableLayoutPanel18.AutoScroll = true;
            //this.tableLayoutPanel18.AutoScrollMargin = new System.Drawing.Size(895, 500);
            this.tableLayoutPanel18.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel18.ColumnCount = 6;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel18.Controls.Add(this.labelTipAmount, 0, 6);
            this.tableLayoutPanel18.Controls.Add(this.tipAmount, 1, 6);
            this.tableLayoutPanel18.Controls.Add(this.labelSignatureThreshold, 0, 7);
            this.tableLayoutPanel18.Controls.Add(this.signatureThreshold, 1, 7);
            this.tableLayoutPanel18.Controls.Add(this.flowLayoutPanel1, 0, 8);
            this.tableLayoutPanel18.Controls.Add(this.flowLayoutPanelCNP, 0, 9);
            this.tableLayoutPanel18.Controls.Add(this.flowLayoutPanel6, 0, 10);
            this.tableLayoutPanel18.Controls.Add(this.flowLayoutPanel4, 0, 11);
            this.tableLayoutPanel18.Controls.Add(this.flowLayoutPanelTipMode, 0, 12);
            this.tableLayoutPanel18.Controls.Add(this.flowLayoutPanel94, 0, 13);
            this.tableLayoutPanel18.Controls.Add(this.flowLayoutPanelForceOffline, 0, 14);
            this.tableLayoutPanel18.Controls.Add(this.DisableCashBack, 0, 15);
            this.tableLayoutPanel18.Controls.Add(this.DisableRestartTransactionOnFailure, 0, 16);
            this.tableLayoutPanel18.Controls.Add(this.disablePrintingCB, 0, 17);
            this.tableLayoutPanel18.Controls.Add(this.disableReceiptOptionsCB, 0, 18);
            this.tableLayoutPanel18.Controls.Add(this.disableDuplicateCheckingCB, 0, 19);
            this.tableLayoutPanel18.Controls.Add(this.automaticSignatureConfirmationCB, 0, 20);
            this.tableLayoutPanel18.Controls.Add(this.automaticPaymentConfirmationCB, 0, 21);
            this.tableLayoutPanel18.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel18.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 15;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel18.Size = new System.Drawing.Size(1112, 892);
            this.tableLayoutPanel18.TabIndex = 0;

            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 25);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 17);
            this.label9.TabIndex = 23;
            this.label9.Text = "Display Message: ";
            // 
            // DisplayMessageTextbox
            // 
            this.DisplayMessageTextbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DisplayMessageTextbox.Location = new System.Drawing.Point(339, 23);
            this.DisplayMessageTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.DisplayMessageTextbox.Name = "DisplayMessageTextbox";
            this.DisplayMessageTextbox.Size = new System.Drawing.Size(132, 22);
            this.DisplayMessageTextbox.TabIndex = 22;
            // 
            // DisplayMessageButton
            // 
            this.DisplayMessageButton.BackColor = System.Drawing.Color.White;
            this.DisplayMessageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DisplayMessageButton.Location = new System.Drawing.Point(483, 4);
            this.DisplayMessageButton.Margin = new System.Windows.Forms.Padding(4);
            this.DisplayMessageButton.Name = "DisplayMessageButton";
            this.DisplayMessageButton.Size = new System.Drawing.Size(100, 60);
            this.DisplayMessageButton.TabIndex = 21;
            this.DisplayMessageButton.Text = "Display";
            this.DisplayMessageButton.UseVisualStyleBackColor = false;
            this.DisplayMessageButton.Click += new System.EventHandler(this.DisplayMessageButton_Click);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 93);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 17);
            this.label8.TabIndex = 20;
            this.label8.Text = "Print Message: ";
            // 
            // PrintTextBox
            // 
            this.PrintTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PrintTextBox.Location = new System.Drawing.Point(339, 91);
            this.PrintTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PrintTextBox.Name = "PrintTextBox";
            this.PrintTextBox.Size = new System.Drawing.Size(132, 22);
            this.PrintTextBox.TabIndex = 19;
            
            //new System.EventHandler(this.PrintTextButton_Click);
            // 
            // tableLayoutPanel96
            // 
            this.tableLayoutPanel96.ColumnCount = 1;
            this.tableLayoutPanel96.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel96.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel96.Controls.Add(this.label12, 0, 1);
            this.tableLayoutPanel96.Location = new System.Drawing.Point(0, 136);
            this.tableLayoutPanel96.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel96.Name = "tableLayoutPanel96";
            this.tableLayoutPanel96.RowCount = 2;
            this.tableLayoutPanel96.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel96.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel96.Size = new System.Drawing.Size(147, 69);
            this.tableLayoutPanel96.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 5);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 17);
            this.label11.TabIndex = 29;
            this.label11.Text = "Select Image: ";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 40);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 17);
            this.label12.TabIndex = 0;
            this.label12.Text = "Image URL: ";
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.ColumnCount = 1;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Controls.Add(this.BrowseImageButton, 0, 0);
            this.tableLayoutPanel16.Controls.Add(this.PrintURLTextBox, 0, 1);
            this.tableLayoutPanel16.Location = new System.Drawing.Point(335, 136);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 2;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(141, 69);
            this.tableLayoutPanel16.TabIndex = 31;
            // 
            // BrowseImageButton
            // 
            this.BrowseImageButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BrowseImageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseImageButton.Location = new System.Drawing.Point(20, 1);
            this.BrowseImageButton.Margin = new System.Windows.Forms.Padding(0);
            this.BrowseImageButton.Name = "BrowseImageButton";
            this.BrowseImageButton.Size = new System.Drawing.Size(100, 26);
            this.BrowseImageButton.TabIndex = 0;
            this.BrowseImageButton.Text = "Browse...";
            this.BrowseImageButton.UseVisualStyleBackColor = true;
            this.BrowseImageButton.Click += new System.EventHandler(this.BrowseImageButton_Click);
            // 
            // PrintURLTextBox
            // 
            this.PrintURLTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PrintURLTextBox.Location = new System.Drawing.Point(4, 37);
            this.PrintURLTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PrintURLTextBox.Name = "PrintURLTextBox";
            this.PrintURLTextBox.Size = new System.Drawing.Size(132, 22);
            this.PrintURLTextBox.TabIndex = 0;
            // 
            // PrintImageButton
            // 
            this.PrintImageButton.BackColor = System.Drawing.Color.White;
            this.PrintImageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrintImageButton.Location = new System.Drawing.Point(483, 140);
            this.PrintImageButton.Margin = new System.Windows.Forms.Padding(4);
            this.PrintImageButton.Name = "PrintImageButton";
            this.PrintImageButton.Size = new System.Drawing.Size(100, 60);
            this.PrintImageButton.TabIndex = 30;
            this.PrintImageButton.Text = "Print Image";
            this.PrintImageButton.UseVisualStyleBackColor = false;
            
            // PrintImage
            // 
            this.PrintImage.Location = new System.Drawing.Point(591, 140);
            this.PrintImage.Margin = new System.Windows.Forms.Padding(4);
            this.PrintImage.Name = "PrintImage";
            this.PrintImage.Size = new System.Drawing.Size(133, 62);
            this.PrintImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PrintImage.TabIndex = 1;
            this.PrintImage.TabStop = false;
            // 
            // flowLayoutPanel5
            // 
            this.tableLayoutPanel8.SetColumnSpan(this.flowLayoutPanel5, 5);
            this.flowLayoutPanel5.Controls.Add(this.ShowWelcomeButton);
            this.flowLayoutPanel5.Controls.Add(this.ShowThankYouButton);
            this.flowLayoutPanel5.Controls.Add(this.OpenCashDrawerButton);
            this.flowLayoutPanel5.Controls.Add(this.RetrievePrintJobStatusButton);
            this.flowLayoutPanel5.Controls.Add(this.CardDataButton);
            this.flowLayoutPanel5.Controls.Add(this.ResetButton);
            this.flowLayoutPanel5.Controls.Add(this.DeviceStatusButton);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(4, 276);
            this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.flowLayoutPanel5.Size = new System.Drawing.Size(1104, 92);
            this.flowLayoutPanel5.TabIndex = 37;
            // 
            // ShowWelcomeButton
            // 
            this.ShowWelcomeButton.BackColor = System.Drawing.Color.White;
            this.ShowWelcomeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowWelcomeButton.Location = new System.Drawing.Point(4, 10);
            this.ShowWelcomeButton.Margin = new System.Windows.Forms.Padding(4);
            this.ShowWelcomeButton.Name = "ShowWelcomeButton";
            this.ShowWelcomeButton.Size = new System.Drawing.Size(100, 60);
            this.ShowWelcomeButton.TabIndex = 24;
            this.ShowWelcomeButton.Text = "Show Welcome";
            this.ShowWelcomeButton.UseVisualStyleBackColor = false;
            this.ShowWelcomeButton.Click += new System.EventHandler(this.ShowWelcomeButton_Click);
            // 
            // ShowThankYouButton
            // 
            this.ShowThankYouButton.BackColor = System.Drawing.Color.White;
            this.ShowThankYouButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowThankYouButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.ShowThankYouButton.Location = new System.Drawing.Point(112, 10);
            this.ShowThankYouButton.Margin = new System.Windows.Forms.Padding(4);
            this.ShowThankYouButton.Name = "ShowThankYouButton";
            this.ShowThankYouButton.Size = new System.Drawing.Size(100, 62);
            this.ShowThankYouButton.TabIndex = 27;
            this.ShowThankYouButton.Text = "Show Thank You";
            this.ShowThankYouButton.UseVisualStyleBackColor = false;
            this.ShowThankYouButton.Click += new System.EventHandler(this.ShowThankYouButton_Click);
            // 
            // OpenCashDrawerButton
            // 
            this.OpenCashDrawerButton.BackColor = System.Drawing.Color.White;
            this.OpenCashDrawerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenCashDrawerButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.OpenCashDrawerButton.Location = new System.Drawing.Point(220, 10);
            this.OpenCashDrawerButton.Margin = new System.Windows.Forms.Padding(4);
            this.OpenCashDrawerButton.Name = "OpenCashDrawerButton";
            this.OpenCashDrawerButton.Size = new System.Drawing.Size(100, 60);
            this.OpenCashDrawerButton.TabIndex = 28;
            this.OpenCashDrawerButton.Text = "Open Cash Drawer";
            this.OpenCashDrawerButton.UseVisualStyleBackColor = false;
            

            //
            // RetrievePrintJobStatusButton
            //
            this.RetrievePrintJobStatusButton.BackColor = System.Drawing.Color.White;
            this.RetrievePrintJobStatusButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RetrievePrintJobStatusButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.RetrievePrintJobStatusButton.Location = new System.Drawing.Point(220, 10);
            this.RetrievePrintJobStatusButton.Margin = new System.Windows.Forms.Padding(4);
            this.RetrievePrintJobStatusButton.Name = "RetrievePrintJobStatusButton";
            this.RetrievePrintJobStatusButton.Size = new System.Drawing.Size(100, 60);
            this.RetrievePrintJobStatusButton.TabIndex = 28;
            this.RetrievePrintJobStatusButton.Text = "Print Job Status";
            this.RetrievePrintJobStatusButton.UseVisualStyleBackColor = false;
            this.RetrievePrintJobStatusButton.Click += new System.EventHandler(this.RetrievePrintJobStatusButton_Click);

            // 
            // CardDataButton
            // 
            this.CardDataButton.BackColor = System.Drawing.Color.White;
            this.CardDataButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CardDataButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.CardDataButton.Location = new System.Drawing.Point(436, 10);
            this.CardDataButton.Margin = new System.Windows.Forms.Padding(4);
            this.CardDataButton.Name = "CardDataButton";
            this.CardDataButton.Size = new System.Drawing.Size(100, 60);
            this.CardDataButton.TabIndex = 28;
            this.CardDataButton.Text = "Read Card Data";
            this.CardDataButton.UseVisualStyleBackColor = false;
            this.CardDataButton.Click += new System.EventHandler(this.CardDataButton_Click);
            // 
            // labelTS
            // 
            this.labelTS.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTS.AutoSize = true;
            this.labelTS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.labelTS.Location = new System.Drawing.Point(4, 372);
            this.labelTS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTS.Name = "labelTS";
            this.labelTS.Size = new System.Drawing.Size(327, 25);
            this.labelTS.TabIndex = 23;
            this.labelTS.Text = "Transaction Settings (Overrides)";
            // 
            // labelTipAmount
            // 
            this.labelTipAmount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTipAmount.AutoSize = true;
            this.labelTipAmount.Location = new System.Drawing.Point(4, 403);
            this.labelTipAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTipAmount.Name = "labelTipAmount";
            this.labelTipAmount.Size = new System.Drawing.Size(88, 17);
            this.labelTipAmount.TabIndex = 23;
            this.labelTipAmount.Text = "Tip Amount: ";
            // 
            // tipAmount
            // 
            this.tipAmount.Location = new System.Drawing.Point(339, 401);
            this.tipAmount.Margin = new System.Windows.Forms.Padding(4);
            this.tipAmount.Name = "tipAmount";
            this.tipAmount.Size = new System.Drawing.Size(132, 22);
            this.tipAmount.TabIndex = 38;
            // 
            // labelSignatureThreshold
            // 
            this.labelSignatureThreshold.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelSignatureThreshold.AutoSize = true;
            this.labelSignatureThreshold.Location = new System.Drawing.Point(4, 433);
            this.labelSignatureThreshold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSignatureThreshold.Name = "labelSignatureThreshold";
            this.labelSignatureThreshold.Size = new System.Drawing.Size(145, 17);
            this.labelSignatureThreshold.TabIndex = 23;
            this.labelSignatureThreshold.Text = "Signature Threshold: ";
            // 
            // signatureThreshold
            // 
            this.signatureThreshold.Location = new System.Drawing.Point(339, 431);
            this.signatureThreshold.Margin = new System.Windows.Forms.Padding(4);
            this.signatureThreshold.Name = "signatureThreshold";
            this.signatureThreshold.Size = new System.Drawing.Size(132, 22);
            this.signatureThreshold.TabIndex = 39;
            // 
            // flowLayoutPanel1
            // 
            this.tableLayoutPanel8.SetColumnSpan(this.flowLayoutPanel1, 5);
            this.tableLayoutPanel18.SetColumnSpan(this.flowLayoutPanel1, 5);
            this.flowLayoutPanel1.Controls.Add(this.label52);
            this.flowLayoutPanel1.Controls.Add(this.ManualEntryCheckbox);
            this.flowLayoutPanel1.Controls.Add(this.MagStripeCheckbox);
            this.flowLayoutPanel1.Controls.Add(this.ChipCheckbox);
            this.flowLayoutPanel1.Controls.Add(this.ContactlessCheckbox);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 461);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(667, 32);
            this.flowLayoutPanel1.TabIndex = 34;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(4, 4);
            this.label52.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(229, 17);
            this.label52.TabIndex = 37;
            this.label52.Text = "Card Entry Methods (Sale && Auth): ";
            // 
            // ManualEntryCheckbox
            // 
            this.ManualEntryCheckbox.AutoSize = true;
            this.ManualEntryCheckbox.Location = new System.Drawing.Point(241, 4);
            this.ManualEntryCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.ManualEntryCheckbox.Name = "ManualEntryCheckbox";
            this.ManualEntryCheckbox.Size = new System.Drawing.Size(76, 21);
            this.ManualEntryCheckbox.TabIndex = 33;
            this.ManualEntryCheckbox.Text = "Manual";
            this.ManualEntryCheckbox.UseVisualStyleBackColor = true;
            this.ManualEntryCheckbox.CheckedChanged += new System.EventHandler(this.EntryCheckbox_CheckedChanged);
            // 
            // MagStripeCheckbox
            // 
            this.MagStripeCheckbox.AutoSize = true;
            this.MagStripeCheckbox.Checked = true;
            this.MagStripeCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MagStripeCheckbox.Location = new System.Drawing.Point(325, 4);
            this.MagStripeCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.MagStripeCheckbox.Name = "MagStripeCheckbox";
            this.MagStripeCheckbox.Size = new System.Drawing.Size(98, 21);
            this.MagStripeCheckbox.TabIndex = 34;
            this.MagStripeCheckbox.Text = "Mag Stripe";
            this.MagStripeCheckbox.UseVisualStyleBackColor = true;
            this.MagStripeCheckbox.CheckedChanged += new System.EventHandler(this.EntryCheckbox_CheckedChanged);
            // 
            // ChipCheckbox
            // 
            this.ChipCheckbox.AutoSize = true;
            this.ChipCheckbox.Checked = true;
            this.ChipCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChipCheckbox.Location = new System.Drawing.Point(431, 4);
            this.ChipCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.ChipCheckbox.Name = "ChipCheckbox";
            this.ChipCheckbox.Size = new System.Drawing.Size(58, 21);
            this.ChipCheckbox.TabIndex = 35;
            this.ChipCheckbox.Text = "Chip";
            this.ChipCheckbox.UseVisualStyleBackColor = true;
            this.ChipCheckbox.CheckedChanged += new System.EventHandler(this.EntryCheckbox_CheckedChanged);
            // 
            // ContactlessCheckbox
            // 
            this.ContactlessCheckbox.AutoSize = true;
            this.ContactlessCheckbox.Checked = true;
            this.ContactlessCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ContactlessCheckbox.Location = new System.Drawing.Point(497, 4);
            this.ContactlessCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.ContactlessCheckbox.Name = "ContactlessCheckbox";
            this.ContactlessCheckbox.Size = new System.Drawing.Size(103, 21);
            this.ContactlessCheckbox.TabIndex = 36;
            this.ContactlessCheckbox.Tag = "";
            this.ContactlessCheckbox.Text = "Contactless";
            this.ContactlessCheckbox.UseVisualStyleBackColor = true;
            this.ContactlessCheckbox.CheckedChanged += new System.EventHandler(this.EntryCheckbox_CheckedChanged);
            // 
            // flowLayoutPanelCNP
            // 
            this.tableLayoutPanel8.SetColumnSpan(this.flowLayoutPanelCNP, 5);
            this.tableLayoutPanel18.SetColumnSpan(this.flowLayoutPanelCNP, 5);
            this.flowLayoutPanelCNP.Controls.Add(this.labelCNP);
            this.flowLayoutPanelCNP.Controls.Add(this.CardNotPresentCheckbox);
            this.flowLayoutPanelCNP.Location = new System.Drawing.Point(4, 501);
            this.flowLayoutPanelCNP.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanelCNP.Name = "flowLayoutPanelCNP";
            this.flowLayoutPanelCNP.Size = new System.Drawing.Size(667, 32);
            this.flowLayoutPanelCNP.TabIndex = 0;
            // 
            // labelCNP
            // 
            this.labelCNP.AutoSize = true;
            this.labelCNP.Location = new System.Drawing.Point(4, 4);
            this.labelCNP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.labelCNP.MinimumSize = new System.Drawing.Size(227, 16);
            this.labelCNP.Name = "labelCNP";
            this.labelCNP.Size = new System.Drawing.Size(227, 17);
            this.labelCNP.TabIndex = 0;
            this.labelCNP.Text = " ";
            // 
            // CardNotPresentCheckbox
            // 
            this.CardNotPresentCheckbox.AutoSize = true;
            this.CardNotPresentCheckbox.Enabled = false;
            this.CardNotPresentCheckbox.Location = new System.Drawing.Point(239, 4);
            this.CardNotPresentCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.CardNotPresentCheckbox.Name = "CardNotPresentCheckbox";
            this.CardNotPresentCheckbox.Size = new System.Drawing.Size(361, 21);
            this.CardNotPresentCheckbox.TabIndex = 37;
            this.CardNotPresentCheckbox.Text = "Card Not Present (only applies to Manual entry type)";
            this.CardNotPresentCheckbox.UseVisualStyleBackColor = true;
            this.CardNotPresentCheckbox.CheckedChanged += new System.EventHandler(this.EntryCheckbox_CheckedChanged);
            // 
            // flowLayoutPanelForceOffline
            // 
            this.tableLayoutPanelForceOffline.SetColumnSpan(this.flowLayoutPanelForceOffline, 4);
            this.flowLayoutPanelForceOffline.Controls.Add(this.labelForceOffline);
            this.flowLayoutPanelForceOffline.Controls.Add(this.forceOfflineDefault);
            this.flowLayoutPanelForceOffline.Controls.Add(this.forceOfflineYes);
            this.flowLayoutPanelForceOffline.Controls.Add(this.forceOfflineNo);
            this.flowLayoutPanelForceOffline.Location = new System.Drawing.Point(4, 541);
            this.flowLayoutPanelForceOffline.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanelForceOffline.Name = "flowLayoutPanelForceOffline";
            this.flowLayoutPanelForceOffline.Size = new System.Drawing.Size(1104, 30);
            this.flowLayoutPanelForceOffline.TabIndex = 40;
            // 
            // labelForceOffline
            // 
            this.labelForceOffline.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelForceOffline.AutoSize = true;
            this.labelForceOffline.Location = new System.Drawing.Point(4, 6);
            this.labelForceOffline.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelForceOffline.Name = "labelForceOffline";
            this.labelForceOffline.Size = new System.Drawing.Size(150, 17);
            this.labelForceOffline.TabIndex = 0;
            this.labelForceOffline.Text = "Force Offline Payment";
            // 
            // forceOfflineDefault
            // 
            this.forceOfflineDefault.AutoSize = true;
            this.forceOfflineDefault.Checked = true;
            this.forceOfflineDefault.Location = new System.Drawing.Point(165, 4);
            this.forceOfflineDefault.Margin = new System.Windows.Forms.Padding(4);
            this.forceOfflineDefault.Name = "forceOfflineDefault";
            this.forceOfflineDefault.Size = new System.Drawing.Size(82, 21);
            this.forceOfflineDefault.TabIndex = 1;
            this.forceOfflineDefault.TabStop = true;
            this.forceOfflineDefault.Text = "Default";
            this.forceOfflineDefault.UseVisualStyleBackColor = true;
            this.forceOfflineDefault.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // forceOfflineYes
            // 
            this.forceOfflineYes.AutoSize = true;
            this.forceOfflineYes.Location = new System.Drawing.Point(253, 4);
            this.forceOfflineYes.Margin = new System.Windows.Forms.Padding(4);
            this.forceOfflineYes.Name = "forceOfflineYes";
            this.forceOfflineYes.Size = new System.Drawing.Size(61, 21);
            this.forceOfflineYes.TabIndex = 2;
            this.forceOfflineYes.Text = "Yes";
            this.forceOfflineYes.UseVisualStyleBackColor = true;
            this.forceOfflineYes.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // forceOfflineNo
            // 
            this.forceOfflineNo.AutoSize = true;
            this.forceOfflineNo.Location = new System.Drawing.Point(320, 4);
            this.forceOfflineNo.Margin = new System.Windows.Forms.Padding(4);
            this.forceOfflineNo.Name = "forceOfflineNo";
            this.forceOfflineNo.Size = new System.Drawing.Size(55, 21);
            this.forceOfflineNo.TabIndex = 3;
            this.forceOfflineNo.Text = "No";
            this.forceOfflineNo.UseVisualStyleBackColor = true;
            this.forceOfflineNo.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // flowLayoutPanel6
            // 
            this.tableLayoutPanel8.SetColumnSpan(this.flowLayoutPanel6, 4);
            this.tableLayoutPanel18.SetColumnSpan(this.flowLayoutPanel6, 4);
            this.flowLayoutPanel6.Controls.Add(this.label13);
            this.flowLayoutPanel6.Controls.Add(this.offlineDefault);
            this.flowLayoutPanel6.Controls.Add(this.offlineYes);
            this.flowLayoutPanel6.Controls.Add(this.offlineNo);
            this.flowLayoutPanel6.Location = new System.Drawing.Point(4, 541);
            this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(1104, 30);
            this.flowLayoutPanel6.TabIndex = 40;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 6);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 17);
            this.label13.TabIndex = 0;
            this.label13.Text = "Allow Offline Payment";
            // 
            // offlineDefault
            // 
            this.offlineDefault.AutoSize = true;
            this.offlineDefault.Checked = true;
            this.offlineDefault.Location = new System.Drawing.Point(156, 4);
            this.offlineDefault.Margin = new System.Windows.Forms.Padding(4);
            this.offlineDefault.Name = "offlineDefault";
            this.offlineDefault.Size = new System.Drawing.Size(74, 21);
            this.offlineDefault.TabIndex = 1;
            this.offlineDefault.TabStop = true;
            this.offlineDefault.Text = "Default";
            this.offlineDefault.UseVisualStyleBackColor = true;
            this.offlineDefault.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // offlineYes
            // 
            this.offlineYes.AutoSize = true;
            this.offlineYes.Location = new System.Drawing.Point(238, 4);
            this.offlineYes.Margin = new System.Windows.Forms.Padding(4);
            this.offlineYes.Name = "offlineYes";
            this.offlineYes.Size = new System.Drawing.Size(53, 21);
            this.offlineYes.TabIndex = 2;
            this.offlineYes.Text = "Yes";
            this.offlineYes.UseVisualStyleBackColor = true;
            this.offlineYes.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // offlineNo
            // 
            this.offlineNo.AutoSize = true;
            this.offlineNo.Location = new System.Drawing.Point(299, 4);
            this.offlineNo.Margin = new System.Windows.Forms.Padding(4);
            this.offlineNo.Name = "offlineNo";
            this.offlineNo.Size = new System.Drawing.Size(47, 21);
            this.offlineNo.TabIndex = 3;
            this.offlineNo.Text = "No";
            this.offlineNo.UseVisualStyleBackColor = true;
            this.offlineNo.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // flowLayoutPanel4
            // 
            this.tableLayoutPanel8.SetColumnSpan(this.flowLayoutPanel4, 4);
            this.tableLayoutPanel18.SetColumnSpan(this.flowLayoutPanel4, 4);
            this.flowLayoutPanel4.Controls.Add(this.label14);
            this.flowLayoutPanel4.Controls.Add(this.approveOfflineDefault);
            this.flowLayoutPanel4.Controls.Add(this.approveOfflineYes);
            this.flowLayoutPanel4.Controls.Add(this.approveOfflineNo);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(4, 579);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(1104, 30);
            this.flowLayoutPanel4.TabIndex = 41;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 6);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(236, 17);
            this.label14.TabIndex = 0;
            this.label14.Text = "Accept Offline Payment W/O Prompt";
            // 
            // approveOfflineDefault
            // 
            this.approveOfflineDefault.AutoSize = true;
            this.approveOfflineDefault.Checked = true;
            this.approveOfflineDefault.Location = new System.Drawing.Point(248, 4);
            this.approveOfflineDefault.Margin = new System.Windows.Forms.Padding(4);
            this.approveOfflineDefault.Name = "approveOfflineDefault";
            this.approveOfflineDefault.Size = new System.Drawing.Size(74, 21);
            this.approveOfflineDefault.TabIndex = 1;
            this.approveOfflineDefault.TabStop = true;
            this.approveOfflineDefault.Text = "Default";
            this.approveOfflineDefault.UseVisualStyleBackColor = true;
            this.approveOfflineDefault.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // approveOfflineYes
            // 
            this.approveOfflineYes.AutoSize = true;
            this.approveOfflineYes.Location = new System.Drawing.Point(330, 4);
            this.approveOfflineYes.Margin = new System.Windows.Forms.Padding(4);
            this.approveOfflineYes.Name = "approveOfflineYes";
            this.approveOfflineYes.Size = new System.Drawing.Size(53, 21);
            this.approveOfflineYes.TabIndex = 2;
            this.approveOfflineYes.Text = "Yes";
            this.approveOfflineYes.UseVisualStyleBackColor = true;
            this.approveOfflineYes.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // approveOfflineNo
            // 
            this.approveOfflineNo.AutoSize = true;
            this.approveOfflineNo.Location = new System.Drawing.Point(391, 4);
            this.approveOfflineNo.Margin = new System.Windows.Forms.Padding(4);
            this.approveOfflineNo.Name = "approveOfflineNo";
            this.approveOfflineNo.Size = new System.Drawing.Size(47, 21);
            this.approveOfflineNo.TabIndex = 3;
            this.approveOfflineNo.Text = "No";
            this.approveOfflineNo.UseVisualStyleBackColor = true;
            this.approveOfflineNo.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // flowLayoutPanelTipMode
            // 
            this.tableLayoutPanel8.SetColumnSpan(this.flowLayoutPanelTipMode, 5);
            this.tableLayoutPanel18.SetColumnSpan(this.flowLayoutPanelTipMode, 5);
            this.flowLayoutPanelTipMode.Controls.Add(this.labelTipMode);
            this.flowLayoutPanelTipMode.Controls.Add(this.tipModeDefault);
            this.flowLayoutPanelTipMode.Controls.Add(this.tipModeProvided);
            this.flowLayoutPanelTipMode.Controls.Add(this.tipModeOnScreen);
            this.flowLayoutPanelTipMode.Controls.Add(this.tipModeNone);
            this.flowLayoutPanelTipMode.Location = new System.Drawing.Point(4, 617);
            this.flowLayoutPanelTipMode.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanelTipMode.Name = "flowLayoutPanelTipMode";
            this.flowLayoutPanelTipMode.Size = new System.Drawing.Size(1104, 30);
            this.flowLayoutPanelTipMode.TabIndex = 0;
            // 
            // labelTipMode
            // 
            this.labelTipMode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelTipMode.Location = new System.Drawing.Point(4, 6);
            this.labelTipMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTipMode.Name = "labelTipMode";
            this.labelTipMode.Size = new System.Drawing.Size(133, 16);
            this.labelTipMode.TabIndex = 0;
            this.labelTipMode.Text = "Tip Mode";
            // 
            // tipModeDefault
            // 
            this.tipModeDefault.AutoSize = true;
            this.tipModeDefault.Checked = true;
            this.tipModeDefault.Location = new System.Drawing.Point(145, 4);
            this.tipModeDefault.Margin = new System.Windows.Forms.Padding(4);
            this.tipModeDefault.Name = "tipModeDefault";
            this.tipModeDefault.Size = new System.Drawing.Size(74, 21);
            this.tipModeDefault.TabIndex = 2;
            this.tipModeDefault.TabStop = true;
            this.tipModeDefault.Text = "Default";
            this.tipModeDefault.UseVisualStyleBackColor = true;
            this.tipModeDefault.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // tipModeProvided
            // 
            this.tipModeProvided.AutoSize = true;
            this.tipModeProvided.Location = new System.Drawing.Point(227, 4);
            this.tipModeProvided.Margin = new System.Windows.Forms.Padding(4);
            this.tipModeProvided.Name = "tipModeProvided";
            this.tipModeProvided.Size = new System.Drawing.Size(85, 21);
            this.tipModeProvided.TabIndex = 2;
            this.tipModeProvided.Text = "Provided";
            this.tipModeProvided.UseVisualStyleBackColor = true;
            this.tipModeProvided.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // tipModeOnScreen
            // 
            this.tipModeOnScreen.AutoSize = true;
            this.tipModeOnScreen.Location = new System.Drawing.Point(320, 4);
            this.tipModeOnScreen.Margin = new System.Windows.Forms.Padding(4);
            this.tipModeOnScreen.Name = "tipModeOnScreen";
            this.tipModeOnScreen.Size = new System.Drawing.Size(97, 21);
            this.tipModeOnScreen.TabIndex = 3;
            this.tipModeOnScreen.Text = "On Screen";
            this.tipModeOnScreen.UseVisualStyleBackColor = true;
            this.tipModeOnScreen.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // tipModeNone
            // 
            this.tipModeNone.AutoSize = true;
            this.tipModeNone.Location = new System.Drawing.Point(425, 4);
            this.tipModeNone.Margin = new System.Windows.Forms.Padding(4);
            this.tipModeNone.Name = "tipModeNone";
            this.tipModeNone.Size = new System.Drawing.Size(63, 21);
            this.tipModeNone.TabIndex = 3;
            this.tipModeNone.Text = "None";
            this.tipModeNone.UseVisualStyleBackColor = true;
            this.tipModeNone.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // flowLayoutPanel94
            // 
            this.tableLayoutPanel8.SetColumnSpan(this.flowLayoutPanel94, 5);
            this.tableLayoutPanel18.SetColumnSpan(this.flowLayoutPanel94, 5);
            this.flowLayoutPanel94.Controls.Add(this.label84);
            this.flowLayoutPanel94.Controls.Add(this.signatureDefault);
            this.flowLayoutPanel94.Controls.Add(this.signatureOnScreen);
            this.flowLayoutPanel94.Controls.Add(this.signatureOnPaper);
            this.flowLayoutPanel94.Controls.Add(this.signatureNone);
            this.flowLayoutPanel94.Location = new System.Drawing.Point(4, 655);
            this.flowLayoutPanel94.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel94.Name = "flowLayoutPanel94";
            this.flowLayoutPanel94.Size = new System.Drawing.Size(1104, 30);
            this.flowLayoutPanel94.TabIndex = 0;
            // 
            // label84
            // 
            this.label84.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label84.AutoSize = true;
            this.label84.Location = new System.Drawing.Point(4, 6);
            this.label84.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(127, 17);
            this.label84.TabIndex = 0;
            this.label84.Text = "Signature Location";
            // 
            // signatureDefault
            // 
            this.signatureDefault.AutoSize = true;
            this.signatureDefault.Checked = true;
            this.signatureDefault.Location = new System.Drawing.Point(139, 4);
            this.signatureDefault.Margin = new System.Windows.Forms.Padding(4);
            this.signatureDefault.Name = "signatureDefault";
            this.signatureDefault.Size = new System.Drawing.Size(74, 21);
            this.signatureDefault.TabIndex = 2;
            this.signatureDefault.TabStop = true;
            this.signatureDefault.Text = "Default";
            this.signatureDefault.UseVisualStyleBackColor = true;
            this.signatureDefault.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // signatureOnScreen
            // 
            this.signatureOnScreen.AutoSize = true;
            this.signatureOnScreen.Location = new System.Drawing.Point(221, 4);
            this.signatureOnScreen.Margin = new System.Windows.Forms.Padding(4);
            this.signatureOnScreen.Name = "signatureOnScreen";
            this.signatureOnScreen.Size = new System.Drawing.Size(97, 21);
            this.signatureOnScreen.TabIndex = 2;
            this.signatureOnScreen.TabStop = true;
            this.signatureOnScreen.Text = "On Screen";
            this.signatureOnScreen.UseVisualStyleBackColor = true;
            this.signatureOnScreen.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // signatureOnPaper
            // 
            this.signatureOnPaper.AutoSize = true;
            this.signatureOnPaper.Location = new System.Drawing.Point(326, 4);
            this.signatureOnPaper.Margin = new System.Windows.Forms.Padding(4);
            this.signatureOnPaper.Name = "signatureOnPaper";
            this.signatureOnPaper.Size = new System.Drawing.Size(90, 21);
            this.signatureOnPaper.TabIndex = 3;
            this.signatureOnPaper.Text = "On Paper";
            this.signatureOnPaper.UseVisualStyleBackColor = true;
            this.signatureOnPaper.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // signatureNone
            // 
            this.signatureNone.AutoSize = true;
            this.signatureNone.Location = new System.Drawing.Point(424, 4);
            this.signatureNone.Margin = new System.Windows.Forms.Padding(4);
            this.signatureNone.Name = "signatureNone";
            this.signatureNone.Size = new System.Drawing.Size(63, 21);
            this.signatureNone.TabIndex = 3;
            this.signatureNone.Text = "None";
            this.signatureNone.UseVisualStyleBackColor = true;
            this.signatureNone.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // DisableCashBack
            // 
            this.DisableCashBack.AutoSize = true;
            this.DisableCashBack.Location = new System.Drawing.Point(4, 693);
            this.DisableCashBack.Margin = new System.Windows.Forms.Padding(4);
            this.DisableCashBack.Name = "DisableCashBack";
            this.DisableCashBack.Size = new System.Drawing.Size(144, 21);
            this.DisableCashBack.TabIndex = 43;
            this.DisableCashBack.Text = "Disable CashBack";
            this.DisableCashBack.UseVisualStyleBackColor = true;
            // 
            // DisableRestartTransactionOnFailure
            // 
            this.DisableRestartTransactionOnFailure.AutoSize = true;
            this.DisableRestartTransactionOnFailure.Location = new System.Drawing.Point(4, 722);
            this.DisableRestartTransactionOnFailure.Margin = new System.Windows.Forms.Padding(4);
            this.DisableRestartTransactionOnFailure.Name = "DisableRestartTransactionOnFailure";
            this.DisableRestartTransactionOnFailure.Size = new System.Drawing.Size(289, 21);
            this.DisableRestartTransactionOnFailure.TabIndex = 44;
            this.DisableRestartTransactionOnFailure.Text = "Disable Restart of Transaction on Failure";
            this.DisableRestartTransactionOnFailure.UseVisualStyleBackColor = true;
            // 
            // disablePrintingCB
            // 
            this.disablePrintingCB.AutoSize = true;
            this.disablePrintingCB.Location = new System.Drawing.Point(4, 751);
            this.disablePrintingCB.Margin = new System.Windows.Forms.Padding(4);
            this.disablePrintingCB.Name = "disablePrintingCB";
            this.disablePrintingCB.Size = new System.Drawing.Size(129, 21);
            this.disablePrintingCB.TabIndex = 42;
            this.disablePrintingCB.Text = "Disable Printing";
            this.disablePrintingCB.UseVisualStyleBackColor = true;
            // 
            // disableReceiptOptionsCB
            // 
            this.disableReceiptOptionsCB.AutoSize = true;
            this.disableReceiptOptionsCB.Location = new System.Drawing.Point(4, 780);
            this.disableReceiptOptionsCB.Margin = new System.Windows.Forms.Padding(4);
            this.disableReceiptOptionsCB.Name = "disableReceiptOptionsCB";
            this.disableReceiptOptionsCB.Size = new System.Drawing.Size(182, 21);
            this.disableReceiptOptionsCB.TabIndex = 45;
            this.disableReceiptOptionsCB.Text = "Disable Receipt Options";
            this.disableReceiptOptionsCB.UseVisualStyleBackColor = true;
            this.disableReceiptOptionsCB.CheckedChanged += new System.EventHandler(this.disableReceiptOptionsCB_CheckedChanged);
            // 
            // disableDuplicateCheckingCB
            // 
            this.disableDuplicateCheckingCB.AutoSize = true;
            this.disableDuplicateCheckingCB.Location = new System.Drawing.Point(4, 809);
            this.disableDuplicateCheckingCB.Margin = new System.Windows.Forms.Padding(4);
            this.disableDuplicateCheckingCB.Name = "disableDuplicateCheckingCB";
            this.disableDuplicateCheckingCB.Size = new System.Drawing.Size(202, 21);
            this.disableDuplicateCheckingCB.TabIndex = 46;
            this.disableDuplicateCheckingCB.Text = "Disable Duplicate Checking";
            this.disableDuplicateCheckingCB.UseVisualStyleBackColor = true;
            // 
            // automaticSignatureConfirmationCB
            // 
            this.automaticSignatureConfirmationCB.AutoSize = true;
            this.automaticSignatureConfirmationCB.Location = new System.Drawing.Point(4, 838);
            this.automaticSignatureConfirmationCB.Margin = new System.Windows.Forms.Padding(4);
            this.automaticSignatureConfirmationCB.Name = "automaticSignatureConfirmationCB";
            this.automaticSignatureConfirmationCB.Size = new System.Drawing.Size(225, 21);
            this.automaticSignatureConfirmationCB.TabIndex = 47;
            this.automaticSignatureConfirmationCB.Text = "Automatically Accept Signature";
            this.automaticSignatureConfirmationCB.UseVisualStyleBackColor = true;
            // 
            // automaticPaymentConfirmationCB
            // 
            this.automaticPaymentConfirmationCB.AutoSize = true;
            this.automaticPaymentConfirmationCB.Location = new System.Drawing.Point(4, 867);
            this.automaticPaymentConfirmationCB.Margin = new System.Windows.Forms.Padding(4);
            this.automaticPaymentConfirmationCB.Name = "automaticPaymentConfirmationCB";
            this.automaticPaymentConfirmationCB.Size = new System.Drawing.Size(293, 21);
            this.automaticPaymentConfirmationCB.TabIndex = 48;
            this.automaticPaymentConfirmationCB.Text = "Automatically Accept Payment Challenges";
            this.automaticPaymentConfirmationCB.UseVisualStyleBackColor = true;
            // 
            // RetrievePaymentLabel
            // 
            this.RetrievePaymentLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RetrievePaymentLabel.AutoSize = true;
            this.RetrievePaymentLabel.Location = new System.Drawing.Point(3, 230);
            this.RetrievePaymentLabel.Name = "RetrievePaymentLabel";
            this.RetrievePaymentLabel.Size = new System.Drawing.Size(137, 17);
            this.RetrievePaymentLabel.TabIndex = 49;
            this.RetrievePaymentLabel.Text = "External Payment Id:";
            this.RetrievePaymentLabel.Click += new System.EventHandler(this.label15_Click);
            // 
            // RetrievePaymentButton
            // 
            this.RetrievePaymentButton.BackColor = System.Drawing.Color.White;
            this.RetrievePaymentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RetrievePaymentButton.Location = new System.Drawing.Point(482, 209);
            this.RetrievePaymentButton.Name = "RetrievePaymentButton";
            this.RetrievePaymentButton.Size = new System.Drawing.Size(100, 60);
            this.RetrievePaymentButton.TabIndex = 50;
            this.RetrievePaymentButton.Text = "Retrieve Payment";
            this.RetrievePaymentButton.UseVisualStyleBackColor = false;
            this.RetrievePaymentButton.Click += new System.EventHandler(this.RetrievePayment_Click);
            // 
            // RetrievePaymentText
            // 
            this.RetrievePaymentText.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RetrievePaymentText.Location = new System.Drawing.Point(338, 228);
            this.RetrievePaymentText.Name = "RetrievePaymentText";
            this.RetrievePaymentText.Size = new System.Drawing.Size(138, 22);
            this.RetrievePaymentText.TabIndex = 51;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.pendingPaymentListView);
            this.tabPage8.Controls.Add(this.refreshPendingPayments);
            this.tabPage8.Location = new System.Drawing.Point(4, 28);
            this.tabPage8.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage8.Size = new System.Drawing.Size(1204, 635);
            this.tabPage8.TabIndex = 6;
            this.tabPage8.Text = "Pending Payments";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // pendingPaymentListView
            // 
            this.pendingPaymentListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pendingPaymentListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.paymentIdHeader,
            this.paymentAmountHeader});
            this.pendingPaymentListView.FullRowSelect = true;
            this.pendingPaymentListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.pendingPaymentListView.Location = new System.Drawing.Point(9, 7);
            this.pendingPaymentListView.Margin = new System.Windows.Forms.Padding(4);
            this.pendingPaymentListView.MultiSelect = false;
            this.pendingPaymentListView.Name = "pendingPaymentListView";
            this.pendingPaymentListView.Size = new System.Drawing.Size(1182, 548);
            this.pendingPaymentListView.TabIndex = 40;
            this.pendingPaymentListView.UseCompatibleStateImageBehavior = false;
            this.pendingPaymentListView.View = System.Windows.Forms.View.Details;
            // 
            // paymentIdHeader
            // 
            this.paymentIdHeader.Text = "Payment ID";
            this.paymentIdHeader.Width = 300;
            // 
            // paymentAmountHeader
            // 
            this.paymentAmountHeader.Text = "Amount";
            this.paymentAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.paymentAmountHeader.Width = 150;
            // 
            // refreshPendingPayments
            // 
            this.refreshPendingPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshPendingPayments.BackColor = System.Drawing.Color.White;
            this.refreshPendingPayments.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshPendingPayments.Location = new System.Drawing.Point(1092, 564);
            this.refreshPendingPayments.Margin = new System.Windows.Forms.Padding(4);
            this.refreshPendingPayments.Name = "refreshPendingPayments";
            this.refreshPendingPayments.Size = new System.Drawing.Size(100, 60);
            this.refreshPendingPayments.TabIndex = 39;
            this.refreshPendingPayments.Text = "Refresh";
            this.refreshPendingPayments.UseVisualStyleBackColor = false;
            this.refreshPendingPayments.Click += new System.EventHandler(this.refreshPendingPayments_Click);
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.SendMessageBtn);
            this.tabPage9.Controls.Add(this.StartCustomActivityBtn);
            this.tabPage9.Controls.Add(this.nonBlockingCB);
            this.tabPage9.Controls.Add(this.customActivityAction);
            this.tabPage9.Controls.Add(this.customActivityPayload);
            this.tabPage9.Location = new System.Drawing.Point(4, 28);
            this.tabPage9.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage9.Size = new System.Drawing.Size(1204, 635);
            this.tabPage9.TabIndex = 7;
            this.tabPage9.Text = "Custom Activities";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // SendMessageBtn
            // 
            this.SendMessageBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SendMessageBtn.BackColor = System.Drawing.Color.White;
            this.SendMessageBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendMessageBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendMessageBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SendMessageBtn.Location = new System.Drawing.Point(589, 9);
            this.SendMessageBtn.Margin = new System.Windows.Forms.Padding(4);
            this.SendMessageBtn.Name = "SendMessageBtn";
            this.SendMessageBtn.Size = new System.Drawing.Size(95, 62);
            this.SendMessageBtn.TabIndex = 29;
            this.SendMessageBtn.Text = "Send";
            this.SendMessageBtn.UseVisualStyleBackColor = false;
            this.SendMessageBtn.Click += new System.EventHandler(this.SendMessageBtn_Click);
            // 
            // StartCustomActivityBtn
            // 
            this.StartCustomActivityBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.StartCustomActivityBtn.BackColor = System.Drawing.Color.White;
            this.StartCustomActivityBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartCustomActivityBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartCustomActivityBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.StartCustomActivityBtn.Location = new System.Drawing.Point(12, 218);
            this.StartCustomActivityBtn.Margin = new System.Windows.Forms.Padding(4);
            this.StartCustomActivityBtn.Name = "StartCustomActivityBtn";
            this.StartCustomActivityBtn.Size = new System.Drawing.Size(95, 62);
            this.StartCustomActivityBtn.TabIndex = 28;
            this.StartCustomActivityBtn.Text = "Start Activity";
            this.StartCustomActivityBtn.UseVisualStyleBackColor = false;
            this.StartCustomActivityBtn.Click += new System.EventHandler(this.startCustomActivity_Click);
            // 
            // nonBlockingCB
            // 
            this.nonBlockingCB.AutoSize = true;
            this.nonBlockingCB.Location = new System.Drawing.Point(12, 190);
            this.nonBlockingCB.Margin = new System.Windows.Forms.Padding(4);
            this.nonBlockingCB.Name = "nonBlockingCB";
            this.nonBlockingCB.Size = new System.Drawing.Size(114, 21);
            this.nonBlockingCB.TabIndex = 2;
            this.nonBlockingCB.Text = "Non-Blocking";
            this.nonBlockingCB.UseVisualStyleBackColor = true;
            // 
            // customActivityAction
            // 
            this.customActivityAction.Location = new System.Drawing.Point(12, 156);
            this.customActivityAction.Margin = new System.Windows.Forms.Padding(4);
            this.customActivityAction.Name = "customActivityAction";
            this.customActivityAction.Size = new System.Drawing.Size(379, 22);
            this.customActivityAction.TabIndex = 1;
            // 
            // customActivityPayload
            // 
            this.customActivityPayload.Location = new System.Drawing.Point(12, 9);
            this.customActivityPayload.Margin = new System.Windows.Forms.Padding(4);
            this.customActivityPayload.Multiline = true;
            this.customActivityPayload.Name = "customActivityPayload";
            this.customActivityPayload.Size = new System.Drawing.Size(568, 138);
            this.customActivityPayload.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel14, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel15, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 672);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1212, 62);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 3;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Controls.Add(this.DeviceCurrentStatus, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.UIStateButtonPanel, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(1004, 54);
            this.tableLayoutPanel14.TabIndex = 18;
            // 
            // DeviceCurrentStatus
            // 
            this.DeviceCurrentStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DeviceCurrentStatus.AutoSize = true;
            this.DeviceCurrentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceCurrentStatus.Location = new System.Drawing.Point(111, 14);
            this.DeviceCurrentStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DeviceCurrentStatus.Name = "DeviceCurrentStatus";
            this.DeviceCurrentStatus.Size = new System.Drawing.Size(30, 25);
            this.DeviceCurrentStatus.TabIndex = 22;
            this.DeviceCurrentStatus.Text = "...";
            // 
            // UIStateButtonPanel
            // 
            this.UIStateButtonPanel.AutoSize = true;
            this.UIStateButtonPanel.Location = new System.Drawing.Point(90, 4);
            this.UIStateButtonPanel.Margin = new System.Windows.Forms.Padding(4);
            this.UIStateButtonPanel.MinimumSize = new System.Drawing.Size(13, 0);
            this.UIStateButtonPanel.Name = "UIStateButtonPanel";
            this.UIStateButtonPanel.Size = new System.Drawing.Size(13, 0);
            this.UIStateButtonPanel.TabIndex = 21;
            this.UIStateButtonPanel.WrapContents = false;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 25);
            this.label4.TabIndex = 19;
            this.label4.Text = "Device:";
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 1;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Controls.Add(this.ConnectStatusLabel, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(1012, 0);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(200, 62);
            this.tableLayoutPanel15.TabIndex = 19;
            // 
            // tableLayoutPanelTipAmount
            // 
            this.tableLayoutPanelTipAmount.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTipAmount.Name = "tableLayoutPanelTipAmount";
            this.tableLayoutPanelTipAmount.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanelTipAmount.TabIndex = 0;
            // 
            // tableLayoutPanelSignatureThreshold
            // 
            this.tableLayoutPanelSignatureThreshold.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSignatureThreshold.Name = "tableLayoutPanelSignatureThreshold";
            this.tableLayoutPanelSignatureThreshold.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanelSignatureThreshold.TabIndex = 0;
            // 
            // tipModeOnPaper
            // 
            this.tipModeOnPaper.Location = new System.Drawing.Point(0, 0);
            this.tipModeOnPaper.Name = "tipModeOnPaper";
            this.tipModeOnPaper.Size = new System.Drawing.Size(104, 24);
            this.tipModeOnPaper.TabIndex = 0;
            // 
            // flowLayoutPanelSigLoc
            // 
            this.flowLayoutPanelSigLoc.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelSigLoc.Name = "flowLayoutPanelSigLoc";
            this.flowLayoutPanelSigLoc.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanelSigLoc.TabIndex = 0;
            // 
            // labelSigLoc
            // 
            this.labelSigLoc.Location = new System.Drawing.Point(0, 0);
            this.labelSigLoc.Name = "labelSigLoc";
            this.labelSigLoc.Size = new System.Drawing.Size(100, 23);
            this.labelSigLoc.TabIndex = 0;
            // 
            // CloverExamplePOSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 734);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CloverExamplePOSForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clover Example POS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExamplePOSForm_Closed);
            this.Load += new System.EventHandler(this.ExamplePOSForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel11.ResumeLayout(false);
            this.RegisterTabs.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.SelectedItemPanel.ResumeLayout(false);
            this.SelectedItemPanel.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel17.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.Cards.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage10.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tableLayoutPanel18.PerformLayout();
            this.tableLayoutPanel96.ResumeLayout(false);
            this.tableLayoutPanel96.PerformLayout();
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel16.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PrintImage)).EndInit();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanelCNP.ResumeLayout(false);
            this.flowLayoutPanelCNP.PerformLayout();
            this.flowLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel6.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.flowLayoutPanelTipMode.ResumeLayout(false);
            this.flowLayoutPanelTipMode.PerformLayout();
            this.flowLayoutPanel94.ResumeLayout(false);
            this.flowLayoutPanel94.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel14.PerformLayout();
            this.tableLayoutPanel15.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label ConnectStatusLabel;
        private ToolStripMenuItem DeviceMenu;
        private ToolStripMenuItem TestDeviceMenuItem;
        private ToolStripMenuItem CloverMiniUSBMenuItem;
        private ToolStripMenuItem WebSocketMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private TabControl TabControl;
        private TabPage tabPage1;
        private FlowLayoutPanel StoreDiscounts;
        private FlowLayoutPanel StoreItems;
        private Label DiscountLabel;
        private Label label10;
        private Button newOrderBtn;
        private Label currentOrder;
        private Label label3;
        private Label label6;
        private Label SubTotal;
        private Label TaxAmount;
        private Label TotalAmount;
        private Label label2;
        private Label label1;
        private DropDownButton SaleButton;
        private ListView OrderItems;
        private ColumnHeader Quantity;
        private ColumnHeader Item;
        private ColumnHeader Price;
        private ColumnHeader DiscountHeader;
        private TabPage tabPage2;
        private TableLayoutPanel tableLayoutPanel3;
        private SplitContainer splitContainer1;
        private ListView OrdersListView;
        private ColumnHeader idHeader;
        private ColumnHeader totalHeader;
        private ColumnHeader dateHeader;
        private ColumnHeader statusHeader;
        private ColumnHeader subTotalHeader;
        private ColumnHeader taxHeader;
        private SplitContainer splitContainer2;
        private ListView OrderDetailsListView;
        private ColumnHeader quantityHeader;
        private ColumnHeader itemHeader;
        private ColumnHeader itemPriceHeader;
        private ListView OrderPaymentsView;
        private ColumnHeader PayStatusHeader;
        private ColumnHeader amountHeader;
        private ColumnHeader tipPaymentHeader;
        private ColumnHeader paymentTotalHeader1;
        private TabPage tabPage3;
        private ListView TransactionsListView;
        private ColumnHeader TransAmountHeader;
        private ColumnHeader TransDateHeader;
        private ColumnHeader TransLast4;
        private TabPage tabPage4;
        private TabPage tabPage10;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel4;
        private FlowLayoutPanel flowLayoutPanel3;
        private Button CloseoutButton;
        private Button ResetButton;
        private TableLayoutPanel tableLayoutPanel7;
        private Button RefundPaymentButton;
        private Button VoidButton;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        //private TableLayoutPanel tableLayoutPanelReceiptButton;
        //private Button ManualRefundReceiptButton;
        private Button ManualRefundButton;
        private TextBox RefundAmount;
        private Label label5;
        private CustomTableLayoutPanel tableLayoutPanel8;
        private CustomTableLayoutPanel tableLayoutPanel18;
        private Label label9;
        private Label labelTS;
        private TextBox DisplayMessageTextbox;
        private Label label8;
        private TextBox PrintTextBox;
        private TextBox PrintURLTextBox;
        private SplitContainer splitContainer3;
        private TableLayoutPanel tableLayoutPanel9;
        private FlowLayoutPanel flowLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel10;
        private TableLayoutPanel tableLayoutPanel11;
        private TabControl RegisterTabs;
        private TabPage tabPage5;
        private TableLayoutPanel tableLayoutPanel12;
        private TabPage tabPage6;
        private TableLayoutPanel tableLayoutPanel13;
        private Panel SelectedItemPanel;
        private Button DiscountButton;
        private Label ItemNameLabel;
        private Button DoneEditingLineItemButton;
        private Button RemoveItemButton;
        private TextBox ItemQuantityTextbox;
        private Label label7;
        private Label labelTipAmount;
        private Label labelSignatureThreshold;
        private Button IncrementQuantityButton;
        private Button DecrementQuantityButton;
        private TableLayoutPanel tableLayoutPanel14;
        private Label DeviceCurrentStatus;
        private FlowLayoutPanel UIStateButtonPanel;
        private Label label4;
        private Button DisplayMessageButton;
        private DropDownButton PrintTextButton;
        private Button ShowWelcomeButton;
        private Button ShowReceiptButton;
        private Button ShowThankYouButton;
        private DropDownButton OpenCashDrawerButton;
        private Button RetrievePrintJobStatusButton;
        private Button CardDataButton;
        private TableLayoutPanel tableLayoutPanel15;
        private Label label11;
        private Label label12;
        private Label label52;
        private DropDownButton PrintImageButton;
        private TableLayoutPanel tableLayoutPanel16;
        private TableLayoutPanel tableLayoutPanel96;
        private TableLayoutPanel tableLayoutPanelTipAmount;
        private TableLayoutPanel tableLayoutPanelSignatureThreshold;
        private Button BrowseImageButton;
        private PictureBox PrintImage;
        private ToolStripMenuItem RemoteRESTServiceMenuItem;
        private DropDownButton AuthButton;
        private Button TipAdjustButton;
        private FlowLayoutPanel flowLayoutPanel5;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanelCNP;
        private Label labelCNP;
        private CheckBox DisableCashBack;
        private CheckBox DisableRestartTransactionOnFailure;
        private CheckBox ManualEntryCheckbox;
        private CheckBox MagStripeCheckbox;
        private CheckBox ChipCheckbox;
        private CheckBox ContactlessCheckbox;
        private CheckBox CardNotPresentCheckbox;
        private TabPage Cards;
        private ListView cardsListView;
        private ColumnHeader CardName;
        private ColumnHeader First6;
        private ColumnHeader Last4;
        private ColumnHeader Exp_Month;
        private ColumnHeader Exp_Year;
        private ColumnHeader Token;
        private ListView ratingsListView;
        private ColumnHeader RatingID;
        private ColumnHeader RatingQuestion;
        private ColumnHeader RatingValue;
        private Button VaultCardBtn;
        private TableLayoutPanel tableLayoutPanel17;
        private TableLayoutPanel tableLayoutPanelForceOffline;
        private Button OpenOrder_Button;
        private TabPage tabPage7;
        private ListView PreAuthListView;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private Button PreAuthButton;
        private FlowLayoutPanel flowLayoutPanelForceOffline;
        private FlowLayoutPanel flowLayoutPanel6;
        private Label label13;
        private FlowLayoutPanel flowLayoutPanel4;
        private FlowLayoutPanel flowLayoutPanel94;
        private FlowLayoutPanel flowLayoutPanelTipMode;
        private FlowLayoutPanel flowLayoutPanelSigLoc;
        private Label labelTipMode;
        private Label labelSigLoc;
        private Label label14;
        private Label label84;
        private Label labelForceOffline;
        private RadioButton offlineDefault;
        private RadioButton offlineYes;
        private RadioButton offlineNo;
        private RadioButton signatureDefault;
        private RadioButton signatureOnScreen;
        private RadioButton signatureOnPaper;
        private RadioButton signatureNone;
        private CurrencyTextBox tipAmount;
        private CurrencyTextBox signatureThreshold;
        private RadioButton tipModeDefault;
        private RadioButton tipModeOnScreen;
        private RadioButton tipModeOnPaper;
        private RadioButton tipModeProvided;
        private RadioButton tipModeNone;
        private RadioButton approveOfflineDefault;
        private RadioButton approveOfflineYes;
        private RadioButton approveOfflineNo;
        private RadioButton forceOfflineDefault;
        private RadioButton forceOfflineYes;
        private RadioButton forceOfflineNo;
        private ColumnHeader externalID;
        private TabPage tabPage8;
        private ListView pendingPaymentListView;
        private ColumnHeader paymentIdHeader;
        private ColumnHeader paymentAmountHeader;
        private Button refreshPendingPayments;
        private CheckBox disablePrintingCB;
        private CheckBox disableReceiptOptionsCB;
        private CheckBox disableDuplicateCheckingCB;
        private CheckBox automaticSignatureConfirmationCB;
        private CheckBox automaticPaymentConfirmationCB;
        private CheckBox nonBlockingCB;
        private ComboBox customActivityAction;
        private TextBox customActivityPayload;
        private TabPage tabPage9;
        private Button StartCustomActivityBtn;
        private Button SendMessageBtn;
        private DropDownButton DeviceStatusButton;
        private Label RetrievePaymentLabel;
        private Button RetrievePaymentButton;
        private TextBox RetrievePaymentText;
    }
    public class ComboboxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
