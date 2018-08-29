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
using System.Text;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace com.clover.remotepay.transport
{
    class MiniInitializer
    {
        public static short ACCESSORY_STRING_MANUFACTURER = 0;
        public static short ACCESSORY_STRING_MODEL = 1;
        public static short ACCESSORY_STRING_DESCRIPTION = 2;
        public static byte ACCESSORY_STRING_VERSION = 3;
        public static byte ACCESSORY_STRING_URI = 4;
        public static byte ACCESSORY_STRING_SERIAL = 5;
        public static byte ACCESSORY_GET_PROTOCOL = 51;
        public static byte ACCESSORY_SEND_STRING = 52;
        public static byte ACCESSORY_START = 53;

        public static bool initializeDeviceConnectionAccessoryMode(UsbDevice device)
        {
            bool result = false;

            if (device != null)
            {
                bool shouldRun = true;
                bool ok = checkProtocol(device);

                // Console.WriteLine("checkProtocol:" + ok + "\n");

                if (shouldRun)
                {
                    ok = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_MANUFACTURER,
                                             "Clover");
                    if (!ok)
                    {
                        // Console.WriteLine("SendControlMessage: ACCESSORY_STRING_MANUFACTURER\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    ok = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_MODEL,
                                             "Adapter");
                    if (!ok)
                    {
                        // Console.WriteLine("SendControlMessage: ACCESSORY_STRING_MODEL\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    ok = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_DESCRIPTION,
                                             "Windows POS Device");
                    if (!ok)
                    {
                        // Console.WriteLine("SendControlMessage: ACCESSORY_STRING_DESCRIPTION\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    ok = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_VERSION,
                                             "0.01");
                    if (!ok)
                    {
                        // Console.WriteLine("SendControlMessage: ACCESSORY_STRING_VERSION\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    ok = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_URI,
                                             "market://details?id=com.clover.remote.protocol.usb");
                    if (!ok)
                    {
                        // Console.WriteLine("SendControlMessage: ACCESSORY_STRING_URI\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    ok = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_SERIAL,
                                             "C415000");
                    if (!ok)
                    {
                        // Console.WriteLine("SendControlMessage: ACCESSORY_STRING_SERIAL\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    ok = sendControlMessage(device,
                                             ACCESSORY_START,
                                             0,
                                             null);
                    if (!ok)
                    {
                        // Console.WriteLine("SendControlMessage: ACCESSORY_START\n");
                        shouldRun = false;
                    }
                }

                result = shouldRun;
            }
            return result;
        }

        public static bool sendControlMessage(UsbDevice device, byte requestCode, short index, string message)
        {
            short messageLength = 0;

            if (message != null)
            {
                messageLength = (short)message.Length;
            }

            UsbSetupPacket setupPacket = new UsbSetupPacket();
            setupPacket.RequestType = (byte)((byte)UsbConstants.USB_DIR_OUT | (byte)UsbConstants.USB_TYPE_VENDOR);

            setupPacket.Request = requestCode;
            setupPacket.Value = 0;
            setupPacket.Index = index;
            setupPacket.Length = messageLength;

            // Console.WriteLine("RequestType:" + setupPacket.RequestType);
            // Console.WriteLine("setupPacket.Request:" + setupPacket.Request);
            // Console.WriteLine("setupPacket.Value:" + setupPacket.Value);
            // Console.WriteLine("setupPacket.Index:" + setupPacket.Index);
            // Console.WriteLine("setupPacket.Length:" + setupPacket.Length);

            // Console.WriteLine("message:" + message);

            byte[] messageBytes = null;
            if (null != message)
            {
                messageBytes = Encoding.UTF8.GetBytes(message);
            }

            return device.ControlTransfer(ref setupPacket, messageBytes, messageLength, out int resultTransferred);
        }

        public static bool checkProtocol(UsbDevice device)
        {
            string message = "";
            short messageLength = 2;

            UsbSetupPacket setupPacket = new UsbSetupPacket();
            setupPacket.RequestType = (byte)((byte)UsbConstants.USB_DIR_IN | (byte)UsbConstants.USB_TYPE_VENDOR);
            setupPacket.Request = ACCESSORY_GET_PROTOCOL;
            setupPacket.Value = 0;
            setupPacket.Index = 0;
            setupPacket.Length = 0;

            return device.ControlTransfer(ref setupPacket, message, messageLength, out int resultTransferred);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
