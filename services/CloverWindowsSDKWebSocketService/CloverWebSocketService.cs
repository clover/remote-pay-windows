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


using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using com.clover.sdk.remote.websocket;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Linq;
using System.Diagnostics;
using com.clover.sdk.v3.payments;
using com.clover.remotepay.sdk.service.client;

namespace CloverWindowsSDKWebSocketService
{
    public class CloverWebSocketService : ServiceBase
    {

        public static readonly string SERVICE_NAME = "Clover Connector WebSocket Service";

        ICloverConnector cloverConnector = null;
        List<IWebSocketConnection> clientConnections = new List<IWebSocketConnection>();
        CloverWebSocketConnectorListener connectorListener = new CloverWebSocketConnectorListener();
        private bool Debug = false;
        private int Timer = 3;

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
            string logSource = "_TransportEventLog";
            if (!EventLog.SourceExists(logSource))
                EventLog.CreateEventSource(logSource, logSource);

            EventLogTraceListener myTraceListener = new EventLogTraceListener(logSource);

            // Add the event log trace listener to the collection.
            Trace.Listeners.Add(myTraceListener);

            if (args.Length == 0)
            {
                // Retrieve the arguments from the service ImagePath
                args = Environment.GetCommandLineArgs();
            }

            if (args.Length > 0)
            {
                if (((ICollection<string>)args).Contains("-debug"))
                {
                    Debug = true;
                }

                if (((ICollection<string>)args).Any(a => a.Contains("-timer")))
                {
                    IEnumerable<string> timerStrings = ((ICollection<string>)args).Where(a => a.Contains("-timer"));
                    if (timerStrings.Count() == 1)
                    {
                        try
                        {
                            string timerString = timerStrings.First();
                            int index = timerString.IndexOf('=');
                            string timerSeconds = timerString.Substring(index + 1);
                            Timer = Convert.ToInt32(timerSeconds);
                        }
                        catch
                        {
                            Timer = 1;
                            EventLog.WriteEntry(SERVICE_NAME, "Error parsing the -timer command line argument.  Setting timer to 1 second.");
                        }
                    }
                }
            }

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
                    else if (i + 1 < args.Length)
                    {
                        parameters.Add(args[i], args[++i]);
                    }

                }
            }

            IWebSocketConnection sendSocket = null;
            Action<IWebSocketConnection> serverSocket = socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open! " + clientConnections.Count);
                    if (clientConnections.Count > 0)
                    {
                        if (clientConnections[0].IsAvailable)
                        {
                            socket.Close();
                            connectorListener.OnDeviceError(new CloverDeviceErrorEvent(CloverDeviceErrorEvent.CloverDeviceErrorType.EXCEPTION, 0, null, "Another client is already connected"));
                            return;
                        }
                    }
                    sendSocket = socket;
                    clientConnections.Add(socket);

                    connectorListener.WebSocket = sendSocket;
                    connectorListener.SendConnectionStatus();
                };
                socket.OnClose = () =>
                {
                    clientConnections.Remove(socket);
                    Console.WriteLine("Close!");
                    connectorListener.WebSocket = null;
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
                                    OpenCashDrawerRequest request = JsonUtils.deserialize<OpenCashDrawerRequest>(payload.ToString());
                                    cloverConnector.OpenCashDrawer(request);
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
                            case WebSocketMethod.Break: // deprecated. use ResetDevice
                            case WebSocketMethod.ResetDevice:
                                {
                                    cloverConnector.ResetDevice();
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
                                    cloverConnector.Print(new PrintRequest() { text = messageList });
                                    break;
                                }
                            case WebSocketMethod.PrintImage:
                                {
                                    string base64Img = ((JObject)payload).GetValue("Bitmap").Value<string>();
                                    byte[] imgBytes = Convert.FromBase64String(base64Img);
                                    MemoryStream ms = new MemoryStream();
                                    ms.Write(imgBytes, 0, imgBytes.Length);
                                    Bitmap bp = new Bitmap(ms);
                                    ms.Close();
                                    cloverConnector.Print(new PrintRequest() { images = new List<Bitmap> { bp } });
                                    break;
                                }
                            case WebSocketMethod.PrintImageFromURL:
                                {
                                    string url = ((JObject)payload).GetValue("Url").Value<string>();
                                    cloverConnector.Print(new PrintRequest() { imageURLs = new List<string> { url } });
                                    break;
                                }
                            case WebSocketMethod.Auth:
                                {
                                    AuthRequest authRequest = JsonUtils.deserialize<AuthRequest>(payload.ToString());
                                    cloverConnector.Auth(authRequest);
                                    break;
                                }
                            case WebSocketMethod.PreAuth:
                                {
                                    PreAuthRequest preAuthRequest = JsonUtils.deserialize<PreAuthRequest>(payload.ToString());
                                    cloverConnector.PreAuth(preAuthRequest);
                                    break;
                                }
                            case WebSocketMethod.TipAdjustAuth:
                                {
                                    TipAdjustAuthRequest tipAdjustRequest = JsonUtils.deserialize<TipAdjustAuthRequest>(payload.ToString());
                                    cloverConnector.TipAdjustAuth(tipAdjustRequest);
                                    break;
                                }
                            case WebSocketMethod.CapturePreAuth:
                                {
                                    CapturePreAuthRequest capturePreAuthRequest = JsonUtils.deserialize<CapturePreAuthRequest>(payload.ToString());
                                    cloverConnector.CapturePreAuth(capturePreAuthRequest);
                                    break;
                                }
                            case WebSocketMethod.Sale:
                                {
                                    SaleRequest saleRequest = JsonUtils.deserialize<SaleRequest>(payload.ToString());
                                    cloverConnector.Sale(saleRequest);
                                    break;
                                }
                            case WebSocketMethod.InvokeInputOption:
                                {
                                    InputOption io = JsonUtils.deserialize<InputOption>(payload.ToString());
                                    cloverConnector.InvokeInputOption(io);
                                    break;
                                }
                            case WebSocketMethod.VoidPayment:
                                {
                                    VoidPaymentRequest request = JsonUtils.deserialize<VoidPaymentRequest>(payload.ToString());
                                    cloverConnector.VoidPayment(request);
                                    break;
                                }
                            case WebSocketMethod.ManualRefund:
                                {
                                    ManualRefundRequest mrr = JsonUtils.deserialize<ManualRefundRequest>(payload.ToString());
                                    cloverConnector.ManualRefund(mrr);
                                    break;
                                }
                            case WebSocketMethod.RefundPayment:
                                {
                                    RefundPaymentRequest request = JsonUtils.deserialize<RefundPaymentRequest>(payload.ToString());
                                    cloverConnector.RefundPayment(request);
                                    break;
                                }
                            case WebSocketMethod.DisplayPaymentReceiptOptions:
                                {
                                    DisplayPaymentReceiptOptionsRequest request = JsonUtils.deserialize<DisplayPaymentReceiptOptionsRequest>(payload.ToString());
                                    cloverConnector.DisplayPaymentReceiptOptions(request);
                                    break;
                                }
                            case WebSocketMethod.ShowDisplayOrder:
                                {
                                    com.clover.remote.order.DisplayOrder displayOrder = JsonUtils.deserialize<com.clover.remote.order.DisplayOrder>(payload.ToString());
                                    cloverConnector.ShowDisplayOrder(displayOrder);
                                    break;
                                }
                            case WebSocketMethod.AcceptSignature:
                                {
                                    WSVerifySignatureRequest svr = JsonUtils.deserialize<WSVerifySignatureRequest>(payload.ToString());
                                    cloverConnector.AcceptSignature(svr);
                                    break;
                                }
                            case WebSocketMethod.RejectSignature:
                                {
                                    WSVerifySignatureRequest svr = JsonUtils.deserialize<WSVerifySignatureRequest>(payload.ToString());
                                    cloverConnector.RejectSignature(svr);
                                    break;
                                }
                            case WebSocketMethod.ConfirmPayment:
                                {
                                    AcceptPayment acceptPayment = JsonUtils.deserialize<AcceptPayment>(payload.ToString());
                                    cloverConnector.AcceptPayment(acceptPayment.Payment);
                                    break;
                                }
                            case WebSocketMethod.RejectPayment:
                                {
                                    RejectPayment rp = JsonUtils.deserialize<RejectPayment>(payload.ToString());
                                    cloverConnector.RejectPayment(rp.Payment, rp.Challenge);
                                    break;
                                }
                            case WebSocketMethod.VaultCard:
                                {
                                    VaultCardMessage vcm = JsonUtils.deserialize<VaultCardMessage>(payload.ToString());
                                    cloverConnector.VaultCard(vcm.cardEntryMethods);
                                    break;
                                }
                            case WebSocketMethod.ReadCardData:
                                {
                                    ReadCardDataRequest request = JsonUtils.deserialize<ReadCardDataRequest>(payload.ToString());
                                    cloverConnector.ReadCardData(request);
                                    break;
                                }
                            case WebSocketMethod.Closeout:
                                {
                                    CloseoutRequest cr = new CloseoutRequest();
                                    cloverConnector.Closeout(cr);
                                    break;
                                }
                            case WebSocketMethod.RetrievePendingPayments:
                                {
                                    cloverConnector.RetrievePendingPayments();
                                    break;
                                }
                            case WebSocketMethod.StartCustomActivity:
                                {
                                    CustomActivityRequest request = JsonUtils.deserialize<CustomActivityRequest>(payload.ToString());
                                    cloverConnector.StartCustomActivity(request);
                                    break;
                                }
                            case WebSocketMethod.RetrieveDeviceStatus:
                                {
                                    RetrieveDeviceStatusRequest request = JsonUtils.deserialize<RetrieveDeviceStatusRequest>(payload.ToString());
                                    cloverConnector.RetrieveDeviceStatus(request);
                                    break;
                                }
                            case WebSocketMethod.SendMessageToActivity:
                                {
                                    MessageToActivity mta = JsonUtils.deserialize<MessageToActivity>(payload.ToString());
                                    cloverConnector.SendMessageToActivity(mta);
                                    break;
                                }
                            case WebSocketMethod.RetrievePaymentRequest:
                                {
                                    RetrievePaymentRequest rpr = JsonUtils.deserialize<RetrievePaymentRequest>(payload.ToString());
                                    cloverConnector.RetrievePayment(rpr);
                                    break;
                                }
                            case WebSocketMethod.RetrievePrintersRequest:
                                {
                                    RetrievePrintersRequest rpr = JsonUtils.deserialize<RetrievePrintersRequest>(payload.ToString());
                                    cloverConnector.RetrievePrinters(rpr);
                                    break;
                                }
                            case WebSocketMethod.PrintJobStatusRequest:
                                {
                                    PrintJobStatusRequest req = JsonUtils.deserialize<PrintJobStatusRequest>(payload.ToString());
                                    cloverConnector.RetrievePrintJobStatus(req);
                                    break;
                                }
                            case WebSocketMethod.PrintRequest:
                                {
                                    PrintRequest64Message request = JsonUtils.deserialize<PrintRequest64Message>(payload.ToString());
                                    PrintRequest printRequest = null;
                                    if(request.base64strings.Count > 0)
                                    {
                                        byte[] imgBytes = Convert.FromBase64String(request.base64strings[0]);
                                        MemoryStream ms = new MemoryStream();
                                        ms.Write(imgBytes, 0, imgBytes.Length);
                                        Bitmap bp = new Bitmap(ms);
                                        ms.Close();
                                        printRequest = new PrintRequest(bp, request.externalPrintJobId, request.printDeviceId);
                                    }
                                    else if(request.imageUrls.Count > 0)
                                    {
                                        printRequest = new PrintRequest(request.imageUrls[0], request.externalPrintJobId, request.printDeviceId);
                                    }
                                    else if(request.textLines.Count > 0)
                                    {
                                        printRequest = new PrintRequest(request.textLines, request.externalPrintJobId, request.printDeviceId);
                                    }
                                    cloverConnector.Print(printRequest);
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("received unknown websocket method: " + method.ToString() + " in CloverWebSocketService.");
                                    break;
                                }
                        }
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
            //string lanConfig = null;
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
            //parameters.TryGetValue("/L", out lanConfig);


            server = new WebSocketServer(protocol + "://127.0.0.1:" + port);
            if (certPath != null)
            {
                server.Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath);
            }
            CloverDeviceConfiguration config = null;
            if (testConfig)
            {
                //config = new TestCloverDeviceConfiguration();
            }
            else
            {
                config = new USBCloverDeviceConfiguration(null, getPOSNameAndVersion(), Debug, Timer);
            }

            cloverConnector = CloverConnectorFactory.createICloverConnector(config);
            cloverConnector.AddCloverConnectorListener(connectorListener);
            cloverConnector.InitializeConnection();
        }

        private String getPOSNameAndVersion()
        {
            string REG_KEY = "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\CloverSDK";
            String name = "unset";
            String version = "unset";
            try
            {
                Object rName = Registry.GetValue(REG_KEY, "ExternalPOSName", "unset");
                Object rVersion = Registry.GetValue(REG_KEY, "ExternalPOSVersion", "unset");
                name = rName.ToString();
                version = rVersion.ToString();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(SERVICE_NAME, e.Message);
            }
            // not needed if the target Platform in the build is set to x86. The previous key path will resolve to the WOW6443Node as needed
            /*
            if (name.Equals("unset"))
            {
                REG_KEY = "HKEY_LOCAL_MACHINE\\Software\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\CloverSDK";
                try
                {
                    Object rName = Registry.GetValue(REG_KEY, "ExternalPOSName", "unset");
                    Object rVersion = Registry.GetValue(REG_KEY, "ExternalPOSVersion", "unset");
                    name = rName.ToString();
                    version = rVersion.ToString();
                }
                catch (Exception e)
                {
                    System.Diagnostics.EventLog.WriteEntry(SERVICE_NAME, e.Message);
                }
            }
            if (name.Equals("unset") || version.Equals("unset"))
            {
                System.Diagnostics.EventLog.WriteEntry(SERVICE_NAME, "POS Name or Version is not correctly set.  The service will not run until they are appropriately intialized.");
                throw new Exception("Invalid external POS name or version. The REST service cannot run without correctly configured <ExternalPOSName> and <ExternalPOSVersion> registry keys.");
            }
            */
            EventLog.WriteEntry(SERVICE_NAME, "POS Name:Version from registry = " + name + ":" + version);
            return name + ":" + version;
        }

        protected override void OnStop()
        {
            base.OnStop();
            server.ListenerSocket.Close();
        }

        private T Deserialize<T>(String msg)
        {
            T obj = JsonUtils.deserialize<T>(msg);
            return obj;
        }
    }

    /// <summary>
    /// VerifySignatureRequest is abstract, need a concreate class to instantiate
    /// </summary>
    class WSVerifySignatureRequest : VerifySignatureRequest
    {
        public override void Accept() { }
        public override void Reject() { }
    }
}
