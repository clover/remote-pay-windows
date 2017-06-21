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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Net.Security;
using WebSocket4Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace com.clover.remotepay.transport
{
    class WebSocketCloverTransport : CloverTransport
    {
        private static readonly uint REMOTE_STRING_MAGIC_START_TOKEN = 0xcc771122;
        private static readonly int REMOTE_STRING_HEADER_BYTE_COUNT = 4 + 4; // 2 ints
        // Defined by AOA
        private static readonly int MAX_PACKET_BYTES = 16384;
        // A short
        private static readonly short PACKET_HEADER_SIZE = 2;
        private static readonly int REMOTE_STRING_LENGTH_MAX = 4 * 1024 * 1024;

        private static int EMPTY_STRING = 1;
        private bool shutdown = false;
        private bool initialized = false;
        private readonly int ERROR_LIMIT = 3;

        String pairingAuthToken { get; set; }
        String posName { get; set; }
        String serialNumber { get; set; }
        Boolean isPairing = true;

        private WebSocket websocket;

        Queue<string> messageQueue = new Queue<string>();

        private string endpoint { get; set; }

        PairingDeviceConfiguration config { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostname">The hostname or IP of the Clover device to which you are connecting</param>
        /// <param name="port">The port of the Clover device to which you are connecting</param>
        public WebSocketCloverTransport(string endpoint, PairingDeviceConfiguration pairingConfig, String posName, String serialNumber, String pairingAuthToken)
        {
            this.endpoint = endpoint;
            this.config = pairingConfig;
            this.posName = posName;
            this.serialNumber = serialNumber;
            this.pairingAuthToken = pairingAuthToken;
            connect(endpoint);
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            initialized = true;

            onDeviceConnected();
            //onDeviceReady();
            SendPairingRequest();
        }

        private void SendPairingRequest()
        {
            isPairing = true;
            PairingRequest pr = new PairingRequest();
            pr.name = this.posName;
            pr.authenticationToken = this.pairingAuthToken;
            pr.serialNumber = this.serialNumber;

            PairingRequestMessage prm = new PairingRequestMessage(pr);
            sendMessage(JsonUtils.serialize(prm));
        }

        protected  void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("Received message: " + e.Message);
#endif
            if (isPairing)
            {
                var definition = new { id = "", method = "", payload = "" , type = "", version = 0};
                var dynObj = JsonConvert.DeserializeAnonymousType(e.Message, definition);
                if(dynObj.method != null)
                {
                    String method = dynObj.method;

                    if(PairingCodeMessage.METHOD.Equals(method))
                    {
                        PairingCodeMessage pcm = JsonUtils.deserialize<PairingCodeMessage>(dynObj.payload);
                        if(config.OnPairingCode != null)
                        {
                            config.OnPairingCode(pcm.pairingCode);
                        }
                        else
                        {
                            throw new Exception("OnPairingCode handler not set");
                        }
                    }
                    else if(PairingResponse.METHOD.Equals(method))
                    {
                        PairingResponse pr = JsonUtils.deserialize<PairingResponse>(dynObj.payload);
                        if(PairingResponse.PAIRED.Equals(pr.pairingState) || PairingResponse.INITIAL.Equals(pr.pairingState))
                        {
                            isPairing = false;
                            pairingAuthToken = pr.authenticationToken;
                            if (config.OnPairingSuccess != null) {
                                config.OnPairingSuccess(pr.authenticationToken);
                            }
                            onDeviceReady();
                        }
                        else if(PairingResponse.FAILED.Equals(pr.pairingState))
                        {
                            pairingAuthToken = null; // 
                            SendPairingRequest();
                        }
                    }
                }
            }
            else
            {
                onMessage(e.Message);
            }

        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            onDeviceDisconnected();
            Console.WriteLine("socket closed");

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += reconnect;
            bw.RunWorkerAsync();
        }

        private bool WSRemoteServerCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
#if DEBUG
            // this will write out the cert received from the device for debugging purposes
            string certFileName = "device_cert.crt";
            using (var file = File.Create(certFileName))
            {
                var cert = certificate.Export(X509ContentType.Cert);
                file.Write(cert, 0, cert.Length);
            }
#endif

            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            else
            {
                base.onDeviceError(-201, "Error connecting: " + sslPolicyErrors);
                shutdown = true;
                return false;
            }
        }

        private void connect(string endpoint)
        {
            if(!shutdown)
            {
                ServicePointManager.ServerCertificateValidationCallback = this.WSRemoteServerCertificateValidationCallback;

                websocket = new WebSocket(endpoint);
                websocket.Opened += new EventHandler(websocket_Opened);
                websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
                websocket.Closed += new EventHandler(websocket_Closed);
                websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
                //websocket.AllowUnstrustedCertificate = true; // for testing ONLY
                websocket.Open();
            }

        }

        private void reconnect(object sender, DoWorkEventArgs e)
        {
            
            if(!shutdown)
            {
                new Timer((obj) => { connect(endpoint); }, null, 3000, System.Threading.Timeout.Infinite);
            }
        }

        private void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine("error: " + e.Exception);
        }

        public override void Dispose()
        {
            shutdown = true;
            websocket.Close();
        }

        protected static int getMaxDataTransferSize()
        {
            return MAX_PACKET_BYTES - PACKET_HEADER_SIZE;
        }

        public override int sendMessage(string message)
        {
            if (websocket != null)
            {
                websocket.Send(message);
                return 0;
            }

            return -1;
        }

        public bool isConnected()
        {
            return (websocket.State == WebSocketState.Open);
        }

        ~WebSocketCloverTransport()
        {
            Console.WriteLine("Entering ~WebSocketCloverTransport");
            disconnect();
            Console.WriteLine("Exiting ~WebSocketCloverTransport");
        }

        public void disconnect()
        {
            Console.WriteLine("Entering disconnect");

            if (websocket != null)
            {
                websocket.Close();
            }

            onDeviceDisconnected();
            Console.WriteLine("Exiting disconnect");
        }

        protected override void onDeviceDisconnected()
        {
            base.onDeviceDisconnected();
        }
    }
}
