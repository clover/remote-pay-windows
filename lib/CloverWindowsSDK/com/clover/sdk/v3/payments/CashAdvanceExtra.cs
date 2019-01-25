using com.clover.sdk.v3.base_;

namespace com.clover.sdk.v3.payments
{
    public class CashAdvanceExtra
    {
        // record

        public string cashAdvanceSerialNum { get; set; }
        public CashAdvanceCustomerIdentification cashAdvanceCustomerIdentification { get; set; }
        /// <summary>
        /// The payment with which this cash advance extra is associated
        /// </summary>
        public Reference paymentRef { get; set; }
    }
}
