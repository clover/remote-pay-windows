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

namespace CloverExamplePOS.UIDialogs
{
    public partial class RefundAmountDialog : Form
    {
        public bool FullRefund { get => FullRefundCheck.Checked; set => FullRefundCheck.Checked = value; }
        public long Amount { get { return GetAmount(); } set { CachedAmount = value; SetAmount(value); } }

        private long CachedAmount = 0;

        public RefundAmountDialog()
        {
            InitializeComponent();
        }

        private void FullRefundCheck_CheckedChanged(object sender, EventArgs e)
        {
            RefundAmountEdit.Enabled = !FullRefundCheck.Checked;

            if (RefundAmountEdit.Enabled)
            {
                SetAmount(CachedAmount);
            }
            else
            {
                CachedAmount = GetAmount();
                RefundAmountEdit.Text = "full refund";
            }
        }

        private void SetAmount(long amount)
        {
            if (RefundAmountEdit.Enabled)
            {
                RefundAmountEdit.Text = (amount / 100.0).ToString("C2");
            }
        }

        private long GetAmount()
        {
            long amount = CachedAmount;
            if (RefundAmountEdit.Enabled)
            {
                if (long.TryParse(Regex.Replace(RefundAmountEdit.Text, "[^0-9]", ""), out long temp))
                {
                    amount = temp;
                }
            }
            return amount;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            if (FullRefundCheck.Checked)
            {
                Amount = 0;
            }
            else
            {
                Amount = GetAmount();
            }

            Close();
        }
    }
}
