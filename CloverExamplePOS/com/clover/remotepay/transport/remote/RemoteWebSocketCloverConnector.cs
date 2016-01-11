using com.clover.remotepay.sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.clover.remote.order;
using System.Drawing;
using WebSocket4Net;
using SuperSocket.ClientEngine;
using com.clover.remotepay.transport.remote;
using com.clover.sdk.remote.websocket;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using com.clover.remotepay.sdk.service.client;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using System.ComponentModel;

namespace com.clover.remotepay.transport.remote
{
    public class RemoteWebSocketCloverConnector : ICloverConnector
    {
        public long CardEntryMethod { get; set; }
        public bool DisableCashBack { get; set; }
        public bool DisablePrinting { get; set; }
        public bool DisableRestartTransactionOnFail { get; set; }
        public bool DisableTip { get; set; }

        List<CloverConnectorListener> listeners = new List<CloverConnectorListener>();
        private CloverDeviceConfiguration config;

        WebSocket websocket;
        string hostname = "localhost";
        int port = 8889;

        //CloverConnector cloverConnector { get; set; }
        //RemoteWebSocketCloverConnectorListener cloverConnectorListener { get; set; }

        public RemoteWebSocketCloverConnector()
        {
            //cloverConnectorListener = new RemoteWebSocketCloverConnectorListener();
            //cloverConnector = new CloverConnector(new USBCloverDeviceConfiguration(null), cloverConnectorListener);
            init();
        }

        public RemoteWebSocketCloverConnector(CloverDeviceConfiguration config)
        {
            this.config = config;
            port = ((RemoteWebSocketCloverConfiguration)config).port;
            hostname = "localhost";// force this for now..
            //cloverConnectorListener = new RemoteWebSocketCloverConnectorListener();
            init();
        }

        private void init()
        {
            CardEntryMethod = CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT | CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE | CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS;
            websocket = new WebSocket("ws://" + hostname + ":" + port + "/");
            websocket.Opened += new EventHandler(websocket_Opened);
            websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
            websocket.Closed += new EventHandler(websocket_Closed);
            websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
            websocket.Open();
        }
        private void websocket_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket connection open");
            websocket.Send(JsonUtils.serialize(new StatusRequestMessage()));
        }

        private void websocket_Error(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket error");
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("WebSocket connection closed");
            //websocket.Dispose();
#if DEBUG
            System.GC.Collect();
#endif
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
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message + " => " + e.Message);
                listeners.ForEach(listeners => listeners.OnError(exc));
                return;
            }
            
            JToken method = jsonObj.GetValue(ServicePayloadConstants.PROP_METHOD);
            if(method == null)
            {
                listeners.ForEach(listeners => listeners.OnError(new NullReferenceException("Invalid message: " + e.Message)));
                return;
            }
            JObject payload = (JObject)jsonObj.GetValue(ServicePayloadConstants.PROP_PAYLOAD);
            WebSocketMethod wsm = (WebSocketMethod)Enum.Parse(typeof(WebSocketMethod), method.ToString());

            switch(wsm)
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
                        listeners.ForEach(listener => listener.OnDeviceReady());
                        break;
                    }
                case WebSocketMethod.SignatureVerifyRequest:
                    {
                        SignatureVerifyRequest svr = JsonUtils.deserialize<SignatureVerifyRequest>(payload.ToString());
                        WebSocketSigVerRequestHandler handler = new WebSocketSigVerRequestHandler(this, svr);
                        listeners.ForEach(listener => listener.OnSignatureVerifyRequest(handler));
                        break;
                    }
                case WebSocketMethod.SaleResponse:
                    {
                        SaleResponse sr = JsonUtils.deserialize<SaleResponse>(payload.ToString());
                        listeners.ForEach(listener => listener.OnSaleResponse(sr));
                        break;
                    }
                case WebSocketMethod.AuthResponse:
                    {
                        AuthResponse sr = JsonUtils.deserialize<AuthResponse>(payload.ToString());
                        listeners.ForEach(listener => listener.OnAuthResponse(sr));
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
                        listeners.ForEach(listener => listener.OnAuthTipAdjustResponse(taar));
                        break;
                    }
            }
        }


        public void AddCloverConnectorListener(CloverConnectorListener connectorListener)
        {
            listeners.Add(connectorListener);
        }

        public void RemoveCloverConnectorListener(CloverConnectorListener connectorListener)
        {
            listeners.Remove(connectorListener);
        }

        public int AcceptSignature(SignatureVerifyRequest request)
        {
            if(websocket != null)
            {
                AcceptSignatureRequestMessage message = new AcceptSignatureRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
                return 0;
            }
            return -1;
        }

        public int RejectSignature(SignatureVerifyRequest request)
        {
            if (websocket != null)
            {
                RejectSignatureRequestMessage message = new RejectSignatureRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
                return 0;
            }
            return -1;
        }

        public int Auth(AuthRequest request)
        {
            if(websocket != null)
            {
                AuthRequestMessage message = new AuthRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
                return 0;
            }
            return -1;
        }

        public int Cancel()
        {
            if(websocket != null)
            {
                websocket.Send(JsonUtils.serialize(new CancelRequestMessage()));
                return 0;
            }
            return -1;
        }

        public int CaptureAuth(CaptureAuthRequest request)
        {
            if(websocket != null)
            {
                CaptureAuthRequestMessage message = new CaptureAuthRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
                return 0;
            }
            return -1;
        }

        public int Closeout(CloseoutRequest request)
        {
            if (websocket != null)
            {
                CloseoutRequestMessage message = new CloseoutRequestMessage();
                websocket.Send(JsonUtils.serialize(message));
                return 0;
            }
            return -1;
        }

        public void DisplayOrder(DisplayOrder order)
        {
            if (websocket != null)
            {
                DisplayOrderRequestMessage message = new DisplayOrderRequestMessage();
                message.payload = order;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void DisplayOrderDelete(DisplayOrder order)
        {
            /*
            if (websocket != null)
            {
                DisplayOrderDeleteRequestMessage message = new DisplayOrderDeleteRequestMessage();
                message.payload = order;
                websocket.Send(JsonUtils.serialize(message));
                return 0;
            }
            return -1;
            */
        }

        public void DisplayOrderDiscountAdded(DisplayOrder order, DisplayDiscount discount)
        {
            if (websocket != null)
            {
                DisplayOrderDiscountAddedRequestMessage message = new DisplayOrderDiscountAddedRequestMessage();
                DisplayOrderDiscountAdded payload = new DisplayOrderDiscountAdded();
                payload.DisplayDiscount = discount;
                payload.DisplayOrder = order;
                message.payload = payload;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void DisplayOrderDiscountRemoved(DisplayOrder order, DisplayDiscount discount)
        {
            if (websocket != null)
            {
                DisplayOrderDiscountRemovedRequestMessage message = new DisplayOrderDiscountRemovedRequestMessage();
                DisplayOrderDiscountRemoved payload = new DisplayOrderDiscountRemoved();
                payload.DisplayDiscount = discount;
                payload.DisplayOrder = order;
                message.payload = payload;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void DisplayOrderLineItemAdded(DisplayOrder order, DisplayLineItem lineItem)
        {
            if(websocket != null)
            {
                DisplayOrderLineItemAddedRequestMessage message = new DisplayOrderLineItemAddedRequestMessage();
                DisplayOrderLineItemAdded payload = new DisplayOrderLineItemAdded();
                payload.DisplayLineItem = lineItem;
                payload.DisplayOrder = order;
                message.payload = payload;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void DisplayOrderLineItemRemoved(DisplayOrder order, DisplayLineItem lineItem)
        {
            if (websocket != null)
            {
                DisplayOrderLineItemRemovedRequestMessage message = new DisplayOrderLineItemRemovedRequestMessage();
                DisplayOrderLineItemRemoved payload = new DisplayOrderLineItemRemoved();
                payload.DisplayLineItem = lineItem;
                payload.DisplayOrder = order;
                message.payload = payload;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public void DisplayReceiptOptions()
        {
            if (websocket != null)
            {
                DisplayReceiptOptionsRequestMessage message = new DisplayReceiptOptionsRequestMessage();
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
            if(websocket != null)
            {
                InvokeInputOptionRequestMessage message = new InvokeInputOptionRequestMessage();
                message.payload = io;
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public int ManualRefund(ManualRefundRequest request)
        {
            if (websocket != null)
            {
                ManualRefundRequestMessage message = new ManualRefundRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
                return 0;
            }
            return -1;
        }

        public void OpenCashDrawer(string reason)
        {
            if (websocket != null)
            {
                OpenCashDrawerRequestMessage message = new OpenCashDrawerRequestMessage();
                websocket.Send(JsonUtils.serialize(message));
            }
        }

        public int PrintImage(Bitmap bitmap)
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
                return 0;
            }
            return -1;
        }

        public int PrintText(List<string> messages)
        {
            if (websocket != null)
            {
                PrintTextRequestMessage message = new PrintTextRequestMessage();
                PrintText pt = new PrintText();
                pt.Messages = messages;
                message.payload = pt;
                websocket.Send(JsonUtils.serialize(message));
                return 0;
            }
            return -1;
        }

        public int RefundPayment(RefundPaymentRequest request)
        {
            if (websocket != null)
            {
                RefundPaymentRequestMessage message = new RefundPaymentRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
                return 0;
            }
            return -1;
        }

        public int Sale(SaleRequest request)
        {
            if(websocket != null)
            {
                SaleRequestMessage message = new SaleRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));

            }
            return 0;
        }

        public int ShowMessage(string message)
        {
            if (websocket != null)
            {
                ShowMessageRequestMessage msg = new ShowMessageRequestMessage();
                sdk.service.client.ShowMessage payload = new sdk.service.client.ShowMessage();
                payload.Message = message;
                msg.payload = payload;

                websocket.Send(JsonUtils.serialize(msg));
            }
            return 0;
        }

        public void ShowThankYouScreen()
        {
            if (websocket != null)
            {
                websocket.Send(JsonUtils.serialize(new ShowThankYouScreenRequestMessage()));
            }
        }

        public int ShowWelcomeScreen()
        {
            if(websocket != null)
            {
                websocket.Send(JsonUtils.serialize(new ShowWelcomeScreenRequestMessage()));
            }
            return 0;
        }

        public int TipAdjustAuth(TipAdjustAuthRequest request)
        {
            if (websocket != null)
            {
                TipAdjustAuthRequestMessage message = new TipAdjustAuthRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));
            }
            return 0;
        }

        public int VoidPayment(VoidPaymentRequest request)
        {
            if (websocket != null)
            {
                VoidPaymentRequestMessage message = new VoidPaymentRequestMessage();
                message.payload = request;
                websocket.Send(JsonUtils.serialize(message));

            }
            return 0;
        }

        public int VoidTransaction(VoidTransactionRequest request)
        {
            throw new NotImplementedException();
        }
    }

    public class WebSocketSigVerRequestHandler : SignatureVerifyRequest
    {
        SignatureVerifyRequest svr;
        RemoteWebSocketCloverConnector WSCloverConnector;
        public WebSocketSigVerRequestHandler(RemoteWebSocketCloverConnector cloverConnector, SignatureVerifyRequest request)
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
