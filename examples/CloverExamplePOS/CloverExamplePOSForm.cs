// Copyright (C) 2018 Clover Network, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using CloverExamplePOS.CustomActivity;
using CloverExamplePOS.UIDialogs;
using com.clover.remote.order;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using com.clover.sdk.v3.payments;
using com.clover.sdk.v3.printer;
using Newtonsoft.Json;
using Transport = com.clover.remotepay.transport;

namespace CloverExamplePOS
{
    public partial class CloverExamplePOSForm : Form, ICloverConnectorListener
    {
        CloverExamplePosData data = new CloverExamplePosData();
        public ICloverConnector Connector { get => data.CloverConnector; }

        SynchronizationContext uiThread;
        DisplayOrder DisplayOrder;
        Dictionary<POSLineItem, DisplayLineItem> posLineItemToDisplayLineItem = new Dictionary<POSLineItem, DisplayLineItem>();
        POSLineItem SelectedLineItem = null;
        bool Connected = false;
        RatingsListForm rlForm;
        public Printer selectedPrinter = new Printer();
        AlertForm pairingForm;
        private int buttonPressed = 0;
        public static string lastPrintJobId = "";
        private Dictionary<string, object> TempObjectMap = new Dictionary<string, object>();
        public MenuItem textPrintersMenuItem;
        public MenuItem imagePrintersMenuItem;
        public MenuItem cashDrawerPrintersMenuItem;
        string OriginalFormTitle;
        enum ClientTab { ORDER, ORDERLIST, REFUND, TEST }
        private Boolean _suspendUpdateOrderUI = false;
        private MerchantInfo currentDeviceMerchantInfo;

        Color ConnectionStatusColor_Connected = Color.FromArgb(0xD6, 0xFF, 0xD6); // green
        Color ConnectionStatusColor_Connecting = Color.FromArgb(0xFF, 0xFF, 0xD6); // yellow
        Color ConnectionStatusColor_Disconnected = Color.FromArgb(0xFF, 0xD6, 0xD6); // red

        public void SubscribeToStoreChanges(Store Store)
        {
            data.Store.OrderListChange += new Store.OrderListChangeHandler(OrderListChanged);
            data.Store.PreAuthListChange += new Store.PreAuthListChangeHandler(PreAuthListChanged);
        }
        public void SubscribeToOrderChanges(POSOrder order)
        {
            order.OrderChange += new POSOrder.OrderDataChangeHandler(OrderChanged);
        }
        public void UnsubscribeToOrderChanges(POSOrder order)
        {
            order.OrderChange -= new POSOrder.OrderDataChangeHandler(OrderChanged);
        }
        public CloverExamplePOSForm()
        {
            InitializeComponent();

            uiThread = SynchronizationContext.Current;

            uiThread.Send(delegate (object state)
            {
                new StartupForm(this, OnPairingCode, OnPairingSuccess, OnPairingState).Show();
            }, null);

        }

        public void OnPairingCode(string pairingCode)
        {
            Invoke(new Action(() =>
            {
                pairingForm?.Dispose();
                pairingForm = new AlertForm(this);
                pairingForm.Title = "Pairing Code";
                pairingForm.Label = "Enter this code on the Clover Mini: " + pairingCode;
                pairingForm.Show();
            }
            ));
        }

        public void OnPairingSuccess(string pairingAuthToken)
        {
            Properties.Settings.Default.pairingAuthToken = pairingAuthToken;
            Properties.Settings.Default.Save();
            Invoke(new Action(() => pairingForm?.Dispose()));
        }

        public void OnPairingState(string state, string message)
        {
            if (state == "AUTHENTICATING")
            {
                Invoke(new Action(() =>
                    {
                        pairingForm?.Dispose();
                        pairingForm = new AlertForm(this);
                        pairingForm.Title = "Pairing Security Pin";
                        pairingForm.Label = message;
                        pairingForm.Show();
                    }
                ));
            }
        }

        private void ExamplePOSForm_Load(object sender, EventArgs e)
        {
            // some UI cleanup...
            RegisterTabs.Appearance = TabAppearance.FlatButtons;
            RegisterTabs.ItemSize = new Size(0, 1);
            RegisterTabs.SizeMode = TabSizeMode.Fixed;

            OriginalFormTitle = Text;

            // Set the backing data, state, & clover connector object on child controls
            loyaltyApiPage.Data = data;

            // Set connection UI state
            ConnectStatusLabel.BackColor = ConnectionStatusColor_Disconnected;

            data.CreateStore();

            SaleButton.ContextMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem("Sale with Vaulted Card");
            menuItem.Enabled = false;
            menuItem.Click += delegate
            {
                uiThread.Send(delegate
                {
                    VaultedCardListForm vclForm = new VaultedCardListForm(this);
                    vclForm.setCardsListView(cardsListView);
                    vclForm.setCardAction(VaultedCardListForm.VaultedCardAction.PAY);
                    vclForm.FormClosing += vaultedCardListClosing;
                    vclForm.Show(this);
                }, null);
            };
            SaleButton.ContextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem("Sale with Pre-Auth");
            menuItem.Enabled = false;
            menuItem.Click += delegate
            {
                uiThread.Send(delegate
                {
                    PreAuthListForm palForm = new PreAuthListForm(this);
                    palForm.PreAuths = data.Store.PreAuths;
                    palForm.FormClosing += preAuthFormClosing;
                    palForm.Show(this);

                }, null);
            };
            SaleButton.ContextMenu.MenuItems.Add(menuItem);
            SaleButton.Click.Add(PayButton_Click);

            AuthButton.ContextMenu = new ContextMenu();
            menuItem = new MenuItem("Auth with Vaulted Card");
            menuItem.Enabled = false;
            menuItem.Click += delegate
            {
                uiThread.Send(delegate
                {
                    VaultedCardListForm vclForm = new VaultedCardListForm(this);
                    vclForm.setCardsListView(cardsListView);
                    vclForm.setCardAction(VaultedCardListForm.VaultedCardAction.AUTH);
                    vclForm.FormClosing += vaultedCardListClosing;
                    vclForm.Show(this);
                }, null);
            };
            AuthButton.ContextMenu.MenuItems.Add(menuItem);

            AuthButton.Click.Add(AuthButton_Click);

            //DeviceStatusButton
            DeviceStatusButton.ContextMenu = new ContextMenu();
            menuItem = new MenuItem("Send Last Message");
            menuItem.Enabled = true;
            menuItem.Click += delegate (object sen, EventArgs args)
            {
                data.CloverConnector.RetrieveDeviceStatus(new RetrieveDeviceStatusRequest(true));
            };
            DeviceStatusButton.ContextMenu.MenuItems.Add(menuItem);
            DeviceStatusButton.Click.Add(DeviceStatusBtn_Click);

            //PrintTextButton
            PrintTextButton.ContextMenu = new ContextMenu();
            menuItem = new MenuItem("Default");
            menuItem.Enabled = true;
            menuItem.Click += delegate (object sen, EventArgs args)
            {
                selectedPrinter = null;
                PrintTextMenu_Click();
            };
            DeviceStatusButton.ContextMenu.MenuItems.Add(menuItem);
            textPrintersMenuItem = new MenuItem("Printers");
            textPrintersMenuItem.Enabled = true;
            PrintTextButton.ContextMenu.MenuItems.Add(menuItem);
            PrintTextButton.ContextMenu.MenuItems.Add(textPrintersMenuItem);
            PrintTextButton.ContextMenu.Popup += delegate (object sen, EventArgs args)
            {
                buttonPressed = 1;
                RetrievePrintersRequest request = new RetrievePrintersRequest();
                data.CloverConnector.RetrievePrinters(request);
            };
            PrintTextButton.Click = new List<EventHandler>();
            PrintTextButton.Click.Add(PrintTextBtn_Click);


            //PrintImageButton
            PrintImageButton.ContextMenu = new ContextMenu();
            PrintImageButton.ContextMenu.Popup += delegate (object sen, EventArgs args)
            {
                buttonPressed = 2;
                RetrievePrintersRequest request = new RetrievePrintersRequest();
                data.CloverConnector.RetrievePrinters(request);
            };
            menuItem = new MenuItem("Default");
            menuItem.Click += delegate (object sen, EventArgs args)
            {
                selectedPrinter = null;
                PrintImageMenu_Click();
            };
            menuItem.Enabled = true;
            menuItem.Index = 0;
            PrintImageButton.ContextMenu.MenuItems.Add(menuItem);
            imagePrintersMenuItem = new MenuItem("Printers");
            imagePrintersMenuItem.Enabled = true;
            imagePrintersMenuItem.Index = 1;
            PrintImageButton.ContextMenu.MenuItems.Add(imagePrintersMenuItem);
            PrintImageButton.Click = new List<EventHandler>();
            PrintImageButton.Click.Add(PrintImageButton_Click);


            OpenCashDrawerButton.ContextMenu = new ContextMenu();
            menuItem = new MenuItem("Default");
            menuItem.Click += delegate (object sen, EventArgs args)
            {
                selectedPrinter = null;
                OpenCashDrawerMenu_Click();
            };
            menuItem.Enabled = true;
            menuItem.Index = 0;
            OpenCashDrawerButton.ContextMenu.MenuItems.Add(menuItem);
            cashDrawerPrintersMenuItem = new MenuItem("Printers");
            cashDrawerPrintersMenuItem.Enabled = true;
            cashDrawerPrintersMenuItem.Index = 1;
            OpenCashDrawerButton.ContextMenu.MenuItems.Add(cashDrawerPrintersMenuItem);

            OpenCashDrawerButton.ContextMenu.Popup += delegate (object sen, EventArgs args)
            {
                buttonPressed = 3;
                RetrievePrintersRequest request = new RetrievePrintersRequest();
                data.CloverConnector.RetrievePrinters(request);
            };
            OpenCashDrawerButton.Click = new List<EventHandler>();
            OpenCashDrawerButton.Click.Add(OpenCashDrawerButton_Click);

            CopyExternalIdMenuItem.Click += OrderPaymentsView_CopyExternalIdMenuItem_Click;
            ViewPaymentMenuItem.Click += ViewPaymentMenuItem_Click;

            foreach (POSItem item in data.Store.AvailableItems)
            {
                StoreItem si = new StoreItem();
                si.Item = item;
                si += StoreItems_ItemSelected;

                StoreItems.Controls.Add(si);
            }

            foreach (POSDiscount discount in data.Store.AvailableDiscounts)
            {
                StoreDiscount si = new StoreDiscount();
                si.Discount = discount;
                si += StoreItems_DiscountSelected;

                StoreDiscounts.Controls.Add(si);
            }
            SubscribeToStoreChanges(data.Store);
            NewOrder(0);
        }

        private void PrintTextMenu_Click()
        {
            List<string> messages = new List<string>();
            messages.Add(PrintTextBox.Text);
            lastPrintJobId = ExternalIDUtil.GenerateRandomString(16);
            PrintRequest req = new PrintRequest(messages, lastPrintJobId, null);
            if (selectedPrinter != null)
            {
                req.printDeviceId = selectedPrinter.id;
            }
            data.CloverConnector.Print(req);
        }

        private void OpenCashDrawerMenu_Click()
        {
            OpenCashDrawerRequest req = new OpenCashDrawerRequest("Test");
            if (selectedPrinter != null)
            {
                req.printerId = selectedPrinter.id;
            }
            data.CloverConnector.OpenCashDrawer(req);
        }

        private void PrintImageMenu_Click()
        {
            if (PrintURLTextBox.Text != null && PrintURLTextBox.Text != "")
            {
                try
                {
                    PrintImage.Load(PrintURLTextBox.Text);
                }
                catch (Exception ex)
                {
                    AlertForm.Show(this, "Invalid Image", ex.Message);
                    return;
                }
                lastPrintJobId = ExternalIDUtil.GenerateRandomString(16);
                PrintRequest req = new PrintRequest(PrintURLTextBox.Text, lastPrintJobId, null);
                if (selectedPrinter != null)
                {
                    req.printDeviceId = selectedPrinter.id;
                }
                data.CloverConnector.Print(req);

            }
            else if (PrintImage.Image != null && PrintImage.Image is Bitmap)
            {
                lastPrintJobId = ExternalIDUtil.GenerateRandomString(16);
                PrintRequest req = new PrintRequest((Bitmap)PrintImage.Image, lastPrintJobId, null);
                data.CloverConnector.Print(req);

            }
            else
            {
                AlertForm.Show(this, "Invalid Image", "Invalid Image");
            }
        }

        private ClientTab GetCurrentTab()
        {
            ClientTab ct = 0; //initialized to first tab, but then reassigned by logic below.
            uiThread.Send(delegate (object state)
            {
                ct = (ClientTab)TabControl.SelectedIndex;
            }, null);
            return ct;
        }

        public void OrderListChanged(Store Store, Store.OrderListAction action)
        {
            autoResizeColumns(OrdersListView);
            switch (GetCurrentTab())
            {
                case ClientTab.ORDER:
                    {
                        UpdateOrderUI();
                        break;
                    }
                case ClientTab.ORDERLIST:
                    {
                        UpdateOrdersListView();
                        break;
                    }
                default:
                    break;
            }
        }

        public void PreAuthListChanged(POSPayment payment, Store.PreAuthAction action)
        {
            if (action == Store.PreAuthAction.ADDED)
            {
                string[] columns = new string[]
                {
                    "PRE-AUTH",
                    (payment.Amount / 100.0).ToString("C2"),
                    payment.PaymentID,
                    payment.ExternalID,
                };
                ListViewItem lvi = new ListViewItem(columns);
                lvi.Tag = payment;
                PreAuthListView.Items.Add(lvi);
            }
            else if (action == Store.PreAuthAction.REMOVED)
            {
                foreach (ListViewItem lvi in PreAuthListView.Items)
                {
                    if (lvi.Tag.Equals(payment))
                    {
                        PreAuthListView.Items.Remove(lvi);
                        break;
                    }
                }
            }
            SaleButton.ContextMenu.MenuItems[1].Enabled = data.Store.PreAuths.Count > 0;
            autoResizeColumns(PreAuthListView);
        }

        private void PreAuthListViewContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = (PreAuthListView.SelectedItems.Count == 0);
        }

        private void PreAuthListViewContextMenu_CopyPaymentID_Click(object sender, EventArgs e)
        {
            POSPayment payment = PreAuthListView.SelectedItems[0].Tag as POSPayment;
            Clipboard.SetText(payment?.PaymentID ?? "");
        }

        private void PreAuthListViewContextMenu_CopyExternalID_Click(object sender, EventArgs e)
        {
            POSPayment payment = PreAuthListView.SelectedItems[0].Tag as POSPayment;
            Clipboard.SetText(payment?.ExternalID ?? "");
        }

        private void PayButtonContext_Click(object sender, EventArgs e)
        {
            Pay(null);
        }
        private void PayButtonCard_Click(object sender, EventArgs e)
        {
            if (((MenuItem)sender).Tag is POSCard card)
            {
                Pay(card);
            }
        }
        private void PayButton_Click(object sender, EventArgs e)
        {
            Pay(null);
        }
        //////////////// Sale methods /////////////
        private void Pay(POSCard card)
        {
            StoreItems.BringToFront();
            StoreDiscounts.BringToFront();

            SaleButton.Enabled = false;
            StoreItems.Enabled = false;
            newOrderBtn.Enabled = false;

            SaleRequest request = new SaleRequest();
            request.ExternalId = ExternalIDUtil.GenerateRandomString(32);
            request.Amount = data.Store.CurrentOrder.Total;

            request.CardEntryMethods = TransactionSettingsEdit.CardEntryMethod;
            request.CardNotPresent = TransactionSettingsEdit.CardNotPresent;

            // SaleRequest supported TipModes: TIP_PROVIDED, NO_TIP, ON_SCREEN_BEFORE_PAYMENT
            // NOTE: ON_PAPER would turn the Sale into an AUTH, so it is not valid here
            request.TipMode = TransactionSettingsEdit.TipMode;
            if (TransactionSettingsEdit.TipMode == com.clover.remotepay.sdk.TipMode.TIP_PROVIDED)
            {
                request.TipAmount = TransactionSettingsEdit.TipAmount;
            }

            if (TransactionSettingsEdit.HasSignatureThreshold)
            {
                request.SignatureThreshold = TransactionSettingsEdit.SignatureThreshold;
            }

            if (TransactionSettingsEdit.HasSignatureEntryLocation)
            {
                request.SignatureEntryLocation = TransactionSettingsEdit.SignatureEntryLocation;
            }

            request.TipSuggestions = TransactionSettingsEdit.TipSuggestions;

            request.TaxAmount = data.Store.CurrentOrder.TaxAmount;

            request.DisableCashback = TransactionSettingsEdit.DisableCashback;
            request.DisableRestartTransactionOnFail = TransactionSettingsEdit.DisableRestartTransactionOnFail;
            request.DisablePrinting = TransactionSettingsEdit.DisablePrinting;
            request.DisableReceiptSelection = TransactionSettingsEdit.DisableReceiptSelection;
            request.DisableDuplicateChecking = TransactionSettingsEdit.DisableDuplicateChecking;
            request.AutoAcceptSignature = TransactionSettingsEdit.AutomaticSignatureConfirmation;
            request.AutoAcceptPaymentConfirmations = TransactionSettingsEdit.AutomaticPaymentConfirmation;
            request.AllowOfflinePayment = TransactionSettingsEdit.AllowOfflinePayment;
            request.ApproveOfflinePaymentWithoutPrompt = TransactionSettingsEdit.ApproveOfflinePaymentWithoutPrompt;
            request.ForceOfflinePayment = TransactionSettingsEdit.ForceOfflinePayment;

            foreach (KeyValuePair<string, string> items in TransactionSettingsEdit.RegionalExtras)
            {
                request.RegionalExtras[items.Key] = items.Value;
            }

            if (card != null)
            {
                request.VaultedCard = new com.clover.sdk.v3.payments.VaultedCard();
                request.VaultedCard.cardholderName = card.Name;
                request.VaultedCard.first6 = "" + card.First6;
                request.VaultedCard.last4 = "" + card.Last4;
                request.VaultedCard.expirationDate = card.Month + "" + card.Year;
                request.VaultedCard.token = card.Token;
            }

            if (data.Store.CurrentOrder.TippableAmount != data.Store.CurrentOrder.Total)
            {
                request.TippableAmount = data.Store.CurrentOrder.TippableAmount;
            }
            data.CloverConnector.Sale(request);
        }

        public void OnSaleResponse(SaleResponse response)
        {
            if (response.Success)
            {
                POSPayment payment = new POSPayment(response.Payment.id, response.Payment.externalPaymentId, response.Payment.order.id, response.Payment.employee.id, response.Payment.amount, response.Payment.tipAmount ?? 0, response.Payment.cashbackAmount);
                payment.PaymentSource = response.Payment;

                // Look for partial payment scenario, enabled on some merchants, where POS asks for $10, succeed with $5 (on one card, gift card, etc.), and expects to do another sale for the remaining balance
                // A real point of sale either uses this to support multiple payments per order, or can void this payment to reject a partial payment.
                // Merchant settings control whether partial payments are allowed, many merchants cannot succeed with partial payments.
                if (response.Payment.amount < data.Store.CurrentOrder.Total)
                {
                    string message = $"Sale succeeded as a Partial Payment of the order total.\n\n\tTotal:\t{(data.Store.CurrentOrder.Total) / 100.0:C2}\n\tPayment:\t{(response.Payment.amount) / 100.0:C2}\nRemainder:\t{(data.Store.CurrentOrder.Total - response.Payment.amount) / 100.0:C2}\n\nFull Point of Sale should either request remainder of total amount, or void the payment to reject and reverse it.";
                    MessageBox.Show(message, "Partial Payment Details", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Partial Payments are beyond the scope of this POS Sample application
                    // Real Point of Sale Architecture TODO
                    // 1. Void payment if partial payments are not supported in this POS Software
                    // 2. Update order total to subtract this partial payment, request further payments to complete order
                }

                if (response.IsAuth) //Tip Adjustable
                {
                    data.Store.CurrentOrder.Status = POSOrder.OrderStatus.AUTHORIZED;
                    payment.PaymentStatus = POSPayment.Status.AUTHORIZED;
                }
                else
                {
                    data.Store.CurrentOrder.Status = POSOrder.OrderStatus.CLOSED;
                    payment.PaymentStatus = POSPayment.Status.PAID;
                }
                data.Store.CurrentOrder.AddPayment(payment);
                data.Store.CurrentOrder.Date = (new DateTime(1970, 1, 1)).AddMilliseconds(response.Payment.createdTime).ToLocalTime();

                uiThread.Send(delegate (object state)
                {
                    if (payment.CashBackAmount > 0)
                    {
                        ShowCashBackForm(payment.CashBackAmount);
                    }
                    RegisterTabs.SelectedIndex = 0;
                    NewOrder(3000);
                }, null);
            }
            else if (response.Result.Equals(ResponseCode.FAIL))
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                    PaymentReset();
                }, null);
            }
            else if (response.Result.Equals(ResponseCode.CANCEL))
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                    PaymentReset();
                }, null);
            }
        }

        //////////////// Auth methods /////////////
        private void AuthButtonContext_Click(object sender, EventArgs e)
        {
            Auth(null);
        }

        private void AuthButton_Click(object sender, EventArgs e)
        {
            Auth(null);
        }

        private void AuthButtonCard_Click(object sender, EventArgs e)
        {
            object obj = ((MenuItem)sender).Tag;
            if (obj is POSCard)
            {
                Auth((POSCard)obj);
            }
        }

        private void Auth(POSCard card)
        {
            StoreItems.BringToFront();
            StoreDiscounts.BringToFront();

            SaleButton.Enabled = false;
            StoreItems.Enabled = false;
            newOrderBtn.Enabled = false;

            AuthRequest request = new AuthRequest();
            request.Amount = data.Store.CurrentOrder.Total;
            request.TaxAmount = data.Store.CurrentOrder.TaxAmount;
            request.ExternalId = ExternalIDUtil.GenerateRandomString(32);

            request.CardEntryMethods = TransactionSettingsEdit.CardEntryMethod;
            request.CardNotPresent = TransactionSettingsEdit.CardNotPresent;
            request.DisablePrinting = TransactionSettingsEdit.DisablePrinting;

            request.DisableCashback = TransactionSettingsEdit.DisableCashback;
            request.AllowOfflinePayment = TransactionSettingsEdit.AllowOfflinePayment;
            request.ApproveOfflinePaymentWithoutPrompt = TransactionSettingsEdit.ApproveOfflinePaymentWithoutPrompt;
            request.ForceOfflinePayment = TransactionSettingsEdit.ForceOfflinePayment;
            if (card != null)
            {
                request.VaultedCard = new com.clover.sdk.v3.payments.VaultedCard();
                request.VaultedCard.cardholderName = card.Name;
                request.VaultedCard.first6 = card.First6;
                request.VaultedCard.last4 = card.Last4;
                request.VaultedCard.expirationDate = card.Month + "" + card.Year;
                request.VaultedCard.token = card.Token;
            }

            if (TransactionSettingsEdit.HasSignatureThreshold)
            {
                request.SignatureThreshold = TransactionSettingsEdit.SignatureThreshold;
            }

            if (TransactionSettingsEdit.HasSignatureEntryLocation)
            {
                request.SignatureEntryLocation = TransactionSettingsEdit.SignatureEntryLocation;
            }

            // Note: There are no tips on-device in an Auth, they are on paper. The on-device tip related TransactionSettings are irrelevant.

            request.DisableRestartTransactionOnFail = TransactionSettingsEdit.DisableRestartTransactionOnFail;
            request.DisablePrinting = TransactionSettingsEdit.DisablePrinting;
            request.DisableReceiptSelection = TransactionSettingsEdit.DisableReceiptSelection;
            request.DisableDuplicateChecking = TransactionSettingsEdit.DisableDuplicateChecking;
            request.AutoAcceptSignature = TransactionSettingsEdit.AutomaticSignatureConfirmation;
            request.AutoAcceptPaymentConfirmations = TransactionSettingsEdit.AutomaticPaymentConfirmation;
            if (data.Store.CurrentOrder.TippableAmount != data.Store.CurrentOrder.Total)
            {
                request.TippableAmount = data.Store.CurrentOrder.TippableAmount;
            }
            // AuthRequest supported TipModes: Always ON_PAPER, so anything else is ignored
            // NOTE: Anything else would turn the Auth into an Sale, so it is not valid here
            data.CloverConnector.Auth(request);
        }

        public void OnAuthResponse(AuthResponse response)
        {
            if (response.Success)
            {
                if (Result.SUCCESS.Equals(response.Payment.result))
                {
                    POSPayment payment = new POSPayment(response.Payment.id, response.Payment.externalPaymentId, response.Payment.order.id, response.Payment.employee.id, response.Payment.amount, response.Payment.tipAmount ?? 0, response.Payment.cashbackAmount);
                    if (response.IsAuth)
                    {
                        data.Store.CurrentOrder.Status = POSOrder.OrderStatus.AUTHORIZED;
                        payment.PaymentStatus = POSPayment.Status.AUTHORIZED;
                    }
                    else
                    {
                        data.Store.CurrentOrder.Status = POSOrder.OrderStatus.CLOSED;
                        payment.PaymentStatus = POSPayment.Status.PAID;
                    }

                    data.Store.CurrentOrder.AddPayment(payment);
                    data.Store.CurrentOrder.Date = (new DateTime(1970, 1, 1)).AddMilliseconds(response.Payment.createdTime).ToLocalTime();

                    uiThread.Send(delegate (object state)
                    {
                        if (payment.CashBackAmount > 0)
                        {
                            ShowCashBackForm(payment.CashBackAmount);
                        }
                        RegisterTabs.SelectedIndex = 0;
                        NewOrder(3000);
                    }, null);
                }
            }
            else if (response.Result == ResponseCode.FAIL || response.Result == ResponseCode.CANCEL || response.Result == ResponseCode.UNSUPPORTED)
            {
                uiThread.Send(delegate
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                    PaymentReset();
                }, null);
            }
            else
            {
                uiThread.Send(delegate
                {
                    AlertForm.Show(this, response.Reason, $"Unexpected Error Result: {response.Result}\n\n" + response.Message);
                    PaymentReset();
                }, null);
            }
        }

        private void PreAuth(POSCard card)
        {
            StoreItems.BringToFront();
            StoreDiscounts.BringToFront();

            SaleButton.Enabled = false;
            StoreItems.Enabled = false;
            newOrderBtn.Enabled = false;

            PreAuthRequest request = new PreAuthRequest();
            request.Amount = data.Store.CurrentOrder.Total;
            request.ExternalId = ExternalIDUtil.GenerateRandomString(32);

            request.CardEntryMethods = TransactionSettingsEdit.CardEntryMethod;
            request.CardNotPresent = TransactionSettingsEdit.CardNotPresent;

            request.DisableRestartTransactionOnFail = TransactionSettingsEdit.DisableRestartTransactionOnFail;
            request.DisablePrinting = TransactionSettingsEdit.DisablePrinting;
            request.DisableReceiptSelection = TransactionSettingsEdit.DisableReceiptSelection;
            request.DisableDuplicateChecking = TransactionSettingsEdit.DisableDuplicateChecking;
            data.CloverConnector.PreAuth(request);
        }

        public void OnPreAuthResponse(PreAuthResponse response)
        {
            if (response.Success)
            {
                if (Result.AUTH.Equals(response.Payment.result))
                {
                    uiThread.Send(delegate (object state)
                    {
                        AlertForm.Show(this, "Card Authorized", "Payment was Pre-Authorized");
                        POSPayment preAuth = new POSPayment(response.Payment.id, response.Payment.externalPaymentId, response.Payment.order.id, null, response.Payment.amount);
                        data.Store.AddPreAuth(preAuth);

                    }, null);
                }
            }
            else if (response.Result == ResponseCode.FAIL || response.Result == ResponseCode.CANCEL)
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                    PaymentReset();
                }, null);
            }
            else
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, $"Unexpected Response {response.Result}" + response.Message);
                    PaymentReset();
                }, null);
            }
        }

        public void OnRetrievePendingPaymentsResponse(RetrievePendingPaymentsResponse response)
        {
            uiThread.Send(delegate (object state)
            {
                pendingPaymentListView.Items.Clear();
                Console.WriteLine(response.PendingPayments.Count);
                foreach (PendingPaymentEntry ppe in response.PendingPayments)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = ppe;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                    lvi.SubItems[0].Text = ppe.paymentId;
                    lvi.SubItems[1].Text = ppe.externalPaymentId;
                    lvi.SubItems[2].Text = (ppe.amount / 100.0).ToString("C2");
                    lvi.SubItems[3].Text = (ppe.tipAmount / 100.0).ToString("C2");

                    pendingPaymentListView.Items.Add(lvi);
                }
            }, null);
        }

        public virtual void OnCustomActivityResponse(CustomActivityResponse response)
        {
            uiThread.Send(delegate (object state)
            {
                try
                {
                    dynamic parsedPayload = JsonConvert.DeserializeObject(response.Payload);
                    string formattedPayload = JsonConvert.SerializeObject(parsedPayload, Formatting.Indented);
                    Console.WriteLine(formattedPayload);

                    AlertForm.Show(this, "Custom Activity Response" + (response.Success ? "" : ": Canceled"), formattedPayload);
                }
                catch
                {
                    AlertForm.Show(this, "Custom Activity Response" + (response.Success ? "" : ": Canceled"), response.Payload + Environment.NewLine + response.Reason);
                }
            }, null);
        }


        public void OnMessageFromActivity(MessageFromActivity message)
        {
            // CustomActivity message routing 
            if (message.Action.ToLower().Contains("com.clover.remote_clover_loyalty"))
            {
                loyaltyApiPage.OnMessageFromActivity(message);
            }
            else if (message.Action.ToLower().Contains("com.clover.cfp.examples.ratingsexample"))
            {
                RatingsActivityExample(message);
            }
        }

        public void RatingsActivityExample(MessageFromActivity message)
        {
            PayloadMessage payloadMessage = JsonConvert.DeserializeObject<PayloadMessage>(message.Payload);
            switch (payloadMessage.messageType)
            {
                case MessageType.REQUEST_RATINGS:
                    handleRequestRatings();
                    break;
                case MessageType.RATINGS:
                    handleRatings(message.Payload);
                    break;
                case MessageType.PHONE_NUMBER:
                    handleCustomerLookup(message.Payload);
                    break;
                case MessageType.CONVERSATION_RESPONSE:
                    handleJokeResponse(message.Payload);
                    break;
                default:
                    break;
            }
        }

        private void handleCustomerLookup(String payloadMessage)
        {
            PhoneNumberMessage message = JsonUtils.DeserializeSdk<PhoneNumberMessage>(payloadMessage);
            String phoneNumber = message.phoneNumber;
            CustomActivity.CustomerInfo customerInfo = new CustomActivity.CustomerInfo();
            customerInfo.customerName = "Ron Burgundy";
            customerInfo.phoneNumber = phoneNumber;
            CustomActivity.CustomerInfoMessage customerInfoMessage = new CustomActivity.CustomerInfoMessage();
            customerInfoMessage.customerInfo = customerInfo;
            customerInfoMessage.payloadClassName = "CustomerInfoMessage";
            customerInfoMessage.messageType = MessageType.CUSTOMER_INFO.ToString();
            String jsonPayload = JsonConvert.SerializeObject(customerInfoMessage);
            SendMessageToActivity(jsonPayload, "com.clover.cfp.examples.RatingsExample");
        }

        private void handleRequestRatings()
        {
            Rating rating1 = new Rating();
            rating1.id = "Quality";
            rating1.question = "How would you rate the overall quality of your entree?";
            rating1.value = 0;
            Rating rating2 = new Rating();
            rating2.id = "Server";
            rating2.question = "How would you rate the overall performance of your server?";
            rating2.value = 0;
            Rating rating3 = new Rating();
            rating3.id = "Value";
            rating3.question = "How would you rate the overall value of your dining experience?";
            rating3.value = 0;
            Rating rating4 = new Rating();
            rating4.id = "RepeatBusiness";
            rating4.question = "How likely are you to dine at this establishment again in the near future?";
            rating4.value = 0;
            Rating[] ratings = new Rating[] { rating1, rating2, rating3, rating4 };
            RatingsMessage ratingsMessage = new RatingsMessage();
            ratingsMessage.ratings = ratings;
            ratingsMessage.messageType = MessageType.RATINGS.ToString();
            ratingsMessage.payloadClassName = "RatingsMessage";
            String jsonPayload = JsonConvert.SerializeObject(ratingsMessage);
            SendMessageToActivity(jsonPayload, "com.clover.cfp.examples.RatingsExample");
        }

        private void handleRatings(String payload)
        {
            RatingsMessage ratingsMessage = JsonUtils.DeserializeSdk<RatingsMessage>(payload);
            Rating[] ratings = ratingsMessage.ratings;
            showRatingsDialog(ratings);
        }

        private void rlForm_Disposed(object sender, EventArgs args)
        {
            rlForm = null;
        }

        private void showRatingsDialog(Rating[] ratings)
        {
            uiThread.Send(delegate (object state)
            {
                addRatingsToUI(ratings);

                if (rlForm == null)
                {
                    rlForm = new RatingsListForm(this);
                    rlForm.Disposed += this.rlForm_Disposed;
                }
                rlForm.setRatings(ratings);
                if (!rlForm.Visible)
                {
                    rlForm.Show(this);
                }
                else
                {
                    rlForm.Invalidate();
                }
            }, null);
        }

        private void handleJokeResponse(String payload)
        {
            ConversationResponseMessage jokeResponseMessage = JsonUtils.DeserializeSdk<ConversationResponseMessage>(payload);
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Received JokeResponse of: ", jokeResponseMessage.message);
            }, null);
        }

        public virtual void OnRetrieveDeviceStatusResponse(RetrieveDeviceStatusResponse response)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "OnRetrieveDeviceStatusResponse: ", response.State + ":" + JsonUtils.Serialize(response.Data));
            }, null);
        }

        public virtual void OnResetDeviceResponse(ResetDeviceResponse response)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "OnResetDeviceResponse", response.State.ToString());
            }, null);
        }

        public virtual void OnPrintManualRefundReceipt(PrintManualRefundReceiptMessage printManualRefundReceiptMessage)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Print ManualRefund Receipt", (printManualRefundReceiptMessage.Credit.amount / 100.0).ToString("C2"));
            }, null);
        }

        public virtual void OnPrintManualRefundDeclineReceipt(PrintManualRefundDeclineReceiptMessage printManualRefundDeclineReceiptMessage)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Print ManualRefund Declined Receipt", printManualRefundDeclineReceiptMessage.Reason);
            }, null);
        }

        public virtual void OnPrintPaymentReceipt(PrintPaymentReceiptMessage printPaymentReceiptMessage)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Print Payment Receipt", (printPaymentReceiptMessage.Payment.amount / 100.0).ToString("C2"));
            }, null);
        }

        public virtual void OnPrintPaymentDeclineReceipt(PrintPaymentDeclineReceiptMessage printPaymentDeclineReceiptMessage)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Print Payment Declined Receipt", printPaymentDeclineReceiptMessage.Reason);
            }, null);
        }

        public virtual void OnPrintPaymentMerchantCopyReceipt(PrintPaymentMerchantCopyReceiptMessage printPaymentMerchantCopyReceiptMessage)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Print Merchant Payment Copy Receipt", (printPaymentMerchantCopyReceiptMessage.Payment.amount / 100.0).ToString("C2"));
            }, null);
        }

        public virtual void OnPrintRefundPaymentReceipt(PrintRefundPaymentReceiptMessage printRefundPaymentReceiptMessage)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Print Refund Payment Receipt", ((printRefundPaymentReceiptMessage.Refund.amount ?? 0) / 100.0).ToString("C2"));
            }, null);
        }

        private void vaultCardClosing(object sender, FormClosingEventArgs e)
        {
            POSCard card = ((CustomerCardCaptureForm)sender).Card;
            addCardToUI(card);
        }

        private void vaultedCardListClosing(object sender, FormClosingEventArgs e)
        {
            POSCard card = ((VaultedCardListForm)sender).getCard();
            if (card != null)
            {
                if (((VaultedCardListForm)sender).getCardAction() == VaultedCardListForm.VaultedCardAction.PAY)
                {
                    Pay(card);
                }
                else
                {
                    Auth(card);
                }
            }
        }

        private void preAuthFormClosing(object sender, FormClosingEventArgs args)
        {
            POSPayment payment = ((PreAuthListForm)sender).SelectedPayment;
            if (payment != null)
            {
                CapturePreAuthRequest captureAuthRequest = new CapturePreAuthRequest();
                captureAuthRequest.PaymentID = payment.PaymentID;
                captureAuthRequest.Amount = data.Store.CurrentOrder.Total;
                if (TransactionSettingsEdit.TipMode == com.clover.remotepay.sdk.TipMode.TIP_PROVIDED)
                {
                    captureAuthRequest.TipAmount = TransactionSettingsEdit.TipAmount ?? 0;
                }
                else
                {
                    captureAuthRequest.TipAmount = 0;
                }
                data.CloverConnector.CapturePreAuth(captureAuthRequest);
            }
        }

        private void addCardToUI(POSCard card)
        {
            if (card != null)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = card;
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                lvi.SubItems[0].Text = card.Name;
                lvi.SubItems[1].Text = card.First6;
                lvi.SubItems[2].Text = card.Last4;
                lvi.SubItems[3].Text = card.Month;
                lvi.SubItems[4].Text = card.Year;
                lvi.SubItems[5].Text = card.Token;

                cardsListView.Items.Add(lvi);
                autoResizeColumns(cardsListView);
                uiThread.Send(delegate (object state)
                {
                    if (SaleButton.ContextMenu.MenuItems[0].Enabled == false)
                    {
                        SaleButton.ContextMenu.MenuItems[0].Enabled = true;
                        AuthButton.ContextMenu.MenuItems[0].Enabled = true;
                    }
                }, null);
            }
        }

        private static void autoResizeColumns(ListView lv)
        {
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            ListView.ColumnHeaderCollection cc = lv.Columns;
            for (int i = 0; i < cc.Count; i++)
            {
                int colWidth = TextRenderer.MeasureText(cc[i].Text, lv.Font).Width + 10;
                if (colWidth > cc[i].Width)
                {
                    cc[i].Width = colWidth;
                }
            }
        }

        private void addRatingsToUI(Rating[] ratings)
        {
            ratingsListView.Items.Clear();
            if (ratings != null)
            {
                foreach (Rating rating in ratings)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = rating;
                    lvi.Text = rating.value.ToString();
                    lvi.Name = "Rating";
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, rating.id));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, rating.question));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, rating.value.ToString() + " STARS"));
                    ratingsListView.Items.Add(lvi);
                }
                autoResizeColumns(ratingsListView);
            }
        }

        public void ShowCashBackForm(long Amount)
        {
            AlertForm.Show(this, "Cash Back", "Cash Back " + (Amount / 100.0).ToString("C2"));
        }

        public void OnCapturePreAuthResponse(CapturePreAuthResponse response)
        {
            if (response.Success)
            {
                foreach (POSPayment preAuth in data.Store.PreAuths)
                {
                    if (preAuth.PaymentID.Equals(response.PaymentId))
                    {
                        uiThread.Send(delegate (object state)
                        {
                            data.Store.CurrentOrder.Status = POSOrder.OrderStatus.AUTHORIZED;
                            preAuth.PaymentStatus = POSPayment.Status.AUTHORIZED;
                            preAuth.Amount = response.Amount;
                            preAuth.TipAmount = response.TipAmount ?? 0;
                            data.Store.CurrentOrder.AddOrderPayment(preAuth);

                            NewOrder(3000);
                            data.Store.RemovePreAuth(preAuth);
                            AlertForm.Show(this, "Payment Captured", "Payment was successfully captured using Pre-Authorization");
                        }, null);
                        break;
                    }
                }
            }
            else
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                }, null);

            }
        }

        public void OnIncrementPreAuthResponse(IncrementPreAuthResponse response)
        {
            if (response.Success)
            {
                foreach (POSPayment preAuth in data.Store.PreAuths)
                {
                    if (preAuth.PaymentID.Equals(response.Authorization.payment.id))
                    {
                        uiThread.Send(delegate (object state)
                        {
                            preAuth.Amount = response.Authorization.payment.amount;
                            data.Store.RemovePreAuth(preAuth);
                            data.Store.AddPreAuth(preAuth);
                            data.Store.CurrentOrder.AddOrderPayment(preAuth);
                            AlertForm.Show(this, "Payment Incremented", "Pre-Authorization Payment was successfully incremented");
                        }, null);
                        break;
                    }
                }
            } else
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, "Increment PreAuth failure", $"{response.Reason}\n\n{response.Message}");
                }, null);
            }
        }

        public void OnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {
            if (response.Success)
            {
                POSOrder order = data.Store.GetOrder(response.PaymentId);
                order.ModifyTipAmount(response.PaymentId, response.TipAmount);
            }
            else
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                }, null);
            }
        }

        //////////////// Reset methods /////////////
        private void ResetButton_Click(object sender, EventArgs e)
        {
            data.CloverConnector.ResetDevice();
        }

        //////////////// Closeout methods /////////////
        private void CloseoutButton_Click(object sender, EventArgs e)
        {
            data.CloverConnector.Closeout(new CloseoutRequest());
        }

        public void OnCloseoutResponse(CloseoutResponse response)
        {
            if (response != null && response.Success)
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, "Batch Closed", "Batch " + response.Batch.id + " was successfully processed.");
                }, null);

            }
            if (response != null && response.Result.Equals(ResponseCode.FAIL))
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, "Close Attempt Failed", "Reason: " + response.Reason + ".");
                }, null);

            }
        }

        //////////////// Order methods /////////////
        private void OpenOrder_Button_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = OrdersListView.SelectedItems[0];
            if (lvi != null)
            {
                POSOrder selOrder = (POSOrder)lvi.Tag;
                DisplayOrder = DisplayFactory.createDisplayOrder();
                DisplayOrder.title = Guid.NewGuid().ToString();
                posLineItemToDisplayLineItem.Clear();
                data.Store.CurrentOrder = selOrder;
                uiThread.Send(delegate (object state)
                {
                    RegisterTabs.SelectedIndex = 0;
                    TabControl.SelectedIndex = 0;
                    RebuildOrderOnDevice();
                    UpdateOrderUI();
                }, null);
            }
        }

        //////////////// Void methods /////////////
        private void VoidButton_Click(object sender, EventArgs e)
        {
            VoidPaymentRequest request = new VoidPaymentRequest();
            if (OrderPaymentsView.SelectedItems.Count == 1)
            {
                POSPayment payment = ((POSPayment)OrderPaymentsView.SelectedItems[0].Tag);
                request.PaymentId = payment.PaymentID;
                request.EmployeeId = payment.EmployeeID;
                request.OrderId = payment.OrderID;
                request.VoidReason = "USER_CANCEL";

                data.CloverConnector.VoidPayment(request);
            }
        }

        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            bool voided = false;
            foreach (POSOrder order in data.Store.Orders)
            {
                foreach (POSExchange payment in order.Payments)
                {
                    if (payment.PaymentID == response.PaymentId)
                    {
                        ((POSPayment)payment).PaymentStatus = POSPayment.Status.VOIDED;
                        order.Status = POSOrder.OrderStatus.OPEN; //re-open order for editing/payment
                        voided = true;
                        break;
                    }
                }
                if (voided)
                {
                    break;
                }
            }
            uiThread.Send(delegate
            {
                VoidButton.Enabled = false;
                RefundPaymentButton.Enabled = false;
            }, null);

        }

        public void OnVoidPaymentRefundResponse(VoidPaymentRefundResponse response)
        {
            // Nothing to do - example doesn't call VoidPaymentRefund, so never gets this response
        }

        //////////////// Manual Refund methods /////////////
        private void ManualRefundButton_Click(object sender, EventArgs e)
        {
            ManualRefundRequest request = new ManualRefundRequest();
            request.ExternalId = ExternalIDUtil.GenerateRandomString(32);
            request.Amount = int.Parse(RefundAmount.Text);

            request.CardEntryMethods = TransactionSettingsEdit.CardEntryMethod;
            request.CardNotPresent = TransactionSettingsEdit.CardNotPresent;
            request.DisablePrinting = TransactionSettingsEdit.DisablePrinting;
            request.DisableReceiptSelection = TransactionSettingsEdit.DisableReceiptSelection;

            data.CloverConnector.ManualRefund(request);
        }

        public void OnManualRefundResponse(ManualRefundResponse response)
        {
            if (response.Success)
            {
                uiThread.Send(delegate (object state)
                {
                    ListViewItem lvi = new ListViewItem();
                    POSManualRefund manualRefund = new POSManualRefund(response.Credit.id, response.Credit.orderRef.id, response.Credit.amount);
                    lvi.Tag = manualRefund;

                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                    lvi.SubItems[0].Text = (response.Credit.amount / 100.0).ToString("C2");
                    lvi.SubItems[1].Text = new DateTime(1970, 1, 1).AddMilliseconds(response.Credit.createdTime).ToLocalTime().ToString();
                    lvi.SubItems[2].Text = response.Credit.cardTransaction.last4;

                    string msg = "Refund of " + (response.Credit.amount / 100.0).ToString("C2") + " was applied to card ending with " + response.Credit.cardTransaction.last4;
                    AlertForm.Show(this, "Refund applied", msg);

                    RefundAmount.Text = "";
                    ManualRefundButton.Enabled = false;

                    TransactionsListView.Items.Add(lvi);
                    autoResizeColumns(TransactionsListView);
                }, null);
            }
            else if (response.Result.Equals(ResponseCode.FAIL))
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                    PaymentReset();
                }, null);
            }
            else if (response.Result.Equals(ResponseCode.CANCEL))
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                    PaymentReset();
                }, null);
            }

        }

        private void TransactionsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem item = TransactionsListView.SelectedItems.Cast<ListViewItem>().SingleOrDefault();
            POSManualRefund refund = item?.Tag as POSManualRefund;
            bool refunded = refund != null;
            RefundReceiptOptButton.Enabled = refunded;
        }

        //////////////// Payment Refund methods /////////////
        private void RefundPaymentButton_Click(object sender, EventArgs e)
        {
            RefundPaymentRequest request = new RefundPaymentRequest();

            if (OrderPaymentsView.SelectedItems.Count == 1 && OrdersListView.SelectedItems.Count == 1)
            {
                POSPayment payment = ((POSPayment)OrderPaymentsView.SelectedItems[0].Tag);
                POSOrder order = (POSOrder)OrdersListView.SelectedItems[0].Tag;

                // Ask user for a refund amount
                RefundAmountDialog dialog = new RefundAmountDialog();
                dialog.FullRefund = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    request.PaymentId = payment.PaymentID;
                    request.OrderId = payment.OrderID;

                    request.Amount = dialog.Amount;
                    request.FullRefund = dialog.FullRefund;

                    request.DisablePrinting = TransactionSettingsEdit.DisablePrinting;
                    request.DisableReceiptSelection = TransactionSettingsEdit.DisableReceiptSelection;

                    TempObjectMap.Clear();
                    TempObjectMap.Add(payment.PaymentID, order);

                    data.CloverConnector.RefundPayment(request);
                }
            }
        }

        public void OnRefundPaymentResponse(RefundPaymentResponse response)
        {
            if (response.Success)
            {
                string paymentID = response.PaymentId;
                string employeeID = null;
                if (paymentID != null)
                {
                    uiThread.Send(delegate (object state)
                    {
                        object orderObj = null;

                        TempObjectMap.TryGetValue(paymentID, out orderObj);
                        if (orderObj != null)
                        {
                            TempObjectMap.Remove(paymentID);
                            if (response.Refund.employee != null)
                            {
                                employeeID = response.Refund.employee.id;
                            }

                            POSRefund refund = new POSRefund(response.Refund.id, response.PaymentId, response.OrderId, employeeID, response.Refund.amount ?? 0);

                            ((POSOrder)orderObj).Status = POSOrder.OrderStatus.OPEN; //re-open order for editing/payment
                            ((POSOrder)orderObj).AddRefund(refund);
                        }
                        else
                        {
                            AlertForm.Show(this, "Error", "Couldn't find order for refunded payment");
                        }

                    }, null);
                }
                else
                {
                    AlertForm.Show(this, "Error", "Couldn't find paymentID");
                }
            }
            else if (response.Result.Equals(ResponseCode.FAIL))
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, "Error", response.Message);
                    PaymentReset();
                }, null);
            }
        }

        ////////////////// CloverDeviceLister Methods //////////////////////
        private void PrintPaymentButton_Click(object sender, EventArgs e)
        {
            //TODO: data.CloverConnector.ChooseReceipt(orderID, paymentID);
        }

        ////////////////// CloverDeviceLister Methods //////////////////////

        public void OnDeviceConnected()
        {
            uiThread.Send(delegate (object state)
            {
                ConnectStatusLabel.Text = "Connecting...";
                ConnectStatusLabel.BackColor = ConnectionStatusColor_Connecting;
            }, null);
        }

        public void OnDeviceReady(MerchantInfo merchantInfo)
        {
            uiThread.Send(delegate (object state)
            {
                ConnectStatusLabel.Text = "Ready! " + merchantInfo.merchantName + Environment.NewLine + merchantInfo.Device.Serial;
                ConnectStatusLabel.BackColor = ConnectionStatusColor_Connected;
                Connected = true;
                currentDeviceMerchantInfo = merchantInfo;
                AboutSdkInfoDisplay.CloverConnector = data.CloverConnector as CloverConnector;
                if (DisplayOrder != null && DisplayOrder.lineItems != null && DisplayOrder.lineItems.elements.Count > 0)
                {
                    UpdateDisplayOrderTotals();
                }
                PaymentReset();
            }, null);
        }

        public void OnDeviceDisconnected()
        {
            try
            {
                uiThread.Send(delegate (object state)
                {
                    ConnectStatusLabel.Text = "Disconnected";
                    ConnectStatusLabel.BackColor = ConnectionStatusColor_Disconnected;
                    Connected = false;
                    currentDeviceMerchantInfo = null;
                }, null);

            }
            catch
            {
                // uiThread is gone on shutdown
            }
        }


        ////////////////// CloverDeviceListener Methods //////////////////////
        public void OnDeviceActivityStart(CloverDeviceEvent deviceEvent)
        {
            uiThread.Send(delegate (object state)
            {
                //this.TabControl.Enabled = false; // should do this, but allows negative testing
                if (TabControl.SelectedIndex == 0)
                {
                    newOrderBtn.Enabled = false;
                    SaleButton.Enabled = false;
                    AuthButton.Enabled = false;
                }

                UIStateButtonPanel.Controls.Clear();
                if (deviceEvent.InputOptions != null)
                {
                    foreach (InputOption io in deviceEvent.InputOptions)
                    {
                        Button b = new Button();
                        b.FlatStyle = FlatStyle.Flat;
                        b.BackColor = Color.White;
                        b.Text = io.description;
                        b.Click += getHandler(io);
                        UIStateButtonPanel.Controls.Add(b);
                    }

                }
                UIStateButtonPanel.Parent.PerformLayout();
                DeviceCurrentStatus.Text = deviceEvent.Message;
            }, null);
        }

        public void OnDeviceActivityEnd(CloverDeviceEvent deviceEvent)
        {
            try
            {
                uiThread.Send(delegate (object state)
                {
                    this.TabControl.Enabled = true;
                    UIStateButtonPanel.Controls.Clear();
                    DeviceCurrentStatus.Text = " ";
                }, null);
            }
            catch (Exception)
            {
                // if UI goes away, uiThread may be disposed
            }
        }

        public void OnDeviceError(CloverDeviceErrorEvent deviceErrorEvent)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Device Error", deviceErrorEvent.Message);
            }, null);
        }


        ////////////////// CloverSignatureListener Methods //////////////////////
        /// <summary>
        /// Handle a request from the Clover device to verify a signature
        /// </summary>
        /// <param name="request"></param>
        public void OnVerifySignatureRequest(VerifySignatureRequest request)
        {
            CloverExamplePOSForm parentForm = this;
            uiThread.Send(delegate (object state)
            {
                SignatureForm sigForm = new SignatureForm(parentForm);
                sigForm.VerifySignatureRequest = request;
                sigForm.Show();
            }, null);
        }

        public void OnConfirmPaymentRequest(ConfirmPaymentRequest request)
        {
            CloverExamplePOSForm parentForm = this;
            AutoResetEvent confirmPaymentFormBusy = new AutoResetEvent(false);
            bool lastChallenge = false;
            for (int i = 0; i < request.Challenges.Count; i++)
            {
                uiThread.Send(delegate (object state)
                {
                    if (i == request.Challenges.Count - 1) // if this is the last challenge
                    {
                        lastChallenge = true;
                    }
                    Challenge challenge = request.Challenges[i];
                    ConfirmPaymentForm confirmForm = new ConfirmPaymentForm(parentForm, challenge, lastChallenge);
                    confirmForm.FormClosed += (object s, FormClosedEventArgs ce) =>
                    {
                        if (confirmForm.Status == DialogResult.No)
                        {
                            data.CloverConnector.RejectPayment(request.Payment, challenge);
                            i = request.Challenges.Count;
                        }
                        else if (confirmForm.Status == DialogResult.OK) // Last challenge was accepted
                        {
                            data.CloverConnector.AcceptPayment(request.Payment);
                        }
                        confirmPaymentFormBusy.Set(); //release the confirmPaymentFormBusy WaitOne lock
                    };
                    confirmForm.Show();
                }, null);
                confirmPaymentFormBusy.WaitOne(); //wait here until Accept or Reject pressed
            }
        }

        public void OnDeviceError(Exception e)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, e.GetType().ToString(), e.Message);
            }, null);

        }

        ////////////////// CloverTipListener Methods //////////////////////
        public void OnTipAdded(TipAddedMessage message)
        {
            if (message.tipAmount > 0)
            {
                string msg = "Tip Added: " + (message.tipAmount / 100.0).ToString("C2");
                OnDeviceActivityStart(new CloverDeviceEvent(0, msg));
            }
        }

        ////////////////// CloverReceiptListener Methods //////////////////////
        public void OnDisplayReceiptOptionsResponse(DisplayReceiptOptionsResponse response)
        {
            if (!response.Success)
            {
                uiThread.Send(state =>
                {
                    AlertForm.Show(this, "Invalid Receipt Option", $"{response.status}\n{response.Reason}");
                }, null);
            }
        }


        ////////////////// Loyalty API CustomerProvidedData Method //////////////////////
        public void OnCustomerProvidedData(CustomerProvidedDataEvent response)
        {
            // Pass the Clover Loyalty API message along to the LoyaltyAPI UI control
            loyaltyApiPage?.OnCustomerProvidedData(response);
        }

        ////////////////// Invalid State Transition Notification //////////////////////
        public void OnInvalidStateTransitionResponse(InvalidStateTransitionNotification notification)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Invalid State Transition", $"{notification.Reason}\nFailing transition from {notification.State}::{notification.Substate} to {notification.RequestedTransition}.");
            }, null);
        }

        ////////////////// UI Events and UI Management //////////////////////
        private void StoreItems_ItemSelected(object sender, EventArgs e)
        {
            POSItem item = ((StoreItem)((Control)sender).Parent).Item;
            POSLineItem lineItem = data.Store.CurrentOrder.AddItem(item, 1);

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
                data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            }
            else
            {
                displayLineItem.quantity = lineItem.Quantity.ToString();
                UpdateDisplayOrderTotals();
                data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            }
        }


        ////////////////// UI Events and UI Management //////////////////////
        private void StoreItems_DiscountSelected(object sender, EventArgs e)
        {
            POSDiscount discount = ((StoreDiscount)((Control)sender).Parent).Discount;

            data.Store.CurrentOrder.Discount = discount;
            data.Store.NewDiscount = true;  //temporarily disables reapplication of a discount during UpdateDisplayOrderTotals() calls

            DisplayDiscount DisplayDiscount = new DisplayDiscount();
            DisplayDiscount.name = discount.Name;
            DisplayDiscount.amount = Decimal.Divide(discount.Value(data.Store.CurrentOrder.PreDiscountSubTotal), 100).ToString("C2");
            DisplayDiscount.percentage = (discount.PercentageOff * 100).ToString("###");

            // our example POS business rules say only 1 order discount
            while (DisplayOrder.discounts.elements.Count > 0)
            {
                DisplayDiscount RemovedDisplayDiscount = (DisplayDiscount)DisplayOrder.discounts.elements[0];
                DisplayOrder.discounts.Remove(RemovedDisplayDiscount);
                UpdateDisplayOrderTotals();
                data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            }

            if (discount.Value(1000) != 0)
            {
                DisplayOrder.addDisplayDiscount(DisplayDiscount);
                UpdateDisplayOrderTotals();
                data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            }
            data.Store.NewDiscount = false; //enables reapplication of a discount during UpdateDisplayOrderTotals() calls
        }

        private void UpdateDisplayOrderTotals()
        {
            DisplayOrder.tax = (data.Store.CurrentOrder.TaxAmount / 100.0).ToString("C2");
            DisplayOrder.subtotal = (data.Store.CurrentOrder.PreTaxSubTotal / 100.0).ToString("C2");
            DisplayOrder.total = (data.Store.CurrentOrder.Total / 100.0).ToString("C2");

            // This block of code handles reapplying an existing order discount when new items are added or removed
            // If this method call is the result of a new discount being applied or removed, then this logic should be bypassed
            // as it is already handled as part of the add/remove discount logic
            if (data.Store.CurrentOrder.Discount != null && !data.Store.NewDiscount)
            {
                ReapplyOrderDiscount(data.Store.CurrentOrder.Discount);
            }
        }

        private void ReapplyOrderDiscount(POSDiscount discount)
        {
            DisplayDiscount DisplayDiscount = new DisplayDiscount();
            DisplayDiscount.name = discount.Name;
            DisplayDiscount.amount = Decimal.Divide(discount.Value(data.Store.CurrentOrder.PreDiscountSubTotal), 100).ToString("C2");
            DisplayDiscount.percentage = (discount.PercentageOff * 100).ToString("###");

            // our example POS business rules say only 1 order discount
            while (DisplayOrder.discounts.elements.Count > 0)
            {
                DisplayDiscount RemovedDisplayDiscount = (DisplayDiscount)DisplayOrder.discounts.elements[0];
                DisplayOrder.discounts.Remove(RemovedDisplayDiscount);
                data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            }

            if (discount.Value(1000) != 0)
            {
                DisplayOrder.addDisplayDiscount(DisplayDiscount);
                data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            }

        }

        private void NewOrder_Click(object sender, EventArgs e)
        {
            NewOrder(0);
        }

        private void NewOrder(int welcomeDelay)
        {
            foreach (POSOrder order in data.Store.Orders) //any pending orders will be removed when creating a new one
            {
                if (order.Status == POSOrder.OrderStatus.PENDING)
                {
                    UnsubscribeToOrderChanges(order);
                }
            }
            data.Store.CreateOrder();
            SubscribeToOrderChanges(data.Store.CurrentOrder);
            StoreItems.BringToFront();
            StoreDiscounts.BringToFront();

            DisplayOrder = DisplayFactory.createDisplayOrder();
            DisplayOrder.title = Guid.NewGuid().ToString();
            posLineItemToDisplayLineItem.Clear();

            StoreItems.Enabled = true;
            TabControl.Enabled = true;

            RegisterTabs.SelectedIndex = 0;
            TabControl.SelectedIndex = 0;
            if (Connected)
            {
                if (welcomeDelay > 0)
                {
                    System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
                    t.Interval = welcomeDelay;
                    t.Tick += new EventHandler(timerShowWelcomeScreen);
                    t.Start();
                }
                else
                {
                    data.CloverConnector.ShowWelcomeScreen(); //This will make sure that the customer sees a
                }                                        //Welcome screen anytime a new order is initiated.
            }
        }
        private void timerShowWelcomeScreen(object sender, EventArgs e)
        {
            data.CloverConnector.ShowWelcomeScreen();
            ((System.Windows.Forms.Timer)sender).Stop();
        }

        private void RebuildOrderOnDevice()
        {
            _suspendUpdateOrderUI = true;
            foreach (POSLineItem item in data.Store.CurrentOrder.Items)
            {
                //////////// Now make sure the display map contains all of the items for the order ////////////
                DisplayLineItem displayLineItem = null;
                posLineItemToDisplayLineItem.TryGetValue(item, out displayLineItem);
                if (displayLineItem == null)
                {
                    displayLineItem = DisplayFactory.createDisplayLineItem();
                    posLineItemToDisplayLineItem[item] = displayLineItem;
                    displayLineItem.quantity = item.Quantity.ToString();
                    displayLineItem.name = item.Item.Name;
                    displayLineItem.price = (item.Item.Price / 100.0).ToString("C2");
                    DisplayOrder.addDisplayLineItem(displayLineItem);
                }
                else
                {
                    displayLineItem.quantity = item.Quantity.ToString();
                }
            }
            UpdateDisplayOrderTotals();
            data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            _suspendUpdateOrderUI = false;
        }

        private void UpdateOrderUI()
        {
            if (!_suspendUpdateOrderUI)
            {
                currentOrder.Text = data.Store.CurrentOrder.ID;
                OrderItems.Items.Clear();
                if (data.Store.CurrentOrder.Items.Count > 0)
                {
                    AuthButton.Enabled = true;
                    DiscountButton.Enabled = true;
                    SaleButton.Enabled = true;
                    newOrderBtn.Enabled = true;
                }
                else
                {
                    AuthButton.Enabled = false;
                    DiscountButton.Enabled = false;
                    SaleButton.Enabled = false;
                    newOrderBtn.Enabled = false;
                }

                foreach (POSLineItem item in data.Store.CurrentOrder.Items)
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
                    autoResizeColumns(OrderItems);
                }

                if (data.Store.CurrentOrder.Discount.Value(1000) != 0)
                {
                    DiscountLabel.Text = (data.Store.CurrentOrder.Discount.Name) + "     -" + (data.Store.CurrentOrder.Discount.Value(data.Store.CurrentOrder.PreDiscountSubTotal) / 100.0).ToString("C2");
                }
                else
                {
                    DiscountLabel.Text = " ";
                }
                SubTotal.Text = (data.Store.CurrentOrder.PreTaxSubTotal / 100.0).ToString("C2");
                TaxAmount.Text = (data.Store.CurrentOrder.TaxAmount / 100.0).ToString("C2");
                TotalAmount.Text = (data.Store.CurrentOrder.Total / 100.0).ToString("C2");
            }
        }

        private void TabControl_SelectedIndexChanged(Object sender, EventArgs ev)
        {
            if (TabControl.SelectedIndex == 0) // Register Tab
            {
                if (DisplayOrder.lineItems != null && DisplayOrder.lineItems.elements.Count > 0)
                {
                    data.CloverConnector.ShowDisplayOrder(DisplayOrder);
                }
                UpdateOrderUI();
            }
            else if (TabControl.SelectedIndex == 1) // Orders Tab
            {
                OrdersListViewRefresh();
            }
        }

        private void RefreshSelectedOrderData()
        {
            ListViewItem lvi = OrdersListView.SelectedItems[0];
            POSOrder order = (POSOrder)lvi.Tag;

            lvi.SubItems[0].Text = order.ID;
            lvi.SubItems[1].Text = (order.Total / 100.0).ToString("C2");
            lvi.SubItems[2].Text = order.Date.Year >= 2000 ? order.Date.ToString() : "";
            lvi.SubItems[3].Text = order.Status.ToString();
            lvi.SubItems[4].Text = (order.PreTaxSubTotal / 100.0).ToString("C2");
            lvi.SubItems[5].Text = (order.TaxAmount / 100.0).ToString("C2");

        }
        private void OrdersListViewRefresh()
        {
            OrdersListView.Items.Clear();

            if (TabControl.SelectedIndex == 1)
            {
                foreach (POSOrder order in data.Store.Orders)
                {
                    if (order.Status != POSOrder.OrderStatus.PENDING) //Don't display PENDING orders 
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
                        lvi.SubItems[2].Text = order.Date.Year >= 2000 ? order.Date.ToString() : "";
                        lvi.SubItems[3].Text = order.Status.ToString();
                        lvi.SubItems[4].Text = (order.PreTaxSubTotal / 100.0).ToString("C2");
                        lvi.SubItems[5].Text = (order.TaxAmount / 100.0).ToString("C2");

                        OrdersListView.Items.Add(lvi);
                    }
                }
                RefundPaymentButton.Enabled = false;
                VoidButton.Enabled = false;
                TipAdjustButton.Enabled = false;
                OpenOrder_Button.Enabled = false;
                ShowReceiptButton.Enabled = false;
            }
            else if (TabControl.SelectedIndex == 3)
            {
                cardsListView.Items.Clear();
                foreach (POSCard card in data.Store.Cards)
                {
                    addCardToUI(card);
                }
            }
            autoResizeColumns(OrdersListView);
            autoResizeColumns(cardsListView);
        }

        private void OrdersListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefundPaymentButton.Enabled = false;
            VoidButton.Enabled = false;
            TipAdjustButton.Enabled = false;
            OpenOrder_Button.Enabled = false;
            ShowReceiptButton.Enabled = false;
            UpdateOrdersListView();
        }

        private void UpdateOrdersListView()
        {
            if (OrdersListView.SelectedIndices.Count == 1)
            {
                POSOrder selOrder = (POSOrder)OrdersListView.SelectedItems[0].Tag;
                RefreshSelectedOrderData();
                OrderDetailsListView.Items.Clear();

                // update order items table
                foreach (POSLineItem lineItem in selOrder.Items)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = lineItem;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem());
                    item.SubItems.Add(new ListViewItem.ListViewSubItem());
                    item.SubItems.Add(new ListViewItem.ListViewSubItem());

                    item.SubItems[0].Text = lineItem.Quantity + "";
                    item.SubItems[1].Text = lineItem.Item.Name;
                    item.SubItems[2].Text = (lineItem.Item.Price / 100.0).ToString("C2");

                    OrderDetailsListView.Items.Add(item);
                }

                // update order payments table
                OrderPaymentsView.Items.Clear();
                foreach (POSExchange exchange in selOrder.Payments)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = exchange;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem());
                    item.SubItems.Add(new ListViewItem.ListViewSubItem());
                    item.SubItems.Add(new ListViewItem.ListViewSubItem());
                    item.SubItems.Add(new ListViewItem.ListViewSubItem());
                    item.SubItems.Add(new ListViewItem.ListViewSubItem());

                    if (exchange is POSPayment payment)
                    {
                        item.SubItems[0].Text = payment.PaymentStatus.ToString();
                        item.SubItems[1].Text = (payment.Amount / 100.0).ToString("C2");
                        item.SubItems[2].Text = (payment.TipAmount / 100.0).ToString("C2");
                        item.SubItems[3].Text = ((payment.TipAmount + payment.Amount) / 100.0).ToString("C2");
                        item.SubItems[4].Text = payment.ExternalID;
                    }
                    else if (exchange is POSRefund)
                    {
                        item.SubItems[0].Text = "REFUND";
                        item.SubItems[3].Text = (exchange.Amount / 100.0).ToString("C2");
                    }

                    OrderPaymentsView.Items.Add(item);
                }
                if (selOrder.Status == POSOrder.OrderStatus.OPEN) //Allow editing of the order if it is in Open status
                {
                    OpenOrder_Button.Enabled = true;
                }
                ResetOrderPaymentButtons(); // enable/disable payment buttons
                autoResizeColumns(OrderPaymentsView);
                autoResizeColumns(OrderDetailsListView);

                if (OrderDetailsListView.Items.Count > 0)
                {
                    OrderDetailsListView.Items[0].Selected = true;
                    if (OrderPaymentsView.Items.Count > 0)
                    {
                        OrderPaymentsView.Items[0].Selected = true;
                    }
                }
            }
        }

        public void PaymentReset()
        {
            StoreItems.Enabled = true;
            TabControl.Enabled = true;

            if (DisplayOrder != null && DisplayOrder.lineItems != null && DisplayOrder.lineItems.elements.Count > 0)
            {
                data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            }

            UpdateOrderUI();
        }

        private void OrderPaymentsView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetOrderPaymentButtons();
        }

        private void ResetOrderPaymentButtons()
        {
            ListViewItem item = OrderPaymentsView.SelectedItems.Cast<ListViewItem>().SingleOrDefault();
            if (item != null)
            {
                POSPayment payment = item.Tag as POSPayment;
                POSRefund refund = item.Tag as POSRefund;

                bool paid = payment?.PaymentStatus == POSPayment.Status.PAID;
                bool authorized = payment?.PaymentStatus == POSPayment.Status.AUTHORIZED;
                bool refunded = refund != null;

                VoidButton.Enabled = paid || authorized;
                RefundPaymentButton.Enabled = paid || authorized;
                ShowReceiptButton.Enabled = paid || authorized || refunded;
                TipAdjustButton.Enabled = authorized;
            }
        }

        private void OrderPaymentsView_CopyExternalIdMenuItem_Click(object sender, System.EventArgs e)
        {
            int retry = 3;
            string text = "";

            try
            {
                text = OrderPaymentsView.SelectedItems[0].SubItems[4].Text;
            }
            catch
            {
                // suppress invalid lists
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                do
                {
                    try
                    {
                        // Clipboard lock sometimes failes first try, try multiple times
                        Clipboard.SetText(text);
                        retry = 0;
                    }
                    catch
                    {
                        if (retry > 0)
                        {
                            Thread.Sleep(500);
                        }
                    }
                }
                while (retry-- > 0);
            }
        }

        private void ViewPaymentMenuItem_Click(object sender, EventArgs e)
        {
            if (OrderPaymentsView.SelectedItems.Count > 0)
            {
                if (OrderPaymentsView.SelectedItems[0].Tag is POSPayment pos)
                {
                    if (pos.PaymentSource is Payment payment)
                    {
                        PropertiesDialog dialog = new PropertiesDialog();
                        dialog.Text = "Clover Connector API Payment Object";
                        dialog.Content = Utils.SummaryReport(payment);
                        dialog.ShowDialog();
                    }
                }
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

        private void CloverExamplePOSForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            data.CloverConnector?.ShowWelcomeScreen(); // this may not fire, if the queue is processed before Exit();
        }

        private void ExamplePOSForm_Closed(object sender, FormClosedEventArgs e)
        {
            try
            {
                data.CloverConnector?.Dispose();
            }
            catch
            {
                // app closing, suppress any errors as irrelevant
            }
            finally
            {
                data.CloverConnector = null;
            }
        }

        private void OrderItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OrderItems.SelectedItems.Count == 1)
            {
                POSLineItem lineItem = (POSLineItem)((ListViewItem)OrderItems.SelectedItems[0]).Tag;
                SelectedLineItem = lineItem;
                ItemNameLabel.Text = lineItem.Item.Name;
                ItemQuantityTextbox.Text = lineItem.Quantity.ToString();
                // enable/disable Discount button. Can't add it twice...
                DiscountButton.Enabled = lineItem.Discount == null;
            }
            RegisterTabs.SelectedIndex = 1;

        }

        private void IncrementQuantityButton_Click(object sender, EventArgs e)
        {
            SelectedLineItem.Quantity++;
            ItemQuantityTextbox.Text = "" + SelectedLineItem.Quantity;
            UpdateDisplayOrderTotals();
            posLineItemToDisplayLineItem[SelectedLineItem].quantity = "" + SelectedLineItem.Quantity;
            data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            UpdateOrderUI();
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
                data.CloverConnector.ShowDisplayOrder(DisplayOrder);
                UpdateOrderUI();
            }
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            RemoveSelectedItemFromCurrentOrder();
        }

        private void RemoveSelectedItemFromCurrentOrder()
        {
            data.Store.CurrentOrder.RemoveItem(SelectedLineItem);
            DisplayLineItem dli = posLineItemToDisplayLineItem[SelectedLineItem];
            DisplayOrder.removeDisplayLineItem(dli);
            UpdateDisplayOrderTotals();
            data.CloverConnector.ShowDisplayOrder(DisplayOrder);
            RegisterTabs.SelectedIndex = 0;
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
            data.CloverConnector.ShowDisplayOrder(DisplayOrder);

            DiscountButton.Enabled = false;
            UpdateOrderUI();
        }

        private void DoneEditingLineItem_Click(object sender, EventArgs e)
        {
            RegisterTabs.SelectedIndex = 0;
            UpdateOrderUI();
        }

        public void InitializeConnector(CloverDeviceConfiguration config)
        {
            if (data.CloverConnector != null)
            {
                data.CloverConnector.RemoveCloverConnectorListener(this);

                OnDeviceDisconnected(); // for any disabling, messaging, etc.
                SaleButton.Enabled = false; // everything can work except Pay
                data.CloverConnector.Dispose();
            }

            data.CloverConnector = CloverConnectorFactory.createICloverConnector(config);
            data.CloverConnector.AddCloverConnectorListener(this);
            data.CloverConnector.InitializeConnection();

            //UI cleanup
            this.Text = OriginalFormTitle + " - " + config.getName();

            TransactionSettingsEdit.CardEntryMethod = 34567;
        }

        private void PrintTextBtn_Click(object sender, EventArgs e)
        {
            List<string> messages = new List<string>();
            messages.Add(PrintTextBox.Text);
            data.CloverConnector.Print(new PrintRequest() { text = messages });
        }

        private void BrowseImageButton_Click(object sender, EventArgs e)
        {
            PrintURLTextBox.Clear();
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "Image Files(*.jpg, *.jpeg, *.gif, *.bmp, *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (open.ShowDialog() == DialogResult.OK)
            {
                string filename = open.FileName;
                try
                {
                    Bitmap img = (Bitmap)Image.FromFile(filename);
                    PrintImage.Image = img;
                }
                catch (FileNotFoundException)
                {
                    AlertForm.Show(this, "Invalid file", "Invalid file: " + filename);
                }
                catch (ArgumentException)
                {
                    AlertForm.Show(this, "Invalid Image", "Invalid Image file");
                }
            }
        }

        private void PrintImageButton_Click(object sender, EventArgs e)
        {
            if (PrintURLTextBox.Text != null && PrintURLTextBox.Text != "")
            {
                try
                {
                    PrintImage.Load(PrintURLTextBox.Text);
                }
                catch (Exception ex)
                {
                    AlertForm.Show(this, "Invalid Image", ex.Message);
                    return;
                }
                data.CloverConnector.Print(new PrintRequest() { imageURLs = new List<string> { PrintURLTextBox.Text } });
            }
            else if (PrintImage.Image != null && PrintImage.Image is Bitmap)
            {
                data.CloverConnector.Print(new PrintRequest() { images = new List<Bitmap> { (Bitmap)PrintImage.Image } });
            }
            else
            {
                AlertForm.Show(this, "Invalid Image", "Invalid Image");
            }
        }

        private void DisplayMessageButton_Click(object sender, EventArgs e)
        {
            data.CloverConnector.ShowMessage(DisplayMessageTextbox.Text);

        }

        private void ShowWelcomeButton_Click(object sender, EventArgs e)
        {
            data.CloverConnector.ShowWelcomeScreen();
        }

        private void ShowReceiptButton_Click(object sender, EventArgs e)
        {
            ListViewItem item = OrderPaymentsView.SelectedItems.Cast<ListViewItem>().SingleOrDefault();
            if (item != null)
            {
                POSPayment payment = item.Tag as POSPayment;
                POSRefund refund = item.Tag as POSRefund;

                bool paid = payment?.PaymentStatus == POSPayment.Status.PAID;
                bool authorized = payment?.PaymentStatus == POSPayment.Status.AUTHORIZED;
                bool refunded = refund != null;

                if (paid || authorized || refunded)
                {
                    DisplayReceiptOptionsRequest request = new DisplayReceiptOptionsRequest
                    {
                        orderId = payment?.OrderID ?? refund?.OrderID,
                        paymentId = payment?.PaymentID ?? refund?.PaymentID,
                        refundId = refund?.RefundID,
                    };
                    data.CloverConnector.DisplayReceiptOptions(request);
                }
            }
        }

        private void RefundReceiptOptButton_Click(object sender, EventArgs e)
        {
            ListViewItem item = TransactionsListView.SelectedItems.Cast<ListViewItem>().SingleOrDefault();
            if (item != null)
            {
                POSManualRefund refund = item.Tag as POSManualRefund;

                DisplayReceiptOptionsRequest request = new DisplayReceiptOptionsRequest
                {
                    creditId = refund?.CreditID,
                    orderId = refund?.OrderID,
                };
                data.CloverConnector.DisplayReceiptOptions(request);
            }
        }

        private void ShowThankYouButton_Click(object sender, EventArgs e)
        {
            data.CloverConnector.ShowThankYouScreen();
        }

        private void OpenCashDrawerButton_Click(object sender, EventArgs e)
        {
            data.CloverConnector.OpenCashDrawer(new OpenCashDrawerRequest("Test"));
        }

        private void RetrievePrintJobStatusButton_Click(object sender, EventArgs e)
        {
            PrintJobStatusRequest req = new PrintJobStatusRequest();
            if (RetrievePrintJobStatusText.Text != "")
            {
                req.printRequestId = RetrievePrintJobStatusText.Text;
            }
            else
            {
                RetrievePrintJobStatusText.Text = lastPrintJobId;
                req.printRequestId = lastPrintJobId;
            }

            data.CloverConnector.RetrievePrintJobStatus(req);
        }

        private void DisplayReceiptOptionsButton_Click(object sender, EventArgs e)
        {
            DisplayReceiptOptionsRequest request = new DisplayReceiptOptionsRequest();
            if (DisplayReceiptOptionsText.Text != "")
            {
                request.paymentId = DisplayReceiptOptionsText.Text;
            }
            request.disablePrinting = TransactionSettingsEdit.DisablePrinting ?? false;

            data.CloverConnector.DisplayReceiptOptions(request);
        }

        private void CardDataButton_Click(object sender, EventArgs e)
        {
            ReadCardDataRequest CardDataRequest = new ReadCardDataRequest();
            CardDataRequest.CardEntryMethods = TransactionSettingsEdit.CardEntryMethod;
            CardDataRequest.IsForceSwipePinEntry = false;
            data.CloverConnector.ReadCardData(CardDataRequest);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            data.CloverConnector.InvokeInputOption(new InputOption() { keyPress = Transport.KeyPress.ESC, description = "Cancel" });
        }

        /// <summary>
        /// Used to get a handler for an InputOption that may be sent during a UI Event
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        public EventHandler getHandler(InputOption io)
        {
            return new EventHandler(delegate (object sender, EventArgs args)
            {
                data.CloverConnector.InvokeInputOption(io);
            });
        }

        private void TipAdjustButton_Click(object sender, EventArgs e)
        {
            InputForm inputForm = new InputForm(this);
            inputForm.Label = "Enter Tip Amount";
            inputForm.Title = "Adjust Tip Amount";
            inputForm.FormClosed += (object s, FormClosedEventArgs ce) =>
            {
                if (inputForm.Status == DialogResult.OK)
                {
                    string tipValue = inputForm.Value;
                    try
                    {
                        int tipAmount = int.Parse(tipValue);
                        TipAdjustAuthRequest taRequest = new TipAdjustAuthRequest();

                        if (OrderPaymentsView.SelectedItems.Count == 1)
                        {
                            POSPayment posPayment = OrderPaymentsView.SelectedItems[0].Tag as POSPayment;

                            taRequest.OrderID = posPayment.OrderID;
                            taRequest.PaymentID = posPayment.PaymentID;
                            taRequest.TipAmount = tipAmount;

                            data.CloverConnector.TipAdjustAuth(taRequest);
                        }

                    }
                    catch (Exception)
                    {
                        AlertForm.Show(this, "Invalid Value", "Invalid tip amount: " + tipValue + " Format of 625 expected.");
                    }
                }
            };
            inputForm.Show(this);
        }

        private void VaultCardBtn_Click(object sender, EventArgs e)
        {
            data.CloverConnector.VaultCard(TransactionSettingsEdit.CardEntryMethod);
        }

        public void OnVaultCardResponse(VaultCardResponse vcResponse)
        {
            String screenResponseMsg = "";
            if (vcResponse.Success && vcResponse.Card.token != null)
            {
                POSCard posCard = new POSCard();
                posCard.Name = vcResponse.Card.cardholderName;
                posCard.First6 = vcResponse.Card.first6;
                posCard.Last4 = vcResponse.Card.last4;
                posCard.Token = vcResponse.Card.token;
                posCard.Month = vcResponse.Card.expirationDate.Substring(0, 2);
                posCard.Year = vcResponse.Card.expirationDate.Substring(2, 2);
                data.Store.Cards.Add(posCard);

                uiThread.Send(delegate (object state)
                {
                    addCardToUI(posCard);
                    screenResponseMsg = "Card" + vcResponse.Card.first6 + "xxxxxx" + vcResponse.Card.last4 + " was added";
                    AlertForm.Show(this, "Card Vaulted", screenResponseMsg);
                }, null);
            }
            else
            {
                uiThread.Send(delegate (object state)
                {
                    if (vcResponse.Success)
                    {
                        screenResponseMsg = "Card token was not populated by the payment gateway.  This card cannot be saved.";
                    }
                    else
                    {
                        screenResponseMsg = "Card was not successfully saved";
                    }
                    AlertForm.Show(this, screenResponseMsg, vcResponse.Reason);
                }, null);
            }
        }

        public void OnReadCardDataResponse(ReadCardDataResponse rcdResponse)
        {
            String screenResponseMsg = "";
            if (rcdResponse.Success && rcdResponse.CardData != null &&
                (rcdResponse.CardData.Track1 != null ||
                 rcdResponse.CardData.Track2 != null ||
                 rcdResponse.CardData.Pan != null))
            {
                uiThread.Send(delegate (object state)
                {
                    if (rcdResponse.CardData.Track1 != null)
                    {
                        screenResponseMsg = "Track1: " + rcdResponse.CardData.Track1;
                    }
                    else
                    {
                        if (rcdResponse.CardData.Track2 != null)
                        {
                            screenResponseMsg = "Track2: " + rcdResponse.CardData.Track2;
                        }
                        else
                        {
                            screenResponseMsg = "Pan: " + rcdResponse.CardData.Pan;
                        }
                    }
                    AlertForm.Show(this, "Card Data Info", screenResponseMsg);
                }, null);
            }
            else
            {
                uiThread.Send(delegate (object state)
                {
                    if (rcdResponse.Success)
                    {
                        screenResponseMsg = "Card track and pan information was blank.";
                    }
                    else
                    {
                        screenResponseMsg = "Card was not successfully read";
                    }
                    AlertForm.Show(this, rcdResponse.Reason, screenResponseMsg);
                }, null);
            }
        }

        public void OrderChanged(POSOrder order, POSOrder.OrderChangeTarget target)
        {
            switch (target)
            {
                case POSOrder.OrderChangeTarget.ORDER:
                    {
                        switch (GetCurrentTab())
                        {
                            case ClientTab.ORDER:
                                {
                                    if (order.ID == data.Store.CurrentOrder.ID)
                                    {
                                        uiThread.Send(delegate (object state)
                                        {
                                            UpdateOrderUI();
                                        }, null);
                                    }
                                    break;
                                }
                            case ClientTab.ORDERLIST:
                                {
                                    uiThread.Send(delegate (object state)
                                    {
                                        UpdateOrdersListView();
                                    }, null);
                                    break;
                                }
                        }
                        break;
                    }
                case POSOrder.OrderChangeTarget.ITEM:
                    {
                        switch (GetCurrentTab())
                        {
                            case ClientTab.ORDER:
                                {
                                    if (order.ID == data.Store.CurrentOrder.ID)
                                    {
                                        uiThread.Send(delegate (object state)
                                        {
                                            UpdateOrderUI();
                                        }, null);
                                    }
                                    break;
                                }
                            case ClientTab.ORDERLIST:
                                {
                                    uiThread.Send(delegate (object state)
                                    {
                                        UpdateOrdersListView();
                                    }, null);
                                    break;
                                }
                        }
                        break;
                    }
                case POSOrder.OrderChangeTarget.PAYMENT:
                    {
                        if (GetCurrentTab() == ClientTab.ORDERLIST)
                        {
                            uiThread.Send(delegate (object state)
                            {
                                UpdateOrdersListView();
                            }, null);
                        }
                        break;
                    }
            }
        }

        private void PreAuthButton_Click(object sender, EventArgs e)
        {
            PreAuthRequest request = new PreAuthRequest();
            request.ExternalId = ExternalIDUtil.GenerateRandomString(32);
            request.Amount = 5000; // for the example app, always do $50

            request.CardEntryMethods = TransactionSettingsEdit.CardEntryMethod;
            request.CardNotPresent = TransactionSettingsEdit.CardNotPresent;

            request.DisableRestartTransactionOnFail = TransactionSettingsEdit.DisableRestartTransactionOnFail;
            request.DisablePrinting = TransactionSettingsEdit.DisablePrinting;
            request.DisableReceiptSelection = TransactionSettingsEdit.DisableReceiptSelection;
            request.DisableDuplicateChecking = TransactionSettingsEdit.DisableDuplicateChecking;
            request.AutoAcceptPaymentConfirmations = TransactionSettingsEdit.AutomaticPaymentConfirmation;
            data.CloverConnector.PreAuth(request);
        }

        private void ManualCaptureButton_Click(object sender, EventArgs e)
        {
            ManualCaptureDialog dialog = new ManualCaptureDialog(this);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                CapturePreAuthRequest request = new CapturePreAuthRequest();
                request.PaymentID = dialog.PaymentID;
                request.Amount = dialog.Amount;
                request.TipAmount = dialog.TipAmount;

                data.CloverConnector.CapturePreAuth(request);
            }
        }

        private void IncrementPreAuthButton_Click(object sender, EventArgs e)
        {
            IncrementPreAuthDialog dialog = new IncrementPreAuthDialog(this);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                IncrementPreAuthRequest request = new IncrementPreAuthRequest();
                request.PaymentID = dialog.PaymentID;
                request.Amount = dialog.Amount;

                data.CloverConnector.IncrementPreAuth(request);
            }
        }

        private void refreshPendingPayments_Click(object sender, EventArgs e)
        {
            data.CloverConnector.RetrievePendingPayments();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }

        private void startCustomActivity_Click(object sender, EventArgs e)
        {
            if (customActivityAction.SelectedItem is ComboboxItem item)
            {
                CustomActivityRequest request = new CustomActivityRequest();
                request.Action = item.Value;
                request.Payload = customActivityPayload.Text;
                request.NonBlocking = nonBlockingCB.Checked;
                data.CloverConnector.StartCustomActivity(request);

                // Known example Custom Activity commands - refer to the Clover Android Examples for details
                if (item.Text.Equals("BasicConversationalExample"))
                {
                    customActivityPayload.Text = "Why did the Storm Trooper buy an iPhone?";
                }
                else if (item.Text.Equals("WebViewExample"))
                {
                    customActivityPayload.Text = "Load helloworld";
                }
            }
        }

        private void SendMessageBtn_Click(object sender, EventArgs e)
        {
            if (customActivityAction.SelectedItem is ComboboxItem item)
            {
                MessageToActivity mta = new MessageToActivity();
                mta.Action = item.Value;
                if (item.Text.Equals("BasicConversationalExample"))
                {
                    ConversationQuestionMessage question = new ConversationQuestionMessage();
                    question.message = "Why did the Storm Trooper buy an iPhone?";
                    question.messageType = MessageType.CONVERSATION_QUESTION.ToString();
                    question.payloadClassName = "ConversationQuestionMessage";
                    String jsonPayload = JsonConvert.SerializeObject(question);
                    mta.Payload = jsonPayload;
                }
                else if (item.Text.Equals("WebViewExample"))
                {
                    WebViewMessage wvm = new WebViewMessage();
                    wvm.html = "<html><body><h1>Hello world</h1><a href=</body></html>";
                    wvm.messageType = MessageType.WEBVIEW.ToString();
                    wvm.payloadClassName = "WebViewMessage";
                    wvm.url = "helloworld.com";
                    String jsonPayload = JsonConvert.SerializeObject(wvm);
                    mta.Payload = jsonPayload;
                }
                else
                {
                    mta.Payload = customActivityPayload.Text;
                }
                data.CloverConnector.SendMessageToActivity(mta);
            }
        }

        private void SendMessageToActivity(String payload, String action)
        {
            MessageToActivity mta = new MessageToActivity();
            mta.Action = action;
            mta.Payload = payload;
            data.CloverConnector.SendMessageToActivity(mta);
        }

        private void DeviceStatusBtn_Click(object sender, EventArgs e)
        {
            data.CloverConnector.RetrieveDeviceStatus(new RetrieveDeviceStatusRequest());
        }

        private void RetrievePayment_Click(object sender, EventArgs e)
        {
            RetrievePaymentRequest request = new RetrievePaymentRequest();
            request.externalPaymentId = RetrievePaymentText.Text;
            data.CloverConnector.RetrievePayment(request);
        }

        private void label15_Click(object sender, EventArgs e)
        {
        }

        public event EventHandler<ResponseEventArgs<RetrievePaymentResponse>> PaymentRetrieved;
        public void OnRetrievePaymentResponse(RetrievePaymentResponse response)
        {
            PaymentRetrieved?.Invoke(this, ResponseEventArgs.From(response));

            if (response.Success)
            {
                uiThread.Send(delegate (object state)
                {
                    String details = "No matching payment";
                    Payment payment = response.Payment;
                    if (payment != null)
                    {
                        details = "Created:" + dateFormat(payment.createdTime) + "\nResult: " + payment.result
                       + "\nPaymentId: " + payment.id + "\nOrderId: " + payment.order.id
                        + "\nAmount: " + currencyFormat(payment.amount) + " Tip: " + currencyFormat(payment.tipAmount) + " Tax: " + currencyFormat(payment.taxAmount);
                    }
                    AlertForm.Show(this, response.QueryStatus.ToString(), details);
                }, null);
            }
            else if (response.Result.Equals(ResponseCode.FAIL))
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                }, null);
            }
            else if (response.Result.Equals(ResponseCode.CANCEL))
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, response.Reason, response.Message);
                }, null);
            }
        }
        public static string currencyFormat(long? value)
        {
            double amount = (double)value.GetValueOrDefault(0) / 100;
            return string.Format("{0:C}", amount);
        }

        public static string dateFormat(double datetime)
        {
            var date = (new DateTime(1970, 1, 1)).AddMilliseconds(datetime).ToLocalTime();
            return date.ToString("g", CultureInfo.CreateSpecificCulture("en-us"));
        }

        private void disableReceiptOptionsCB_CheckedChanged(object sender, EventArgs e)
        {
        }

        public void OnPrintJobStatusResponse(PrintJobStatusResponse response)
        {
            if (response.status != PrintJobStatus.PRINTING && response.status != PrintJobStatus.IN_QUEUE)
            {
                uiThread.Send(delegate (object state)
                {
                    AlertForm.Show(this, "Print Job Status", "Print Job Status: " + response.status + "\n" + "Print Request ID: " + response.printRequestId);
                }, null);
            }
        }

        public void OnRetrievePrintersResponse(RetrievePrintersResponse response)
        {
            int numberOfPrinters = response.printers.Count;

            if (buttonPressed == 1)
            {
                PopulatePrintTextDropDown(numberOfPrinters, response.printers);
            }
            else if (buttonPressed == 2)
            {
                PopulatePrintImageDropDown(numberOfPrinters, response.printers);
            }
            else if (buttonPressed == 3)
            {
                PopulateOpenCashDrawerDropDown(numberOfPrinters, response.printers);
            }
        }

        private void PopulatePrintTextDropDown(int numberOfPrinters, List<Printer> printers)
        {
            MenuItem menuItem;
            if (numberOfPrinters != 0)
            {
                textPrintersMenuItem.MenuItems.Clear();
                for (int i = 0; i < numberOfPrinters; i++)
                {
                    Printer printer = printers[i];
                    menuItem = new MenuItem("ID: " + printer.id + " Name: " + printer.name + " Type: " + printer.type);
                    menuItem.Enabled = true;
                    menuItem.Visible = true;
                    menuItem.Click += delegate (object sen, EventArgs args)
                    {
                        selectedPrinter = printer;
                        PrintTextMenu_Click();
                    };
                    textPrintersMenuItem.MenuItems.Add(menuItem);
                }
            }
        }

        private void PopulatePrintImageDropDown(int numberOfPrinters, List<Printer> printers)
        {
            MenuItem menuItem;
            if (numberOfPrinters != 0)
            {
                imagePrintersMenuItem.MenuItems.Clear();
                for (int i = 0; i < numberOfPrinters; i++)
                {
                    Printer printer = printers[i];
                    menuItem = new MenuItem("ID: " + printer.id + " Name: " + printer.name + " Type: " + printer.type);
                    menuItem.Enabled = true;
                    menuItem.Click += delegate (object sen, EventArgs args)
                    {
                        selectedPrinter = printer;
                        PrintImageMenu_Click();
                    };

                    imagePrintersMenuItem.MenuItems.Add(menuItem);

                }
            }

        }

        private void PopulateOpenCashDrawerDropDown(int numberOfPrinters, List<Printer> printers)
        {
            MenuItem menuItem;
            if (numberOfPrinters != 0)
            {
                cashDrawerPrintersMenuItem.MenuItems.Clear();
                for (int i = 0; i < numberOfPrinters; i++)
                {
                    Printer printer = printers[i];
                    menuItem = new MenuItem("ID: " + printer.id + " Name: " + printer.name + " Type: " + printer.type);
                    menuItem.Enabled = true;
                    menuItem.Click += delegate (object sen, EventArgs args)
                    {
                        selectedPrinter = printer;
                        OpenCashDrawerMenu_Click();
                    };

                    cashDrawerPrintersMenuItem.MenuItems.Add(menuItem);

                }
            }
        }

        public void OnPrintJobStatusRequest(PrintJobStatusRequest request)
        {
        }

        private void ConnectStatusLabel_DoubleClick(object sender, EventArgs e)
        {
            StringBuilder report = new StringBuilder();

            report.AppendLine($"{ConnectStatusLabel.Text}");

            if (currentDeviceMerchantInfo != null)
            {
                report.AppendLine();
                report.AppendLine($"Merchant:   { currentDeviceMerchantInfo.merchantName}");
                report.AppendLine($"ID:   {currentDeviceMerchantInfo.merchantID}");
                report.AppendLine($"MID:   {currentDeviceMerchantInfo.merchantMId}");
                report.AppendLine();
                report.AppendLine($"Device Model: {currentDeviceMerchantInfo.Device.Model}");
                report.AppendLine($"Device Name: {currentDeviceMerchantInfo.Device.Name}");
                report.AppendLine($"Device Serial#: {currentDeviceMerchantInfo.Device.Serial}");
            }

            MessageBox.Show(report.ToString(), "Device Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /* This class is used to keep the autoscroll from going crazy when selecting controls on the
     * Miscellaneous tab
    */
    public class CustomTableLayoutPanel : System.Windows.Forms.TableLayoutPanel
    {
        protected override System.Drawing.Point ScrollToControl(System.Windows.Forms.Control activeControl)
        {
            return DisplayRectangle.Location;
        }
    }

    /// <summary>
    /// Autoformatting for currency input
    /// </summary>
    public class CurrencyTextBox : System.Windows.Forms.TextBox
    {
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            CurrencyTextBox_TextChanged(this, EventArgs.Empty);
        }

        bool inTextChanged = false;
        private void CurrencyTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!inTextChanged)
            {
                inTextChanged = true;

                TextBox textbox = (TextBox)sender;
                string text = Regex.Replace(textbox.Text, "[^0-9]", "");

                //Check we are indeed handling a number
                if (decimal.TryParse(text, out decimal value))
                {
                    value /= 100;
                    //Format the text as currency
                    textbox.Text = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C2}", value);
                    textbox.Select(textbox.Text.Length, 0);
                }

                inTextChanged = false;
            }
        }

        Regex money = new Regex(@"^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$");
        private bool TextisValid(string text)
        {
            return money.IsMatch(text);
        }

        Regex regex = new Regex(@"^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$");
        private bool KeyEnteredIsValid(string key)
        {
            return regex.IsMatch(key);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)0x8: break; // Allow backspace
                default:
                    if (!KeyEnteredIsValid(e.KeyChar.ToString()))
                    {
                        e.Handled = true; //ignore this key press
                    }
                    break;
            }
            base.OnKeyPress(e);
        }

        public long? GetAmount()
        {
            //Check we are indeed handling a number
            string text = Regex.Replace(Text, "[^0-9]", "").TrimStart('0');
            if (long.TryParse(text, out long value))
            {
                return value;
            }
            return null;
        }
    }
}
