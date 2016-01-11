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
    public partial class AlertForm : OverlayForm
    {
        public AlertForm(Form formToCover) : base(formToCover)
        {
            InitializeComponent();
        }

        private void AlertForm_Load(object sender, EventArgs e)
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

        private void OkButton_Click(object sender, EventArgs e)
        {
            Status = DialogResult.OK;
            this.Close();
        }
    }
}
