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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using com.clover.sdk.v3.payments;

namespace CloverExamplePOS
{
    class CustomerCardCaptureForm : OverlayForm
    {
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox Last4TextBox;
        private System.Windows.Forms.TextBox First6TextBox;
        private System.Windows.Forms.TextBox CustomerNameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label TitleTextBox;
        private ComboBox MonthComboBox;
        private ComboBox YearComboBox;
        private System.Windows.Forms.Label label1;

        public Store Store { get; private set; }
        public Payment Payment { get; private set; }
        public POSCard Card { get; private set; }

        public CustomerCardCaptureForm (Form toCover) : base(toCover)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.MonthComboBox = new System.Windows.Forms.ComboBox();
            this.YearComboBox = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.TitleTextBox = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.Last4TextBox = new System.Windows.Forms.TextBox();
            this.First6TextBox = new System.Windows.Forms.TextBox();
            this.CustomerNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.MonthComboBox);
            this.panel1.Controls.Add(this.YearComboBox);
            this.panel1.Controls.Add(this.cancelButton);
            this.panel1.Controls.Add(this.TitleTextBox);
            this.panel1.Controls.Add(this.okButton);
            this.panel1.Controls.Add(this.Last4TextBox);
            this.panel1.Controls.Add(this.First6TextBox);
            this.panel1.Controls.Add(this.CustomerNameTextBox);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(66, 40);
            this.panel1.MinimumSize = new System.Drawing.Size(100, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(311, 221);
            this.panel1.TabIndex = 0;
            // 
            // MonthComboBox
            // 
            this.MonthComboBox.FormattingEnabled = true;
            this.MonthComboBox.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.MonthComboBox.Location = new System.Drawing.Point(111, 140);
            this.MonthComboBox.Name = "MonthComboBox";
            this.MonthComboBox.Size = new System.Drawing.Size(83, 21);
            this.MonthComboBox.TabIndex = 12;
            // 
            // YearComboBox
            // 
            this.YearComboBox.FormattingEnabled = true;
            this.YearComboBox.Location = new System.Drawing.Point(200, 141);
            this.YearComboBox.Name = "YearComboBox";
            this.YearComboBox.Size = new System.Drawing.Size(95, 21);
            this.YearComboBox.TabIndex = 11;
            // 
            // cancelButton
            // 
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(139, 185);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.BackColor = System.Drawing.Color.SeaGreen;
            this.TitleTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleTextBox.ForeColor = System.Drawing.Color.White;
            this.TitleTextBox.Location = new System.Drawing.Point(0, 0);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(311, 28);
            this.TitleTextBox.TabIndex = 9;
            this.TitleTextBox.Text = "Save Card";
            this.TitleTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // okButton
            // 
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.okButton.Location = new System.Drawing.Point(220, 185);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // Last4TextBox
            // 
            this.Last4TextBox.Location = new System.Drawing.Point(111, 108);
            this.Last4TextBox.Margin = new System.Windows.Forms.Padding(10, 10, 10, 3);
            this.Last4TextBox.Name = "Last4TextBox";
            this.Last4TextBox.Size = new System.Drawing.Size(184, 20);
            this.Last4TextBox.TabIndex = 6;
            // 
            // First6TextBox
            // 
            this.First6TextBox.Location = new System.Drawing.Point(111, 75);
            this.First6TextBox.Margin = new System.Windows.Forms.Padding(10, 10, 10, 3);
            this.First6TextBox.Name = "First6TextBox";
            this.First6TextBox.Size = new System.Drawing.Size(184, 20);
            this.First6TextBox.TabIndex = 5;
            // 
            // CustomerNameTextBox
            // 
            this.CustomerNameTextBox.Location = new System.Drawing.Point(111, 42);
            this.CustomerNameTextBox.Margin = new System.Windows.Forms.Padding(10, 10, 10, 3);
            this.CustomerNameTextBox.Name = "CustomerNameTextBox";
            this.CustomerNameTextBox.Size = new System.Drawing.Size(184, 20);
            this.CustomerNameTextBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 144);
            this.label4.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Expiration";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Last 4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "First 6";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Customer Name";
            // 
            // CustomerCardCaptureForm
            // 
            this.ClientSize = new System.Drawing.Size(446, 324);
            this.Controls.Add(this.panel1);
            this.Name = "CustomerCardCaptureForm";
            this.Load += new System.EventHandler(this.CustomerCardCaptureForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        internal void setPayment(Payment payment)
        {
            this.Payment = payment;
        }

        internal void setStore(Store store)
        {
            this.Store = store;
        }

        internal POSCard GetCard()
        {
            throw new NotImplementedException();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // need to save this information
            Card = new POSCard();
            Card.Name = CustomerNameTextBox.Text;
            Card.First6 = First6TextBox.Text;
            Card.Last4 = Last4TextBox.Text;
            Card.Month = MonthComboBox.SelectedItem.ToString();
            Card.Year = YearComboBox.SelectedItem.ToString();
            Card.Token = Payment.cardTransaction.token;

            Store.Cards.Add(Card);

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CustomerCardCaptureForm_Load(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            
            for(int i=0; i<10; i++)
            {
                this.YearComboBox.Items.Add(start.ToString("yy"));
                start = start.AddYears(1);
            }
            this.YearComboBox.SelectedItem = this.YearComboBox.Items[0];
            this.MonthComboBox.SelectedItem = this.MonthComboBox.Items[0];
        }
    }
}
