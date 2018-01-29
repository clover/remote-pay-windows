// Copyright (C) 2018 Clover Network, Inc.
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
