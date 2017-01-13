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

namespace com.clover.remotepay.transport.remote
{
    /// <summary>
    /// Configuration object used for initializing the
    /// RemoteRESTCloverConnector, which is primarly
    /// used as an example and for testing and validation
    /// </summary>
    class RemoteRESTCloverConfiguration : CloverDeviceConfiguration
    {
        private string hostname;
        private int port;
        private string remoteApplicationID;
        private bool enableLogging = false;
        private int pingSleepSeconds = 1;

        public RemoteRESTCloverConfiguration(string host, int port, String remoteApplicationId) : this(host, port, remoteApplicationId, false, 1)
        {
        }

        public RemoteRESTCloverConfiguration(string host, int port, string remoteApplicationID, bool enableLogging, int pingSleepSeconds)
        {
            this.hostname = host;
            this.port = port;
            if (remoteApplicationID == null || remoteApplicationID.Trim().Equals(""))
            {
                throw new ArgumentException("remoteApplicatoinID is required");
            }
            this.remoteApplicationID = remoteApplicationID;
            this.enableLogging = enableLogging;
            this.pingSleepSeconds = pingSleepSeconds;
        }

        public string getCloverDeviceTypeName()
        {
 	        throw new NotImplementedException();
        }

        public string getMessagePackageName()
        {
 	        throw new NotImplementedException();
        }

        public string getName()
        {
            return "REST Service Mini";
        }

        public CloverTransport getCloverTransport()
        {
 	        throw new NotImplementedException();
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
