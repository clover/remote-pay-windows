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
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public partial class PreAuthListForm : OverlayForm
    {
        public POSPayment SelectedPayment { get; set; }
        public List<POSPayment> PreAuths { get; set; }

        public PreAuthListForm()
        {
            InitializeComponent();
        }

        public PreAuthListForm(Form formToCover) : base(formToCover)
        {
            InitializeComponent();
        }

        private void PreAuthListForm_Load(object sender, EventArgs e)
        {
            foreach (POSPayment preauth in PreAuths)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = preauth;
                item.SubItems.Add(new ListViewItem.ListViewSubItem());

                item.SubItems[0].Text = "PRE-AUTH";
                item.SubItems[1].Text = (preauth.Amount / 100).ToString("C2");

                PreAuthsListView.Items.Add(item);
            }
            UpdateUi();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            if (PreAuthsListView.SelectedItems.Count == 1)
            {
                SelectedPayment = PreAuthsListView.SelectedItems[0].Tag as POSPayment;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            SelectedPayment = null;
            Close();
        }

        private void PreAuthsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUi();
        }

        private void UpdateUi()
        {
            OkBtn.Enabled = PreAuthsListView.SelectedItems.Count > 0;
        }
    }
}
