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

namespace CloverExamplePOS
{
    public partial class CloverExamplePOSForm : Form, CloverConnectionListener, CloverDeviceListener, CloverSaleListener, CloverVoidTransactionListener, CloverManualRefundListener
    {
        CloverConnector cloverConnector;
        Store Store;
        SynchronizationContext uiThread;

        public CloverExamplePOSForm()
        {
            //new CaptureLog();

            InitializeComponent();
            uiThread = WindowsFormsSynchronizationContext.Current;
        }

        private void ExamplePOSForm_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("Would ");
            // what to do in the background thread
            //CloverDeviceConfiguration config = new USBCloverDeviceConfiguration("__deviceID__");
            CloverDeviceConfiguration config = new TestCloverDeviceConfiguration();
            cloverConnector = new CloverConnector(config);

            cloverConnector.Connections += this;
            cloverConnector.Devices += this;

            cloverConnector.Sales += this;
            cloverConnector.Voids += this;
            cloverConnector.ManualRefunds += this;

            Store = new Store();
            Store.AvailableItems.Add(new POSItem("abc123", "Hamburger ", 239));
            Store.AvailableItems.Add(new POSItem("def456", "Cheeseburger ", 269));
            Store.AvailableItems.Add(new POSItem("ace135", "Dbl. Hamburger ", 329));
            Store.AvailableItems.Add(new POSItem("fda321", "Dbl. Cheeseburger ", 379));
            Store.AvailableItems.Add(new POSItem("fdc742", "Chicken Sandwich ", 379));
            Store.AvailableItems.Add(new POSItem("cea987", "French Fries - Small ", 129));
            Store.AvailableItems.Add(new POSItem("acb654", "French Fries - Medium ", 159));
            Store.AvailableItems.Add(new POSItem("dfa342", "French Fries - Large ", 179));
            Store.AvailableItems.Add(new POSItem("dea937", "Soft Drink - Small ", 119));
            Store.AvailableItems.Add(new POSItem("afc470", "Soft Drink - Medium ", 139));
            Store.AvailableItems.Add(new POSItem("bce328", "Soft Drink - Large ", 189));
            Store.AvailableItems.Add(new POSItem("eda216", "Gift Card ", 3000));

            foreach (POSItem item in Store.AvailableItems)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.Tag = item;
                lvi.Name = item.Name;

                lvi.Text = item.Name + (item.Price / 100.0).ToString("C2");
                StoreItems.Items.Add(lvi);
            }
            UpdateUI();

        }

        private void StoreItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StoreItems.SelectedItems.Count == 1)
            {
                ListViewItem lvi = StoreItems.SelectedItems[0];
                POSItem item = (POSItem)lvi.Tag;

                Store.CurrentOrder.AddItem(item, 1);

                UpdateUI();
            }
            StoreItems.SelectedIndices.Clear();
        }

        private void NewOrder_Click_1(object sender, EventArgs e)
        {
            Store.CreateOrder();

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

                //lvi.Text = item.Item.Name + " - " + (item.Item.Price/100.0).ToString("C2") + " x" + item.Quantity;
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                lvi.SubItems[0].Text = "" + item.Quantity;
                lvi.SubItems[1].Text = item.Item.Name;
                lvi.SubItems[2].Text = (item.Item.Price / 100.0).ToString("C2");

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

                OrderPaymentsView.Items.Clear();
                foreach (var Exchange in selOrder.Payments)
                {
                    lvi = new ListViewItem();
                    lvi.Tag = Exchange;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                    lvi.SubItems[0].Text = (Exchange is POSPayment) ? ((POSPayment)Exchange).PaymentStatus.ToString() : "";
                    lvi.SubItems[1].Text = (Exchange.Amount / 100.0).ToString("C2");

                    OrderPaymentsView.Items.Add(lvi);
                }
            }
        }
        public void PaymentFinished()
        {
            PayButton.Enabled = true;
            StoreItems.Enabled = true;
            TabControl.Enabled = true;

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

        //////////////// Sale methods /////////////
        private void PayButton_Click(object sender, EventArgs e)
        {
            PayButton.Enabled = false;
            StoreItems.Enabled = false;
            newOrderBtn.Enabled = false;

            SaleRequest request = new SaleRequest();
            request.Amount = Store.CurrentOrder.Total;
            request.TipAmount = 0;
            cloverConnector.Sale(request);
        }
        public void OnSaleResponse(SaleResponse response)
        {
            Store.CurrentOrder.Status = POSOrder.OrderStatus.CLOSED;
            POSPayment payment = new POSPayment(response.Payment.id, response.Payment.order.id, response.Payment.employee.id, response.Payment.amount);
            payment.PaymentStatus = POSPayment.Status.PAID;
            Store.CurrentOrder.AddPayment(payment);
            Store.CreateOrder();

            uiThread.Send(delegate (object state) {
                PaymentFinished();
            }, null);
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
            foreach (POSOrder order in Store.Orders)
            {
                foreach (POSPayment payment in order.Payments)
                {
                    payment.PaymentStatus = POSPayment.Status.VOIDED;
                    break;
                }
            }
            uiThread.Send(delegate (object state) {
                VoidButton.Enabled = false;
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
            uiThread.Send(delegate (object state) {
                MessageBox.Show("Refund of " + (response.credit.amount / 100.0).ToString("c2") + " was applied to card ending with " + response.credit.cardTransaction.last4);
                RefundAmount.Text = "";
            }, null);
            
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
            }, null);
        }

        public void OnDeviceDisconnected()
        {
            try
            {
                uiThread.Send(delegate (object state) {
                    ConnectStatusLabel.Text = "Disconnected";
                }, null);
            }
            catch(Exception e)
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
            uiThread.Send(delegate (object state) {
                DeviceCurrentStatus.Text = "";
            }, null);
        }


        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            throw new NotImplementedException();
        }
    }
}
