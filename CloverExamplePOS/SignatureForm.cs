using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using com.clover.remotepay.sdk;

namespace CloverExamplePOS
{
    public partial class SignatureForm : OverlayForm
    {
        private SignatureVerifyRequest signatureVerifyRequest;
        public SignatureVerifyRequest SignatureVerifyRequest {
            get {
                return signatureVerifyRequest;
            }
            set {
                signatureVerifyRequest = value;
                signaturePanel1.Signature = signatureVerifyRequest.Signature;
            }
        }
        public SignatureForm(Form toCover) : base(toCover)
        {
            InitializeComponent();
        }

        private void SignatureForm_Load(object sender, EventArgs e)
        {
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            SignatureVerifyRequest.Accept();
        }

        private void RejectButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            SignatureVerifyRequest.Reject();
        }
    }
}
