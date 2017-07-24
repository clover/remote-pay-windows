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
    class CloverDeviceExample
    {
        public static void Main(string[] args)
        {
            // Send output to a file
            new CaptureLog();

            CloverDeviceConfiguration configuration =
                new USBCloverDeviceConfiguration("ThisWillBeTheDeviceId (possibly)", "com.clover.remotepay.transport.example.CloverDeviceExample", false, 1);
            CloverDevice device = CloverDeviceFactory.Get(configuration);

            device.Subscribe(new CloverListener(device));
            System.Threading.Thread.Sleep(20000000);
        }
    }

    class CloverListener : CloverTransportObserver
    {
        CloverDevice device;

        public CloverListener(CloverDevice device)
        {
            this.device = device;
        }
        public void onDeviceReady(CloverTransport transport)
        {
            bool stop = false;
            ConsoleKeyInfo info;
            do
            {
                // Wait for user input..
                info = Console.ReadKey();

                switch (info.KeyChar)
                {
                    case 'x': stop = true; break;
                    case '1': device.doDiscoveryRequest(); break;
                    case '2': device.doShowThankYouScreen(); break;
                    case '3': device.doShowWelcomeScreen(); break;
                    case '4': device.doTerminalMessage("Holy jumping weasel critters on a hot cross bun!"); break;
                }
            } while (!stop);
        }

        public void onDeviceDisconnected(CloverTransport transport)
        {
            Console.WriteLine("Device disconnect.");
        }

        public void onMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void onDeviceConnected(CloverTransport transport)
        {
            Console.WriteLine("Device Connected");
        }

        public void onDeviceError(int code, string message)
        {
            Console.WriteLine("Code: " + code.ToString() + " //  Message: " + message);
        }
    }
}
