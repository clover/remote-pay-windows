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
using com.clover.remotepay.transport;
using Grapevine.Server;
using System;
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

namespace CloverWindowsSDKREST
{
    class CloverRESTService : ServiceBase
    {
        public static string SERVICE_NAME = "Clover Mini REST Service";

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
                        try {
                            string timerString = timerStrings.First();
                            int index = timerString.IndexOf('=');
                            string timerSeconds = timerString.Substring(index + 1);
                            Timer = Convert.ToInt32(timerSeconds);
                        } catch (Exception e)
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
            else
            {
                //listenAddress = "http://127.0.0.1:" + listenAddress + "/";
            }
            //Uri baseAddress = new Uri(listenAddress);

            //Service service = new Service();
            ServiceEndpoints endpoints = new ServiceEndpoints();

            string callbackEndpoint = null;
            if (!argsMap.TryGetValue("/C", out callbackEndpoint))
            {
                callbackEndpoint = "http://127.0.0.1:8182/CloverCallback";
            }

            server = new CloverRESTServer("localhost", listenAddress, "http");// "127.0.0.1", listenAddress, "http");

            CloverRESTConnectorListener connectorListener = new CloverRESTConnectorListener();
            Console.WriteLine("callback endpoint: " + callbackEndpoint);
            connectorListener.RestClient = new RestSharp.RestClient(callbackEndpoint);
            //service.CloverConnector.AddCloverConnectorListener(connectorListener);
            server.ForwardToClientListener = connectorListener;
            string webSocketEndpoint = null;
            if(argsMap.TryGetValue("/L", out webSocketEndpoint))
            {
                string[] tokens = webSocketEndpoint.Split(new char[]{':'});
                if(tokens.Length != 2) {
                    throw new Exception("Invalid host and port. must be <hostname>:<port>");
                }
                string hostname = tokens[0];
                int port = int.Parse(tokens[1]);
                server.CloverConnector = new CloverConnector(new WebSocketCloverDeviceConfiguration(hostname, port, "Clover Example", Debug, Timer), connectorListener);
            }
            else
            {
                server.CloverConnector = new CloverConnector(new USBCloverDeviceConfiguration(null, "Clover Example", Debug, Timer), connectorListener);
            }

            StartRESTListener();
            server.OnAfterStart += new ToggleServerHandler(this.OnServerStart);
            server.OnStop += new ToggleServerHandler(this.OnServerStop);
        }

        private void OnServerStart()
        {
            System.Diagnostics.EventLog.WriteEntry(SERVICE_NAME, "Opened...");
        }

        private void OnServerStop()
        {
            System.Diagnostics.EventLog.WriteEntry(SERVICE_NAME, "Closed...");
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

    public class CloverRESTServer : RESTServer
    {
        public CloverRESTServer(string host = "localhost", string port = "1234", string protocol = "http", string dirindex = "index.html", string webroot = null, int maxthreads = 5) : base(host, port, protocol, dirindex, webroot, maxthreads)
        {
        }

        public CloverRESTConnectorListener ForwardToClientListener { get; set; }

        public CloverConnector CloverConnector { get; set; }

    }
}
