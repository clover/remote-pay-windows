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
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Newtonsoft.Json;
using WebSocket4Net;

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

        string pairingAuthToken { get; set; }
        string posName { get; set; }
        string serialNumber { get; set; }
        bool isPairing = true;

        private WebSocket websocket;

        Queue<string> messageQueue = new Queue<string>();

        private string endpoint { get; set; }

        PairingDeviceConfiguration config { get; set; }

        public override string ShortTitle() => "WS";

        /// <summary>
        /// Create WebSocketCloverTransport with the endpoing, custom pairing config, POS name, serial #, stored auth token if available
        /// </summary>
        /// <param name="hostname">The hostname or IP of the Clover device to which you are connecting</param>
        /// <param name="port">The port of the Clover device to which you are connecting</param>
        public WebSocketCloverTransport(string endpoint, PairingDeviceConfiguration pairingConfig, string posName, string serialNumber, string pairingAuthToken)
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

        protected void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("Received message: " + e.Message);
#endif
            if (isPairing)
            {
                var definition = new { id = "", method = "", payload = "", type = "", version = 0 };
                var dynObj = JsonConvert.DeserializeAnonymousType(e.Message, definition);
                if (dynObj.method != null)
                {
                    string method = dynObj.method;

                    if (PairingCodeMessage.METHOD.Equals(method))
                    {
                        PairingCodeMessage pcm = JsonUtils.deserialize<PairingCodeMessage>(dynObj.payload);
                        if (config.OnPairingCode != null)
                        {
                            config.OnPairingCode(pcm.pairingCode);
                        }
                        else
                        {
                            throw new Exception("OnPairingCode handler not set");
                        }
                    }
                    else if (PairingResponse.METHOD.Equals(method))
                    {
                        PairingResponse pr = JsonUtils.deserialize<PairingResponse>(dynObj.payload);
                        if (PairingResponse.PAIRED.Equals(pr.pairingState) || PairingResponse.INITIAL.Equals(pr.pairingState))
                        {
                            isPairing = false;
                            pairingAuthToken = pr.authenticationToken;
                            if (config.OnPairingSuccess != null)
                            {
                                config.OnPairingSuccess(pr.authenticationToken);
                            }
                            onDeviceReady();
                        }
                        else if (PairingResponse.FAILED.Equals(pr.pairingState))
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

        /// <summary>
        /// Validate the device's WebSocket security certificate (e.g. for securing TLS 1.2)
        /// Return false to reject certificate and close connection, return true to accept certificate and use connection. (For debugging without installing certificates, return true.)
        /// 
        /// This is a certificate chain based on the Clover Device Server Root CA.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool WSRemoteServerCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
#if DEBUG
            // this will write out the cert received from the device for debugging purposes
            string certFileName = "device_cert.crt";
            using (FileStream file = System.IO.File.Create(certFileName))
            {
                byte[] cert = certificate.Export(X509ContentType.Cert);
                file.Write(cert, 0, cert.Length);
            }
#endif

            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            else
            {
                base.onDeviceError(-201, null, "Error connecting: " + sslPolicyErrors);
                shutdown = true;
                return false;
            }
        }

        private void connect(string endpoint)
        {
            if (!shutdown)
            {
                ServicePointManager.ServerCertificateValidationCallback = this.WSRemoteServerCertificateValidationCallback;
                try
                {
                    Uri uri = new Uri(endpoint);
                    switch (uri.Scheme.ToLower())
                    {
                        case "ws":
                            websocket = new WebSocket(endpoint);
                            break;
                        case "wss":
#if NET40
                            throw new ArgumentException("Clover Windows SDK does not support SSL connections in .NET 4.0, only in .NET 4.5 configurations.", nameof(endpoint));
#elif NET45
                            websocket = new WebSocket(endpoint, sslProtocols: SslProtocols.Tls12);
                            // websocket.Security.AllowUnstrustedCertificate = true; // for testing ONLY
#endif
                            break;
                        default:
                            Exception ex = new ArgumentException($"Unknown endpoint scheme \"{uri.Scheme}\". Expecting \"wss://\" or \"ws://\" secure or unsecure WebSocket scheme.", nameof(endpoint));
                            onDeviceError(/* CloverDeviceErrorEvent.InvalidEndpoint */ -16381, ex, ex.Message);
                            break;
                    }
                }
                catch (UriFormatException exception)
                {
                    Exception ex = new ArgumentException("Invalid endpoint uri format.", nameof(endpoint), exception);
                    onDeviceError(/* CloverDeviceErrorEvent.InvalidEndpoint */ -16381, ex, ex.Message);
                }

                if (websocket != null)
                {
                    websocket.Opened += websocket_Opened;
                    websocket.Error += websocket_Error;
                    websocket.Closed += websocket_Closed;
                    websocket.MessageReceived += websocket_MessageReceived;

                    websocket.Open();
                }
            }
        }

        private void reconnect(object sender, DoWorkEventArgs e)
        {
            if (!shutdown)
            {
                new Timer((obj) => { connect(endpoint); }, null, 3000, System.Threading.Timeout.Infinite);
            }
        }

        private void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            // TOD: Trace error
            Console.WriteLine("error: " + e.Exception);
        }

        /// <summary>
        /// Close websocket
        /// </summary>
        public override void Dispose()
        {
            shutdown = true;
            websocket.Close();
        }

        protected static int getMaxDataTransferSize()
        {
            return MAX_PACKET_BYTES - PACKET_HEADER_SIZE;
        }

        /// <summary>
        /// Send message to device
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override int sendMessage(string message)
        {
            if (websocket != null)
            {
                websocket.Send(message);
                return 0;
            }

            return -1;
        }

        /// <summary>
        /// Is websocket connected to device
        /// </summary>
        /// <returns></returns>
        public bool isConnected()
        {
            return websocket.State == WebSocketState.Open;
        }

        /// <summary>
        /// Destructor to insure websocket is disconnected and resources released
        /// </summary>
        ~WebSocketCloverTransport()
        {
            Console.WriteLine("Entering ~WebSocketCloverTransport");
            disconnect();
            Console.WriteLine("Exiting ~WebSocketCloverTransport");
        }

        /// <summary>
        /// Disconnect websocket
        /// </summary>
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
    }
}
