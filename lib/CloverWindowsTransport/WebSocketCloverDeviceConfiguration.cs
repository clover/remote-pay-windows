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
using System.Text;

namespace com.clover.remotepay.transport
{
    public class WebSocketCloverDeviceConfiguration : CloverDeviceConfiguration
    {
        public string hostname;
        public Int32 port;
        public string remoteApplicationID;
        public bool enableLogging = false;
        public int pingSleepSeconds = 0;

        public WebSocketCloverDeviceConfiguration(string hostname, Int32 port)
        {
            this.hostname = hostname;
            this.port = port;
        }
        public WebSocketCloverDeviceConfiguration(string hostname, Int32 port, string remoteApplicationID, bool enableLogging, int pingSleepSeconds)
        {
            this.hostname = hostname;
            this.port = port;
            this.remoteApplicationID = remoteApplicationID;
            this.enableLogging = enableLogging;
            this.pingSleepSeconds = pingSleepSeconds;
        }
        public string getCloverDeviceTypeName()
        {
            return typeof(DefaultCloverDevice).AssemblyQualifiedName;
        }

        public CloverTransport getCloverTransport()
        {
            return new WebSocketCloverTransport(this.hostname, this.port);
        }

        public string getMessagePackageName()
        {
            return "com.clover.remote.protocol.lan";
        }

        public string getName()
        {
            return "Clover via WebSocket";
        }

        public bool getEnableLogging()
        {
            return enableLogging;
        }

        public int getPingSleepSeconds()
        {
            return pingSleepSeconds;
        }

        public string getRemoteApplicationID()
        {
            return remoteApplicationID;
        }
    }
}
