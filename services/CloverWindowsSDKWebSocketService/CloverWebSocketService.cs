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

using com.clover.remote.order;
using com.clover.remotepay.sdk;
using com.clover.remotepay.sdk.service.client;
using com.clover.remotepay.transport;
using com.clover.sdk.remote.websocket;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CloverWindowsSDKWebSocketService
{
    public class CloverWebSocketService : ServiceBase
    {

        public static readonly string SERVICE_NAME = "Clover Mini Web Socket Service";

        WSCloverConnector cloverConnector = null;
        List<IWebSocketConnection> clientConnections = new List<IWebSocketConnection>();
        CloverWebSocketConnectorListener connectorListener = new CloverWebSocketConnectorListener();

        /*private const string STATUS = "Status";
        private const string SALE = "Sale";
        private const string AUTH = "Auth";
        private const string CANCEL = "Cancel";
        private const string CAPTURE_AUTH = "CaptureAuth";
        private const string TIP_ADJUST_AUTH = "TipAdjustAuth";
        private const string VOID_PAYMENT = "VoidPayment";
        private const string REFUND_PAYMENT = "RefundPayment";
        private const string MANUAL_REFUND = "ManualRefund";
        private const string CLOSEOUT = "Closeout";
        private const string DISPLAY_RECEIPT_OPTIONS = "DisplayReceiptOptions";
        private const string PRINT_TEXT = "PrintText";
        private const string PRINT_IMAGE = "PrintImage";
        private const string OPEN_CASH_DRAWER = "OpenCashDrawer";
        private const string SHOW_MESSAGE = "ShowMessage";
        private const string SHOW_WELCOME_SCREEN = "ShowWelcomeScreen";
        private const string SHOW_THANK_YOU_SCREEN = "ShowThankYouScreen";
        private const string DISPLAY_ORDER = "DisplayOrder";
        private const string DISPLAY_ORDER_LINE_ITEM_ADDED = "DisplayOrderLineItemAdded";
        private const string DISPLAY_ORDER_LINE_ITEM_REMOVED = "DisplayOrderLineItemRemoved";
        private const string DISPLAY_ORDER_DISCOUNT_ADDED = "DisplayOrderDiscountAdded";
        private const string DISPLAY_ORDER_DISCOUNT_REMOVED = "DisplayOrderDiscountRemoved";
        // these are extensions to the default CloverConnector
        private const string INVOKE_INPUT_OPTION = "InvokeInputOption";
        private const string ACCEPT_SIGNATURE = "AcceptSignature";
        private const string REJECT_SIGNATURE = "RejectSignature";*/

        WebSocketServer server = null;

        public CloverWebSocketService()
        {
            this.ServiceName = SERVICE_NAME;
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.AutoLog = true;
        }

        public void DebugStart(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            //load args in to dictionary
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if ("/T".Equals(args[i]))
                    {
                        parameters.Add("/T", "true");
                    }
                    else if(i+1 < args.Length)
                    {
                        parameters.Add(args[i], args[++i]);
                    }

                }
            }

            IWebSocketConnection sendSocket = null;
            Action<IWebSocketConnection> serverSocket = socket =>
            {
                socket.OnOpen = () => {
                    Console.WriteLine("Open! " + clientConnections.Count);
                    if(clientConnections.Count > 0)
                    {
                        if (clientConnections[0].IsAvailable)
                        {
                            socket.Close();
                            connectorListener.OnError(new Exception("Another client is already connected"));
                            return;
                        }
                    }
                    sendSocket = socket;
                    clientConnections.Add(socket);

                    connectorListener.WebSocket = sendSocket;
                    connectorListener.SendConnectionStatus();
                    // send the connection status when connected...
                    //JObject retObj = new JObject();
                    //retObj.Add("Status", connectorListener.CurrentConnectionStatus);
                    //socket.Send(retObj.ToString());
                };
                socket.OnClose = () =>
                {
                    clientConnections.Remove(socket);
                    Console.WriteLine("Close!");
                    connectorListener.WebSocket = null;
                    //cloverConnector -= connectorListener;
                    //cloverConnector.Dispose();
                    //cloverConnector = null;
                };
                socket.OnMessage = message =>
                {

                    try
                    {
                        JObject jsonObj = (JObject)JsonConvert.DeserializeObject(message);
                        JToken method = jsonObj.GetValue(ServicePayloadConstants.PROP_METHOD);
                        JObject payload = (JObject)jsonObj.GetValue(ServicePayloadConstants.PROP_PAYLOAD);
                        WebSocketMethod wsm = (WebSocketMethod)Enum.Parse(typeof(WebSocketMethod), method.ToString());

                        switch (wsm)
                        {
                            case WebSocketMethod.Status:
                                {
                                    connectorListener.SendConnectionStatus();
                                    break;
                                }
                            case WebSocketMethod.OpenCashDrawer:
                                {
                                    string reason = ((JObject)payload).GetValue("Reason").Value<string>();
                                    cloverConnector.OpenCashDrawer(reason);
                                    break;
                                }
                            case WebSocketMethod.ShowMessage:
                                {
                                    string msg = ((JObject)payload).GetValue("Message").Value<string>();
                                    cloverConnector.ShowMessage(msg);
                                    break;
                                }
                            case WebSocketMethod.ShowThankYouScreen:
                                {
                                    cloverConnector.ShowThankYouScreen();
                                    break;
                                }
                            case WebSocketMethod.ShowWelcomeScreen:
                                {
                                    cloverConnector.ShowWelcomeScreen();
                                    break;
                                }
                            case WebSocketMethod.Cancel:
                                {
                                    cloverConnector.Cancel();
                                    break;
                                }
                            case WebSocketMethod.PrintText:
                                {
                                    JArray messages = (JArray)payload.GetValue("Messages");
                                    List<string> messageList = new List<string>();
                                    foreach (string msg in messages)
                                    {
                                        messageList.Add(msg);
                                    }

                                    cloverConnector.PrintText(messageList);
                                    break;
                                }
                            case WebSocketMethod.PrintImage :
                                {
                                    string base64Img = ((JObject)payload).GetValue("Bitmap").Value<string>();
                                    byte[] imgBytes = Convert.FromBase64String(base64Img);

                                    MemoryStream ms = new MemoryStream();
                                    ms.Write(imgBytes, 0, imgBytes.Length);
                                    Bitmap bp = new Bitmap(ms);
                                    ms.Close();

                                    cloverConnector.PrintImage(bp);
                                    break;
                                }
                            case WebSocketMethod.Auth:
                                {
                                    //JObject jobj = (JObject)Request.GetValue("SaleRequest");
                                    //SaleRequest saleRequest = JsonUtils.deserialize<SaleRequest>(jobj.ToString());
                                    AuthRequest authRequest = JsonUtils.deserialize<AuthRequest>(payload.ToString());
                                    cloverConnector.Auth(authRequest);
                                    break;
                                }
                            case WebSocketMethod.TipAdjustAuth:
                                {
                                    TipAdjustAuthRequest tipAdjustRequest = JsonUtils.deserialize<TipAdjustAuthRequest>(payload.ToString());
                                    cloverConnector.TipAdjustAuth(tipAdjustRequest);
                                    break;
                                }
                            case WebSocketMethod.Sale:
                                {
                                    //JObject jobj = (JObject)Request.GetValue("SaleRequest");
                                    //SaleRequest saleRequest = JsonUtils.deserialize<SaleRequest>(jobj.ToString());
                                    SaleRequest saleRequest = JsonUtils.deserialize<SaleRequest>(payload.ToString());
                                    cloverConnector.Sale(saleRequest);
                                    break;
                                }
                            case WebSocketMethod.InvokeInputOption:
                                {
                                    //JObject jobj = (JObject)Request.GetValue("InputOption");
                                    //InputOption io = JsonUtils.deserialize<InputOption>(jobj.ToString());
                                    InputOption io = JsonUtils.deserialize<InputOption>(payload.ToString());
                                    cloverConnector.ExecuteInputOption(io);
                                    break;
                                }
                            case WebSocketMethod.VoidPayment:
                                {
                                    //JObject obj = (JObject)Request.GetValue("VoidPaymentRequest");
                                    //VoidPaymentRequest request = JsonUtils.deserialize<VoidPaymentRequest>(obj.ToString());
                                    VoidPaymentRequest request = JsonUtils.deserialize<VoidPaymentRequest>(payload.ToString());
                                    cloverConnector.VoidPayment(request);
                                    break;
                                }
                            case WebSocketMethod.ManualRefund:
                                {
                                    //JObject obj = (JObject)Request.GetValue("ManualRefundRequest");
                                    //ManualRefundRequest mrr = JsonUtils.deserialize<ManualRefundRequest>(obj.ToString());
                                    ManualRefundRequest mrr = JsonUtils.deserialize<ManualRefundRequest>(payload.ToString());
                                    cloverConnector.ManualRefund(mrr);
                                    break;
                                }
                            case WebSocketMethod.RefundPayment :
                                {
                                    //JObject obj = (JObject)Request.GetValue("RefundPaymentRequest");
                                    //RefundPaymentRequest request = JsonUtils.deserialize<RefundPaymentRequest>(obj.ToString());
                                    RefundPaymentRequest request = JsonUtils.deserialize<RefundPaymentRequest>(payload.ToString());
                                    cloverConnector.RefundPayment(request);
                                    break;
                                }
                            case WebSocketMethod.DisplayPaymentReceiptOptions:
                                {
                                    DisplayPaymentReceiptOptionsRequest request = JsonUtils.deserialize<DisplayPaymentReceiptOptionsRequest>(payload.ToString());
                                    cloverConnector.DisplayPaymentReceiptOptions(request.OrderID, request.PaymentID);
                                    break;
                                }
                            case WebSocketMethod.DisplayRefundReceiptOptions:
                                {
                                    DisplayRefundReceiptOptionsRequest request = JsonUtils.deserialize<DisplayRefundReceiptOptionsRequest>(payload.ToString());
                                    cloverConnector.DisplayRefundReceiptOptions(request.OrderID, request.RefundID);
                                    break;
                                }
                            case WebSocketMethod.DisplayCreditReceiptOptions:
                                {
                                    DisplayCreditReceiptOptionsRequest request = JsonUtils.deserialize<DisplayCreditReceiptOptionsRequest>(payload.ToString());
                                    cloverConnector.DisplayCreditReceiptOptions(request.OrderID, request.CreditID);
                                    break;
                                }
                            case WebSocketMethod.DisplayOrder:
                                {
                                    //JObject obj = (JObject)Request.GetValue("DisplayOrder");
                                    //com.clover.remote.order.DisplayOrder displayOrder = JsonUtils.deserialize<com.clover.remote.order.DisplayOrder>(obj.ToString());
                                    com.clover.remote.order.DisplayOrder displayOrder = JsonUtils.deserialize<com.clover.remote.order.DisplayOrder>(payload.ToString());
                                    cloverConnector.DisplayOrder(displayOrder);
                                    break;
                                }
                            case WebSocketMethod.DisplayOrderLineItemAdded:
                                {
                                    JObject obj = (JObject)payload.GetValue("DisplayOrder");
                                    com.clover.remote.order.DisplayOrder displayOrder = JsonUtils.deserialize<com.clover.remote.order.DisplayOrder>(obj.ToString());
                                    obj = (JObject)payload.GetValue("DisplayLineItem");
                                    com.clover.remote.order.DisplayLineItem displayOrderLineItem = JsonUtils.deserialize<com.clover.remote.order.DisplayLineItem>(obj.ToString());
                                    cloverConnector.DisplayOrderLineItemAdded(displayOrder, displayOrderLineItem);
                                    break;
                                }
                            case WebSocketMethod.DisplayOrderLineItemRemoved:
                                {
                                    JObject obj = (JObject)payload.GetValue("DisplayOrder");
                                    com.clover.remote.order.DisplayOrder displayOrder = JsonUtils.deserialize<com.clover.remote.order.DisplayOrder>(obj.ToString());
                                    obj = (JObject)payload.GetValue("DisplayLineItem");
                                    com.clover.remote.order.DisplayLineItem displayOrderLineItem = JsonUtils.deserialize<com.clover.remote.order.DisplayLineItem>(obj.ToString());
                                    cloverConnector.DisplayOrderLineItemRemoved(displayOrder, displayOrderLineItem);
                                    break;
                                }
                            case WebSocketMethod.DisplayOrderDiscountAdded:
                                {
                                    JObject obj = (JObject)payload.GetValue("DisplayOrder");
                                    com.clover.remote.order.DisplayOrder displayOrder = JsonUtils.deserialize<com.clover.remote.order.DisplayOrder>(obj.ToString());
                                    obj = (JObject)payload.GetValue("DisplayDiscount");
                                    com.clover.remote.order.DisplayDiscount displayOrderLineItem = JsonUtils.deserialize<com.clover.remote.order.DisplayDiscount>(obj.ToString());
                                    cloverConnector.DisplayOrderDiscountAdded(displayOrder, displayOrderLineItem);
                                    break;
                                }
                            case WebSocketMethod.DisplayOrderDiscountRemoved:
                                {
                                    JObject obj = (JObject)payload.GetValue("DisplayOrder");
                                    com.clover.remote.order.DisplayOrder displayOrder = JsonUtils.deserialize<com.clover.remote.order.DisplayOrder>(obj.ToString());
                                    obj = (JObject)payload.GetValue("DisplayDiscount");
                                    com.clover.remote.order.DisplayDiscount displayOrderLineItem = JsonUtils.deserialize<com.clover.remote.order.DisplayDiscount>(obj.ToString());
                                    cloverConnector.DisplayOrderDiscountRemoved(displayOrder, displayOrderLineItem);
                                    break;
                                }
                            case WebSocketMethod.AcceptSignature:
                                {
                                    //JObject obj = (JObject)Request.GetValue("SignatureVerifyRequest");
                                    //WSSignatureVerifyRequest svr = JsonUtils.deserialize<WSSignatureVerifyRequest>(obj.ToString());
                                    WSSignatureVerifyRequest svr = JsonUtils.deserialize<WSSignatureVerifyRequest>(payload.ToString());
                                    cloverConnector.AcceptSignature(svr);
                                    break;
                                }
                            case WebSocketMethod.RejectSignature:
                                {
                                    //JObject obj = (JObject)Request.GetValue("SignatureVerifyRequest");
                                    //WSSignatureVerifyRequest svr = JsonUtils.deserialize<WSSignatureVerifyRequest>(obj.ToString());
                                    WSSignatureVerifyRequest svr = JsonUtils.deserialize<WSSignatureVerifyRequest>(payload.ToString());
                                    cloverConnector.RejectSignature(svr);
                                    break;
                                }
                            case WebSocketMethod.VaultCard:
                                {
                                    VaultCardMessage vcm = JsonUtils.deserialize<VaultCardMessage>(payload.ToString());
                                    cloverConnector.VaultCard(vcm.cardEntryMethods);
                                    break;
                                }
                            case WebSocketMethod.Closeout:
                                {
                                    CloseoutRequest cr = new CloseoutRequest();
                                    cloverConnector.Closeout(cr);
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("received unknown websocket method: " + method.ToString() + " in CloverWebSocketService.");
                                    break;
                                }
                        }

                        //Console.WriteLine("received message: " + msg.GetType());
                        //                        Console.WriteLine("received message: " + message);
                        //                        socket.Send("Echo: " + message);
                    }
                    catch (InvalidOperationException ioe)
                    {
                        Console.WriteLine(ioe.Message);
                        socket.Send("Error Deserializing");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        socket.Send("Error Parsing: " + message);
                    }
                    


                    //cloverConnector.
                };
            };

            InitializeConnector(parameters);
            server.Start(serverSocket);
        }

        private void InitializeConnector(Dictionary<string, string> parameters)
        {


            string protocol;
            string port;

            bool testConfig = false;
            string lanConfig = null;
            string testConfigString;

            if (!parameters.TryGetValue("/P", out port))
            {
                port = "8889";// default
            }
            string certPath = null;
            if (!parameters.TryGetValue("/C", out certPath))
            {
                protocol = "ws";// default
            }
            else
            {
                protocol = "wss";
            }
            if (!parameters.TryGetValue("/T", out testConfigString))
            {
                testConfig = false;
            }
            else
            {
                testConfig = true; //
            }
            parameters.TryGetValue("/L", out lanConfig);


            server = new WebSocketServer(protocol + "://127.0.0.1:" + port);
            if (certPath != null)
            {
                server.Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath);
            }
            CloverDeviceConfiguration config = null;
            if (testConfig)
            {
                config = new TestCloverDeviceConfiguration();
            }
            else if (lanConfig != null)
            {
                int loc = lanConfig.IndexOf(':');
                if (loc == -1)
                {
                    throw new InvalidDataException("invalid lan host. arguments must be '/L <hostname>:<port>'");
                }
                try
                {
                    string lanHostname = lanConfig.Substring(0, loc);
                    string lanPortStr = lanConfig.Substring(loc + 1);
                    int lanPort = int.Parse(lanPortStr);
                    if (lanPort < 0 || lanPort > 65535)
                    {
                        throw new InvalidDataException("Invalid port. must be between 1 and 65535");
                    }
                    config = new WebSocketCloverDeviceConfiguration(lanHostname, lanPort);
                }
                catch (FormatException fe)
                {
                    throw new InvalidDataException("invalid port: " + lanConfig.Substring(loc + 1));
                }
            }
            else
            {
                config = new USBCloverDeviceConfiguration(null);
            }

            cloverConnector = new WSCloverConnector(config, connectorListener);
        }

        protected override void OnStop()
        {
            base.OnStop();
            server.ListenerSocket.Close();
        }

        private T Deserialize<T>(String msg) 
        {
            T obj = JsonUtils.deserialize<T> (msg);
            //XmlSerializer serializer = new XmlSerializer(typeof(T));
            //T obj = (T)serializer.Deserialize(new StringReader(msg));
            return obj;
        }
    }



    /// <summary>
    /// Becuase SignatureVerifyRequest is abstract, need a concreate class to instantiate
    /// </summary>
    class WSSignatureVerifyRequest : SignatureVerifyRequest
    {
        public override void Accept() { }
        public override void Reject() { }
    }

    class WSCloverConnector : CloverConnector
    {
        public static WSCloverConnector operator +(WSCloverConnector connector, CloverConnectorListener connectorListener)
        {
            connector.AddCloverConnectorListener(connectorListener);
            return connector;
        }
        public static WSCloverConnector operator -(WSCloverConnector connector, CloverConnectorListener connectorListener)
        {
            //connector.RemoveCloverConnectorListener(connectorListener);
            return connector;
        }

        public WSCloverConnector(CloverDeviceConfiguration config, CloverConnectorListener listener) : base(config, listener)
        {
        }

        public void ExecuteInputOption(InputOption inputOption)
        {
            if (Device != null)
            {
                Device.doKeyPress(inputOption.keyPress);
            }
        }

        public void AcceptSignature(SignatureVerifyRequest request)
        {
            if (Device != null)
            {
                Device.doSignatureVerified(request.Payment, true);
            }
        }

        public void RejectSignature(SignatureVerifyRequest request)
        {
            if (Device != null)
            {
                Device.doSignatureVerified(request.Payment, false);
            }
        }
    }
}
