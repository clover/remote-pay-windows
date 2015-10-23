using System;

namespace CloverExamplePOS
{
    partial class CloverExamplePOSForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CloverExamplePOSForm));
            this.ConnectStatusLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.DeviceMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.TestDeviceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloverMiniUSBMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WebSocketMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.PayButton = new System.Windows.Forms.Button();
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
            this.OrdersListView = new System.Windows.Forms.ListView();
            this.idHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.totalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dateHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.subTotalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.taxHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.CloseoutButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.PaymentRefundButton = new System.Windows.Forms.Button();
            this.VoidButton = new System.Windows.Forms.Button();
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
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.DisplayMessageTextbox = new System.Windows.Forms.TextBox();
            this.DisplayMessageButton = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.PrintTextBox = new System.Windows.Forms.TextBox();
            this.PrintTextButton = new System.Windows.Forms.Button();
            this.ShowWelcomeButton = new System.Windows.Forms.Button();
            this.ShowReceiptButton = new System.Windows.Forms.Button();
            this.ShowThankYouButton = new System.Windows.Forms.Button();
            this.OpenCashDrawerButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.DeviceCurrentStatus = new System.Windows.Forms.Label();
            this.UIStateButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.PrintImageButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.BrowseImageButton = new System.Windows.Forms.Button();
            this.PrintImage = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PrintImage)).BeginInit();
            this.SuspendLayout();
            // 
            // ConnectStatusLabel
            // 
            this.ConnectStatusLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ConnectStatusLabel.Location = new System.Drawing.Point(47, 0);
            this.ConnectStatusLabel.Name = "ConnectStatusLabel";
            this.ConnectStatusLabel.Size = new System.Drawing.Size(100, 50);
            this.ConnectStatusLabel.TabIndex = 17;
            this.ConnectStatusLabel.Text = "Not Connected";
            this.ConnectStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeviceMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(909, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // DeviceMenu
            // 
            this.DeviceMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TestDeviceMenuItem,
            this.CloverMiniUSBMenuItem,
            this.WebSocketMenuItem});
            this.DeviceMenu.Name = "DeviceMenu";
            this.DeviceMenu.Size = new System.Drawing.Size(54, 20);
            this.DeviceMenu.Text = "Device";
            // 
            // TestDeviceMenuItem
            // 
            this.TestDeviceMenuItem.Checked = true;
            this.TestDeviceMenuItem.CheckOnClick = true;
            this.TestDeviceMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TestDeviceMenuItem.Name = "TestDeviceMenuItem";
            this.TestDeviceMenuItem.Size = new System.Drawing.Size(169, 22);
            this.TestDeviceMenuItem.Text = "Test Device";
            this.TestDeviceMenuItem.Click += new System.EventHandler(this.TestDeviceMenuItem_Click);
            // 
            // CloverMiniUSBMenuItem
            // 
            this.CloverMiniUSBMenuItem.CheckOnClick = true;
            this.CloverMiniUSBMenuItem.Name = "CloverMiniUSBMenuItem";
            this.CloverMiniUSBMenuItem.Size = new System.Drawing.Size(169, 22);
            this.CloverMiniUSBMenuItem.Text = "Clover Mini (USB)";
            this.CloverMiniUSBMenuItem.Click += new System.EventHandler(this.CloverMiniUSBMenuItem_Click);
            // 
            // WebSocketMenuItem
            // 
            this.WebSocketMenuItem.CheckOnClick = true;
            this.WebSocketMenuItem.Name = "WebSocketMenuItem";
            this.WebSocketMenuItem.Size = new System.Drawing.Size(169, 22);
            this.WebSocketMenuItem.Text = "Clover Mini (LAN)";
            this.WebSocketMenuItem.Click += new System.EventHandler(this.WebSocketMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.TabControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(909, 572);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // TabControl
            // 
            this.TabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Controls.Add(this.tabPage3);
            this.TabControl.Controls.Add(this.tabPage4);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Margin = new System.Windows.Forms.Padding(0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(909, 522);
            this.TabControl.TabIndex = 13;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer3);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(901, 493);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Register";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
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
            this.splitContainer3.Size = new System.Drawing.Size(895, 487);
            this.splitContainer3.SplitterDistance = 251;
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
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(251, 487);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.currentOrder);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(245, 24);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Current Order :";
            // 
            // currentOrder
            // 
            this.currentOrder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.currentOrder.AutoSize = true;
            this.currentOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentOrder.Location = new System.Drawing.Point(117, 0);
            this.currentOrder.Name = "currentOrder";
            this.currentOrder.Size = new System.Drawing.Size(15, 16);
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
            this.OrderItems.Location = new System.Drawing.Point(3, 33);
            this.OrderItems.MultiSelect = false;
            this.OrderItems.Name = "OrderItems";
            this.OrderItems.Size = new System.Drawing.Size(245, 296);
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
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 335);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 4;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(245, 104);
            this.tableLayoutPanel10.TabIndex = 17;
            // 
            // DiscountLabel
            // 
            this.DiscountLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DiscountLabel.ForeColor = System.Drawing.Color.ForestGreen;
            this.DiscountLabel.Location = new System.Drawing.Point(58, 7);
            this.DiscountLabel.Name = "DiscountLabel";
            this.DiscountLabel.Size = new System.Drawing.Size(184, 13);
            this.DiscountLabel.TabIndex = 28;
            this.DiscountLabel.Text = "None";
            this.DiscountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 81);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.label1.Size = new System.Drawing.Size(31, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "Total";
            // 
            // TotalAmount
            // 
            this.TotalAmount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalAmount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TotalAmount.Location = new System.Drawing.Point(58, 67);
            this.TotalAmount.Name = "TotalAmount";
            this.TotalAmount.Size = new System.Drawing.Size(184, 37);
            this.TotalAmount.TabIndex = 20;
            this.TotalAmount.Text = "$0.00";
            this.TotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TaxAmount
            // 
            this.TaxAmount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TaxAmount.Location = new System.Drawing.Point(58, 44);
            this.TaxAmount.Name = "TaxAmount";
            this.TaxAmount.Size = new System.Drawing.Size(184, 16);
            this.TaxAmount.TabIndex = 21;
            this.TaxAmount.Text = "$0.00";
            this.TaxAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SubTotal
            // 
            this.SubTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SubTotal.Location = new System.Drawing.Point(58, 27);
            this.SubTotal.Name = "SubTotal";
            this.SubTotal.Size = new System.Drawing.Size(184, 13);
            this.SubTotal.TabIndex = 22;
            this.SubTotal.Text = "$0.00";
            this.SubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Subtotal";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Discount";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Tax";
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 2;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel11.Controls.Add(this.PayButton, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.newOrderBtn, 0, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 445);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(245, 39);
            this.tableLayoutPanel11.TabIndex = 18;
            // 
            // PayButton
            // 
            this.PayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.PayButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PayButton.Location = new System.Drawing.Point(88, 3);
            this.PayButton.Name = "PayButton";
            this.PayButton.Size = new System.Drawing.Size(154, 33);
            this.PayButton.TabIndex = 17;
            this.PayButton.Text = "Pay";
            this.PayButton.UseVisualStyleBackColor = false;
            this.PayButton.Click += new System.EventHandler(this.PayButton_Click);
            // 
            // newOrderBtn
            // 
            this.newOrderBtn.BackColor = System.Drawing.Color.White;
            this.newOrderBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newOrderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newOrderBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newOrderBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.newOrderBtn.Location = new System.Drawing.Point(3, 3);
            this.newOrderBtn.Name = "newOrderBtn";
            this.newOrderBtn.Size = new System.Drawing.Size(79, 33);
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
            this.RegisterTabs.Size = new System.Drawing.Size(640, 487);
            this.RegisterTabs.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tableLayoutPanel12);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(632, 461);
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
            this.tableLayoutPanel12.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 2;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(626, 455);
            this.tableLayoutPanel12.TabIndex = 0;
            // 
            // StoreItems
            // 
            this.StoreItems.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.StoreItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StoreItems.Location = new System.Drawing.Point(3, 0);
            this.StoreItems.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.StoreItems.Name = "StoreItems";
            this.StoreItems.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.StoreItems.Size = new System.Drawing.Size(620, 380);
            this.StoreItems.TabIndex = 7;
            // 
            // StoreDiscounts
            // 
            this.StoreDiscounts.BackColor = System.Drawing.Color.White;
            this.StoreDiscounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StoreDiscounts.Location = new System.Drawing.Point(3, 380);
            this.StoreDiscounts.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.StoreDiscounts.Name = "StoreDiscounts";
            this.StoreDiscounts.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.StoreDiscounts.Size = new System.Drawing.Size(620, 75);
            this.StoreDiscounts.TabIndex = 18;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.tableLayoutPanel13);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(632, 461);
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
            this.tableLayoutPanel13.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 1;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel13.Size = new System.Drawing.Size(626, 455);
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
            this.SelectedItemPanel.Location = new System.Drawing.Point(57, 7);
            this.SelectedItemPanel.Name = "SelectedItemPanel";
            this.SelectedItemPanel.Size = new System.Drawing.Size(511, 440);
            this.SelectedItemPanel.TabIndex = 18;
            // 
            // DiscountButton
            // 
            this.DiscountButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DiscountButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DiscountButton.Location = new System.Drawing.Point(295, 142);
            this.DiscountButton.Name = "DiscountButton";
            this.DiscountButton.Size = new System.Drawing.Size(131, 32);
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
            this.ItemNameLabel.Name = "ItemNameLabel";
            this.ItemNameLabel.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
            this.ItemNameLabel.Size = new System.Drawing.Size(508, 41);
            this.ItemNameLabel.TabIndex = 6;
            this.ItemNameLabel.Text = "Item Name";
            // 
            // DoneEditingLineItemButton
            // 
            this.DoneEditingLineItemButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DoneEditingLineItemButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoneEditingLineItemButton.Location = new System.Drawing.Point(151, 397);
            this.DoneEditingLineItemButton.Name = "DoneEditingLineItemButton";
            this.DoneEditingLineItemButton.Size = new System.Drawing.Size(275, 32);
            this.DoneEditingLineItemButton.TabIndex = 5;
            this.DoneEditingLineItemButton.Text = "Done";
            this.DoneEditingLineItemButton.UseVisualStyleBackColor = true;
            this.DoneEditingLineItemButton.Click += new System.EventHandler(this.DoneEditingLineItem_Click);
            // 
            // RemoveItemButton
            // 
            this.RemoveItemButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveItemButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveItemButton.Location = new System.Drawing.Point(151, 142);
            this.RemoveItemButton.Name = "RemoveItemButton";
            this.RemoveItemButton.Size = new System.Drawing.Size(138, 32);
            this.RemoveItemButton.TabIndex = 4;
            this.RemoveItemButton.Text = "Remove Item";
            this.RemoveItemButton.UseVisualStyleBackColor = true;
            this.RemoveItemButton.Click += new System.EventHandler(this.RemoveItemButton_Click);
            // 
            // ItemQuantityTextbox
            // 
            this.ItemQuantityTextbox.Enabled = false;
            this.ItemQuantityTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemQuantityTextbox.Location = new System.Drawing.Point(269, 85);
            this.ItemQuantityTextbox.Name = "ItemQuantityTextbox";
            this.ItemQuantityTextbox.Size = new System.Drawing.Size(43, 31);
            this.ItemQuantityTextbox.TabIndex = 3;
            this.ItemQuantityTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(268, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Quantity";
            // 
            // IncrementQuantityButton
            // 
            this.IncrementQuantityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IncrementQuantityButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncrementQuantityButton.Location = new System.Drawing.Point(351, 69);
            this.IncrementQuantityButton.Name = "IncrementQuantityButton";
            this.IncrementQuantityButton.Size = new System.Drawing.Size(75, 47);
            this.IncrementQuantityButton.TabIndex = 1;
            this.IncrementQuantityButton.Text = "+";
            this.IncrementQuantityButton.UseVisualStyleBackColor = true;
            this.IncrementQuantityButton.Click += new System.EventHandler(this.IncrementQuantityButton_Click);
            // 
            // DecrementQuantityButton
            // 
            this.DecrementQuantityButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DecrementQuantityButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DecrementQuantityButton.Location = new System.Drawing.Point(151, 69);
            this.DecrementQuantityButton.Name = "DecrementQuantityButton";
            this.DecrementQuantityButton.Size = new System.Drawing.Size(75, 47);
            this.DecrementQuantityButton.TabIndex = 0;
            this.DecrementQuantityButton.Text = "-";
            this.DecrementQuantityButton.UseVisualStyleBackColor = true;
            this.DecrementQuantityButton.Click += new System.EventHandler(this.DecrementQuantityButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(901, 493);
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
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(895, 487);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.OrdersListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(889, 416);
            this.splitContainer1.SplitterDistance = 272;
            this.splitContainer1.TabIndex = 0;
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
            this.OrdersListView.Location = new System.Drawing.Point(0, 0);
            this.OrdersListView.MultiSelect = false;
            this.OrdersListView.Name = "OrdersListView";
            this.OrdersListView.Size = new System.Drawing.Size(889, 272);
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
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.OrderDetailsListView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.OrderPaymentsView);
            this.splitContainer2.Size = new System.Drawing.Size(889, 140);
            this.splitContainer2.SplitterDistance = 399;
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
            this.OrderDetailsListView.MultiSelect = false;
            this.OrderDetailsListView.Name = "OrderDetailsListView";
            this.OrderDetailsListView.Size = new System.Drawing.Size(399, 140);
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
            this.paymentTotalHeader1});
            this.OrderPaymentsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OrderPaymentsView.FullRowSelect = true;
            this.OrderPaymentsView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.OrderPaymentsView.Location = new System.Drawing.Point(0, 0);
            this.OrderPaymentsView.MultiSelect = false;
            this.OrderPaymentsView.Name = "OrderPaymentsView";
            this.OrderPaymentsView.Size = new System.Drawing.Size(486, 140);
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
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel7, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 425);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(889, 59);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.CloseoutButton);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(438, 53);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // CloseoutButton
            // 
            this.CloseoutButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CloseoutButton.BackColor = System.Drawing.Color.White;
            this.CloseoutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseoutButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseoutButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CloseoutButton.Location = new System.Drawing.Point(3, 3);
            this.CloseoutButton.Name = "CloseoutButton";
            this.CloseoutButton.Size = new System.Drawing.Size(71, 50);
            this.CloseoutButton.TabIndex = 27;
            this.CloseoutButton.Text = "Closeout";
            this.CloseoutButton.UseVisualStyleBackColor = false;
            this.CloseoutButton.Click += new System.EventHandler(this.CloseoutButton_Click);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.Controls.Add(this.PaymentRefundButton, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.VoidButton, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(447, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(439, 53);
            this.tableLayoutPanel7.TabIndex = 1;
            // 
            // PaymentRefundButton
            // 
            this.PaymentRefundButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PaymentRefundButton.BackColor = System.Drawing.Color.White;
            this.PaymentRefundButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PaymentRefundButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PaymentRefundButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.PaymentRefundButton.Location = new System.Drawing.Point(357, 3);
            this.PaymentRefundButton.Name = "PaymentRefundButton";
            this.PaymentRefundButton.Size = new System.Drawing.Size(79, 47);
            this.PaymentRefundButton.TabIndex = 30;
            this.PaymentRefundButton.Text = "Refund";
            this.PaymentRefundButton.UseVisualStyleBackColor = false;
            this.PaymentRefundButton.Click += new System.EventHandler(this.PaymentRefundButton_Click);
            // 
            // VoidButton
            // 
            this.VoidButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.VoidButton.BackColor = System.Drawing.Color.White;
            this.VoidButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VoidButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VoidButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.VoidButton.Location = new System.Drawing.Point(275, 3);
            this.VoidButton.Name = "VoidButton";
            this.VoidButton.Size = new System.Drawing.Size(76, 47);
            this.VoidButton.TabIndex = 31;
            this.VoidButton.Text = "Void";
            this.VoidButton.UseVisualStyleBackColor = false;
            this.VoidButton.Click += new System.EventHandler(this.VoidButton_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel5);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(901, 493);
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
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(895, 487);
            this.tableLayoutPanel5.TabIndex = 11;
            // 
            // TransactionsListView
            // 
            this.TransactionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TransAmountHeader,
            this.TransDateHeader,
            this.TransLast4});
            this.TransactionsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TransactionsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.TransactionsListView.Location = new System.Drawing.Point(3, 68);
            this.TransactionsListView.Name = "TransactionsListView";
            this.TransactionsListView.Size = new System.Drawing.Size(889, 416);
            this.TransactionsListView.TabIndex = 10;
            this.TransactionsListView.UseCompatibleStateImageBehavior = false;
            this.TransactionsListView.View = System.Windows.Forms.View.Details;
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
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.ManualRefundButton, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.RefundAmount, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(889, 59);
            this.tableLayoutPanel6.TabIndex = 11;
            // 
            // ManualRefundButton
            // 
            this.ManualRefundButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ManualRefundButton.BackColor = System.Drawing.Color.White;
            this.ManualRefundButton.Enabled = false;
            this.ManualRefundButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ManualRefundButton.Location = new System.Drawing.Point(180, 5);
            this.ManualRefundButton.Name = "ManualRefundButton";
            this.ManualRefundButton.Size = new System.Drawing.Size(66, 49);
            this.ManualRefundButton.TabIndex = 16;
            this.ManualRefundButton.Text = "Refund";
            this.ManualRefundButton.UseVisualStyleBackColor = false;
            this.ManualRefundButton.Click += new System.EventHandler(this.ManualRefundButton_Click);
            // 
            // RefundAmount
            // 
            this.RefundAmount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RefundAmount.Location = new System.Drawing.Point(90, 19);
            this.RefundAmount.Name = "RefundAmount";
            this.RefundAmount.Size = new System.Drawing.Size(84, 20);
            this.RefundAmount.TabIndex = 15;
            this.RefundAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RefundAmount_KeyPress);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Refund Amount";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(901, 493);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Test";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 5;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.PrintImage, 3, 2);
            this.tableLayoutPanel8.Controls.Add(this.DisplayMessageTextbox, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.DisplayMessageButton, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.PrintTextBox, 1, 1);
            this.tableLayoutPanel8.Controls.Add(this.PrintTextButton, 2, 1);
            this.tableLayoutPanel8.Controls.Add(this.ShowWelcomeButton, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.ShowReceiptButton, 0, 6);
            this.tableLayoutPanel8.Controls.Add(this.ShowThankYouButton, 0, 8);
            this.tableLayoutPanel8.Controls.Add(this.OpenCashDrawerButton, 0, 9);
            this.tableLayoutPanel8.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.PrintImageButton, 2, 2);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel16, 1, 2);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 10;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(895, 487);
            this.tableLayoutPanel8.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Display Message: ";
            // 
            // DisplayMessageTextbox
            // 
            this.DisplayMessageTextbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DisplayMessageTextbox.Location = new System.Drawing.Point(102, 17);
            this.DisplayMessageTextbox.Name = "DisplayMessageTextbox";
            this.DisplayMessageTextbox.Size = new System.Drawing.Size(100, 20);
            this.DisplayMessageTextbox.TabIndex = 22;
            // 
            // DisplayMessageButton
            // 
            this.DisplayMessageButton.BackColor = System.Drawing.Color.White;
            this.DisplayMessageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DisplayMessageButton.Location = new System.Drawing.Point(208, 3);
            this.DisplayMessageButton.Name = "DisplayMessageButton";
            this.DisplayMessageButton.Size = new System.Drawing.Size(75, 49);
            this.DisplayMessageButton.TabIndex = 21;
            this.DisplayMessageButton.Text = "Display";
            this.DisplayMessageButton.UseVisualStyleBackColor = false;
            this.DisplayMessageButton.Click += new System.EventHandler(this.DisplayMessageButton_Click);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Print Message: ";
            // 
            // PrintTextBox
            // 
            this.PrintTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PrintTextBox.Location = new System.Drawing.Point(102, 72);
            this.PrintTextBox.Name = "PrintTextBox";
            this.PrintTextBox.Size = new System.Drawing.Size(100, 20);
            this.PrintTextBox.TabIndex = 19;
            // 
            // PrintTextButton
            // 
            this.PrintTextButton.BackColor = System.Drawing.Color.White;
            this.PrintTextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrintTextButton.Location = new System.Drawing.Point(208, 58);
            this.PrintTextButton.Name = "PrintTextButton";
            this.PrintTextButton.Size = new System.Drawing.Size(75, 49);
            this.PrintTextButton.TabIndex = 18;
            this.PrintTextButton.Text = "Print";
            this.PrintTextButton.UseVisualStyleBackColor = false;
            this.PrintTextButton.Click += new System.EventHandler(this.PrintTextButton_Click);
            // 
            // ShowWelcomeButton
            // 
            this.ShowWelcomeButton.BackColor = System.Drawing.Color.White;
            this.ShowWelcomeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowWelcomeButton.Location = new System.Drawing.Point(3, 169);
            this.ShowWelcomeButton.Name = "ShowWelcomeButton";
            this.ShowWelcomeButton.Size = new System.Drawing.Size(75, 49);
            this.ShowWelcomeButton.TabIndex = 24;
            this.ShowWelcomeButton.Text = "Show Welcome";
            this.ShowWelcomeButton.UseVisualStyleBackColor = false;
            this.ShowWelcomeButton.Click += new System.EventHandler(this.ShowWelcomeButton_Click);
            // 
            // ShowReceiptButton
            // 
            this.ShowReceiptButton.BackColor = System.Drawing.Color.White;
            this.ShowReceiptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowReceiptButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.ShowReceiptButton.Location = new System.Drawing.Point(3, 224);
            this.ShowReceiptButton.Name = "ShowReceiptButton";
            this.ShowReceiptButton.Size = new System.Drawing.Size(75, 49);
            this.ShowReceiptButton.TabIndex = 25;
            this.ShowReceiptButton.Text = "Show Previous Receipt";
            this.ShowReceiptButton.UseVisualStyleBackColor = false;
            this.ShowReceiptButton.Click += new System.EventHandler(this.ShowReceiptButton_Click);
            // 
            // ShowThankYouButton
            // 
            this.ShowThankYouButton.BackColor = System.Drawing.Color.White;
            this.ShowThankYouButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowThankYouButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.ShowThankYouButton.Location = new System.Drawing.Point(3, 279);
            this.ShowThankYouButton.Name = "ShowThankYouButton";
            this.ShowThankYouButton.Size = new System.Drawing.Size(75, 50);
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
            this.OpenCashDrawerButton.Location = new System.Drawing.Point(3, 335);
            this.OpenCashDrawerButton.Name = "OpenCashDrawerButton";
            this.OpenCashDrawerButton.Size = new System.Drawing.Size(75, 49);
            this.OpenCashDrawerButton.TabIndex = 28;
            this.OpenCashDrawerButton.Text = "Open Cash Drawer";
            this.OpenCashDrawerButton.UseVisualStyleBackColor = false;
            this.OpenCashDrawerButton.Click += new System.EventHandler(this.OpenCashDrawerButton_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel14, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel15, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 522);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(909, 50);
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
            this.tableLayoutPanel14.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 1;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(753, 44);
            this.tableLayoutPanel14.TabIndex = 18;
            // 
            // DeviceCurrentStatus
            // 
            this.DeviceCurrentStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DeviceCurrentStatus.AutoSize = true;
            this.DeviceCurrentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceCurrentStatus.Location = new System.Drawing.Point(86, 12);
            this.DeviceCurrentStatus.Name = "DeviceCurrentStatus";
            this.DeviceCurrentStatus.Size = new System.Drawing.Size(24, 20);
            this.DeviceCurrentStatus.TabIndex = 22;
            this.DeviceCurrentStatus.Text = "...";
            // 
            // UIStateButtonPanel
            // 
            this.UIStateButtonPanel.AutoSize = true;
            this.UIStateButtonPanel.Location = new System.Drawing.Point(70, 3);
            this.UIStateButtonPanel.MinimumSize = new System.Drawing.Size(10, 0);
            this.UIStateButtonPanel.Name = "UIStateButtonPanel";
            this.UIStateButtonPanel.Size = new System.Drawing.Size(10, 0);
            this.UIStateButtonPanel.TabIndex = 21;
            this.UIStateButtonPanel.WrapContents = false;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 19;
            this.label4.Text = "Device:";
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 1;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Controls.Add(this.ConnectStatusLabel, 0, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(759, 0);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(150, 50);
            this.tableLayoutPanel15.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 131);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "Select Image";
            // 
            // PrintImageButton
            // 
            this.PrintImageButton.BackColor = System.Drawing.Color.White;
            this.PrintImageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrintImageButton.Location = new System.Drawing.Point(208, 113);
            this.PrintImageButton.Name = "PrintImageButton";
            this.PrintImageButton.Size = new System.Drawing.Size(75, 49);
            this.PrintImageButton.TabIndex = 30;
            this.PrintImageButton.Text = "Print Image";
            this.PrintImageButton.UseVisualStyleBackColor = false;
            this.PrintImageButton.Click += new System.EventHandler(this.PrintImageButton_Click);
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.ColumnCount = 1;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Controls.Add(this.BrowseImageButton, 0, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(99, 110);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 1;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(106, 56);
            this.tableLayoutPanel16.TabIndex = 31;
            // 
            // BrowseImageButton
            // 
            this.BrowseImageButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BrowseImageButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseImageButton.Location = new System.Drawing.Point(15, 16);
            this.BrowseImageButton.Margin = new System.Windows.Forms.Padding(0);
            this.BrowseImageButton.Name = "BrowseImageButton";
            this.BrowseImageButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseImageButton.TabIndex = 0;
            this.BrowseImageButton.Text = "Browse...";
            this.BrowseImageButton.UseVisualStyleBackColor = true;
            this.BrowseImageButton.Click += new System.EventHandler(this.BrowseImageButton_Click);
            // 
            // PrintImage
            // 
            this.PrintImage.Location = new System.Drawing.Point(289, 113);
            this.PrintImage.Name = "PrintImage";
            this.PrintImage.Size = new System.Drawing.Size(100, 50);
            this.PrintImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PrintImage.TabIndex = 1;
            this.PrintImage.TabStop = false;
            // 
            // CloverExamplePOSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 596);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CloverExamplePOSForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clover Example POS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExamplePOSForm_Closed);
            this.Load += new System.EventHandler(this.ExamplePOSForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
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
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel14.PerformLayout();
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PrintImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label ConnectStatusLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem DeviceMenu;
        private System.Windows.Forms.ToolStripMenuItem TestDeviceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloverMiniUSBMenuItem;
        private System.Windows.Forms.ToolStripMenuItem WebSocketMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.FlowLayoutPanel StoreDiscounts;
        private System.Windows.Forms.FlowLayoutPanel StoreItems;
        private System.Windows.Forms.Label DiscountLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button newOrderBtn;
        private System.Windows.Forms.Label currentOrder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label SubTotal;
        private System.Windows.Forms.Label TaxAmount;
        private System.Windows.Forms.Label TotalAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button PayButton;
        private System.Windows.Forms.ListView OrderItems;
        private System.Windows.Forms.ColumnHeader Quantity;
        private System.Windows.Forms.ColumnHeader Item;
        private System.Windows.Forms.ColumnHeader Price;
        private System.Windows.Forms.ColumnHeader DiscountHeader;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView OrdersListView;
        private System.Windows.Forms.ColumnHeader idHeader;
        private System.Windows.Forms.ColumnHeader totalHeader;
        private System.Windows.Forms.ColumnHeader dateHeader;
        private System.Windows.Forms.ColumnHeader statusHeader;
        private System.Windows.Forms.ColumnHeader subTotalHeader;
        private System.Windows.Forms.ColumnHeader taxHeader;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView OrderDetailsListView;
        private System.Windows.Forms.ColumnHeader quantityHeader;
        private System.Windows.Forms.ColumnHeader itemHeader;
        private System.Windows.Forms.ColumnHeader itemPriceHeader;
        private System.Windows.Forms.ListView OrderPaymentsView;
        private System.Windows.Forms.ColumnHeader PayStatusHeader;
        private System.Windows.Forms.ColumnHeader amountHeader;
        private System.Windows.Forms.ColumnHeader tipPaymentHeader;
        private System.Windows.Forms.ColumnHeader paymentTotalHeader1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView TransactionsListView;
        private System.Windows.Forms.ColumnHeader TransAmountHeader;
        private System.Windows.Forms.ColumnHeader TransDateHeader;
        private System.Windows.Forms.ColumnHeader TransLast4;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button CloseoutButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Button PaymentRefundButton;
        private System.Windows.Forms.Button VoidButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button ManualRefundButton;
        private System.Windows.Forms.TextBox RefundAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox DisplayMessageTextbox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox PrintTextBox;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
        private System.Windows.Forms.TabControl RegisterTabs;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
        private System.Windows.Forms.Panel SelectedItemPanel;
        private System.Windows.Forms.Button DiscountButton;
        private System.Windows.Forms.Label ItemNameLabel;
        private System.Windows.Forms.Button DoneEditingLineItemButton;
        private System.Windows.Forms.Button RemoveItemButton;
        private System.Windows.Forms.TextBox ItemQuantityTextbox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button IncrementQuantityButton;
        private System.Windows.Forms.Button DecrementQuantityButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
        private System.Windows.Forms.Label DeviceCurrentStatus;
        private System.Windows.Forms.FlowLayoutPanel UIStateButtonPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button DisplayMessageButton;
        private System.Windows.Forms.Button PrintTextButton;
        private System.Windows.Forms.Button ShowWelcomeButton;
        private System.Windows.Forms.Button ShowReceiptButton;
        private System.Windows.Forms.Button ShowThankYouButton;
        private System.Windows.Forms.Button OpenCashDrawerButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button PrintImageButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.Button BrowseImageButton;
        private System.Windows.Forms.PictureBox PrintImage;
    }
}

