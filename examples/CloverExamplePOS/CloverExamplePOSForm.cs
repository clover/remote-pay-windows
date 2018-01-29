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
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using com.clover.remotepay.transport.remote;
using System.IO;
using com.clover.remote.order;
using com.clover.sdk.v3.payments;
using Newtonsoft.Json;
using com.clover.sdk.v3.printer;

namespace CloverExamplePOS
{
    public partial class CloverExamplePOSForm : Form, ICloverConnectorListener
    {
        ICloverConnector cloverConnector;
        Store Store;
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

        // hold a default value for the card entry method
        public int CardEntryMethod { get; private set; }

        public void SubscribeToStoreChanges(Store store)
        {
            store.OrderListChange += new Store.OrderListChangeHandler(OrderListChanged);
            store.PreAuthListChange += new Store.PreAuthListChangeHandler(PreAuthListChanged);
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
                new StartupForm(this, OnPairingCode, OnPairingSuccess).Show();
            }, null);

        }

        public void OnPairingCode(string pairingCode)
        {
            Invoke(new Action(() =>
            {
                if (pairingForm != null)
                {
                    pairingForm.Dispose();
                }
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
            Invoke(new Action(() =>
            {
                if (pairingForm != null)
                {
                    pairingForm.Dispose();
                }
            }
            ));
        }

        private void AppShutdown(object sender, EventArgs e)
        {
            if (cloverConnector != null)
            {
                try
                {
                    cloverConnector.Dispose();
                }
                catch (Exception)
                {
                    cloverConnector = null;
                }
            }
        }

        private void ExamplePOSForm_Load(object sender, EventArgs e)
        {
            // some UI cleanup...
            RegisterTabs.Appearance = TabAppearance.FlatButtons;
            RegisterTabs.ItemSize = new Size(0, 1);
            RegisterTabs.SizeMode = TabSizeMode.Fixed;
            // register for app shutdown
            Application.ApplicationExit += new EventHandler(this.AppShutdown);

            OriginalFormTitle = this.Text;

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
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Gift Card - $25 ", 2500, false, false));
            Store.AvailableItems.Add(new POSItem(Guid.NewGuid().ToString(), "Gift Card - $50 ", 5000, false, false));

            Store.AvailableDiscounts.Add(new POSDiscount("None", 0));
            Store.AvailableDiscounts.Add(new POSDiscount("10% Off", 0.1f));
            Store.AvailableDiscounts.Add(new POSDiscount("$5 Off", 500));

            SaleButton.ContextMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem("Sale with Vaulted Card");
            menuItem.Enabled = false;
            menuItem.Click += delegate (object sen, EventArgs args)
            {
                uiThread.Send(delegate (object state)
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
            menuItem.Click += delegate (object sen, EventArgs args)
            {
                uiThread.Send(delegate (object state)
                {
                    PreAuthListForm palForm = new PreAuthListForm(this);
                    palForm.preAuths = Store.PreAuths;
                    palForm.FormClosing += preAuthFormClosing;
                    palForm.Show(this);

                }, null);
            };
            SaleButton.ContextMenu.MenuItems.Add(menuItem);
            SaleButton.Click.Add(PayButton_Click);

            AuthButton.ContextMenu = new ContextMenu();
            menuItem = new MenuItem("Auth with Vaulted Card");
            menuItem.Enabled = false;
            menuItem.Click += delegate (object sen, EventArgs args)
            {
                uiThread.Send(delegate (object state)
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
                cloverConnector.RetrieveDeviceStatus(new RetrieveDeviceStatusRequest(true));
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
                cloverConnector.RetrievePrinters(request);
            };
            PrintTextButton.Click = new List<EventHandler>();
            PrintTextButton.Click.Add(PrintTextBtn_Click);


            //PrintImageButton
            PrintImageButton.ContextMenu = new ContextMenu();
            PrintImageButton.ContextMenu.Popup += delegate (object sen, EventArgs args)
            {
                buttonPressed = 2;
                RetrievePrintersRequest request = new RetrievePrintersRequest();
                cloverConnector.RetrievePrinters(request);
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
                cloverConnector.RetrievePrinters(request);
            };
            OpenCashDrawerButton.Click = new List<EventHandler>();
            OpenCashDrawerButton.Click.Add(OpenCashDrawerButton_Click);


            foreach (POSItem item in Store.AvailableItems)
            {
                StoreItem si = new StoreItem();
                si.Item = item;
                si += StoreItems_ItemSelected;

                StoreItems.Controls.Add(si);
            }

            foreach (POSDiscount discount in Store.AvailableDiscounts)
            {
                StoreDiscount si = new StoreDiscount();
                si.Discount = discount;
                si += StoreItems_DiscountSelected;

                StoreDiscounts.Controls.Add(si);
            }
            SubscribeToStoreChanges(Store);
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
            cloverConnector.Print(req);
        }

        private void OpenCashDrawerMenu_Click()
        {
            OpenCashDrawerRequest req = new OpenCashDrawerRequest("Test");
            if (selectedPrinter != null)
            {
                req.printerId = selectedPrinter.id;
            }
            cloverConnector.OpenCashDrawer(req);
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
                cloverConnector.Print(req);

            }
            else if (PrintImage.Image != null && PrintImage.Image is Bitmap)
            {
                lastPrintJobId = ExternalIDUtil.GenerateRandomString(16);
                PrintRequest req = new PrintRequest((Bitmap)PrintImage.Image, lastPrintJobId, null);
                cloverConnector.Print(req);

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

        public void OrderListChanged(Store store, Store.OrderListAction action)
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
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = payment;
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                lvi.SubItems[0].Text = "PRE-AUTH";
                lvi.SubItems[1].Text = (payment.Amount / 100.0).ToString("C2");

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
            SaleButton.ContextMenu.MenuItems[1].Enabled = Store.PreAuths.Count > 0;
            autoResizeColumns(PreAuthListView);
        }

        private void PayButtonContext_Click(object sender, EventArgs e)
        {
            Pay(null);
        }
        private void PayButtonCard_Click(object sender, EventArgs e)
        {
            object obj = ((MenuItem)sender).Tag;
            if (obj is POSCard)
            {
                Pay((POSCard)obj);
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
            request.Amount = Store.CurrentOrder.Total;

            // Card Entry methods
            int CardEntry = 0;
            CardEntry |= ManualEntryCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_MANUAL : 0;
            CardEntry |= MagStripeCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE : 0;
            CardEntry |= ChipCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT : 0;
            CardEntry |= ContactlessCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS : 0;

            request.CardEntryMethods = CardEntry;
            request.CardNotPresent = CardNotPresentCheckbox.Checked;
            // SaleRequest supported TipModes: TIP_PROVIDED, NO_TIP, ON_SCREEN_BEFORE_PAYMENT
            // NOTE: ON_PAPER would turn the Sale into an AUTH, so it is not valid here
            if (tipAmount.TextLength > 0)
            {
                if (tipModeProvided.Checked)
                {
                    request.TipAmount = tipAmount.GetAmount();
                    request.TipMode = com.clover.remotepay.sdk.TipMode.TIP_PROVIDED;
                }
                else
                {
                    // SaleResponse validation error?
                }
            }
            else
            {
                if (!tipModeDefault.Checked)
                // Default is really null here 
                {
                    if (!tipModeProvided.Checked)
                    {
                        request.TipMode = getTipMode();
                    }
                    else
                    {
                        // SaleResponse validation error?
                    }
                }
            }

            if (signatureThreshold.TextLength > 0)
            {
                request.SignatureThreshold = signatureThreshold.GetAmount();
            }

            if (!signatureDefault.Checked)
            {
                request.SignatureEntryLocation = getSignatureEntryLocation();
            }

            request.TaxAmount = Store.CurrentOrder.TaxAmount;

            if (DisableCashBack.Checked)
            {
                request.DisableCashback = true;
            }
            if (DisableRestartTransactionOnFailure.Checked)
            {
                request.DisableRestartTransactionOnFail = true;
            }
            request.DisablePrinting = disablePrintingCB.Checked;
            if (disableReceiptOptionsCB.Checked)
            {
                request.DisableReceiptSelection = true;
            }
            if (disableDuplicateCheckingCB.Checked)
            {
                request.DisableDuplicateChecking = true;
            }
            if (automaticSignatureConfirmationCB.Checked)
            {
                request.AutoAcceptSignature = true;
            }
            if (automaticPaymentConfirmationCB.Checked)
            {
                request.AutoAcceptPaymentConfirmations = true;
            }
            if (!offlineDefault.Checked)
            {
                request.AllowOfflinePayment = offlineYes.Checked;
            }
            if (!approveOfflineDefault.Checked)
            {
                request.ApproveOfflinePaymentWithoutPrompt = approveOfflineYes.Checked;
            }
            if (!forceOfflineDefault.Checked)
            {
                request.ForceOfflinePayment = forceOfflineYes.Checked;
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

            if (Store.CurrentOrder.TippableAmount != Store.CurrentOrder.Total)
            {
                request.TippableAmount = Store.CurrentOrder.TippableAmount;
            }
            cloverConnector.Sale(request);
        }

        private com.clover.remotepay.sdk.TipMode? getTipMode()
        {
            if (tipModeNone.Checked)
            {
                return com.clover.remotepay.sdk.TipMode.NO_TIP;
            }
            else
            {
                if (tipModeOnScreen.Checked)
                {
                    return com.clover.remotepay.sdk.TipMode.ON_SCREEN_BEFORE_PAYMENT;
                }
                else
                {
                    if (tipModeProvided.Checked)
                    {
                        return com.clover.remotepay.sdk.TipMode.TIP_PROVIDED;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        private DataEntryLocation? getSignatureEntryLocation()
        {
            if (signatureNone.Checked)
            {
                return DataEntryLocation.NONE;
            }
            else
            {
                if (signatureOnPaper.Checked)
                {
                    return DataEntryLocation.ON_PAPER;
                }
                else
                {
                    if (signatureOnScreen.Checked)
                    {
                        return DataEntryLocation.ON_SCREEN;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public void OnSaleResponse(SaleResponse response)
        {
            if (response.Success)
            {
                POSPayment payment = new POSPayment(response.Payment.id, response.Payment.externalPaymentId, response.Payment.order.id, response.Payment.employee.id, response.Payment.amount, response.Payment.tipAmount, response.Payment.cashbackAmount);
                if (response.IsAuth) //Tip Adjustable
                {
                    Store.CurrentOrder.Status = POSOrder.OrderStatus.AUTHORIZED;
                    payment.PaymentStatus = POSPayment.Status.AUTHORIZED;
                }
                else
                {
                    Store.CurrentOrder.Status = POSOrder.OrderStatus.CLOSED;
                    payment.PaymentStatus = POSPayment.Status.PAID;
                }
                Store.CurrentOrder.AddPayment(payment);
                Store.CurrentOrder.Date = (new DateTime(1970, 1, 1)).AddMilliseconds(response.Payment.createdTime).ToLocalTime();

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
            request.Amount = Store.CurrentOrder.Total;
            request.TaxAmount = Store.CurrentOrder.TaxAmount;
            request.ExternalId = ExternalIDUtil.GenerateRandomString(32);

            // Card Entry methods
            long CardEntry = 0;
            CardEntry |= ManualEntryCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_MANUAL : 0;
            CardEntry |= MagStripeCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE : 0;
            CardEntry |= ChipCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT : 0;
            CardEntry |= ContactlessCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS : 0;

            request.CardEntryMethods = CardEntry;
            request.CardNotPresent = CardNotPresentCheckbox.Checked;
            request.DisablePrinting = disablePrintingCB.Checked;

            if (DisableCashBack.Checked)
            {
                request.DisableCashback = true;
            }
            if (!offlineDefault.Checked)
            {
                request.AllowOfflinePayment = offlineYes.Checked;
            }
            if (!approveOfflineDefault.Checked)
            {
                request.ApproveOfflinePaymentWithoutPrompt = approveOfflineYes.Checked;
            }
            if (!forceOfflineDefault.Checked)
            {
                request.ForceOfflinePayment = forceOfflineYes.Checked;
            }
            if (card != null)
            {
                request.VaultedCard = new com.clover.sdk.v3.payments.VaultedCard();
                request.VaultedCard.cardholderName = card.Name;
                request.VaultedCard.first6 = card.First6;
                request.VaultedCard.last4 = card.Last4;
                request.VaultedCard.expirationDate = card.Month + "" + card.Year;
                request.VaultedCard.token = card.Token;
            }

            if (signatureThreshold.TextLength > 0)
            {
                request.SignatureThreshold = signatureThreshold.GetAmount();
            }

            if (!signatureDefault.Checked)
            {
                request.SignatureEntryLocation = getSignatureEntryLocation();
            }

            if (DisableRestartTransactionOnFailure.Checked)
            {
                request.DisableRestartTransactionOnFail = true;
            }
            if (disableReceiptOptionsCB.Checked)
            {
                request.DisableReceiptSelection = true;
            }
            if (disableDuplicateCheckingCB.Checked)
            {
                request.DisableDuplicateChecking = true;
            }
            if (automaticSignatureConfirmationCB.Checked)
            {
                request.AutoAcceptSignature = true;
            }
            if (automaticPaymentConfirmationCB.Checked)
            {
                request.AutoAcceptPaymentConfirmations = true;
            }
            if (Store.CurrentOrder.TippableAmount != Store.CurrentOrder.Total)
            {
                request.TippableAmount = Store.CurrentOrder.TippableAmount;
            }
            // AuthRequest supported TipModes: Always ON_PAPER, so anything else is ignored
            // NOTE: Anything else would turn the Auth into an Sale, so it is not valid here
            cloverConnector.Auth(request);
        }

        public void OnAuthResponse(AuthResponse response)
        {
            if (response.Success)
            {
                if (Result.SUCCESS.Equals(response.Payment.result))
                {
                    POSPayment payment = new POSPayment(response.Payment.id, response.Payment.externalPaymentId, response.Payment.order.id, response.Payment.employee.id, response.Payment.amount, response.Payment.tipAmount, response.Payment.cashbackAmount);
                    if (response.IsAuth)
                    {
                        Store.CurrentOrder.Status = POSOrder.OrderStatus.AUTHORIZED;
                        payment.PaymentStatus = POSPayment.Status.AUTHORIZED;
                    }
                    else
                    {
                        Store.CurrentOrder.Status = POSOrder.OrderStatus.CLOSED;
                        payment.PaymentStatus = POSPayment.Status.PAID;
                    }

                    Store.CurrentOrder.AddPayment(payment);
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

        private void PreAuth(POSCard card)
        {
            StoreItems.BringToFront();
            StoreDiscounts.BringToFront();

            SaleButton.Enabled = false;
            StoreItems.Enabled = false;
            newOrderBtn.Enabled = false;

            PreAuthRequest request = new PreAuthRequest();
            request.Amount = Store.CurrentOrder.Total;
            request.ExternalId = ExternalIDUtil.GenerateRandomString(32);

            // Card Entry methods
            long CardEntry = 0;
            CardEntry |= ManualEntryCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_MANUAL : 0;
            CardEntry |= MagStripeCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE : 0;
            CardEntry |= ChipCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT : 0;
            CardEntry |= ContactlessCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS : 0;

            request.CardEntryMethods = CardEntry;
            request.CardNotPresent = CardNotPresentCheckbox.Checked;
            if (signatureThreshold.TextLength > 0)
            {
                request.SignatureThreshold = signatureThreshold.GetAmount();
            }

            if (!signatureDefault.Checked)
            {
                request.SignatureEntryLocation = getSignatureEntryLocation();
            }

            if (DisableRestartTransactionOnFailure.Checked)
            {
                request.DisableRestartTransactionOnFail = true;
            }
            request.DisablePrinting = disablePrintingCB.Checked;
            if (disableReceiptOptionsCB.Checked)
            {
                request.DisableReceiptSelection = true;
            }
            if (disableDuplicateCheckingCB.Checked)
            {
                request.DisableDuplicateChecking = true;
            }
            cloverConnector.PreAuth(request);
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
                        Store.AddPreAuth(preAuth);

                    }, null);
                }
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

                    lvi.SubItems[0].Text = ppe.paymentId;
                    lvi.SubItems[1].Text = (ppe.amount / 100.0).ToString("C2");

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
                    AlertForm.Show(this, "Custom Activity Response" + (response.Success ? "" : ": Canceled"), response.Payload);
                }
            }, null);
        }

        public void OnMessageFromActivity(MessageFromActivity message)
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
            PhoneNumberMessage message = JsonUtils.deserializeSDK<PhoneNumberMessage>(payloadMessage);
            String phoneNumber = message.phoneNumber;
            CustomerInfo customerInfo = new CustomerInfo();
            customerInfo.customerName = "Ron Burgundy";
            customerInfo.phoneNumber = phoneNumber;
            CustomerInfoMessage customerInfoMessage = new CustomerInfoMessage();
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
            RatingsMessage ratingsMessage = JsonUtils.deserializeSDK<RatingsMessage>(payload);
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
            ConversationResponseMessage jokeResponseMessage = JsonUtils.deserializeSDK<ConversationResponseMessage>(payload);
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "Received JokeResponse of: ", jokeResponseMessage.message);
            }, null);
        }

        public virtual void OnRetrieveDeviceStatusResponse(RetrieveDeviceStatusResponse response)
        {
            uiThread.Send(delegate (object state)
            {
                AlertForm.Show(this, "OnRetrieveDeviceStatusResponse: ", response.State + ":" + JsonUtils.serialize(response.Data));
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
                AlertForm.Show(this, "Print Refund Payment Receipt", (printRefundPaymentReceiptMessage.Refund.amount / 100.0).ToString("C2"));
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
            POSPayment payment = ((PreAuthListForm)sender).selectedPayment;
            if (payment != null)
            {
                CapturePreAuthRequest captureAuthRequest = new CapturePreAuthRequest();
                captureAuthRequest.PaymentID = payment.PaymentID;
                captureAuthRequest.Amount = Store.CurrentOrder.Total;
                if (tipAmount.TextLength > 0)
                {
                    if (tipModeProvided.Checked)
                    {
                        long? tip = tipAmount.GetAmount();
                        captureAuthRequest.TipAmount = tip.HasValue ? tip.Value : 0;
                    }
                }
                else
                {
                    captureAuthRequest.TipAmount = 0;
                }
                cloverConnector.CapturePreAuth(captureAuthRequest);
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
                foreach (POSPayment preAuth in Store.PreAuths)
                {
                    if (preAuth.PaymentID.Equals(response.PaymentId))
                    {
                        uiThread.Send(delegate (object state)
                        {
                            Store.CurrentOrder.Status = POSOrder.OrderStatus.AUTHORIZED;
                            preAuth.PaymentStatus = POSPayment.Status.AUTHORIZED;
                            preAuth.Amount = response.Amount;
                            preAuth.TipAmount = response.TipAmount;
                            Store.CurrentOrder.AddOrderPayment(preAuth);

                            NewOrder(3000);
                            Store.RemovePreAuth(preAuth);
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

        public void OnTipAdjustAuthResponse(TipAdjustAuthResponse response)
        {
            if (response.Success)
            {
                POSOrder order = Store.GetOrder(response.PaymentId);
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
            cloverConnector.ResetDevice();
        }

        //////////////// Closeout methods /////////////
        private void CloseoutButton_Click(object sender, EventArgs e)
        {
            cloverConnector.Closeout(new CloseoutRequest());
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
                Store.CurrentOrder = selOrder;
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

                cloverConnector.VoidPayment(request);
            }
        }
        public void OnVoidPaymentResponse(VoidPaymentResponse response)
        {
            bool voided = false;
            foreach (POSOrder order in Store.Orders)
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
            uiThread.Send(delegate (object state)
            {
                VoidButton.Enabled = false;
                RefundPaymentButton.Enabled = false;
            }, null);

        }

        //////////////// Manual Refund methods /////////////
        private void ManualRefundButton_Click(object sender, EventArgs e)
        {
            ManualRefundRequest request = new ManualRefundRequest();
            request.ExternalId = ExternalIDUtil.GenerateRandomString(32);
            request.Amount = int.Parse(RefundAmount.Text);

            // Card Entry methods
            long CardEntry = 0;
            CardEntry |= ManualEntryCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_MANUAL : 0;
            CardEntry |= MagStripeCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE : 0;
            CardEntry |= ChipCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT : 0;
            CardEntry |= ContactlessCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS : 0;

            request.CardEntryMethods = CardEntry;
            request.DisablePrinting = disablePrintingCB.Checked;

            cloverConnector.ManualRefund(request);
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
                    //ManualRefundReceiptButton.Enabled = false;

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
        }

        private void ManualRefundReceiptButton_Click(object sender, EventArgs e)
        {
            AlertForm.Show(this, "More to come...", "This function is not yet implemented.");
        }


        //////////////// Payment Refund methods /////////////
        private void RefundPaymentButton_Click(object sender, EventArgs e)
        {
            RefundPaymentRequest request = new RefundPaymentRequest();

            if (OrderPaymentsView.SelectedItems.Count == 1)
            {
                POSPayment payment = ((POSPayment)OrderPaymentsView.SelectedItems[0].Tag);
                request.PaymentId = payment.PaymentID;
                POSOrder order = (POSOrder)OrdersListView.SelectedItems[0].Tag;
                request.OrderId = payment.OrderID;
                request.Amount = 0;
                request.FullRefund = true;
                TempObjectMap.Clear();
                TempObjectMap.Add(payment.PaymentID, order);
                cloverConnector.RefundPayment(request);
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

                            POSRefund refund = new POSRefund(response.Refund.id, response.PaymentId, response.OrderId, employeeID, response.Refund.amount);

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
            //TODO: cloverConnector.ChooseReceipt(orderID, paymentID);
        }

        ////////////////// CloverDeviceLister Methods //////////////////////

        public void OnDeviceConnected()
        {
            uiThread.Send(delegate (object state)
            {
                ConnectStatusLabel.Text = "Connecting...";
            }, null);
        }

        public void OnDeviceReady(MerchantInfo merchantInfo)
        {
            uiThread.Send(delegate (object state)
            {
                ConnectStatusLabel.Text = "Ready! " + merchantInfo.merchantName + "(" + merchantInfo.Device.Serial + ")";
                Connected = true;
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
                    Connected = false;
                    //PaymentReset();
                }, null);

            }
            catch (Exception)
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
                            cloverConnector.RejectPayment(request.Payment, challenge);
                            i = request.Challenges.Count;
                        }
                        else if (confirmForm.Status == DialogResult.OK) // Last challenge was accepted
                        {
                            cloverConnector.AcceptPayment(request.Payment);
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
                cloverConnector.ShowDisplayOrder(DisplayOrder);
            }
            else
            {
                displayLineItem.quantity = lineItem.Quantity.ToString();
                UpdateDisplayOrderTotals();
                cloverConnector.ShowDisplayOrder(DisplayOrder);
            }
        }


        ////////////////// UI Events and UI Management //////////////////////
        private void StoreItems_DiscountSelected(object sender, EventArgs e)
        {
            POSDiscount discount = ((StoreDiscount)((Control)sender).Parent).Discount;

            Store.CurrentOrder.Discount = discount;
            Store.NewDiscount = true;  //temporarily disables reapplication of a discount during UpdateDisplayOrderTotals() calls

            DisplayDiscount DisplayDiscount = new DisplayDiscount();
            DisplayDiscount.name = discount.Name;
            DisplayDiscount.amount = Decimal.Divide(discount.Value(Store.CurrentOrder.PreDiscountSubTotal), 100).ToString("C2");
            DisplayDiscount.percentage = (discount.PercentageOff * 100).ToString("###");

            // our example POS business rules say only 1 order discount
            while (DisplayOrder.discounts.elements.Count > 0)
            {
                DisplayDiscount RemovedDisplayDiscount = (DisplayDiscount)DisplayOrder.discounts.elements[0];
                DisplayOrder.discounts.Remove(RemovedDisplayDiscount);
                UpdateDisplayOrderTotals();
                cloverConnector.ShowDisplayOrder(DisplayOrder);
            }

            if (discount.Value(1000) != 0)
            {
                DisplayOrder.addDisplayDiscount(DisplayDiscount);
                UpdateDisplayOrderTotals();
                cloverConnector.ShowDisplayOrder(DisplayOrder);
            }
            Store.NewDiscount = false; //enables reapplication of a discount during UpdateDisplayOrderTotals() calls
        }

        private void UpdateDisplayOrderTotals()
        {
            DisplayOrder.tax = (Store.CurrentOrder.TaxAmount / 100.0).ToString("C2");
            DisplayOrder.subtotal = (Store.CurrentOrder.PreTaxSubTotal / 100.0).ToString("C2");
            DisplayOrder.total = (Store.CurrentOrder.Total / 100.0).ToString("C2");

            // This block of code handles reapplying an existing order discount when new items are added or removed
            // If this method call is the result of a new discount being applied or removed, then this logic should be bypassed
            // as it is already handled as part of the add/remove discount logic
            if (Store.CurrentOrder.Discount != null && !Store.NewDiscount)
            {
                ReapplyOrderDiscount(Store.CurrentOrder.Discount);
            }
        }

        private void ReapplyOrderDiscount(POSDiscount discount)
        {
            DisplayDiscount DisplayDiscount = new DisplayDiscount();
            DisplayDiscount.name = discount.Name;
            DisplayDiscount.amount = Decimal.Divide(discount.Value(Store.CurrentOrder.PreDiscountSubTotal), 100).ToString("C2");
            DisplayDiscount.percentage = (discount.PercentageOff * 100).ToString("###");

            // our example POS business rules say only 1 order discount
            while (DisplayOrder.discounts.elements.Count > 0)
            {
                DisplayDiscount RemovedDisplayDiscount = (DisplayDiscount)DisplayOrder.discounts.elements[0];
                DisplayOrder.discounts.Remove(RemovedDisplayDiscount);
                cloverConnector.ShowDisplayOrder(DisplayOrder);
            }

            if (discount.Value(1000) != 0)
            {
                DisplayOrder.addDisplayDiscount(DisplayDiscount);
                cloverConnector.ShowDisplayOrder(DisplayOrder);
            }

        }

        private void NewOrder_Click(object sender, EventArgs e)
        {
            NewOrder(0);
        }

        private void NewOrder(int welcomeDelay)
        {
            foreach (POSOrder order in Store.Orders) //any pending orders will be removed when creating a new one
            {
                if (order.Status == POSOrder.OrderStatus.PENDING)
                {
                    UnsubscribeToOrderChanges(order);
                }
            }
            Store.CreateOrder();
            SubscribeToOrderChanges(Store.CurrentOrder);
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
                    cloverConnector.ShowWelcomeScreen(); //This will make sure that the customer sees a
                }                                        //Welcome screen anytime a new order is initiated.
            }
        }
        private void timerShowWelcomeScreen(object sender, EventArgs e)
        {
            cloverConnector.ShowWelcomeScreen();
            ((System.Windows.Forms.Timer)sender).Stop();
        }

        private void RebuildOrderOnDevice()
        {
            _suspendUpdateOrderUI = true;
            foreach (POSLineItem item in Store.CurrentOrder.Items)
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
            cloverConnector.ShowDisplayOrder(DisplayOrder);
            _suspendUpdateOrderUI = false;
        }

        private void UpdateOrderUI()
        {
            if (!_suspendUpdateOrderUI)
            {
                currentOrder.Text = Store.CurrentOrder.ID;
                OrderItems.Items.Clear();
                if (Store.CurrentOrder.Items.Count > 0)
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
                    autoResizeColumns(OrderItems);
                }

                if (Store.CurrentOrder.Discount.Value(1000) != 0)
                {
                    DiscountLabel.Text = (Store.CurrentOrder.Discount.Name) + "     -" + (Store.CurrentOrder.Discount.Value(Store.CurrentOrder.PreDiscountSubTotal) / 100.0).ToString("C2");
                }
                else
                {
                    DiscountLabel.Text = " ";
                }
                SubTotal.Text = (Store.CurrentOrder.PreTaxSubTotal / 100.0).ToString("C2");
                TaxAmount.Text = (Store.CurrentOrder.TaxAmount / 100.0).ToString("C2");
                TotalAmount.Text = (Store.CurrentOrder.Total / 100.0).ToString("C2");
            }
        }

        private void TabControl_SelectedIndexChanged(Object sender, EventArgs ev)
        {
            if (TabControl.SelectedIndex == 0) // Register Tab
            {
                if (DisplayOrder.lineItems != null && DisplayOrder.lineItems.elements.Count > 0)
                {
                    cloverConnector.ShowDisplayOrder(DisplayOrder);
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
            lvi.SubItems[2].Text = order.Date.ToString();
            lvi.SubItems[3].Text = order.Status.ToString();
            lvi.SubItems[4].Text = (order.PreTaxSubTotal / 100.0).ToString("C2");
            lvi.SubItems[5].Text = (order.TaxAmount / 100.0).ToString("C2");

        }
        private void OrdersListViewRefresh()
        {
            OrdersListView.Items.Clear();

            if (TabControl.SelectedIndex == 1)
            {
                foreach (POSOrder order in Store.Orders)
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
                        lvi.SubItems[2].Text = order.Date.ToString();
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
                foreach (POSCard card in Store.Cards)
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
                ListViewItem lvi = OrdersListView.SelectedItems[0];
                POSOrder selOrder = (POSOrder)lvi.Tag;
                RefreshSelectedOrderData();
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
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                    if (Exchange is POSPayment)
                    {
                        lvi.SubItems[0].Text = (Exchange is POSPayment) ? ((POSPayment)Exchange).PaymentStatus.ToString() : "";
                        lvi.SubItems[1].Text = (Exchange.Amount / 100.0).ToString("C2");
                        lvi.SubItems[2].Text = (Exchange is POSPayment) ? (((POSPayment)Exchange).TipAmount / 100.0).ToString("C2") : "";
                        lvi.SubItems[3].Text = (Exchange is POSPayment) ? ((((POSPayment)Exchange).TipAmount + Exchange.Amount) / 100.0).ToString("C2") : (Exchange.Amount / 100.0).ToString("C2");
                        lvi.SubItems[4].Text = (Exchange is POSPayment) ? (((POSPayment)Exchange).ExternalID) : "";
                    }
                    else if (Exchange is POSRefund)
                    {
                        lvi.SubItems[0].Text = "REFUND";
                        lvi.SubItems[3].Text = (Exchange.Amount / 100.0).ToString("C2");
                    }

                    OrderPaymentsView.Items.Add(lvi);
                }
                if (selOrder.Status == POSOrder.OrderStatus.OPEN) //Allow editing of the order if it is in Open status
                {
                    OpenOrder_Button.Enabled = true;
                }
                ResetOrderPaymentButtons(); // enable/disable payment buttons
                autoResizeColumns(OrderPaymentsView);
                autoResizeColumns(OrderDetailsListView);
            }
        }
        public void PaymentReset()
        {
            StoreItems.Enabled = true;
            TabControl.Enabled = true;

            if (DisplayOrder != null && DisplayOrder.lineItems != null && DisplayOrder.lineItems.elements.Count > 0)
            {
                cloverConnector.ShowDisplayOrder(DisplayOrder);
            }

            UpdateOrderUI();
        }

        private void OrderPaymentsView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetOrderPaymentButtons();
        }

        private void ResetOrderPaymentButtons()
        {
            if (OrderPaymentsView.SelectedIndices.Count == 1 && OrderPaymentsView.SelectedItems[0].Tag is POSRefund)
            {
                // ShowReceiptButton.Enabled = true;
            }
            else
            {
                if (OrderPaymentsView.SelectedIndices.Count == 1 && (OrderPaymentsView.SelectedItems[0].Tag is POSPayment) && (((POSPayment)OrderPaymentsView.SelectedItems[0].Tag).PaymentStatus == POSPayment.Status.PAID || ((POSPayment)OrderPaymentsView.SelectedItems[0].Tag).PaymentStatus == POSPayment.Status.AUTHORIZED))
                {
                    VoidButton.Enabled = true;
                    RefundPaymentButton.Enabled = true;
                    ShowReceiptButton.Enabled = true;

                }
                else
                {
                    VoidButton.Enabled = false;
                    RefundPaymentButton.Enabled = false;
                    ShowReceiptButton.Enabled = false;
                }
            }

            if (OrderPaymentsView.SelectedIndices.Count == 1 && (OrderPaymentsView.SelectedItems[0].Tag is POSPayment) && ((POSPayment)OrderPaymentsView.SelectedItems[0].Tag).PaymentStatus == POSPayment.Status.AUTHORIZED)
            {
                TipAdjustButton.Enabled = true;
            }
            else
            {
                TipAdjustButton.Enabled = false;
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

        private void ExamplePOSForm_Closed(object sender, FormClosedEventArgs e)
        {
            if (cloverConnector != null)
            {
                cloverConnector.ShowWelcomeScreen(); // this may not fire, if the queue is processed before Exit();
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
            cloverConnector.ShowDisplayOrder(DisplayOrder);
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
                cloverConnector.ShowDisplayOrder(DisplayOrder);
                UpdateOrderUI();
            }
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            RemoveSelectedItemFromCurrentOrder();
        }

        private void RemoveSelectedItemFromCurrentOrder()
        {
            Store.CurrentOrder.RemoveItem(SelectedLineItem);
            DisplayLineItem dli = posLineItemToDisplayLineItem[SelectedLineItem];
            DisplayOrder.removeDisplayLineItem(dli);
            UpdateDisplayOrderTotals();
            cloverConnector.ShowDisplayOrder(DisplayOrder);
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
            cloverConnector.ShowDisplayOrder(DisplayOrder);

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
            if (cloverConnector != null)
            {
                cloverConnector.RemoveCloverConnectorListener(this);

                OnDeviceDisconnected(); // for any disabling, messaging, etc.
                SaleButton.Enabled = false; // everything can work except Pay
                cloverConnector.Dispose();
            }

            if (config is RemoteRESTCloverConfiguration)
            {
                cloverConnector = new RemoteRESTCloverConnector(config);
            }
            else if (config is RemoteWebSocketCloverConfiguration)
            {
                cloverConnector = new RemoteWebSocketCloverConnector(config);
            }
            else
            {
                cloverConnector = CloverConnectorFactory.createICloverConnector(config);
            }

            cloverConnector.AddCloverConnectorListener(this);
            cloverConnector.InitializeConnection();

            //UI cleanup
            this.Text = OriginalFormTitle + " - " + config.getName();
            CardEntryMethod = 34567;

            ManualEntryCheckbox.Checked = (CardEntryMethod & CloverConnector.CARD_ENTRY_METHOD_MANUAL) == CloverConnector.CARD_ENTRY_METHOD_MANUAL;
            MagStripeCheckbox.Checked = (CardEntryMethod & CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE) == CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE;
            ChipCheckbox.Checked = (CardEntryMethod & CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT) == CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT;
            ContactlessCheckbox.Checked = (CardEntryMethod & CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS) == CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS;
        }

        private void PrintTextBtn_Click(object sender, EventArgs e)
        {
            List<string> messages = new List<string>();
            messages.Add(PrintTextBox.Text);
            cloverConnector.PrintText(messages);
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
                cloverConnector.PrintImageFromURL(PrintURLTextBox.Text);

            }
            else if (PrintImage.Image != null && PrintImage.Image is Bitmap)
            {
                cloverConnector.PrintImage((Bitmap)PrintImage.Image);
            }
            else
            {
                AlertForm.Show(this, "Invalid Image", "Invalid Image");
            }
        }

        private void DisplayMessageButton_Click(object sender, EventArgs e)
        {
            cloverConnector.ShowMessage(DisplayMessageTextbox.Text);

        }

        private void ShowWelcomeButton_Click(object sender, EventArgs e)
        {
            cloverConnector.ShowWelcomeScreen();
        }

        private void ShowReceiptButton_Click(object sender, EventArgs e)
        {
            if (OrderPaymentsView.SelectedItems.Count == 1)
            {
                if (OrderPaymentsView.SelectedItems[0].Tag is POSPayment)
                {
                    POSPayment payment = ((POSPayment)OrderPaymentsView.SelectedItems[0].Tag);
                    cloverConnector.DisplayPaymentReceiptOptions(payment.OrderID, payment.PaymentID);
                }
            }
        }

        private void ShowThankYouButton_Click(object sender, EventArgs e)
        {
            cloverConnector.ShowThankYouScreen();
        }

        private void OpenCashDrawerButton_Click(object sender, EventArgs e)
        {
            cloverConnector.OpenCashDrawer("Test");
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

            cloverConnector.RetrievePrintJobStatus(req);
        }

        private void CardDataButton_Click(object sender, EventArgs e)
        {
            int CardEntry = 0;
            CardEntry |= ManualEntryCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_MANUAL : 0;
            CardEntry |= MagStripeCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE : 0;
            CardEntry |= ChipCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT : 0;
            CardEntry |= ContactlessCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS : 0;
            ReadCardDataRequest CardDataRequest = new ReadCardDataRequest();
            CardDataRequest.CardEntryMethods = CardEntry;
            CardDataRequest.IsForceSwipePinEntry = false;
            cloverConnector.ReadCardData(CardDataRequest);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            cloverConnector.Cancel();
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
                cloverConnector.InvokeInputOption(io);
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

                            cloverConnector.TipAdjustAuth(taRequest);
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

        private void EntryCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            int CardEntry = 0;
            CardEntry |= ManualEntryCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_MANUAL : 0;
            CardEntry |= MagStripeCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE : 0;
            CardEntry |= ChipCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT : 0;
            CardEntry |= ContactlessCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS : 0;

            CardEntryMethod = CardEntry;
            if (ManualEntryCheckbox.Checked)
                CardNotPresentCheckbox.Enabled = true;
            else
            {
                CardNotPresentCheckbox.Enabled = false;
                CardNotPresentCheckbox.Checked = false;
            }
        }

        private void VaultCardBtn_Click(object sender, EventArgs e)
        {
            // Card Entry methods, if you want to override what is set as the default in the connector
            int CardEntry = 0;
            CardEntry |= ManualEntryCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_MANUAL : 0;
            CardEntry |= MagStripeCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE : 0;
            CardEntry |= ChipCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT : 0;
            CardEntry |= ContactlessCheckbox.Checked ? CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS : 0;

            cloverConnector.VaultCard(CardEntry);
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
                Store.Cards.Add(posCard);

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
                                    if (order.ID == Store.CurrentOrder.ID)
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
                                    if (order.ID == Store.CurrentOrder.ID)
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

            // Card Entry methods
            long CardEntry = 0;
            CardEntry |= ManualEntryCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_MANUAL : 0;
            CardEntry |= MagStripeCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE : 0;
            CardEntry |= ChipCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT : 0;
            CardEntry |= ContactlessCheckbox.Checked ? (uint)CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS : 0;

            request.CardEntryMethods = CardEntry;
            request.CardNotPresent = CardNotPresentCheckbox.Checked;
            if (signatureThreshold.TextLength > 0)
            {
                request.SignatureThreshold = signatureThreshold.GetAmount();
            }

            if (!signatureDefault.Checked)
            {
                request.SignatureEntryLocation = getSignatureEntryLocation();
            }

            if (DisableRestartTransactionOnFailure.Checked)
            {
                request.DisableRestartTransactionOnFail = true;
            }
            request.DisablePrinting = disablePrintingCB.Checked;
            if (disableReceiptOptionsCB.Checked)
            {
                request.DisableReceiptSelection = true;
            }
            if (disableDuplicateCheckingCB.Checked)
            {
                request.DisableDuplicateChecking = true;
            }
            if (automaticPaymentConfirmationCB.Checked)
            {
                request.AutoAcceptPaymentConfirmations = true;
            }
            cloverConnector.PreAuth(request);
        }

        private void refreshPendingPayments_Click(object sender, EventArgs e)
        {
            cloverConnector.RetrievePendingPayments();
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
            CustomActivityRequest car = new CustomActivityRequest();
            ComboboxItem item = customActivityAction.Items[customActivityAction.SelectedIndex] as ComboboxItem;
            car.Action = item.Value;
            car.Payload = customActivityPayload.Text;
            car.NonBlocking = nonBlockingCB.Checked;
            cloverConnector.StartCustomActivity(car);
            if (item.Text.Equals("BasicConversationalExample"))
            {
                customActivityPayload.Text = "Why did the Storm Trooper buy an iPhone?";
            }
            else if (item.Text.Equals("WebViewExample"))
            {
                customActivityPayload.Text = "Load helloworld";
            }
        }

        private void SendMessageBtn_Click(object sender, EventArgs e)
        {
            MessageToActivity mta = new MessageToActivity();
            ComboboxItem item = customActivityAction.Items[customActivityAction.SelectedIndex] as ComboboxItem;
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
            cloverConnector.SendMessageToActivity(mta);
        }

        private void SendMessageToActivity(String payload, String action)
        {
            MessageToActivity mta = new MessageToActivity();
            mta.Action = action;
            mta.Payload = payload;
            cloverConnector.SendMessageToActivity(mta);
        }

        private void DeviceStatusBtn_Click(object sender, EventArgs e)
        {
            cloverConnector.RetrieveDeviceStatus(new RetrieveDeviceStatusRequest());
        }

        private void RetrievePayment_Click(object sender, EventArgs e)
        {
            RetrievePaymentRequest request = new RetrievePaymentRequest();
            request.externalPaymentId = RetrievePaymentText.Text;
            cloverConnector.RetrievePayment(request);
        }

        private void label15_Click(object sender, EventArgs e)
        {
        }

        public void OnRetrievePaymentResponse(RetrievePaymentResponse response)
        {
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
            double amount = (double)value / 100;
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
            throw new NotImplementedException();
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

    /* 
      Autoformatting for currency input
    */
    public class CurrencyTextBox : System.Windows.Forms.TextBox
    {
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            CurrencyTextBox_TextChanged(this, EventArgs.Empty);
        }
        private void CurrencyTextBox_TextChanged(object sender, EventArgs e)
        {
            //Remove previous formatting, or the decimal check will fail including leading zeros
            TextBox textBox1 = (TextBox)sender;
            string value = textBox1.Text.Replace(",", "")
                .Replace("$", "").Replace(".", "").TrimStart('0');
            decimal ul;
            //Check we are indeed handling a number
            if (decimal.TryParse(value, out ul))
            {
                ul /= 100;
                //Unsub the event so we don't enter a loop
                textBox1.TextChanged -= CurrencyTextBox_TextChanged;
                //Format the text as currency
                textBox1.Text = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C2}", ul);
                textBox1.TextChanged += CurrencyTextBox_TextChanged;
                textBox1.Select(textBox1.Text.Length, 0);
            }
        }

        private bool TextisValid(string text)
        {
            Regex money = new Regex(@"^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$");
            return money.IsMatch(text);
        }
        private bool KeyEnteredIsValid(string key)
        {
            Regex regex;
            regex = new Regex(@"^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$");
            return regex.IsMatch(key);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!KeyEnteredIsValid(e.KeyChar.ToString()))
            {
                e.Handled = true; //ignore this key press
            }
            base.OnKeyPress(e);
        }
        public long? GetAmount()
        {
            string value = this.Text.Replace(",", "").Replace("$", "").Replace(".", "").TrimStart('0');
            long ul;
            //Check we are indeed handling a number
            if (long.TryParse(value, out ul))
            {
                return ul;
            }
            return null;
        }
    }

    public class DropDownButton : Button
    {
        public new List<EventHandler> Click { get; set; }

        public DropDownButton()
        {
            Click = new List<EventHandler>();
            init();
        }

        private void init()
        {
            Padding = new Padding(Padding.Left, Padding.Top, Padding.Right + 15, Padding.Bottom);
            base.Click += Clicked;
        }

        private void Clicked(object sender, EventArgs e)
        {
            if (e is MouseEventArgs)
            {
                if (((MouseEventArgs)e).X < this.Size.Width - 15)
                {
                    foreach (EventHandler ch in Click)
                    {
                        if (ch != null)
                        {
                            ch(sender, e);
                        }
                    }
                }
                else
                {
                    //ContextMenu.Show(this, new Point(this.Location.X-this.Size.Width, this.Location.Y+this.Size.Height));
                    ContextMenu.Show(this, new Point(0, this.Size.Height - Margin.Bottom));
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // draw box

            Pen borderPen = new Pen(Color.FromArgb(16, 0, 0, 0)); // TODO: needs to be text color
            int ddAreaX = this.Size.Width - 15;
            int ddAreaHeight = this.Size.Height - Padding.Top - Padding.Bottom - 4; // 3 px space on top and bottom
            pevent.Graphics.DrawLine(borderPen, new Point(ddAreaX, 3), new Point(ddAreaX, ddAreaHeight));

            // draw triangle
            int triangleCenterX = ddAreaX + 7;
            int triangleCenterY = this.Size.Height / 2;

            Point[] triangle = new Point[4];
            triangle[0] = new Point(triangleCenterX - 3, triangleCenterY - 2);
            triangle[1] = new Point(triangleCenterX + 3, triangleCenterY - 2);
            triangle[2] = new Point(triangleCenterX, triangleCenterY + 2);
            triangle[3] = triangle[0];
            Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0)); // TODO: needs to be text color
            pevent.Graphics.FillPolygon(brush, triangle);
        }
    }
}
