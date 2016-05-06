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

        private WebSocket websocket;

        Queue<string> messageQueue = new Queue<string>();

        private string hostname { get; set; }
        private int port { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostname">The hostname or IP of the Clover device to which you are connecting</param>
        /// <param name="port">The port of the Clover device to which you are connecting</param>
        public WebSocketCloverTransport(string hostname, Int32 port)
        {
            this.hostname = hostname;
            this.port = port;
            connect(hostname, port);
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            initialized = true;

            onDeviceConnected();
            onDeviceReady();
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            onMessage(e.Message);
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            onDeviceDisconnected();
            Console.WriteLine("socket closed");

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += reconnect;
            bw.RunWorkerAsync();
        }

        private void connect(string hostname, int port)
        {
            if(!shutdown)
            {
                websocket = new WebSocket("ws://" + hostname + ":" + port + "/");
                websocket.Opened += new EventHandler(websocket_Opened);
                websocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocket_Error);
                websocket.Closed += new EventHandler(websocket_Closed);
                websocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocket_MessageReceived);
                websocket.Open();
            }

        }

        private void reconnect(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
            if(!shutdown)
            {
                connect(hostname, port);
            }
        }

        private void websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine("error: " + e.Exception);
        }

        public override void Dispose()
        {
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
            shutdown = true;
            base.onDeviceDisconnected();
        }
    }
}
