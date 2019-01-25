using com.clover.sdk.v3.base_;

namespace com.clover.sdk.v3.payments
{
    public class GermanInfo
    {
        public string cardTrack2 { get; set; }
        public string cardSequenceNumber { get; set; }
        public string transactionCaseGermany { get; set; }
        public string transactionTypeGermany { get; set; }
        public string terminalID { get; set; }
        public string traceNumber { get; set; }
        public string oldTraceNumber { get; set; }
        public string receiptNumber { get; set; }
        public string transactionAID { get; set; }
        public string transactionMSApp { get; set; }
        public string transactionScriptResults { get; set; }
        public string receiptType { get; set; }
        public string customerTransactionDOLValues { get; set; }
        public string merchantTransactionDOLValues { get; set; }
        public string merchantJournalDOL { get; set; }
        public string merchantJournalDOLValues { get; set; }
        public string configMerchantId { get; set; }
        public string configProductLabel { get; set; }
        public string hostResponseAidParBMP53 { get; set; }
        public string hostResponsePrintDataBM60 { get; set; }
        public string sepaElvReceiptFormat { get; set; }
        public string sepaElvExtAppLabel { get; set; }
        public string sepaElvPreNotification { get; set; }
        public string sepaElvMandate { get; set; }
        public string sepaElvCreditorId { get; set; }
        public string sepaElvMandateId { get; set; }
        public string sepaElvIban { get; set; }
        /// <summary>
        /// The payment with which this German info is associated
        /// </summary>
        public Reference paymentRef { get; set; }
        /// <summary>
        /// The credit with which this German info is associated
        /// </summary>
        public Reference creditRef { get; set; }
        /// <summary>
        /// The refund with which this German info is associated
        /// </summary>
        public Reference refundRef { get; set; }
        /// <summary>
        /// The credit refund with which this German info is associated
        /// </summary>
        public Reference creditRefundRef { get; set; }
    }
}
