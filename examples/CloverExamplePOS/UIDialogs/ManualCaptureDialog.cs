using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;

namespace CloverExamplePOS.UIDialogs
{
    public partial class ManualCaptureDialog : Form
    {
        private CloverExamplePOSForm POSForm { get; set; }
        public string PaymentID { get; set; }
        public long Amount { get; set; }
        public long TipAmount { get; set; }

        public ManualCaptureDialog(CloverExamplePOSForm form)
        {
            InitializeComponent();
            POSForm = form;
            POSForm.PaymentRetrieved += POSForm_PaymentRetrieved;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PaymentIdEdit.Text))
            {
                MessageBox.Show("Payment ID is required.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!long.TryParse(AmountEdit.Text, out long amount))
            {
                MessageBox.Show("Amount must be numbers", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!long.TryParse(TipAmountEdit.Text, out long tipAmount))
            {
                MessageBox.Show("Tip Amount must be numbers", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PaymentID = PaymentIdEdit.Text;
            Amount = amount;
            TipAmount = tipAmount;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ExternalPaymentIdCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ExternalPaymentIdCheckbox.Checked)
            {
                ExternalPaymentIdEdit.Enabled = true;
                LookupButton.Enabled = true;
                PaymentIdEdit.Enabled = false;
                PaymentIdEdit.Text = "";
            } else
            {
                ExternalPaymentIdEdit.Enabled = false;
                LookupButton.Enabled = false;
                PaymentIdEdit.Enabled = true;
                ExternalPaymentIdEdit.Text = "";
            }
        }

        private void LookupButton_Click(object sender, EventArgs e)
        {
            POSForm.Connector.RetrievePayment(new RetrievePaymentRequest { externalPaymentId = ExternalPaymentIdEdit.Text });
        }

        private void POSForm_PaymentRetrieved(object sender, ResponseEventArgs<RetrievePaymentResponse> e)
        {
            if (ExternalPaymentIdCheckbox.Checked && e.Response.Success)
            {
                Invoke(new Action(() =>
                {
                    PaymentIdEdit.Text = e.Response.Payment?.id;
                }));
            }
        }

        private void ManualCaptureDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            POSForm.PaymentRetrieved -= POSForm_PaymentRetrieved;
        }
    }
}
