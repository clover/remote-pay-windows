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
    public class WebSocketCloverDeviceConfiguration : PairingDeviceConfiguration, CloverDeviceConfiguration
    {
        public string endpoint;
        public string remoteApplicationID;
        public bool enableLogging = false;
        public int pingSleepSeconds = 1;
        public string posName;
        public string serialNumber;
        public string pairingAuthToken;


        public WebSocketCloverDeviceConfiguration(string endpoint, String remoteApplicationID, string posName, string serialNumber, string pairingAuthToken) : this(endpoint, remoteApplicationID, false, 1, posName, serialNumber, pairingAuthToken, null, null)
        {

        }
        public WebSocketCloverDeviceConfiguration(string endpoint, string remoteApplicationID, bool enableLogging, int pingSleepSeconds, string posName, string serialNumber, string pairingAuthToken, OnPairingCodeHandler pairingCodeHandler, OnPairingSuccessHandler pairingSuccessHandler)
        {
            this.endpoint = endpoint;
            if (remoteApplicationID == null || remoteApplicationID.Trim().Equals(""))
            {
                throw new ArgumentException("remoteApplicationID is required");
            }
            this.remoteApplicationID = remoteApplicationID;
            this.enableLogging = enableLogging;
            this.pingSleepSeconds = pingSleepSeconds;
            this.posName = posName;
            this.serialNumber = serialNumber;
            this.pairingAuthToken = pairingAuthToken;
            this.OnPairingCode = pairingCodeHandler;
            this.OnPairingSuccess = pairingSuccessHandler;

        }
        public string getCloverDeviceTypeName()
        {
            return typeof(DefaultCloverDevice).AssemblyQualifiedName;
        }

        public CloverTransport getCloverTransport()
        {
            return new WebSocketCloverTransport(this.endpoint, this, posName, serialNumber, pairingAuthToken);
        }

        public string getMessagePackageName()
        {
            return "com.clover.remote_protocol_broadcast.app";
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
