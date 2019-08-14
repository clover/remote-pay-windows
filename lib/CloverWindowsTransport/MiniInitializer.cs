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
using System.Collections.Generic;
using System.Linq;
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
        public delegate bool Step();

        public static bool InitializeDeviceConnectionAccessoryMode(UsbDevice device)
        {
            List<Step> steps = new List<Step>
            {
                () => device != null,
                () => CheckProtocol(device),
                () => SendControlMessage(device, ACCESSORY_SEND_STRING, ACCESSORY_STRING_MANUFACTURER, "Clover"),
                () => SendControlMessage(device, ACCESSORY_SEND_STRING, ACCESSORY_STRING_MODEL, "Adapter"),
                () => SendControlMessage(device, ACCESSORY_SEND_STRING, ACCESSORY_STRING_DESCRIPTION, "Windows POS Device"),
                () => SendControlMessage(device, ACCESSORY_SEND_STRING, ACCESSORY_STRING_VERSION, "0.01"),
                () => SendControlMessage(device, ACCESSORY_SEND_STRING, ACCESSORY_STRING_URI, "market://details?id=com.clover.remote.protocol.usb"),
                () => SendControlMessage(device, ACCESSORY_SEND_STRING, ACCESSORY_STRING_SERIAL, "C415000"),
                () => SendControlMessage(device, ACCESSORY_START, 0, null),
            };

            return steps.All(step => step());
        }

        private static bool SendControlMessage(UsbDevice device, byte requestCode, short index, string message)
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

            byte[] messageBytes = null;
            if (null != message)
            {
                messageBytes = Encoding.UTF8.GetBytes(message);
            }

            return device.ControlTransfer(ref setupPacket, messageBytes, messageLength, out int resultTransferred);
        }

        private static bool CheckProtocol(UsbDevice device)
        {
            // 2 byte buffer, intialized to default zero value
            byte[] message = new byte[2];
            short messageLength = 2;

            UsbSetupPacket setupPacket = new UsbSetupPacket();
            setupPacket.RequestType = (byte)((byte)UsbConstants.USB_DIR_IN | (byte)UsbConstants.USB_TYPE_VENDOR);
            setupPacket.Request = ACCESSORY_GET_PROTOCOL;
            setupPacket.Value = 0;
            setupPacket.Index = 0;
            setupPacket.Length = 0;

            return device.ControlTransfer(ref setupPacket, message, messageLength, out int resultTransferred);
        }
    }
}
