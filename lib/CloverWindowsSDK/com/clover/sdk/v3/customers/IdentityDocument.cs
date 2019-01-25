namespace com.clover.sdk.v3.customers
{
    public class IdentityDocument
    {
        /// <summary>
        /// Type of personal identification: National Document, Passport, etc
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The identification number
        /// </summary>
        public string Number { get; set; }
    }
}
