using System.Collections.Generic;
using com.clover.sdk.v3.base_;

namespace com.clover.sdk.v3.payments
{
    public class SignatureDisclaimer
    {
        public string disclaimerText { get; set; }

        /// <summary>
        /// Values that will be substituted in standard disclaimer text (txn date/time, account number, product label, etc.
        /// </summary>
        public Dictionary<string, string> disclaimerValues { get; set; }
        /// <summary>
        /// The payment with which this Signature disclaimer is associated
        /// </summary>
        public Reference paymentRef { get; set; }
    }
}
