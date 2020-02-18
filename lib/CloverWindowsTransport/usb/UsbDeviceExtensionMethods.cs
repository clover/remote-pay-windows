using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibUsbDotNet;
using LibUsbDotNet.Main;

namespace com.clover.remotepay.transport.usb
{
    internal static class UsbDeviceExtensionMethods
    {
        private const int USB_DIR_OUT = 0;
        private const int USB_DIR_IN = 0x80;
        private const int USB_TYPE_VENDOR = 0x02 << 5;

        public static bool SendControlMessage(this UsbDevice device, byte requestCode, short index, string message)
        {
            short messageLength = 0;

            if (message != null)
            {
                messageLength = (short)message.Length;
            }

            var setupPacket = new UsbSetupPacket
            {
                RequestType = USB_DIR_OUT | USB_TYPE_VENDOR,
                Request = requestCode,
                Value = 0,
                Index = index,
                Length = messageLength,
            };

            byte[] messageBytes = message is null ? null : Encoding.UTF8.GetBytes(message);
            var result = device.ControlTransfer(ref setupPacket, messageBytes, messageLength, out int lengthTransferred);
            return result;
        }

        public static bool CheckProtocol(this UsbDevice device, byte protocol)
        {
            // 2 byte buffer, intialized to default zero value
            byte[] message = new byte[2];
            short messageLength = 2;

            var setupPacket = new UsbSetupPacket
            {
                RequestType = USB_DIR_IN | USB_TYPE_VENDOR,
                Request = protocol,
                Value = 0,
                Index = 0,
                Length = 0,
            };

            var result = device.ControlTransfer(ref setupPacket, message, messageLength, out int lengthTransferred);
            var s1 = System.BitConverter.ToInt16(message, 0);
            return s1 >= 1;
        }

        public static bool TryExtractEndpointPair(this UsbDevice device, out ReadEndpointID readId, out WriteEndpointID writeId)
        {
            var ids = (
                from info in device.Configs.SelectMany(config => config.InterfaceInfoList)
                let r = info.EndpointInfoList.Select(ep => ep.Descriptor.EndpointID).FirstOrDefault(id => (id & 0x80) > 0)
                let w = info.EndpointInfoList.Select(ep => ep.Descriptor.EndpointID).FirstOrDefault(id => (id & 0x80) == 0)
                where r > 0 && w > 0
                select new
                {
                    ReadId = (ReadEndpointID)r,
                    WriteId = (WriteEndpointID)w,
                }).FirstOrDefault();

            readId = ids?.ReadId ?? 0;
            writeId = ids?.WriteId ?? 0;
            return ids != null;
        }
    }
}
