using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloverExamplePOS.UIDialogs
{
    public partial class PropertiesDialog : Form
    {
        public string Content { get => richTextBox1.Text; set => richTextBox1.Text = value; }

        public PropertiesDialog()
        {
            InitializeComponent();
        }
    }
}
