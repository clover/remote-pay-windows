// Copyright (C) 2016 Clover Network, Inc.
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

using com.clover.remotepay.sdk;
using com.clover.remote.order;
using com.clover.sdk.remote.websocket;
using com.clover.remotepay.sdk.service.client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using System.ComponentModel;
using WebSocket4Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using com.clover.sdk.v3.payments;

namespace com.clover.remotepay.transport.remote
{

    /// <summary>
    /// Custom ICloverConnector that talks to the 
    /// Clover Connector WebSocket Service. This wouldn't normally
    /// be used because it is in .NET, and it would normally
    /// make more sense to use the DLL directly in .NET
    /// </summary>
    public class RemoteWebSocketCloverConnector : ICloverConnector
    {
        private bool _isReady = false;

        public bool IsReady
        {
            get { return _isReady; }
            set { }
        }
        public int CardEntryMethod { get; set; }
        public bool DisableCashBack { get; set; }
        public bool DisablePrinting { get; set; }
        public bool DisableRestartTransactionOnFail { get; set; }
        public bool DisableTip { get; set; }
        string _SDKInfo;
        public string SDKInfo
        {
            get
            {
                return this._SDKInfo;
            }
        }

        List<ICloverConnectorListener> listeners = new List<ICloverConnectorListener>();
        private CloverDeviceConfiguration config;

        WebSocket websocket;
        string endpoint = "ws://localhost:8889";

        public RemoteWebSocketCloverConnector()
        {
        }

        public RemoteWebSocketCloverConnector(CloverDeviceConfiguration config)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("CloverConnector");
            _SDKInfo = AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyDescriptionAttribute>(assembly).Description + ":"
                + (AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyFileVersionAttribute>(assembly)).Version
                + (AssemblyUtils.GetAssemblyAttribute<System.Reflection.AssemblyInformationalVersionAttribute>(assembly)).InformationalVersion;
            this.config = config;
            endpoint = ((RemoteWebSocketCloverConfiguration)config).endpoint;
        }
        public void InitializeConnection()
        {
            CardEntryMethod = CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT | CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE | CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS;
            websocket = new WebSocket(endpoint);
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket connection open");
            _isReady = true;
            websocket.Send(JsonUtils.serialize(new StatusRequestMessage()));
        }

        private void websocket_Error(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket error");
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket connection closed");
#if DEBUG
            System.GC.Collect();
#endif
            _isReady = false;
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += (object s, DoWorkEventArgs dwea) =>
            {
                Thread.Sleep(100);
                websocket.Open();
                Console.WriteLine("Done trying to open");
            };
            bg.RunWorkerAsync();
        }


        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            // do the parsing of the message, targeted for the callback listener
            JObject jsonObj = null;
            try
            {
                jsonObj = (JObject)JsonConvert.DeserializeObject(e.Message);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message + " => " + e.Message);
                listeners.ForEach(listener => listener.OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.EXCEPTION, 0, null, exc.Message + " => " + e.Message)));
                return;
            }

            JToken method = jsonObj.GetValue(ServicePayloadConstants.PROP_METHOD);
            if (method == null)
            {
                listeners.ForEach(listener => listener.OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.VALIDATION_ERROR, 0, null, "Invalid message: " + e.Message)));
                return;
            }
            JObject payload = (JObject)jsonObj.GetValue(ServicePayloadConstants.PROP_PAYLOAD);
            WebSocketMethod wsm = (WebSocketMethod)Enum.Parse(typeof(WebSocketMethod), method.ToString());

            try
            {
                switch (wsm)
                {
                    case WebSocketMethod.DeviceActivityStart:
                        {
                            CloverDeviceEvent deviceEvent = JsonUtils.deserialize<CloverDeviceEvent>(payload.ToString());
                            listeners.ForEach(listener => listener.OnDeviceActivityStart(deviceEvent));
                            break;
                        }
                    case WebSocketMethod.DeviceActivityEnd:
                        {
                            CloverDeviceEvent deviceEvent = JsonUtils.deserialize<CloverDeviceEvent>(payload.ToString());
                            listeners.ForEach(listener => listener.OnDeviceActivityEnd(deviceEvent));
                            break;
                        }
                    case WebSocketMethod.DeviceError:
                        {
                            CloverDeviceErrorEvent deviceErrorEvent = JsonUtils.deserialize<CloverDeviceErrorEvent>(payload.ToString());
                            listeners.ForEach(listener => listener.OnDeviceError(deviceErrorEvent));
                            break;
                        }
                    case WebSocketMethod.DeviceConnected:
                        {
                            listeners.ForEach(listener => listener.OnDeviceConnected());
                            break;
                        }
                    case WebSocketMethod.DeviceDisconnected:
                        {
                            listeners.ForEach(listener => listener.OnDeviceDisconnected());
                            break;
                        }
                    case WebSocketMethod.DeviceReady:
                        {
                            MerchantInfo merchantInfo = JsonUtils.deserialize<MerchantInfo>(payload.ToString());
                            listeners.ForEach(listener => listener.OnDeviceReady(merchantInfo));
                            break;
                        }
                    case WebSocketMethod.VerifySignatureRequest:
                        {
                            VerifySignatureRequest svr = JsonUtils.deserialize<VerifySignatureRequest>(payload.ToString());
                            WebSocketSigVerRequestHandler handler = new WebSocketSigVerRequestHandler(this, svr);
                            listeners.ForEach(listener => listener.OnVerifySignatureRequest(handler));
                            break;
                        }
                    case WebSocketMethod.SaleResponse:
                        {
                            SaleResponse sr = JsonUtils.deserialize<SaleResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnSaleResponse(sr));
                            break;
                        }
                    case WebSocketMethod.PreAuthResponse:
                        {
                            PreAuthResponse pr = JsonUtils.deserialize<PreAuthResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnPreAuthResponse(pr));
                            break;
                        }
                    case WebSocketMethod.AuthResponse:
                        {
                            AuthResponse ar = JsonUtils.deserialize<AuthResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnAuthResponse(ar));
                            break;
                        }
                    case WebSocketMethod.CapturePreAuthResponse:
                        {
                            CapturePreAuthResponse ar = JsonUtils.deserialize<CapturePreAuthResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnCapturePreAuthResponse(ar));
                            break;
                        }
                    case WebSocketMethod.RefundPaymentResponse:
                        {
                            RefundPaymentResponse sr = JsonUtils.deserialize<RefundPaymentResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnRefundPaymentResponse(sr));
                            break;
                        }
                    case WebSocketMethod.VoidPaymentResponse:
                        {
                            VoidPaymentResponse sr = JsonUtils.deserialize<VoidPaymentResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnVoidPaymentResponse(sr));
                            break;
                        }
                    case WebSocketMethod.ManualRefundResponse:
                        {
                            ManualRefundResponse sr = JsonUtils.deserialize<ManualRefundResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnManualRefundResponse(sr));
                            break;
                        }
                    case WebSocketMethod.TipAdjustAuthResponse:
                        {
                            TipAdjustAuthResponse taar = JsonUtils.deserialize<TipAdjustAuthResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnTipAdjustAuthResponse(taar));
                            break;
                        }
                    case WebSocketMethod.VaultCardResponse:
                        {
                            VaultCardResponse vcr = JsonUtils.deserialize<VaultCardResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnVaultCardResponse(vcr));
                            break;
                        }
                    case WebSocketMethod.ReadCardDataResponse:
                        {
                            ReadCardDataResponse rcdr = JsonUtils.deserialize<ReadCardDataResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnReadCardDataResponse(rcdr));
                            break;
                        }
                    case WebSocketMethod.CloseoutResponse:
                        {
                            CloseoutResponse cr = JsonUtils.deserialize<CloseoutResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnCloseoutResponse(cr));
                            break;
                        }
                    case WebSocketMethod.ConfirmPaymentRequest:
                        {
                            ConfirmPaymentRequest cpr = JsonUtils.deserialize<ConfirmPaymentRequest>(payload.ToString());
                            listeners.ForEach(listener => listener.OnConfirmPaymentRequest(cpr));
                            break;
                        }
                    case WebSocketMethod.RetrievePendingPaymentsResponse:
                        {
                            RetrievePendingPaymentsResponse rppr = JsonUtils.deserialize<RetrievePendingPaymentsResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnRetrievePendingPaymentsResponse(rppr));
                            break;
                        }
                    case WebSocketMethod.PrintManualRefundDeclinedReceipt:
                        {
                            PrintManualRefundDeclineReceiptMessage pmrdrm = JsonUtils.deserialize<PrintManualRefundDeclineReceiptMessage>(payload.ToString());
                            listeners.ForEach(listener => listener.OnPrintManualRefundDeclineReceipt(pmrdrm));
                            break;
                        }
                    case WebSocketMethod.PrintManualRefundReceipt:
                        {
                            PrintManualRefundReceiptMessage pmrrm = JsonUtils.deserialize<PrintManualRefundReceiptMessage>(payload.ToString());
                            listeners.ForEach(listener => listener.OnPrintManualRefundReceipt(pmrrm));
                            break;
                        }
                    case WebSocketMethod.PrintPaymentDeclinedReceipt:
                        {
                            PrintPaymentDeclineReceiptMessage ppdrm = JsonUtils.deserialize<PrintPaymentDeclineReceiptMessage>(payload.ToString());
                            listeners.ForEach(listener => listener.OnPrintPaymentDeclineReceipt(ppdrm));
                            break;
                        }
                    case WebSocketMethod.PrintPaymentMerchantCopyReceipt:
                        {
                            PrintPaymentMerchantCopyReceiptMessage ppmcrm = JsonUtils.deserialize<PrintPaymentMerchantCopyReceiptMessage>(payload.ToString());
                            listeners.ForEach(listener => listener.OnPrintPaymentMerchantCopyReceipt(ppmcrm));
                            break;
                        }
                    case WebSocketMethod.PrintPaymentReceipt:
                        {
                            PrintPaymentReceiptMessage pprm = JsonUtils.deserialize<PrintPaymentReceiptMessage>(payload.ToString());
                            listeners.ForEach(listener => listener.OnPrintPaymentReceipt(pprm));
                            break;
                        }
                    case WebSocketMethod.PrintPaymentRefundReceipt:
                        {
                            PrintRefundPaymentReceiptMessage prprm = JsonUtils.deserialize<PrintRefundPaymentReceiptMessage>(payload.ToString());
                            listeners.ForEach(listener => listener.OnPrintRefundPaymentReceipt(prprm));
                            break;
                        }
                    case WebSocketMethod.CustomActivityResponse:
                        {
                            CustomActivityResponse car = JsonUtils.deserialize<CustomActivityResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnCustomActivityResponse(car));
                            break;
                        }
                    case WebSocketMethod.MessageFromActivity:
                        {
                            MessageFromActivity mta = JsonUtils.deserialize<MessageFromActivity>(payload.ToString());
                            listeners.ForEach(listener => listener.OnMessageFromActivity(mta));
                            break;
                        }
                    case WebSocketMethod.RetrieveDeviceStatusResponse:
                        {
                            RetrieveDeviceStatusResponse rdsr = JsonUtils.deserialize<RetrieveDeviceStatusResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnRetrieveDeviceStatusResponse(rdsr));
                            break;
                        }
                    case WebSocketMethod.ResetDeviceResponse:
                        {
                            ResetDeviceResponse rdr = JsonUtils.deserialize<ResetDeviceResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnResetDeviceResponse(rdr));
                            break;
                        }
                    case WebSocketMethod.RetrievePaymentResponse:
                        {
                            RetrievePaymentResponse rpr = JsonUtils.deserialize<RetrievePaymentResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnRetrievePaymentResponse(rpr));
                            break;
                        }
                    case WebSocketMethod.RetrievePrintersResponse:
                        {
                            RetrievePrintersResponse rpr = JsonUtils.deserialize<RetrievePrintersResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnRetrievePrintersResponse(rpr));
                            break;
                        }
                    case WebSocketMethod.PrintJobStatusRequest:
                        {
                            PrintJobStatusRequest pjsr = JsonUtils.deserialize<PrintJobStatusRequest>(payload.ToString());
                            listeners.ForEach(listener => listener.OnPrintJobStatusRequest(pjsr));
                            break;
                        }
                    case WebSocketMethod.PrintJobStatusResponse:
                        {
                            PrintJobStatusResponse pjsr = JsonUtils.deserialize<PrintJobStatusResponse>(payload.ToString());
                            listeners.ForEach(listener => listener.OnPrintJobStatusResponse(pjsr));
                            break;
                        }
                    case WebSocketMethod.RetrievePrintersRequest:
                        {
                            RetrievePrintersRequest rpr = JsonUtils.deserialize<RetrievePrintersRequest>(payload.ToString());
                            listeners.ForEach(listener => listener.OnRetrievePrintersRequest(rpr));
                            break;
                        }
                    
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            listeners.Add(connectorListener);
        }

        public void RemoveCloverConnectorListener(ICloverConnectorListener connectorListener)
        {
            listeners.Remove(connectorListener);
        }

        public void AcceptSignature(VerifySignatureRequest request)
        {
            if (websocket != null)
            {
                AcceptSignatureRequestMessage message = new AcceptSignatureRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void RejectSignature(VerifySignatureRequest request)
        {
            if (websocket != null)
            {
                RejectSignatureRequestMessage message = new RejectSignatureRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void Auth(AuthRequest request)
        {
            if (websocket != null)
            {
                AuthRequestMessage message = new AuthRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void PreAuth(PreAuthRequest request)
        {
            if (websocket != null)
            {
                PreAuthRequestMessage message = new PreAuthRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void Cancel()
        {
            if (websocket != null)
            {
                websocket.Send(JsonUtils.serialize(new CancelRequestMessage()));
            }
        }

        public void CapturePreAuth(CapturePreAuthRequest request)
        {
            if (websocket != null)
            {
                CapturePreAuthRequestMessage message = new CapturePreAuthRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void VaultCard(int? CardEntryMethods)
        {
            if (websocket != null)
            {
                VaultCardRequestMessage message = new VaultCardRequestMessage();
                message.payload = new VaultCardMessage(CardEntryMethods);
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void ReadCardData(ReadCardDataRequest request)
        {
            if (websocket != null)
            {
                ReadCardDataRequestMessage message = new ReadCardDataRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void StartCustomActivity(CustomActivityRequest request)
        {
            if (websocket != null)
            {
                CustomActivityRequestMessage message = new CustomActivityRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void Closeout(CloseoutRequest request)
        {
            if (websocket != null)
            {
                CloseoutRequestMessage message = new CloseoutRequestMessage();
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void ShowDisplayOrder(DisplayOrder order)
        {
            if (websocket != null)
            {
                DisplayOrderRequestMessage message = new DisplayOrderRequestMessage();
                message.payload = order;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void DiscountAddedToDisplayOrder(DisplayOrder order, DisplayDiscount discount)
        {
            if (websocket != null)
            {
                DiscountAddedToDisplayOrderRequestMessage message = new DiscountAddedToDisplayOrderRequestMessage();
                DiscountAddedToDisplayOrder payload = new DiscountAddedToDisplayOrder();
                payload.DisplayDiscount = discount;
                payload.DisplayOrder = order;
                message.payload = payload;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void DiscountRemovedFromDisplayOrder(DisplayOrder order, DisplayDiscount discount)
        {
            if (websocket != null)
            {
                DiscountRemovedFromDisplayOrderRequestMessage message = new DiscountRemovedFromDisplayOrderRequestMessage();
                DiscountRemovedFromDisplayOrder payload = new DiscountRemovedFromDisplayOrder();
                payload.DisplayDiscount = discount;
                payload.DisplayOrder = order;
                message.payload = payload;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void LineItemAddedToDisplayOrder(DisplayOrder order, DisplayLineItem lineItem)
        {
            if (websocket != null)
            {
                LineItemAddedToDisplayOrderRequestMessage message = new LineItemAddedToDisplayOrderRequestMessage();
                LineItemAddedToDisplayOrder payload = new LineItemAddedToDisplayOrder();
                payload.DisplayLineItem = lineItem;
                payload.DisplayOrder = order;
                message.payload = payload;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void LineItemRemovedFromDisplayOrder(DisplayOrder order, DisplayLineItem lineItem)
        {
            if (websocket != null)
            {
                LineItemRemovedFromDisplayOrderRequestMessage message = new LineItemRemovedFromDisplayOrderRequestMessage();
                LineItemRemovedFromDisplayOrder payload = new LineItemRemovedFromDisplayOrder();
                payload.DisplayLineItem = lineItem;
                payload.DisplayOrder = order;
                message.payload = payload;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void RemoveDisplayOrder(DisplayOrder displayOrder)
        {
            ShowWelcomeScreen();
        }

        public void DisplayPaymentReceiptOptions(String orderId, String paymentId)
        {
            if (websocket != null)
            {
                DisplayPaymentReceiptOptionsRequestMessage message = new DisplayPaymentReceiptOptionsRequestMessage();
                DisplayPaymentReceiptOptionsRequest req = new DisplayPaymentReceiptOptionsRequest();
                req.OrderID = orderId;
                req.PaymentID = paymentId;
                message.payload = req;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void Dispose()
        {
            websocket.Close();
        }

        public void GetMerchantInfo()
        {
            throw new NotImplementedException();
        }

        public void InvokeInputOption(InputOption io)
        {
            if (websocket != null)
            {
                InvokeInputOptionRequestMessage message = new InvokeInputOptionRequestMessage();
                message.payload = io;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void ManualRefund(ManualRefundRequest request)
        {
            if (websocket != null)
            {
                ManualRefundRequestMessage message = new ManualRefundRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void OpenCashDrawer(string reason)
        {
            if (websocket != null)
            {
                OpenCashDrawerRequestMessage message = new OpenCashDrawerRequestMessage();
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void PrintImage(Bitmap bitmap)
        {
            if (websocket != null)
            {
                PrintImageRequestMessage message = new PrintImageRequestMessage();
                PrintImage pi = new PrintImage();

                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                byte[] imgBytes = ms.ToArray();
                string base64Image = Convert.ToBase64String(imgBytes);

                pi.Bitmap = base64Image; // serialize image to string..
                message.payload = pi;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void PrintText(List<string> messages)
        {
            if (websocket != null)
            {
                PrintTextRequestMessage message = new PrintTextRequestMessage();
                PrintText pt = new PrintText();
                pt.Messages = messages;
                message.payload = pt;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void RefundPayment(RefundPaymentRequest request)
        {
            if (websocket != null)
            {
                RefundPaymentRequestMessage message = new RefundPaymentRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void Sale(SaleRequest request)
        {
            if (websocket != null)
            {
                SaleRequestMessage message = new SaleRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));

            }
        }

        public void ShowMessage(string message)
        {
            if (websocket != null)
            {
                ShowMessageRequestMessage msg = new ShowMessageRequestMessage();
                sdk.service.client.ShowMessage payload = new sdk.service.client.ShowMessage();
                payload.Message = message;
                msg.payload = payload;

                websocket.Send(JsonUtils.serialize(msg));
            }
        }

        public void ShowThankYouScreen()
        {
            if (websocket != null)
            {
                websocket.Send(JsonUtils.serialize(new ShowThankYouScreenRequestMessage()));
            }
        }

        public void ShowWelcomeScreen()
        {
            if (websocket != null)
            {
                websocket.Send(JsonUtils.serialize(new ShowWelcomeScreenRequestMessage()));
            }
        }


        public void RetrievePendingPayments()
        {
            if (websocket != null)
            {
                websocket.Send(JsonUtils.serialize(new RetrievePendingPaymentsRequestMessage()));
            }
        }

        public void TipAdjustAuth(TipAdjustAuthRequest request)
        {
            if (websocket != null)
            {
                TipAdjustAuthRequestMessage message = new TipAdjustAuthRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void VoidPayment(VoidPaymentRequest request)
        {
            if (websocket != null)
            {
                VoidPaymentRequestMessage message = new VoidPaymentRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));

            }
        }

        public void ResetDevice()
        {
            if (websocket != null)
            {
                //websocket.Send(JsonUtils.serialize(new BreakRequestMessage())); // deprecated
                websocket.Send(JsonUtils.serialize(new ResetDeviceMessage()));
            }
        }

        public void initializeConnection()
        {
            throw new NotImplementedException();
        }

        public void PrintImageFromURL(string ImgURL)
        {
            if (websocket != null)
            {
                PrintImageFromURLRequestMessage msg = new PrintImageFromURLRequestMessage();
                sdk.service.client.PrintImage payload = new sdk.service.client.PrintImage();
                payload.Url = ImgURL;
                msg.payload = payload;

                websocket.Send(JsonUtils.serialize(msg));
            }
        }

        public void AcceptPayment(Payment payment)
        {
            if (websocket != null)
            {
                AcceptPaymentRequestMessage msg = new AcceptPaymentRequestMessage();
                sdk.AcceptPayment ap = new sdk.AcceptPayment();
                ap.Payment = payment;
                msg.payload = ap;
                websocket.Send(JsonUtils.serialize(msg));

            }
        }

        public void RejectPayment(Payment payment, Challenge challenge)
        {
            if (websocket != null)
            {
                RejectPaymentRequestMessage msg = new RejectPaymentRequestMessage();
                sdk.RejectPayment rp = new sdk.RejectPayment();
                rp.Payment = payment;
                rp.Challenge = challenge;
                msg.payload = rp;
                websocket.Send(JsonUtils.serialize(msg));

            }
        }

        public void SendMessageToActivity(MessageToActivity mta)
        {
            if (websocket != null)
            {
                MessageToActivityMessage msg = new MessageToActivityMessage();
                msg.payload = mta;
                websocket.Send(JsonUtils.serialize(msg));
            }
        }

        public void RetrieveDeviceStatus(RetrieveDeviceStatusRequest rdsr)
        {
            if (websocket != null)
            {
                RetrieveDeviceStatusMessage msg = new RetrieveDeviceStatusMessage();
                msg.payload = rdsr;
                String ms = JsonUtils.serialize(msg);
                websocket.Send(ms);
            }
        }

        public void RetrievePayment(RetrievePaymentRequest rpr)
        {
            if (websocket != null)
            {
               RetrievePaymentRequestMessage msg = new RetrievePaymentRequestMessage();
                msg.payload = rpr;
                websocket.Send(JsonUtils.serialize(msg));
            }
        }

        public void OpenCashDrawer(OpenCashDrawerRequest request)
        {
            if(websocket != null)
            {
                OpenCashDrawerRequestMessage message = new OpenCashDrawerRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
            
        }

        public void Print(PrintRequest request)
        {
            if(websocket != null)
            {
                if (request.text.Count > 0)
                {
                    this.PrintText(request.text);
                }
                else if (request.images.Count > 0)
                {
                    this.PrintImage(request.images[0]);
                }
                else if (request.imageURLs.Count > 0)
                {
                    this.PrintImageFromURL(request.imageURLs[0]);
                }
                else
                {
                    Console.WriteLine("In Print: PrintRequest had no content or had an unhandled content type");
                }
            }
        }

        public void RetrievePrinters(RetrievePrintersRequest request)
        {
            if(websocket != null)
            {
                com.clover.sdk.remote.websocket.RetrievePrintersRequestMessage msg = new com.clover.sdk.remote.websocket.RetrievePrintersRequestMessage();
                msg.payload = request;
                websocket.Send(JsonUtils.serialize(msg));
            }
        }

        public void RetrievePrintJobStatus(PrintJobStatusRequest request)
        {
            if(websocket != null)
            {
                RetrievePrintJobStatusRequestMessage msg = new RetrievePrintJobStatusRequestMessage();
                msg.payload = request;
                websocket.Send(JsonUtils.serialize(msg));
            }
        }

        public class WebSocketSigVerRequestHandler : VerifySignatureRequest
        {
            VerifySignatureRequest svr;
            RemoteWebSocketCloverConnector WSCloverConnector;
            public WebSocketSigVerRequestHandler(RemoteWebSocketCloverConnector cloverConnector, VerifySignatureRequest request)
            {
                WSCloverConnector = cloverConnector;
                svr = request;
                Payment = request.Payment;
                Signature = request.Signature;
            }
            public override void Accept()
            {
                WSCloverConnector.AcceptSignature(svr);
            }

            public override void Reject()
            {
                WSCloverConnector.RejectSignature(svr);
            }
        }
    }
}
