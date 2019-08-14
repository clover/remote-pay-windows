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

namespace com.clover.remotepay.transport
{
    public class USBCloverDeviceConfiguration : CloverDeviceConfiguration
    {
        string deviceId;
        bool enableLogging = false;
        int pingSleepSeconds = 1;
        int maxCharInMessage = 10000;
        string remoteApplicationID;
        string posName;
        string serialNumber;
        CloverTransport transport;

        public USBCloverDeviceConfiguration(string remoteApplicationID, bool enableLogging) : this("", remoteApplicationID, enableLogging, 1)
        {
        }

        public USBCloverDeviceConfiguration(string deviceId, string remoteApplicationID, bool enableLogging, int pingSleepSeconds) : this("", remoteApplicationID, "", "", enableLogging, 1)
        {
        }

        /// <summary>
        /// Configuration for a Clover USB Transport connection to a Clover Device
        /// </summary>
        /// <param name="deviceId">USB Device specification - only one connected device currently allowed</param>
        /// <param name="remoteApplicationID">Application ID for server reporting</param>
        /// <param name="posName">Point of Sale name for server reporting</param>
        /// <param name="serialNumber">Station ID / serial number for server reporting</param>
        /// <param name="enableLogging"></param>
        /// <param name="pingSleepSeconds"></param>
        public USBCloverDeviceConfiguration(string deviceId, string remoteApplicationID, string posName, string serialNumber, bool enableLogging = false, int pingSleepSeconds = 1)
        {
            this.deviceId = deviceId;
            if (remoteApplicationID == null || remoteApplicationID.Trim().Equals(""))
            {
                throw new ArgumentException("remoteApplicatoinID is required");
            }

            this.remoteApplicationID = remoteApplicationID;
            this.enableLogging = enableLogging;
            this.pingSleepSeconds = pingSleepSeconds;
            this.posName = posName;
            this.serialNumber = serialNumber;
        }

        public string getCloverDeviceTypeName()
        {
            return typeof(DefaultCloverDevice).AssemblyQualifiedName;
        }

        public CloverTransport getCloverTransport()
        {
            if (transport == null)
            {
                transport = new USBCloverTransport(this.deviceId, pingSleepSeconds);
            }
            return transport;
        }

        public bool getEnableLogging()
        {
            return enableLogging;
        }

        public int getPingSleepSeconds()
        {
            return pingSleepSeconds;
        }

        public string getMessagePackageName()
        {
            return "com.clover.remote.protocol.usb";
        }

        public string getName()
        {
            return "Clover via USB";
        }

        public string getRemoteApplicationID()
        {
            return remoteApplicationID;
        }

        public int getMaxMessageCharacters()
        {
            return maxCharInMessage;
        }
    }
}
