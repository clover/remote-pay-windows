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
using Grapevine.Server;
using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using com.clover.sdk.v3.payments;
using com.clover.remotepay.sdk.service.client;

namespace CloverWindowsSDKREST
{
    class CloverRESTService : ServiceBase
    {
        public static string SERVICE_NAME = "Clover Connector REST Service";

        CloverRESTServer server;
        //WebServiceHost host = null;
        private bool Debug = false;
        private int Timer = 3;

        public void DebugStart(string[] args)
        {
            args = new string[] { "-timer=3", "-debug" };
            OnStart(args);
        }

        public CloverRESTService()
        {
            this.ServiceName = SERVICE_NAME;
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            if (args.Length == 0)
            {
                // Retrieve the arguments from the service ImagePath
                args = Environment.GetCommandLineArgs();
            }

            string logSource = "_TransportEventLog";
            if (!EventLog.SourceExists(logSource))
                EventLog.CreateEventSource(logSource, logSource);

            EventLogTraceListener myTraceListener = new EventLogTraceListener(logSource);

            // Add the event log trace listener to the collection.
            Trace.Listeners.Add(myTraceListener);

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

            EventLog.WriteEntry(SERVICE_NAME, "Starting...");
            Dictionary<string, string> argsMap = new Dictionary<string, string>();
            for (int i = 1; i < args.Length; i++)
            {
                argsMap.Add(args[i - 1], args[i++]);
            }

            // only allow localhost
            string listenAddress = null;
            if (!argsMap.TryGetValue("/P", out listenAddress))
            {
                listenAddress = "http://127.0.0.1:8181/";
                listenAddress = "8181";
            }

            ServiceEndpoints endpoints = new ServiceEndpoints();

            string callbackEndpoint = null;
            if (!argsMap.TryGetValue("/C", out callbackEndpoint))
            {
                callbackEndpoint = "http://localhost:8182/CloverCallback";
            }

            server = new CloverRESTServer("localhost", listenAddress, "http");// "127.0.0.1", listenAddress, "http");
            CloverRESTConnectorListener connectorListener = new CloverRESTConnectorListener();
            Console.WriteLine("callback endpoint: " + callbackEndpoint);
            connectorListener.RestClient = new RestSharp.RestClient(callbackEndpoint);
            server.ForwardToClientListener = connectorListener;
            server.CloverConnector = (CloverConnector) CloverConnectorFactory.createICloverConnector(new USBCloverDeviceConfiguration(null, getPOSNameAndVersion(), Debug, Timer));
            server.CloverConnector.AddCloverConnectorListener(connectorListener);
            server.CloverConnector.InitializeConnection();
            StartRESTListener();
            server.OnAfterStart += new ToggleServerHandler(this.OnServerStart);
            server.OnStop += new ToggleServerHandler(this.OnServerStop);
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
            */
            if (name.Equals("unset") || version.Equals("unset"))
            {
                EventLog.WriteEntry(SERVICE_NAME, "POS Name or Version is not correctly set.  The service will not run until they are appropriately intialized.");
                throw new Exception("Invalid external POS name or version. The REST service cannot run without correctly configured <ExternalPOSName> and <ExternalPOSVersion> registry keys.");
            }

            EventLog.WriteEntry(SERVICE_NAME, "POS Name:Version from registry = " + name + ":" + version);
            return name + ":" + version;
        }
        private void OnServerStart()
        {
            EventLog.WriteEntry(SERVICE_NAME, "Opened...");
        }

        private void OnServerStop()
        {
            EventLog.WriteEntry(SERVICE_NAME, "Closed...");
        }



        public void StartRESTListener()
        {
            server.Start();
        }

        protected override void OnStop()
        {
            base.OnStop();
            server.Stop();
            server.CloverConnector.Dispose();
            server = null;
        }

    }

    class WSCloverConnector : CloverConnector
    {
        public static WSCloverConnector operator +(WSCloverConnector connector, ICloverConnectorListener connectorListener)
        {
            connector.AddCloverConnectorListener(connectorListener);
            return connector;
        }
        public static WSCloverConnector operator -(WSCloverConnector connector, ICloverConnectorListener connectorListener)
        {
            connector.RemoveCloverConnectorListener(connectorListener);
            return connector;
        }

        public WSCloverConnector(CloverDeviceConfiguration config) : base(config)
        {
            //Initialize(config);
        }

        public void ExecuteInputOption(InputOption inputOption)
        {
            if (Device != null)
            {
                Device.doKeyPress(inputOption.keyPress);
            }
        }

        public new void AcceptSignature(VerifySignatureRequest request)
        {
            if (Device != null)
            {
                Device.doVerifySignature(request.Payment, true);
            }
        }

        public new void RejectSignature(VerifySignatureRequest request)
        {
            if (Device != null)
            {
                Device.doVerifySignature(request.Payment, false);
            }
        }

        public new void AcceptPayment(Payment payment)
        {
            if (Device != null)
            {
                Device.doAcceptPayment(payment);
            }
        }

        public void RejectPayment(RejectPaymentObject rpo)
        {
            if (Device != null)
            {
                Device.doRejectPayment(rpo.Payment, rpo.Challenge);
            }
        }
    }

    public class CloverRESTServer : RESTServer
    {
        public CloverRESTServer(string host = "localhost", string port = "1234", string protocol = "http", string dirindex = "index.html", string webroot = null, int maxthreads = 5) : base(host, port, protocol, dirindex, webroot, maxthreads)
        {
        }

        public CloverRESTConnectorListener ForwardToClientListener { get; set; }

        public CloverConnector CloverConnector { get; set; }

    }
}
