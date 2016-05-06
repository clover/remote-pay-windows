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
using System.Text;
using System.Threading;

namespace com.clover.remotepay.transport
{
    public class TestCloverTransport : CloverTransport
    {
        public TestCloverTransport()
        {
            BackgroundWorker bw = new BackgroundWorker();
            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                Thread.Sleep(3000);
                onDeviceConnected();
                Thread.Sleep(5000);
                onDeviceReady();
            });
            bw.RunWorkerAsync();
        }

        public override void Dispose()
        {
        
        }
        public override int sendMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
