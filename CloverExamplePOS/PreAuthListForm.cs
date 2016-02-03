using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using com.clover.remotepay.sdk;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public partial class PreAuthListForm : OverlayForm
    {
        public POSPayment selectedPayment { get; set; }
        public List<POSPayment> preAuths { get; set; }

        public PreAuthListForm()
        {
            InitializeComponent();
        }

        public PreAuthListForm(Form formToCover) : base(formToCover)
        {
            InitializeComponent();
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            if(PreAuthsListView.SelectedItems.Count == 1)
            {
                selectedPayment = (POSPayment)PreAuthsListView.SelectedItems[0].Tag;
            }
            this.Close();
            this.Dispose();
        }

        private void PreAuthListForm_Load(object sender, EventArgs e)
        {
            foreach(POSPayment preAuth in preAuths)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = preAuth;
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem());

                lvi.SubItems[0].Text = "PRE-AUTH";
                lvi.SubItems[1].Text = (preAuth.Amount / 100).ToString("C2");

                PreAuthsListView.Items.Add(lvi);
            }
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            selectedPayment = null;
            this.Close();
            this.Dispose();
        }
    }
}
