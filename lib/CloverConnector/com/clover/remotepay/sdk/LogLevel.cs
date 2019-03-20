using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.clover.remotepay.sdk
{
    /// <summary>
    /// CloverConnector Log Level convenience constants
    /// </summary>
    public static class LogLevel
    {
        /// <summary>
        /// Minimal logging, only the most critical messages will be logged
        /// </summary>
        public const int MINIMAL = 1_000;

        /// <summary>
        /// Moderate logging, basic flow information without logging excessive details
        /// </summary>
        public const int MODERATE = 3_000;

        /// <summary>
        /// Detailed logging, flow of information and relevant details
        /// </summary>
        public const int DETAILED = 6_000;

        /// <summary>
        /// Debug level logging of everything
        /// </summary>
        public const int DEBUG = 10_000;
    }
}
