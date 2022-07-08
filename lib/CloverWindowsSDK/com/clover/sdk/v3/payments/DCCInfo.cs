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
        public string exchangeRateSourceTimeStamp { get; set; }
        /// <summary>
        /// The payment with which this DCC info is associated
        /// </summary>
        public Reference paymentRef { get; set; }
        /// <summary>
        /// The credit (manual refund) with which this DCC info is associated
        /// </summary>
        public Reference creditRef { get; set; }
        /// <summary>
        /// Flag indicating whether DCC was offered on this txn
        /// </summary>
        public bool dccEligible { get; set; }
        /// <summary>
        /// Exchange rate from the rate request
        /// </summary>
        public string exchangeRateId { get; set; }
        /// <summary>
        /// Rate request id from the rate request
        /// </summary>
        public string rateRequestId { get; set; }
        /// <summary>
        /// Amount sent for exchange in rate request
        /// </summary>
        public long baseAmount { get; set; }
        /// <summary>
        /// Alpha currency code for foreign currency
        /// </summary>
        public string baseCurrencyCode { get; set; }

    }
}
