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

using LibUsbDotNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
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

        public static Boolean initializeDeviceConnectionAccessoryMode(UsbDevice device)
        {
            Boolean r = false;

            if (device != null)
            {
                Boolean bOk = false;
                Boolean shouldRun = true;

                bOk = checkProtocol(device);

                Console.WriteLine("checkProtocol:"+bOk+"\n");

                if (shouldRun)
                {
                    bOk = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_MANUFACTURER,
                                             "Clover");
                    if (!bOk)
                    {
                        Console.WriteLine("SendControlMessage: ACCESSORY_STRING_MANUFACTURER\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    bOk = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_MODEL,
                                             "Adapter");
                    if (!bOk)
                    {
                        Console.WriteLine("SendControlMessage: ACCESSORY_STRING_MODEL\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    bOk = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_DESCRIPTION,
                                             "Windows POS Device");
                    if (!bOk)
                    {
                        Console.WriteLine("SendControlMessage: ACCESSORY_STRING_DESCRIPTION\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    bOk = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_VERSION,
                                             "0.01");
                    if (!bOk)
                    {
                        Console.WriteLine("SendControlMessage: ACCESSORY_STRING_VERSION\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    bOk = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_URI,
                                             "market://details?id=com.clover.remote.protocol.usb");
                    if (!bOk)
                    {
                        Console.WriteLine("SendControlMessage: ACCESSORY_STRING_URI\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    bOk = sendControlMessage(device,
                                             ACCESSORY_SEND_STRING,
                                             ACCESSORY_STRING_SERIAL,
                                             "C415000");
                    if (!bOk)
                    {
                        Console.WriteLine("SendControlMessage: ACCESSORY_STRING_SERIAL\n");
                        shouldRun = false;
                    }
                }

                if (shouldRun)
                {
                    bOk = sendControlMessage(device,
                                             ACCESSORY_START,
                                             0,
                                             null);
                    if (!bOk)
                    {
                        Console.WriteLine("SendControlMessage: ACCESSORY_START\n");
                        shouldRun = false;
                    }
                }

                r = shouldRun;
            }
                return r;
        }

        public static Boolean sendControlMessage(UsbDevice device, byte requestCode, short index, string message)
        {
            Boolean r = false;

            short messageLength = 0;

            if (message != null)
            {
                messageLength = (short)(message.Length);
            }


            UsbSetupPacket setupPacket = new UsbSetupPacket();
            setupPacket.RequestType = (byte)((byte)UsbConstants.USB_DIR_OUT | (byte)UsbConstants.USB_TYPE_VENDOR);

            setupPacket.Request = requestCode; // byte
            setupPacket.Value = 0;
            setupPacket.Index = index; // short
            setupPacket.Length = messageLength; // short

            int resultTransferred;
            Console.WriteLine("RequestType:" + setupPacket.RequestType);
            Console.WriteLine("setupPacket.Request:" + setupPacket.Request);
            Console.WriteLine("setupPacket.Value:" + setupPacket.Value);
            Console.WriteLine("setupPacket.Index:" + setupPacket.Index);
            Console.WriteLine("setupPacket.Length:" + setupPacket.Length);

            Console.WriteLine("message:" + message);


            byte[] messageBytes = null;
            if (null != message)
            {
                messageBytes = Encoding.UTF8.GetBytes(message);
            }

            r = device.ControlTransfer(ref setupPacket, messageBytes, messageLength, out resultTransferred);

            return r;
        }

        public static Boolean checkProtocol(UsbDevice device)
        {
            Boolean r = false;

            string message = "";
            short messageLength = 2;

            UsbSetupPacket setupPacket = new UsbSetupPacket();
            setupPacket.RequestType = (byte)((byte)UsbConstants.USB_DIR_IN | (byte) UsbConstants.USB_TYPE_VENDOR);
            setupPacket.Request = (byte)ACCESSORY_GET_PROTOCOL;
            setupPacket.Value = 0;
            setupPacket.Index = 0;
            setupPacket.Length = 0;

            int resultTransferred;

            r = device.ControlTransfer(ref setupPacket, message, messageLength, out resultTransferred);

            return r;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

    }
}
