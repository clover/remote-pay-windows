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
            this.StoreItems = new System.Windows.Forms.ListView();
            this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.priceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
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
            this.VoidButton = new System.Windows.Forms.Button();
            this.OrderDetailsListView = new System.Windows.Forms.ListView();
            this.quantityHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.itemHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.itemPriceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.RefundAmount = new System.Windows.Forms.TextBox();
            this.ManualRefundButton = new System.Windows.Forms.Button();
            this.DeviceCurrentStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ConnectStatusLabel = new System.Windows.Forms.Label();
            this.TabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // StoreItems
            // 
            this.StoreItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.priceHeader});
            this.StoreItems.Location = new System.Drawing.Point(233, 22);
            this.StoreItems.Name = "StoreItems";
            this.StoreItems.Size = new System.Drawing.Size(574, 468);
            this.StoreItems.TabIndex = 8;
            this.StoreItems.UseCompatibleStateImageBehavior = false;
            this.StoreItems.View = System.Windows.Forms.View.Tile;
            this.StoreItems.SelectedIndexChanged += new System.EventHandler(this.StoreItems_SelectedIndexChanged);
            // 
            // nameHeader
            // 
            this.nameHeader.Text = "Name";
            // 
            // priceHeader
            // 
            this.priceHeader.Text = "Price";
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.tabPage1);
            this.TabControl.Controls.Add(this.tabPage2);
            this.TabControl.Controls.Add(this.tabPage3);
            this.TabControl.Location = new System.Drawing.Point(12, 12);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(817, 522);
            this.TabControl.TabIndex = 12;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.StoreItems);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(809, 496);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Register";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.newOrderBtn.Click += new System.EventHandler(this.NewOrder_Click_1);
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
            this.OrderItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Quantity,
            this.Item,
            this.Price});
            this.OrderItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.OrderItems.Location = new System.Drawing.Point(1, 24);
            this.OrderItems.Name = "OrderItems";
            this.OrderItems.Size = new System.Drawing.Size(223, 349);
            this.OrderItems.TabIndex = 16;
            this.OrderItems.UseCompatibleStateImageBehavior = false;
            this.OrderItems.View = System.Windows.Forms.View.Details;
            // 
            // Quantity
            // 
            this.Quantity.Width = 20;
            // 
            // Item
            // 
            this.Item.Text = "Item";
            this.Item.Width = 120;
            // 
            // Price
            // 
            this.Price.Text = "Price";
            this.Price.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(809, 496);
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
            this.splitContainer1.Size = new System.Drawing.Size(803, 490);
            this.splitContainer1.SplitterDistance = 267;
            this.splitContainer1.TabIndex = 0;
            // 
            // OrdersListView
            // 
            this.OrdersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.idHeader,
            this.totalHeader,
            this.dateHeader,
            this.statusHeader,
            this.subTotalHeader,
            this.taxHeader});
            this.OrdersListView.FullRowSelect = true;
            this.OrdersListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.OrdersListView.Location = new System.Drawing.Point(0, 0);
            this.OrdersListView.MultiSelect = false;
            this.OrdersListView.Name = "OrdersListView";
            this.OrdersListView.Size = new System.Drawing.Size(803, 268);
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
            this.taxHeader.Text = "TaxHeader";
            this.taxHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // OrderPaymentsView
            // 
            this.OrderPaymentsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PayStatusHeader,
            this.amountHeader});
            this.OrderPaymentsView.FullRowSelect = true;
            this.OrderPaymentsView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.OrderPaymentsView.Location = new System.Drawing.Point(365, 4);
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
            this.amountHeader.Text = "Amount";
            this.amountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.amountHeader.Width = 150;
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
            this.OrderDetailsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.quantityHeader,
            this.itemHeader,
            this.itemPriceHeader});
            this.OrderDetailsListView.FullRowSelect = true;
            this.OrderDetailsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.OrderDetailsListView.Location = new System.Drawing.Point(3, 3);
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
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.RefundAmount);
            this.tabPage3.Controls.Add(this.ManualRefundButton);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(809, 496);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Refund";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            this.DeviceCurrentStatus.Location = new System.Drawing.Point(79, 539);
            this.DeviceCurrentStatus.Name = "DeviceCurrentStatus";
            this.DeviceCurrentStatus.Size = new System.Drawing.Size(24, 20);
            this.DeviceCurrentStatus.TabIndex = 13;
            this.DeviceCurrentStatus.Text = "...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 539);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Device:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(803, 540);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // ConnectStatusLabel
            // 
            this.ConnectStatusLabel.AutoSize = true;
            this.ConnectStatusLabel.Location = new System.Drawing.Point(718, 544);
            this.ConnectStatusLabel.Name = "ConnectStatusLabel";
            this.ConnectStatusLabel.Size = new System.Drawing.Size(79, 13);
            this.ConnectStatusLabel.TabIndex = 17;
            this.ConnectStatusLabel.Text = "Not Connected";
            // 
            // CloverExamplePOSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 566);
            this.Controls.Add(this.ConnectStatusLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DeviceCurrentStatus);
            this.Controls.Add(this.TabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CloverExamplePOSForm";
            this.Text = "CloverExamplePOS";
            this.Load += new System.EventHandler(this.ExamplePOSForm_Load);
            this.TabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView StoreItems;
        private System.Windows.Forms.ColumnHeader nameHeader;
        private System.Windows.Forms.ColumnHeader priceHeader;
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
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label ConnectStatusLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox RefundAmount;
        private System.Windows.Forms.Button ManualRefundButton;
    }
}

