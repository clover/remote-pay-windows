using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.clover.sdk.v3.payments
{
    public class RegionalExtras
    {
        //
        // Argentina EXTRAs keys
        //

        /// <summary>
        /// Set the invoice number or to RegionalExtras.SKIP_FISCAL_INVOICE_NUMBER_SCREEN_VALUE to skip the invoice number screen during the payment flow
        /// </summary>
        public const string FISCAL_INVOICE_NUMBER_KEY = "com.clover.regionalextras.ar.FISCAL_INVOICE_NUMBER_KEY";

        /// <summary>
        /// Set to the number of installments to use and skip the installment selection screen during the payment flow
        /// </summary>
        public const string INSTALLMENT_NUMBER_KEY = "com.clover.regionalextras.ar.INSTALLMENT_NUMBER_KEY";

        /// <summary>
        /// Set to the installment plan identifier
        /// </summary>
        public const string INSTALLMENT_PLAN_KEY = "com.clover.regionalextras.ar.INSTALLMENT_PLAN_KEY";

        //
        // Argentina EXTRAs values
        //

        /// <summary>
        /// Value for FISCAL_INVOICE_NUMBER_KEY to skip showing the invoice screen without using an invoice number (same as SKIP button behavior in the device UI)
        /// </summary>
        public const string SKIP_FISCAL_INVOICE_NUMBER_SCREEN_VALUE = "com.clover.regionalextras.ar.SKIP_FISCAL_INVOICE_NUMBER_SCREEN_VALUE";

        /// <summary>
        /// Default value for INSTALLMENT_NUMBER_KEY to skip the screen but use the device's default number of installments. (i.e. 1 installment, 100% paid immediately instead of payments over time.)
        /// </summary>
        public const string INSTALLMENT_NUMBER_DEFAULT_VALUE = "1";
    }
}