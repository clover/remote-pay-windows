using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using com.clover.remotepay.data;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using LibUsbDotNet.WinUsb;

namespace com.clover.remotepay.transport.usb
{
    public partial class CloverUsbDevice
    {
        private const int ACCESSORY_GET_PROTOCOL = 51;
        private const int ACCESSORY_SEND_STRING = 52;
        private const int ACCESSORY_START = 53;
        private const int ACCESSORY_STRING_MANUFACTURER = 0;
        private const int ACCESSORY_STRING_MODEL = 1;
        private const int ACCESSORY_STRING_DESCRIPTION = 2;
        private const int ACCESSORY_STRING_VERSION = 3;
        private const int ACCESSORY_STRING_URI = 4;
        private const int ACCESSORY_STRING_SERIAL = 5;
        private const uint REMOTE_STRING_MAGIC_START_TOKEN = 0xcc771122;
        private const int REMOTE_STRING_LENGTH_MAX = 4 * 1024 * 1024;
        private const int MAX_PACKET_BYTES = 16384;


        private IEnumerable<UsbRegistry> CloverDevices =>
            UsbDevice
                .AllDevices
                .Cast<UsbRegistry>()
                .Where(r => UsbIdentity.AllIdentities.Any(id =>
                    r.Vid == id.Vid &&
                    r.Pid == id.Pid &&
                    r.GetInterfaceId() == 0));
    }

    public sealed partial class CloverUsbDevice : IDisposable
    {
        private readonly object Lock = new object();
        private UsbDeviceWatcher deviceWatcher = new UsbDeviceWatcher();
        private UsbEndpointReader reader;
        private UsbEndpointWriter writer;
        private MessageBuffer buffer = new MessageBuffer();

        public CloverUsbDevice()
        {
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Removed += DeviceWatcher_Removed;
            deviceWatcher.Start();
        }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<ErrorEventArgs> Error;
        public event EventHandler<MessageEventArgs> Message;

        public UsbDevice Device { get; private set; }

        public UsbIdentity Identity { get; private set; }


        public void Write(string message)
        {
            lock (Lock)
            {
                try
                {
                    var stringLength = Encoding.UTF8.GetByteCount(message);
                    if (stringLength > REMOTE_STRING_LENGTH_MAX)
                    {
                        throw new IOException($"String byte length {stringLength} bytes exceeds maximum {REMOTE_STRING_LENGTH_MAX} bytes");
                    }

                    var messageLength = 4 + 4 + stringLength;
                    var messageBytes = new byte[messageLength];

                    Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(unchecked((int)REMOTE_STRING_MAGIC_START_TOKEN))), 0, messageBytes, 0, 4);
                    Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(stringLength)), 0, messageBytes, 4, 4);
                    Array.Copy(Encoding.UTF8.GetBytes(message), 0, messageBytes, 8, stringLength);

                    int sent = 0;
                    while (sent < messageLength)
                    {
                        var packetLength = Math.Min(MAX_PACKET_BYTES, messageLength - sent);
                        var packet = new byte[2 + packetLength];
                        Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(packetLength))), 0, packet, 0, 2);
                        Array.Copy(messageBytes, sent, packet, 2, packetLength);
                        if (writer.Device?.IsOpen != true)
                        {
                            throw new IOException($"The device is not connected. Unable to send message: {message}");
                        }
                        var ecWrite = writer.Write(packet, 5000, out int bytesWritten);
                        if (ecWrite != ErrorCode.None)
                        {
                            throw new IOException($"ErrorCode: {ecWrite} The Clover transport layer can see the USB device, but encountered an error when attempting to send it a message.  Try physically disconnecting/reconnecting the Clover device.");
                        }

                        sent += packetLength;
                    }
                }
                catch (Exception ex)
                {
                    Error.Invoke(this, new ErrorEventArgs { Error = ex });
                }
            }
        }

        public void Dispose()
        {
            lock (Lock)
            {
                CloseUsbDevice();

                try
                {
                    deviceWatcher?.Dispose();
                }
                catch { }

                try
                {
                    buffer?.Dispose();
                }
                catch { }
            }
        }

        private void CloseUsbDevice()
        {
            lock (Lock)
            {
                try
                {
                    if (reader != null)
                    {
                        reader.DataReceivedEnabled = false;
                        if (!reader.IsDisposed)
                        {
                            reader?.Abort();
                            reader?.Flush();
                            reader?.Dispose();
                        }
                    }
                }
                catch { }

                try
                {
                    if (writer != null && !writer.IsDisposed)
                    {
                        writer?.Abort();
                        writer?.Flush();
                        writer?.Dispose();
                    }
                }
                catch { }

                if (Device != null)
                {
                    try
                    {
                        Device?.Close();
                    }
                    catch { }

                    try
                    {
                        (Device as IDisposable)?.Dispose();
                    }
                    catch { }

                    Device = null;
                    Disconnected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void OpenUsbDevice(UsbRegistry registry)
        {
            lock (Lock)
            {
                CloseUsbDevice();

                if (registry != null)
                {
                    if (registry.Open(out UsbDevice device))
                    {
                        Device = device;
                        Identity = UsbIdentity.AllIdentities.FirstOrDefault(id => id.Vid == device.UsbRegistryInfo.Vid && id.Pid == device.UsbRegistryInfo.Pid);

                        if (Identity?.Type == UsbIdentityType.Merchant)
                        {
                            EnableCustomerAccessoryMode();
                            CloseUsbDevice();
                        }

                        if (Identity?.Type == UsbIdentityType.Customer)
                        {
                            // If this is a "whole" usb device (libusb-win32, linux libusb-1.0)
                            // it exposes an IUsbDevice interface. If not (WinUSB) the 
                            // 'wholeUsbDevice' variable will be null indicating this is 
                            // an interface of a device; it does not require or support 
                            // configuration and interface selection.
                            if (Device is IUsbDevice whole)
                            {
                                // This is a "whole" USB device. Before it can be used, 
                                // the desired configuration and interface must be selected.
                                whole.SetConfiguration(1);
                                whole.ClaimInterface(0);
                            }

                            if (Device.TryExtractEndpointPair(out ReadEndpointID readId, out WriteEndpointID writeId))
                            {
                                reader = Device.OpenEndpointReader(readId);
                                writer = Device.OpenEndpointWriter(writeId);

                                reader.DataReceived += Reader_DataReceived;
                                reader.DataReceivedEnabled = true;

                                Connected?.Invoke(this, EventArgs.Empty);
                            }
                        }
                    }
                    else
                    {
                        throw new IOException("Found the device, but can't open it. Some other process(service or application) may already have it open.");
                    }
                }
            }
        }

        private void EnableCustomerAccessoryMode()
        {
            var steps = new List<Func<bool>>
            {
                () => Device != null,
                () => Device.CheckProtocol(ACCESSORY_GET_PROTOCOL),
                () => Device.SendControlMessage(ACCESSORY_SEND_STRING, ACCESSORY_STRING_MANUFACTURER, "Clover"),
                () => Device.SendControlMessage(ACCESSORY_SEND_STRING, ACCESSORY_STRING_MODEL, "Adapter"),
                () => Device.SendControlMessage(ACCESSORY_SEND_STRING, ACCESSORY_STRING_DESCRIPTION, "Windows POS Device"),
                () => Device.SendControlMessage(ACCESSORY_SEND_STRING, ACCESSORY_STRING_VERSION, "0.01"),
                () => Device.SendControlMessage(ACCESSORY_SEND_STRING, ACCESSORY_STRING_URI, "market://details?id=com.clover.remote.protocol.usb"),
                () => Device.SendControlMessage(ACCESSORY_SEND_STRING, ACCESSORY_STRING_SERIAL, "C415000"),
                () => Device.SendControlMessage(ACCESSORY_START, 0, null),
            };
            steps.All(step => step());
        }

        private void Reader_DataReceived(object sender, EndpointDataEventArgs e)
        {
            try
            {
                using (var stream = new MemoryStream(e.Buffer, 0, e.Count))
                using (var reader = new BinaryReader(stream))
                {
                    int size = buffer.New ? IPAddress.NetworkToHostOrder(reader.ReadInt16()) : e.Count;
                    if (size < 0)
                    {
                        throw new IOException($"Invalid size: ${size}");
                    }


                    if (buffer.New)
                    {
                        uint token = (uint)IPAddress.NetworkToHostOrder(reader.ReadInt32());
                        if (token != REMOTE_STRING_MAGIC_START_TOKEN)
                        {
                            throw new IOException($"Unexpected start token: {token}");
                        }

                        int targetLength = IPAddress.NetworkToHostOrder(reader.ReadInt32());
                        if (targetLength > REMOTE_STRING_LENGTH_MAX)
                        {
                            throw new IOException($"Illegal string length: {targetLength} bytes");
                        }

                        buffer.Init(targetLength);
                    }

                    var bytes = reader.ReadBytes(size);
                    buffer.Stream.Write(bytes, 0, bytes.Length);

                    if (buffer.Complete)
                    {
                        Message?.Invoke(this, new MessageEventArgs { Message = buffer.Message });
                        buffer.Reset();
                    }
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, new ErrorEventArgs { Error = ex });
                buffer.Reset();
            }
        }

        private void DeviceWatcher_Added(object sender, UsbRegistryEventArgs e)
        {
            lock (Lock)
            {
                var usb = e.UsbRegistry;
                var identity = UsbIdentity.AllIdentities.FirstOrDefault(id => id.Vid == usb.Vid && id.Pid == usb.Pid);
                var interfaceId = usb.GetInterfaceId();
                if (identity != null && usb.GetInterfaceId() == 0)
                {
                    if (Device is null)
                    {
                        OpenUsbDevice(usb);
                    }
                    else if (identity == Identity)
                    {
                        Error?.Invoke(this, new ErrorEventArgs { Error = new IOException($"Warning: Connected device attempting to reconnect. Current={Identity?.Vid:X}:{Identity?.Pid:X}") });
                    }
                    else
                    {
                        Error?.Invoke(this, new ErrorEventArgs { Error = new IOException($"Warning: Additional device attempting to connect. Current={Identity?.Vid:X}:{Identity?.Pid:X}, Additional={identity?.Vid:X}:{identity?.Pid:X}") });
                    }
                }
            }
        }

        private void DeviceWatcher_Removed(object sender, UsbRegistryEventArgs e)
        {
            lock (Lock)
            {
                var usb = e.UsbRegistry;
                var identity = UsbIdentity.AllIdentities.FirstOrDefault(id => id.Vid == usb.Vid && id.Pid == usb.Pid);
                if (identity != null && usb.GetInterfaceId() == 0)
                {
                    if (Identity == identity)
                    {
                        CloseUsbDevice();
                    }
                    else
                    {
                        Error?.Invoke(this, new ErrorEventArgs { Error = new IOException($"Info: Additional device disconnecting. Current={Identity?.Vid:X}:{Identity?.Pid:X}, Additional={identity?.Vid:X}:{identity?.Pid:X}") });
                    }
                }
            }
        }
    }
}
