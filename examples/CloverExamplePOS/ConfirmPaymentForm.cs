// Copyright (C) 2016 Clover Network, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using com.clover.remotepay.sdk;
using com.clover.remotepay.transport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public partial class ConfirmPaymentForm : OverlayForm
    {
        private Form formToCover = null;
        private Challenge challenge = null;
        private bool lastChallenge = false;
        private string title = "";

        public ConfirmPaymentForm(Form formToCover, Challenge challenge, bool lastChallenge) : base(formToCover)
        {
            this.formToCover = formToCover;
            this.challenge = challenge;
            this.lastChallenge = lastChallenge;
            InitializeComponent();
        }

        private void ConfirmPaymentForm_Load(object sender, EventArgs e)
        {

        }

        public String Title
        {
            get
            {
                //return Text;
                return TitleTextBox.Text;
            }
            set
            {
                //Text = value;
                TitleTextBox.Text = value;
            }
        }

        public string Label
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }


        public DialogResult Status
        {
            get; internal set;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (lastChallenge)
            {
                Status = DialogResult.OK; // set to OK when accepting the last challenge
            } else                        // this is used to trigger the PaymentConfirmedMessage
            {
                Status = DialogResult.Yes; // set to Yes when accepting challenges preceeding
            }                              // the last challenge
            this.Close();
        }
        private void RejectButton_Click(object sender, EventArgs e)
        {
            Status = DialogResult.No;   // Used to trigger the PaymentRejectedMessage
            this.Close();
        }
    }
}
