namespace com.clover.sdk.v3.payments
{
    public class CashAdvanceCustomerIdentification
    {
        public IdType idType { get; set; }
        /// <summary>
        /// Identification serial number
        /// </summary>
        public string serialNumber { get; set; }
        /// <summary>
        /// Masked identification serial number
        /// </summary>
        public string maskedSerialNumber { get; set; }
        /// <summary>
        /// Encrypted identification serial number
        /// </summary>
        public string encryptedSerialNumber { get; set; }
        /// <summary>
        /// Expiration date in format MMDDYYYY
        /// </summary>
        public string expirationDate { get; set; }
        /// <summary>
        /// State in which identification was issued
        /// </summary>
        public string issuingState { get; set; }
        /// <summary>
        /// Country in which identification was issued
        /// </summary>
        public string issuingCountry { get; set; }
        /// <summary>
        /// Full customer name
        /// </summary>
        public string customerName { get; set; }
        public string addressStreet1 { get; set; }
        public string addressStreet2 { get; set; }
        public string addressCity { get; set; }
        public string addressState { get; set; }
        public string addressZipCode { get; set; }
        public string addressCountry { get; set; }
    }
}
