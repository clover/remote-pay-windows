using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using com.clover.sdk.v3.payments;
using System.IO;
using com.clover.remote.order;

namespace CloverExamplePOS
{
    public partial class CloverExamplePOSForm : Form, CloverConnectionListener, CloverDeviceListener, CloverSaleListener, CloverVoidTransactionListener, CloverManualRefundListener, CloverSignatureListener
    {
        CloverConnector cloverConnector;
        Store Store;
        SynchronizationContext uiThread;

        DisplayOrder DisplayOrder;
        Dictionary<POSLineItem, DisplayLineItem> posLineItemToDisplayLineItem = new Dictionary<POSLineItem, DisplayLineItem>();
        POSLineItem SelectedLineItem = null;

        CloverDeviceConfiguration USBConfig = new USBCloverDeviceConfiguration("__deviceID__");
        CloverDeviceConfiguration TestConfig = new TestCloverDeviceConfiguration();

        string OriginalFormTitle;

        public CloverExamplePOSForm()
        {
            //new CaptureLog();

            InitializeComponent();
            uiThread = WindowsFormsSynchronizationContext.Current;


        }

        private void ExamplePOSForm_Load(object sender, EventArgs e)
        {
            OriginalFormTitle = this.Text;
            InitializeConnector(TestConfig);

            Store = new Store();
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Hamburger ", 439));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Cheeseburger ", 499));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Dbl. Hamburger ", 559));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Dbl. Cheeseburger ", 629));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Chicken Sandwich ", 699));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Deluxe Chicken Sandwich ", 749));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "French Fries - Small ", 189));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "French Fries - Medium ", 229));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "French Fries - Large ", 269));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Soft Drink - Small ", 174));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Soft Drink - Medium ", 189));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Soft Drink - Large ", 229));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Milk Shake - Vanilla ", 389));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Milk Shake - Chocolate ", 399));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Milk Shake - Strawberry ", 399));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Gift Card - $25 ", 2500, false));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Gift Card - $50 ", 5000, false));


            foreach (POSItem item in Store.AvailableItems)
            {
                StoreItem si = new StoreItem();
                si.Item = item;
                si += StoreItems_ItemSelected;

                StoreItems.Controls.Add(si);
            }

            NewOrder();

            UpdateUI();

        }

        //////////////// Sale methods /////////////
        private void PayButton_Click(object sender, EventArgs e)
        {
            StoreItems.BringToFront();

            PayButton.Enabled = false;
            StoreItems.Enabled = false;
            newOrderBtn.Enabled = false;

            SaleRequest request = new SaleRequest();
            request.Amount = Store.CurrentOrder.Total;
            request.TipAmount = 0;
            if(cloverConnector.Sale(request)  < 0)
            {
                PaymentReset();
            }
        }
        public void OnSaleResponse(SaleResponse response)
        {
            if(TransactionResponse.SUCCESS.Equals(response.Code))
            {
                Store.CurrentOrder.Status = POSOrder.OrderStatus.CLOSED;
                POSPayment payment = new POSPayment(response.Payment.id, response.Payment.order.id, response.Payment.employee.id, response.Payment.amount, response.Payment.tipAmount);
                payment.PaymentStatus = POSPayment.Status.PAID;
                Store.CurrentOrder.AddPayment(payment);


                uiThread.Send(delegate (object state) {
                    PaymentReset();
                    NewOrder();
                }, null);
            }
            else if(TransactionResponse.FAIL.Equals(response.Code))
            {
                uiThread.Send(delegate (object state) {
                    MessageBox.Show("Card authentication failed or was declined.");
                    PaymentReset();
                }, null);
            }
            else if (TransactionResponse.CANCEL.Equals(response.Code))
            {
                uiThread.Send(delegate (object state) {
                    MessageBox.Show("User canceled transaction.");
                    PaymentReset();
                }, null);
            }
        }


        //////////////// Void methods /////////////
        private void VoidButton_Click(object sender, EventArgs e)
        {
            VoidRequest request = new VoidRequest();
            if (OrderPaymentsView.SelectedItems.Count == 1)
            {
                POSPayment payment = ((POSPayment)OrderPaymentsView.SelectedItems[0].Tag);
                request.PaymentId = payment.PaymentID;
                request.EmployeeId = payment.EmployeeID;
                request.OrderId = payment.OrderID;
                request.VoidReason = "USER_CANCEL";

                cloverConnector.VoidTransaction(request);
            }
        }
        public void OnVoidTransactionResponse(VoidResponse response)
        {
            bool voided = false;
            foreach (POSOrder order in Store.Orders)
            {
                foreach (POSPayment payment in order.Payments)
                {
                    if(payment.PaymentID == response.PaymentId)
                    {
                        payment.PaymentStatus = POSPayment.Status.VOIDED;
                        voided = true;
                        break;
                    }
                }
                if(voided)
                {
                    break;
                }
            }
            uiThread.Send(delegate (object state) {
                VoidButton.Enabled = false;
                // shortbut to refresh UI
                OrderPaymentsView.SelectedItems[0].SubItems[0].Text = POSPayment.Status.VOIDED.ToString();
            }, null);

        }



        //////////////// Manual Refund methods /////////////
        private void ManualRefundButton_Click(object sender, EventArgs e)
        {
            ManualRefundRequest request = new ManualRefundRequest();
            request.Amount = int.Parse(RefundAmount.Text);
            cloverConnector.ManualRefund(request);
        }
        public void OnManualRefundResponse(ManualRefundResponse response)
        {

            if (TransactionResponse.SUCCESS.Equals(response.Code))
            {
                uiThread.Send(delegate (object state) {
                    ListViewItem lvi = new ListViewItem();
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                    lvi.SubItems[0].Text = (response.credit.amount / 100.0).ToString("C2");
                    lvi.SubItems[1].Text = new DateTime(response.credit.createdTime).ToLongDateString();
                    lvi.SubItems[2].Text = response.credit.cardTransaction.last4;


                    MessageBox.Show("Refund of " + (response.credit.amount / 100.0).ToString("C2") + " was applied to card ending with " + response.credit.cardTransaction.last4);
                    RefundAmount.Text = "0";

                    TransactionsListView.Items.Add(lvi);
                }, null);
            }
            else if (TransactionResponse.FAIL.Equals(response.Code))
            {
                uiThread.Send(delegate (object state) {
                    MessageBox.Show("Card authentication failed");
                    PaymentReset();
                }, null);
            }
            else if (TransactionResponse.CANCEL.Equals(response.Code))
            {
                uiThread.Send(delegate (object state) {
                    MessageBox.Show("User canceled transaction.");
                    PaymentReset();
                }, null);
            }

        }




        ////////////////// CloverDeviceLister Methods //////////////////////

        public void OnDeviceConnected()
        {
            uiThread.Send(delegate (object state) {
                ConnectStatusLabel.Text = "Connecting...";
            }, null);
        }

        public void OnDeviceReady()
        {
            uiThread.Send(delegate (object state) {
                ConnectStatusLabel.Text = "Connected";
                if(DisplayOrder.lineItems.elements.Count > 0)
                {
                    UpdateDisplayOrderTotals();
                    cloverConnector.DisplayOrder(DisplayOrder);
                }
                PaymentReset();
            }, null);
        }

        public void OnDeviceDisconnected()
        {
            try
            {
                uiThread.Send(delegate (object state) {
                    ConnectStatusLabel.Text = "Disconnected";
                    PaymentReset();
                }, null);
                
            }
            #pragma warning disable 0168
            catch (Exception e)
            #pragma warning restore 0168
            {
                // uiThread is gone on shutdown
            }
        }




        ////////////////// CloverDeviceLister Methods //////////////////////
        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            uiThread.Send(delegate (object state) {
                DeviceCurrentStatus.Text = deviceEvent.Message;
            }, null);
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            try
            {
                uiThread.Send(delegate (object state) {
                    DeviceCurrentStatus.Text = " ";
                }, null);
            }
            #pragma warning disable 0168
            catch (Exception e)
            #pragma warning restore 0168
            {
                // if UI goes away, uiThread may be disposed
            }
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            MessageBox.Show(deviceErrorEvent.Message);
        }




        ////////////////// CloverSignatureLister Methods //////////////////////
        /// <summary>
        /// Handle a request from the Clover device to verify a signature
        /// </summary>
        /// <param name="request"></param>
        public void OnSignatureVerifyRequest(SignatureVerifyRequest request)
        {
            uiThread.Send(delegate (object state)
            {
                SignatureForm sigForm = new SignatureForm();
                sigForm.SignatureVerifyRequest = request;
                sigForm.ShowDialog(this);
            }, null);
            
        }



        ////////////////// UI Events and UI Management //////////////////////

        private void StoreItems_ItemSelected(object sender, EventArgs e)
        {
            POSItem item = ((StoreItem)((Control)sender).Parent).Item;
            POSLineItem lineItem = Store.CurrentOrder.AddItem(item, 1);

            DisplayLineItem displayLineItem = null;
            posLineItemToDisplayLineItem.TryGetValue(lineItem, out displayLineItem);
            if (displayLineItem == null)
            {
                displayLineItem = DisplayFactory.createDisplayLineItem();
                posLineItemToDisplayLineItem[lineItem] = displayLineItem;
                displayLineItem.quantity = "1";
                displayLineItem.name = lineItem.Item.Name;
                displayLineItem.price = (lineItem.Item.Price / 100.0).ToString("C2");
                DisplayOrder.addDisplayLineItem(displayLineItem);
                UpdateDisplayOrderTotals();
                cloverConnector.DisplayOrderLineItemAdded(DisplayOrder, displayLineItem);
            }
            else
            {
                displayLineItem.quantity = lineItem.Quantity.ToString();
                UpdateDisplayOrderTotals();
                cloverConnector.DisplayOrder(DisplayOrder);
            }

            UpdateUI();
        }

        private void UpdateDisplayOrderTotals()
        {
            DisplayOrder.tax = (Store.CurrentOrder.TaxAmount / 100.0).ToString("C2");
            DisplayOrder.subtotal = (Store.CurrentOrder.SubTotal / 100.0).ToString("C2");
            DisplayOrder.total = (Store.CurrentOrder.Total / 100.0).ToString("C2");

            //DisplayOrder.lineItems.elements.Clear();
        }

        private void NewOrder_Click(object sender, EventArgs e)
        {
            NewOrder();
        }

        private void NewOrder()
        {
            Store.CreateOrder();
            StoreItems.BringToFront();

            DisplayOrder = DisplayFactory.createDisplayOrder();
            DisplayOrder.title = Guid.NewGuid().ToString();
            posLineItemToDisplayLineItem.Clear();

            cloverConnector.ShowWelcomeScreen();
            //cloverConnector.DisplayOrder(DisplayOrder); // want the welcome screen until something is added to the order

            PayButton.Enabled = true;
            StoreItems.Enabled = true;
            TabControl.Enabled = true;

            UpdateUI();
        }

        private void UpdateUI()
        {
            currentOrder.Text = Store.CurrentOrder.ID;
            OrderItems.Items.Clear();

            foreach (POSLineItem item in Store.CurrentOrder.Items)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.Tag = item;
                lvi.Name = item.Item.Name;

                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                lvi.SubItems[0].Text = "" + item.Quantity;
                lvi.SubItems[1].Text = item.Item.Name;
                lvi.SubItems[2].Text = (item.Item.Price / 100.0).ToString("C2");
                lvi.SubItems[3].ForeColor = Color.ForestGreen;
                lvi.SubItems[3].Text = (item.Discount == null) ? "" : "-" + (item.Discount.Value(item.Item) / 100.0).ToString("C2");
                

                OrderItems.Items.Add(lvi);
            }

            SubTotal.Text = (Store.CurrentOrder.SubTotal / 100.0).ToString("C2");
            TaxAmount.Text = (Store.CurrentOrder.TaxAmount / 100.0).ToString("C2");
            TotalAmount.Text = (Store.CurrentOrder.Total / 100.0).ToString("C2");
        }

        private void TabControl_SelectedIndexChanged(Object sender, EventArgs ev)
        {
            OrdersListView.Items.Clear();

            if (TabControl.SelectedIndex == 1)
            {
                foreach (POSOrder order in Store.Orders)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = order;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                    lvi.SubItems[0].Text = order.ID;
                    lvi.SubItems[1].Text = (order.Total / 100.0).ToString("C2");
                    lvi.SubItems[2].Text = order.Date.ToString();
                    lvi.SubItems[3].Text = order.Status.ToString();
                    lvi.SubItems[4].Text = (order.SubTotal / 100.0).ToString("C2");
                    lvi.SubItems[5].Text = (order.TaxAmount / 100.0).ToString("C2");

                    OrdersListView.Items.Add(lvi);
                }
            }
        }

        private void OrdersListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            OrderDetailsListView.Items.Clear();
            if (OrdersListView.SelectedIndices.Count == 1)
            {
                ListViewItem lvi = OrdersListView.SelectedItems[0];

                POSOrder selOrder = (POSOrder)lvi.Tag;

                OrderDetailsListView.Items.Clear();

                // update order items table
                foreach (POSLineItem lineItem in selOrder.Items)
                {
                    lvi = new ListViewItem();
                    lvi.Tag = lineItem;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                    lvi.SubItems[0].Text = lineItem.Quantity + "";
                    lvi.SubItems[1].Text = lineItem.Item.Name;
                    lvi.SubItems[2].Text = (lineItem.Item.Price / 100.0).ToString("C2");

                    OrderDetailsListView.Items.Add(lvi);
                }

                // update order payments table
                OrderPaymentsView.Items.Clear();
                foreach (var Exchange in selOrder.Payments)
                {
                    lvi = new ListViewItem();
                    lvi.Tag = Exchange;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                    lvi.SubItems[0].Text = (Exchange is POSPayment) ? ((POSPayment)Exchange).PaymentStatus.ToString() : "";
                    lvi.SubItems[1].Text = (Exchange.Amount / 100.0).ToString("C2");
                    lvi.SubItems[2].Text = (Exchange is POSPayment) ? (((POSPayment)Exchange).TipAmount / 100.0).ToString("C2") : "";
                    lvi.SubItems[3].Text = (Exchange is POSPayment) ? ((((POSPayment)Exchange).TipAmount + Exchange.Amount) / 100.0).ToString("C2") : (Exchange.Amount / 100.0).ToString("C2");

                    OrderPaymentsView.Items.Add(lvi);
                }
            }
        }
        public void PaymentReset()
        {
            PayButton.Enabled = true;
            StoreItems.Enabled = true;
            TabControl.Enabled = true;

            if(DisplayOrder.lineItems.elements.Count > 0)
            {
                cloverConnector.DisplayOrder(DisplayOrder);
            }

            UpdateUI();
        }
        private void OrderPaymentsView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OrderPaymentsView.SelectedIndices.Count == 1 && ((POSPayment)OrderPaymentsView.SelectedItems[0].Tag).PaymentStatus == POSPayment.Status.PAID)
            {
                VoidButton.Enabled = true;
            }
            else
            {
                VoidButton.Enabled = false;
            }
        }
        // only allow numbers to be entered
        private void RefundAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.')/* && ((sender as TextBox).Text.IndexOf('.') > -1)*/)
            {
                e.Handled = true;
            }


            int result = 0;
            if (!int.TryParse(RefundAmount.Text, out result))
            {
                RefundAmount.BackColor = Color.Red;
                ManualRefundButton.Enabled = false;
            }
            else
            {
                RefundAmount.BackColor = Color.White;
                ManualRefundButton.Enabled = true;
            }
        }

        private void ExamplePOSForm_Closed(object sender, FormClosedEventArgs e)
        {
            cloverConnector.ShowWelcomeScreen();
        }

        private void OrderItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Need to show item panel?
            if (OrderItems.SelectedItems.Count == 1)
            {
                POSLineItem lineItem = (POSLineItem)((ListViewItem)OrderItems.SelectedItems[0]).Tag;
                SelectedLineItem = lineItem;
                ItemNameLabel.Text = lineItem.Item.Name;
                ItemQuantityTextbox.Text = lineItem.Quantity.ToString();
                // enable/disable Discount button. Can't add it twice...
                DiscountButton.Enabled = lineItem.Discount == null;
            }
            SelectedItemPanel.BringToFront();

        }

        private void IncrementQuantityButton_Click(object sender, EventArgs e)
        {
            SelectedLineItem.Quantity++;
            ItemQuantityTextbox.Text = "" + SelectedLineItem.Quantity;
            UpdateDisplayOrderTotals();
            posLineItemToDisplayLineItem[SelectedLineItem].quantity = "" + SelectedLineItem.Quantity;
            cloverConnector.DisplayOrder(DisplayOrder);
            UpdateUI();
        }

        private void DecrementQuantityButton_Click(object sender, EventArgs e)
        {
            SelectedLineItem.Quantity--;
            if (SelectedLineItem.Quantity == 0)
            {
                RemoveSelectedItemFromCurrentOrder();
            }
            else
            {
                ItemQuantityTextbox.Text = "" + SelectedLineItem.Quantity;
                UpdateDisplayOrderTotals();
                posLineItemToDisplayLineItem[SelectedLineItem].quantity = "" + SelectedLineItem.Quantity;
                cloverConnector.DisplayOrder(DisplayOrder);
                UpdateUI();
            }
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            RemoveSelectedItemFromCurrentOrder();
        }

        private void RemoveSelectedItemFromCurrentOrder()
        {
            Store.CurrentOrder.RemoveItem(SelectedLineItem);
            UpdateDisplayOrderTotals();
            cloverConnector.DisplayOrderLineItemRemoved(DisplayOrder, posLineItemToDisplayLineItem[SelectedLineItem]);
            StoreItems.BringToFront();
            UpdateUI();
        }

        private void DiscountButton_Click(object sender, EventArgs e)
        {
            SelectedLineItem.Discount = new POSLineItemDiscount(0.1f, "10% Off");
            DisplayDiscount discount = new DisplayDiscount();
            DisplayLineItem displayLineItem = posLineItemToDisplayLineItem[SelectedLineItem];
            discount.lineItemId = displayLineItem.id;
            displayLineItem.addDiscount(discount);

            discount.name = SelectedLineItem.Discount.Name;
            discount.percentage = "10";
            discount.amount = (SelectedLineItem.Discount.Value(SelectedLineItem.Item) * SelectedLineItem.Quantity / 100.0).ToString("C2");

            UpdateDisplayOrderTotals();
            cloverConnector.DisplayOrder(DisplayOrder);

            DiscountButton.Enabled = false;
            UpdateUI();
        }

        private void DoneEditingLineItem_Click(object sender, EventArgs e)
        {
            StoreItems.BringToFront();
            UpdateUI();
        }

        private void InitializeConnector(CloverDeviceConfiguration config)
        {
            if(cloverConnector != null)
            {
                cloverConnector.Connections -= this;
                cloverConnector.Devices -= this;
                cloverConnector.Signatures -= this;
                cloverConnector.Sales -= this;
                cloverConnector.Voids -= this;
                cloverConnector.ManualRefunds -= this;

                OnDeviceDisconnected(); // for any disabling, messaging, etc.
                PayButton.Enabled = false; // everything can work except Pay
            }

            cloverConnector = new CloverConnector(config);
            
            cloverConnector.Connections += this;
            cloverConnector.Devices += this;
            cloverConnector.Signatures += this;
            cloverConnector.Sales += this;
            cloverConnector.Voids += this;
            cloverConnector.ManualRefunds += this;

            //ui cleanup
            this.Text = OriginalFormTitle + " - " + config.getName();
            if (config is TestCloverDeviceConfiguration)
            {
                TestDeviceMenuItem.Checked = true;
                CloverMiniUSBMenuItem.Checked = false;
            }
            else if (config is USBCloverDeviceConfiguration)
            {
                TestDeviceMenuItem.Checked = false;
                CloverMiniUSBMenuItem.Checked = true;
            }
        }

        private void TestDeviceMenuItem_Click(object sender, EventArgs e)
        {
            InitializeConnector(TestConfig);
        }

        private void CloverMiniUSBMenuItem_Click(object sender, EventArgs e)
        {
            InitializeConnector(USBConfig);
        }

    }
}
