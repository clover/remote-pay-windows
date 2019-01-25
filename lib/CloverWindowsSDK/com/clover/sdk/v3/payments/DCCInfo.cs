using com.clover.sdk.v3.base_;

namespace com.clover.sdk.v3.payments
{
    public class DCCInfo
    {
        /// <summary>
        /// Inquiry Rate ID (IPG)
        /// </summary>
        public long inquiryRateId { get; set; }
        /// <summary>
        /// Flag indicating whether DCC was applied on this txn
        /// </summary>
        public bool dccApplied { get; set; }
        /// <summary>
        /// Foreign currency code
        /// </summary>
        public string foreignCurrencyCode { get; set; }
        /// <summary>
        /// Foreign (transaction) amount
        /// </summary>
        public long foreignAmount { get; set; }
        /// <summary>
        /// Exchange Rate
        /// </summary>
        public double exchangeRate { get; set; }
        /// <summary>
        /// Margin Rate Percentage
        /// </summary>
        public string marginRatePercentage { get; set; }
        /// <summary>
        /// Exchange Rate Source Name
        /// </summary>
        public string exchangeRateSourceName { get; set; }
        /// <summary>
        /// Exchange Rate Source Timestamp
        /// </summary>
        public string ExchangeRateSourceTimeStamp { get; set; }
        /// <summary>
        /// The payment with which this DCC info is associated
        /// </summary>
        public Reference paymentRef { get; set; }
        /// <summary>
        /// The credit (manual refund) with which this DCC info is associated
        /// </summary>
        public Reference creditRef { get; set; }

    }
}
