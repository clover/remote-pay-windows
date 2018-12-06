using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.clover.sdk.v3.payments;
using TipMode = com.clover.remotepay.sdk.TipMode;
using CloverConnector = com.clover.remotepay.sdk.CloverConnector;

namespace CloverExamplePOS.UiPages
{
    public partial class TransactionSettingsPage : UserControl
    {

        public Dictionary<string, string> RegionalExtras { get { return RegionalExtrasListFromTable(); } }

        DataTable RegionalExtrasTable;


        public TransactionSettingsPage()
        {
            InitializeComponent();

            // Setup RegionalExtrasTable
            RegionalExtrasTable = new DataTable();
            RegionalExtrasTable.Columns.Add("Name");
            RegionalExtrasTable.Columns.Add("Value");
            RegionalExtraParametersEditGrid.DataSource = RegionalExtrasTable;
            RegionalExtraParametersEditGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private Dictionary<string, string> RegionalExtrasListFromTable()
        {
            Dictionary<string, string> extras = new Dictionary<string, string>();

            foreach (DataRow row in RegionalExtrasTable.Rows)
            {
                string name = row["Name"] as string ?? "";
                string value = row["Value"] as string ?? "";

                if (!string.IsNullOrWhiteSpace(name))
                {
                    extras[name] = value;
                }
            }

            return extras;
        }

        // Amounts & Values

        public bool HasTipAmount => tipAmount.TextLength > 0;
        public long? TipAmount => tipAmount.GetAmount();

        public bool HasSignatureThreshold => signatureThreshold.TextLength > 0;
        public long? SignatureThreshold => signatureThreshold.GetAmount();

        public TipMode? TipMode => tipModeDefault.Checked ? (TipMode?)null : tipModeProvided.Checked ? com.clover.remotepay.sdk.TipMode.TIP_PROVIDED : tipModeOnScreen.Checked ? com.clover.remotepay.sdk.TipMode.ON_SCREEN_BEFORE_PAYMENT : tipModeNone.Checked ? com.clover.remotepay.sdk.TipMode.NO_TIP : (TipMode?)null;

        public bool HasSignatureEntryLocation => !signatureDefault.Checked;
        public DataEntryLocation? SignatureEntryLocation => signatureNone.Checked ? DataEntryLocation.NONE : signatureOnPaper.Checked ? DataEntryLocation.ON_PAPER : signatureOnScreen.Checked ? DataEntryLocation.ON_SCREEN : (DataEntryLocation?)null;

        // Boolean settings
        // TODO: Convert these Transaction Settings Overrides to default/yes/no instead of just true/false - was historically treated as "don't override default / override with 'yes'" across SDK sample apps.

        private bool HasAutomaticPaymentConfirmation => automaticPaymentConfirmationCB.Checked;
        public bool? AutomaticPaymentConfirmation => HasAutomaticPaymentConfirmation ? automaticPaymentConfirmationCB.Checked : (bool?)null;

        private bool HasAutomaticSignatureConfirmation => automaticSignatureConfirmationCB.Checked;
        public bool? AutomaticSignatureConfirmation => HasAutomaticSignatureConfirmation ? automaticSignatureConfirmationCB.Checked : (bool?)null;

        private bool HasDisableDuplicateChecking => disableDuplicateCheckingCB.Checked;
        public bool? DisableDuplicateChecking => HasDisableDuplicateChecking ? disableDuplicateCheckingCB.Checked : (bool?)null;

        private bool HasDisableReceiptSelection => disableReceiptOptionsCB.Checked;
        public bool? DisableReceiptSelection => HasDisableReceiptSelection ? disableReceiptOptionsCB.Checked : (bool?)null;

        private bool HasDisablePrinting => disablePrintingCB.Checked;
        public bool? DisablePrinting => HasDisablePrinting ? disablePrintingCB.Checked : (bool?)null;

        private bool HasDisableRestartTransactionOnFail => DisableRestartTransactionOnFailure.Checked;
        public bool? DisableRestartTransactionOnFail => HasDisableRestartTransactionOnFail ? DisableRestartTransactionOnFailure.Checked : (bool?)null;

        public bool HasDisableCashback => DisableCashBack.Checked;
        public bool? DisableCashback => HasDisableCashback ? DisableCashBack.Checked : (bool?)null;

        // Default / Yes-true / No-false settings: if default checked return null, else return yes checked; note: ignore no and assume it's checked if default & yes aren't.

        public bool? ForceOfflinePayment => forceOfflineDefault.Checked ? (bool?)null : forceOfflineYes.Checked;
        public bool? ApproveOfflinePaymentWithoutPrompt => approveOfflineDefault.Checked ? (bool?)null : approveOfflineYes.Checked;
        public bool? AllowOfflinePayment => offlineDefault.Checked ? (bool?)null : offlineYes.Checked;

        // Card Entry Methods
        public int CardEntryMethod
        {
            get
            {
                int value = 0;

                if(ManualEntryCheckbox.Checked)
                {
                    value |= CloverConnector.CARD_ENTRY_METHOD_MANUAL;
                }
                if(MagStripeCheckbox.Checked)
                {
                    value |= CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE;
                }
                if(ChipCheckbox.Checked)
                {
                    value |= CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT;
                }
                if(ContactlessCheckbox.Checked)
                {
                    value |= CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS;
                }

                return value;
            }
            set
            {
                ManualEntryCheckbox.Checked = (value & CloverConnector.CARD_ENTRY_METHOD_MANUAL) == CloverConnector.CARD_ENTRY_METHOD_MANUAL;
                MagStripeCheckbox.Checked = (value & CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE) == CloverConnector.CARD_ENTRY_METHOD_MAG_STRIPE;
                ChipCheckbox.Checked = (value & CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT) == CloverConnector.CARD_ENTRY_METHOD_ICC_CONTACT;
                ContactlessCheckbox.Checked = (value & CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS) == CloverConnector.CARD_ENTRY_METHOD_NFC_CONTACTLESS;
            }
        }

        /// <summary>
        /// Card Not Present valid only for Manual Card Entry
        /// </summary>
        public bool CardNotPresent => ManualEntryCheckbox.Checked && CardNotPresentCheckbox.Checked;

    }
}
