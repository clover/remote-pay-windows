using System;
using System.IO;
using System.Text;

namespace com.clover.remotepay.transport.usb
{
    internal class MessageBuffer : IDisposable
    {
        public int TargetLength { get; protected set; }
        public MemoryStream Stream { get; protected set; }
        public bool New => (this.Stream == null);
        public bool Complete => this.Stream?.Position >= this.TargetLength;
        public string Message => Encoding.UTF8.GetString(this.Stream.ToArray());

        public void Reset()
        {
            this.TargetLength = 0;
            this.Stream?.Dispose();
            this.Stream = null;
        }

        public void Init(int targetLength)
        {
            Reset();
            this.TargetLength = targetLength;
            this.Stream = new MemoryStream();
        }

        public void Dispose()
        {
            Reset();
        }
    }
}
