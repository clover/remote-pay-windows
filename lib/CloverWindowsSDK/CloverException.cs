using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.sdk.v3
{
    public class CloverException : Exception
    {
        /// <summary>
        /// Optional Clover error code when it exists
        /// Refer to Clover Inc. documentation such as github.com/clover/remote-pay-windows/wiki for error code details
        /// </summary>
        public string Code { get; set; } = "";

        public CloverException() { }
        public CloverException(string message) : base(message) { }
        public CloverException(string message, Exception innerException) : base(message, innerException) { }
        public CloverException(string message, string code) : this(message) { Code = code; }
        public CloverException(string message, string code, Exception innerException) : this(message, innerException) { Code = code; }
    }
}
