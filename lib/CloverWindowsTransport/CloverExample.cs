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
    class CloverTransportExample
    {
        public static void Main_xx(string[] args)
        {
            USBCloverTransport device = new USBCloverTransport("123");
            device.Subscribe(new USBCloverTransportListener(device));
            System.Threading.Thread.Sleep(20000000);
        }
    }

    class USBCloverTransportListener : CloverTransportObserver
    {

        CloverTransport device;

        public USBCloverTransportListener(CloverTransport device)
        {
            this.device = device;
        }
        public void onDeviceReady(CloverTransport device)
        {

            string message = "{" +
                "\"id\":\"208\"," +
                "\"method\":\"DISCOVERY_REQUEST\"," +
                "\"packageName\":\"com.clover.remote.protocol.usb\"," +
                "\"payload\":\"{\\\"method\\\":\\\"DISCOVERY_REQUEST\\\",\\\"version\\\":1}\"," +
                "\"type\":\"COMMAND\"" +
                "}";
            ConsoleKeyInfo info;
            do
            {
                device.sendMessage(message);
                // Wait for user input..
                info = Console.ReadKey();
            } while (info.KeyChar != 'x');


        }

        public void onDeviceDisconnected(CloverTransport device)
        {
            Console.WriteLine("Device disconnect.");
        }

        public void onDeviceConnected(CloverTransport transport)
        {
            Console.WriteLine("Device found.");
        }

        public void onMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void onDeviceError(int code, string message)
        {
            Console.WriteLine("Code: " + code.ToString() + " //  Message: " + message);
        }
    }
}
