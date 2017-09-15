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
    public class TestCloverDeviceConfiguration : CloverDeviceConfiguration
    {
        public bool enableLogging;
        public int pingSleepSeconds;
        public string remoteApplicationID;

        public string getCloverDeviceTypeName()
        {
            return typeof(TestCloverDevice).AssemblyQualifiedName;
        }

        public string getMessagePackageName()
        {
            return "com.clover.remote.protocol.test";
        }

        public string getName()
        {
            return "Device Emulator";
        }

        public CloverTransport getCloverTransport()
        {
            return new TestCloverTransport();
        }

        public bool getEnableLogging()
        {
            throw new NotImplementedException();
        }

        public int getPingSleepSeconds()
        {
            throw new NotImplementedException();
        }

        public string getRemoteApplicationID()
        {
            return "XXXXXXXXXXXXXXXX";
        }
    }
}
