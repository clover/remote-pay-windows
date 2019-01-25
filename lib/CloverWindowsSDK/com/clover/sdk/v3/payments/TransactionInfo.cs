using com.clover.sdk.v3.base_;
using com.clover.sdk.v3.customers;

namespace com.clover.sdk.v3.payments
{
    public class TransactionInfo
    {
        /// <summary>
        /// 2 character language used for the transaction.  Deprecated in factor of transactionLocale.
        /// </summary>
        public string languageIndicator { get; set; }
        /// <summary>
        /// Locale for the transaction (e.g. en-CA)
        /// </summary>
        public string transactionLocale { get; set; }
        public AccountType accountSelection { get; set; }
        /// <summary>
        /// The payment with which this extra is associated
        /// </summary>
        public Reference paymentRef { get; set; }
        /// <summary>
        /// The credit with which this extra is associated
        /// </summary>
        public Reference creditRef { get; set; }
        /// <summary>
        /// The refund with which this extra is associated
        /// </summary>
        public Reference refundRef { get; set; }
        /// <summary>
        /// The credit refund with which this extra is associated
        /// </summary>
        public Reference creditRefundRef { get; set; }
        /// <summary>
        /// Consists of 4 digits prefix + 8 digits
        /// </summary>
        public string fiscalInvoiceNumber { get; set; }
        /// <summary>
        /// AR Installments: number of installments
        /// </summary>
        public int installmentsQuantity { get; set; }
        /// <summary>
        /// AR Installments: plan alphanum code
        /// </summary>
        public string installmentsPlanCode { get; set; }
        /// <summary>
        /// AR Installments: selected plan id
        /// </summary>
        public string installmentsPlanId { get; set; }
        /// <summary>
        /// AR Installments: selected plan desc
        /// </summary>
        public string installmentsPlanDesc { get; set; }
        /// <summary>
        /// Card type label
        /// </summary>
        public string cardTypeLabel { get; set; }
        /// <summary>
        /// STAN(System Audit Trace Number)
        /// </summary>
        public int stan { get; set; }
        /// <summary>
        /// Customers identification number and type
        /// </summary>
        public IdentityDocument identityDocument { get; set; }
        /// <summary>
        /// Transaction Batch Number
        /// </summary>
        public string batchNumber { get; set; }
        /// <summary>
        /// Transaction Receipt Number
        /// </summary>
        public string receiptNumber { get; set; }
        /// <summary>
        /// STAN for reversal
        /// </summary>
        public int reversalStan { get; set; }
        /// <summary>
        /// MAC for reversal
        /// </summary>
        public string reversalMac { get; set; }
        /// <summary>
        /// MAC KSN for reversal
        /// </summary>
        public string reversalMacKsn { get; set; }
        /// <summary>
        /// Designates the unique location of a terminal at a merchant
        /// </summary>
        public string terminalIdentification { get; set; }
        /// <summary>
        /// When concatenated with the Acquirer Identifier, uniquely identifies a given merchant
        /// </summary>
        public string merchantIdentifier { get; set; }
        /// <summary>
        /// Indicates the name and location of the merchant
        /// </summary>
        public string merchantNameLocation { get; set; }
        /// <summary>
        /// Masked track2 data
        /// </summary>
        public string maskedTrack2 { get; set; }
        /// <summary>
        /// Extra data for receipt
        /// </summary>
        public string receiptExtraData { get; set; }
        /// <summary>
        /// Defines the Financial Service selected for the transaction
        /// </summary>
        public SelectedService selectedService { get; set; }
        /// <summary>
        /// Result of the transaction
        /// </summary>
        public TransactionResult transactionResult { get; set; }
        /// <summary>
        /// Contains a hex string with needed TLV tags for certification
        /// </summary>
        public string transactionTags { get; set; }
        /// <summary>
        /// Contains the information how the data inside transactionTags should be coded - initially we cause default and nexo as formats
        /// </summary>
        public TxFormat txFormat { get; set; }
        /// <summary>
        /// Contains the information how the PAN should masked.
        /// </summary>
        public string panMask { get; set; }
        /// <summary>
        /// Counter maintained by the terminal that is incremented for each transaction at the beginning of the Perform Service function.
        /// </summary>
        public string transactionSequenceCounter { get; set; }
        /// <summary>
        /// Identifies and differentiates cards with the same PAN.
        /// </summary>
        public string applicationPanSequenceNumber { get; set; }
        /// <summary>
        /// Contains the reason why the transaction should be reversed in the host. It has to be mapped in server with the expected value by the corresponding gateway
        /// </summary>
        public ReversalReason reversalReason { get; set; }
        /// <summary>
        /// Boolean to determine if the transaction done using a vaulted card is a token based transaction
        /// </summary>
        public bool isTokenBasedTx { get; set; }
        /// <summary>
        /// For reversal and capture transactions, this contains the reference (transactionSequenceCounter) to the originating transaction.
        /// </summary>
        public string origTransactionSequenceCounter { get; set; }
        /// <summary>
        /// This field is populated when the TSC of a terminal is out of sync and is provided with an update.
        /// </summary>
        public string transactionSequenceCounterUpdate { get; set; }
    }
}
