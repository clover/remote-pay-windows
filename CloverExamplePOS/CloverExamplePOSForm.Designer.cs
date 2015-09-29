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
            this.TabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SelectedItemPanel = new System.Windows.Forms.Panel();
            this.DiscountButton = new System.Windows.Forms.Button();
            this.ItemNameLabel = new System.Windows.Forms.Label();
            this.DoneEditingLineItemButton = new System.Windows.Forms.Button();
            this.RemoveItemButton = new System.Windows.Forms.Button();
            this.ItemQuantityTextbox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.IncrementQuantityButton = new System.Windows.Forms.Button();
            this.DecrementQuantityButton = new System.Windows.Forms.Button();
            this.StoreItems = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.newOrderBtn = new System.Windows.Forms.Button();
            this.currentOrder = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SubTotal = new System.Windows.Forms.Label();
            this.TaxAmount = new System.Windows.Forms.Label();
            this.TotalAmount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PayButton = new System.Windows.Forms.Button();
            this.OrderItems = new System.Windows.Forms.ListView();
            this.Quantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Item = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Price = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DiscountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.OrdersListView = new System.Windows.Forms.ListView();
            this.idHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.totalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dateHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.subTotalHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.taxHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OrderPaymentsView = new System.Windows.Forms.ListView();
            this.PayStatusHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.amountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tipPaymentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.paymentTotalHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VoidButton = new System.Windows.Forms.Button();
            this.OrderDetailsListView = new System.Windows.Forms.ListView();
            this.quantityHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.itemHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.itemPriceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.TransactionsListView = new System.Windows.Forms.ListView();
            this.TransAmountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TransDateHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TransLast4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label5 = new System.Windows.Forms.Label();
            this.RefundAmount = new System.Windows.Forms.TextBox();
            this.ManualRefundButton = new System.Windows.Forms.Button();
            this.DeviceCurrentStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ConnectStatusLabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.DeviceMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.TestDeviceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloverMiniUSBMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SelectedItemPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Controls.Add(this.tabPage3);
            this.TabControl.Location = new System.Drawing.Point(12, 32);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(817, 522);
            this.TabControl.TabIndex = 12;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SelectedItemPanel);
            this.tabPage1.Controls.Add(this.StoreItems);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(809, 493);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Register";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SelectedItemPanel
            // 
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
            this.SelectedItemPanel.Location = new System.Drawing.Point(262, 24);
            this.SelectedItemPanel.Name = "SelectedItemPanel";
            this.SelectedItemPanel.Size = new System.Drawing.Size(511, 440);
            this.SelectedItemPanel.TabIndex = 17;
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
            // StoreItems
            // 
            this.StoreItems.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.StoreItems.Location = new System.Drawing.Point(228, 6);
            this.StoreItems.Name = "StoreItems";
            this.StoreItems.Size = new System.Drawing.Size(581, 487);
            this.StoreItems.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.newOrderBtn);
            this.panel1.Controls.Add(this.currentOrder);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.SubTotal);
            this.panel1.Controls.Add(this.TaxAmount);
            this.panel1.Controls.Add(this.TotalAmount);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.PayButton);
            this.panel1.Controls.Add(this.OrderItems);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(227, 496);
            this.panel1.TabIndex = 16;
            // 
            // newOrderBtn
            // 
            this.newOrderBtn.BackColor = System.Drawing.Color.White;
            this.newOrderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newOrderBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newOrderBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.newOrderBtn.Location = new System.Drawing.Point(3, 449);
            this.newOrderBtn.Name = "newOrderBtn";
            this.newOrderBtn.Size = new System.Drawing.Size(58, 44);
            this.newOrderBtn.TabIndex = 26;
            this.newOrderBtn.Text = "New";
            this.newOrderBtn.UseVisualStyleBackColor = false;
            this.newOrderBtn.Click += new System.EventHandler(this.NewOrder_Click);
            // 
            // currentOrder
            // 
            this.currentOrder.AutoSize = true;
            this.currentOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentOrder.Location = new System.Drawing.Point(121, 5);
            this.currentOrder.Name = "currentOrder";
            this.currentOrder.Size = new System.Drawing.Size(15, 16);
            this.currentOrder.TabIndex = 25;
            this.currentOrder.Text = "0";
            this.currentOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Current Order :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 376);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Subtotal";
            // 
            // SubTotal
            // 
            this.SubTotal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SubTotal.Location = new System.Drawing.Point(67, 376);
            this.SubTotal.Name = "SubTotal";
            this.SubTotal.Size = new System.Drawing.Size(157, 13);
            this.SubTotal.TabIndex = 22;
            this.SubTotal.Text = "$0.00";
            this.SubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TaxAmount
            // 
            this.TaxAmount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TaxAmount.Location = new System.Drawing.Point(67, 389);
            this.TaxAmount.Name = "TaxAmount";
            this.TaxAmount.Size = new System.Drawing.Size(157, 16);
            this.TaxAmount.TabIndex = 21;
            this.TaxAmount.Text = "$0.00";
            this.TaxAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TotalAmount
            // 
            this.TotalAmount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalAmount.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TotalAmount.Location = new System.Drawing.Point(67, 408);
            this.TotalAmount.Name = "TotalAmount";
            this.TotalAmount.Size = new System.Drawing.Size(157, 37);
            this.TotalAmount.TabIndex = 20;
            this.TotalAmount.Text = "$0.00";
            this.TotalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 392);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Tax";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 432);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Total";
            // 
            // PayButton
            // 
            this.PayButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.PayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PayButton.Location = new System.Drawing.Point(67, 449);
            this.PayButton.Name = "PayButton";
            this.PayButton.Size = new System.Drawing.Size(157, 44);
            this.PayButton.TabIndex = 17;
            this.PayButton.Text = "Pay";
            this.PayButton.UseVisualStyleBackColor = false;
            this.PayButton.Click += new System.EventHandler(this.PayButton_Click);
            // 
            // OrderItems
            // 
            this.OrderItems.AutoArrange = false;
            this.OrderItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.OrderItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Quantity,
            this.Item,
            this.Price,
            this.DiscountHeader});
            this.OrderItems.FullRowSelect = true;
            this.OrderItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.OrderItems.Location = new System.Drawing.Point(1, 24);
            this.OrderItems.MultiSelect = false;
            this.OrderItems.Name = "OrderItems";
            this.OrderItems.Size = new System.Drawing.Size(223, 349);
            this.OrderItems.TabIndex = 16;
            this.OrderItems.UseCompatibleStateImageBehavior = false;
            this.OrderItems.View = System.Windows.Forms.View.Details;
            this.OrderItems.SelectedIndexChanged += new System.EventHandler(this.OrderItems_SelectedIndexChanged);
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(809, 493);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Orders";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            this.splitContainer1.Panel2.Controls.Add(this.OrderPaymentsView);
            this.splitContainer1.Panel2.Controls.Add(this.VoidButton);
            this.splitContainer1.Panel2.Controls.Add(this.OrderDetailsListView);
            this.splitContainer1.Size = new System.Drawing.Size(803, 487);
            this.splitContainer1.SplitterDistance = 265;
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
            this.OrdersListView.FullRowSelect = true;
            this.OrdersListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.OrdersListView.Location = new System.Drawing.Point(0, 0);
            this.OrdersListView.MultiSelect = false;
            this.OrdersListView.Name = "OrdersListView";
            this.OrdersListView.Size = new System.Drawing.Size(803, 262);
            this.OrdersListView.TabIndex = 0;
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
            // OrderPaymentsView
            // 
            this.OrderPaymentsView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OrderPaymentsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PayStatusHeader,
            this.amountHeader,
            this.tipPaymentHeader,
            this.paymentTotalHeader1});
            this.OrderPaymentsView.FullRowSelect = true;
            this.OrderPaymentsView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.OrderPaymentsView.Location = new System.Drawing.Point(365, 4);
            this.OrderPaymentsView.MultiSelect = false;
            this.OrderPaymentsView.Name = "OrderPaymentsView";
            this.OrderPaymentsView.Size = new System.Drawing.Size(438, 157);
            this.OrderPaymentsView.TabIndex = 4;
            this.OrderPaymentsView.UseCompatibleStateImageBehavior = false;
            this.OrderPaymentsView.View = System.Windows.Forms.View.Details;
            this.OrderPaymentsView.SelectedIndexChanged += new System.EventHandler(this.OrderPaymentsView_SelectedIndexChanged);
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
            // VoidButton
            // 
            this.VoidButton.Enabled = false;
            this.VoidButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VoidButton.Location = new System.Drawing.Point(724, 167);
            this.VoidButton.Name = "VoidButton";
            this.VoidButton.Size = new System.Drawing.Size(75, 49);
            this.VoidButton.TabIndex = 3;
            this.VoidButton.Text = "Void";
            this.VoidButton.UseVisualStyleBackColor = true;
            this.VoidButton.Click += new System.EventHandler(this.VoidButton_Click);
            // 
            // OrderDetailsListView
            // 
            this.OrderDetailsListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OrderDetailsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.quantityHeader,
            this.itemHeader,
            this.itemPriceHeader});
            this.OrderDetailsListView.FullRowSelect = true;
            this.OrderDetailsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.OrderDetailsListView.Location = new System.Drawing.Point(3, 3);
            this.OrderDetailsListView.MultiSelect = false;
            this.OrderDetailsListView.Name = "OrderDetailsListView";
            this.OrderDetailsListView.Size = new System.Drawing.Size(355, 158);
            this.OrderDetailsListView.TabIndex = 2;
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
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.TransactionsListView);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.RefundAmount);
            this.tabPage3.Controls.Add(this.ManualRefundButton);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(809, 493);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Refund";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // TransactionsListView
            // 
            this.TransactionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TransAmountHeader,
            this.TransDateHeader,
            this.TransLast4});
            this.TransactionsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.TransactionsListView.Location = new System.Drawing.Point(6, 77);
            this.TransactionsListView.Name = "TransactionsListView";
            this.TransactionsListView.Size = new System.Drawing.Size(797, 410);
            this.TransactionsListView.TabIndex = 10;
            this.TransactionsListView.UseCompatibleStateImageBehavior = false;
            this.TransactionsListView.View = System.Windows.Forms.View.Details;
            // 
            // TransDateHeader
            // 
            this.TransDateHeader.Width = 150;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Refund Amount";
            // 
            // RefundAmount
            // 
            this.RefundAmount.Location = new System.Drawing.Point(120, 36);
            this.RefundAmount.Name = "RefundAmount";
            this.RefundAmount.Size = new System.Drawing.Size(100, 20);
            this.RefundAmount.TabIndex = 8;
            this.RefundAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RefundAmount_KeyPress);
            // 
            // ManualRefundButton
            // 
            this.ManualRefundButton.Enabled = false;
            this.ManualRefundButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ManualRefundButton.Location = new System.Drawing.Point(226, 21);
            this.ManualRefundButton.Name = "ManualRefundButton";
            this.ManualRefundButton.Size = new System.Drawing.Size(75, 49);
            this.ManualRefundButton.TabIndex = 7;
            this.ManualRefundButton.Text = "Refund";
            this.ManualRefundButton.UseVisualStyleBackColor = true;
            this.ManualRefundButton.Click += new System.EventHandler(this.ManualRefundButton_Click);
            // 
            // DeviceCurrentStatus
            // 
            this.DeviceCurrentStatus.AutoSize = true;
            this.DeviceCurrentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeviceCurrentStatus.Location = new System.Drawing.Point(79, 559);
            this.DeviceCurrentStatus.Name = "DeviceCurrentStatus";
            this.DeviceCurrentStatus.Size = new System.Drawing.Size(24, 20);
            this.DeviceCurrentStatus.TabIndex = 13;
            this.DeviceCurrentStatus.Text = "...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 559);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Device:";
            // 
            // ConnectStatusLabel
            // 
            this.ConnectStatusLabel.Location = new System.Drawing.Point(725, 564);
            this.ConnectStatusLabel.Name = "ConnectStatusLabel";
            this.ConnectStatusLabel.Size = new System.Drawing.Size(100, 13);
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
            this.menuStrip1.Size = new System.Drawing.Size(841, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // DeviceMenu
            // 
            this.DeviceMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TestDeviceMenuItem,
            this.CloverMiniUSBMenuItem});
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
            this.TestDeviceMenuItem.Size = new System.Drawing.Size(167, 22);
            this.TestDeviceMenuItem.Text = "Test Device";
            this.TestDeviceMenuItem.Click += new System.EventHandler(this.TestDeviceMenuItem_Click);
            // 
            // CloverMiniUSBMenuItem
            // 
            this.CloverMiniUSBMenuItem.CheckOnClick = true;
            this.CloverMiniUSBMenuItem.Name = "CloverMiniUSBMenuItem";
            this.CloverMiniUSBMenuItem.Size = new System.Drawing.Size(167, 22);
            this.CloverMiniUSBMenuItem.Text = "Clover Mini (USB)";
            this.CloverMiniUSBMenuItem.Click += new System.EventHandler(this.CloverMiniUSBMenuItem_Click);
            // 
            // CloverExamplePOSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 587);
            this.Controls.Add(this.ConnectStatusLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DeviceCurrentStatus);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "CloverExamplePOSForm";
            this.Text = "CloverExamplePOS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExamplePOSForm_Closed);
            this.Load += new System.EventHandler(this.ExamplePOSForm_Load);
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.SelectedItemPanel.ResumeLayout(false);
            this.SelectedItemPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label DeviceCurrentStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label SubTotal;
        private System.Windows.Forms.Label TaxAmount;
        private System.Windows.Forms.Label TotalAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button PayButton;
        private System.Windows.Forms.ListView OrderItems;
        private System.Windows.Forms.ColumnHeader Item;
        private System.Windows.Forms.ColumnHeader Price;
        private System.Windows.Forms.Label currentOrder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button newOrderBtn;
        private System.Windows.Forms.ColumnHeader Quantity;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView OrdersListView;
        private System.Windows.Forms.ColumnHeader idHeader;
        private System.Windows.Forms.ColumnHeader totalHeader;
        private System.Windows.Forms.ColumnHeader dateHeader;
        private System.Windows.Forms.ColumnHeader statusHeader;
        private System.Windows.Forms.ColumnHeader subTotalHeader;
        private System.Windows.Forms.ColumnHeader taxHeader;
        private System.Windows.Forms.Button VoidButton;
        private System.Windows.Forms.ListView OrderDetailsListView;
        private System.Windows.Forms.ColumnHeader quantityHeader;
        private System.Windows.Forms.ColumnHeader itemHeader;
        private System.Windows.Forms.ColumnHeader itemPriceHeader;
        private System.Windows.Forms.ListView OrderPaymentsView;
        private System.Windows.Forms.ColumnHeader PayStatusHeader;
        private System.Windows.Forms.ColumnHeader amountHeader;
        private System.Windows.Forms.Label ConnectStatusLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox RefundAmount;
        private System.Windows.Forms.Button ManualRefundButton;
        private System.Windows.Forms.Panel SelectedItemPanel;
        private System.Windows.Forms.Label ItemNameLabel;
        private System.Windows.Forms.Button DoneEditingLineItemButton;
        private System.Windows.Forms.Button RemoveItemButton;
        private System.Windows.Forms.TextBox ItemQuantityTextbox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button IncrementQuantityButton;
        private System.Windows.Forms.Button DecrementQuantityButton;
        private System.Windows.Forms.FlowLayoutPanel StoreItems;
        private System.Windows.Forms.ListView TransactionsListView;
        private System.Windows.Forms.ColumnHeader TransAmountHeader;
        private System.Windows.Forms.ColumnHeader TransDateHeader;
        private System.Windows.Forms.ColumnHeader TransLast4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem DeviceMenu;
        private System.Windows.Forms.ToolStripMenuItem TestDeviceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloverMiniUSBMenuItem;
        private System.Windows.Forms.Button DiscountButton;
        private System.Windows.Forms.ColumnHeader DiscountHeader;
        private System.Windows.Forms.ColumnHeader tipPaymentHeader;
        private System.Windows.Forms.ColumnHeader paymentTotalHeader1;
    }
}

