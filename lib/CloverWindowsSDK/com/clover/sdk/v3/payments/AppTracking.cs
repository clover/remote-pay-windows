using System.Collections.Generic;
using com.clover.sdk.v3.base_;

namespace com.clover.sdk.v3.payments
{
    // Used to track the origin of a distributed call.
    public class AppTracking
    {
        /// <summary>
        /// The uuid from the developer application.  This is typically populated and used only on the back end.
        /// </summary>
        public string developerAppId { get; set; }
        /// <summary>
        /// The name of the developer application.
        /// </summary>
        public string applicationName { get; set; }
        /// <summary>
        /// A string representing an application
        /// </summary>
        public string applicationID { get; set; }
        /// <summary>
        /// A string representing a semanticversion.  See http://semver.org/
        /// </summary>
        public string applicationVersion { get; set; }
        /// <summary>
        /// A string representing a SDK
        /// </summary>
        public string sourceSDK { get; set; }
        /// <summary>
        /// A string representing a semanticversion.  See http://semver.org/
        /// </summary>
        public string sourceSDKVersion { get; set; }
        /// <summary>
        /// The payment with which this app tracking info is associated
        /// </summary>
        public Reference paymentRef { get; set; }
        /// <summary>
        /// The credit with which this app tracking info is associated
        /// </summary>
        public Reference creditRef { get; set; }
        /// <summary>
        /// The refund with which this app tracking info is associated
        /// </summary>
        public Reference refundRef { get; set; }
        /// <summary>
        /// The credit refund with which this app tracking info is associated
        /// </summary>
        public Reference creditRefundRef { get; set; }

    }
}
