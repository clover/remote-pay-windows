using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public partial class StoreDiscount : UserControl
    {
        private POSDiscount _discount = new POSDiscount();
        public POSDiscount Discount {
            get
            {
                return _discount;
            }
            set
            {
                _discount = value;
                DiscountButton.Text = _discount.Name;
            }
        }

        public StoreDiscount()
        {
            InitializeComponent();
        }

        private void StoreDiscount_Load(object sender, EventArgs e)
        {

        }

        public static StoreDiscount operator +(StoreDiscount StoreDiscount, EventHandler handler)
        {
            StoreDiscount.DiscountButton.Click += handler;
            StoreDiscount.DiscountPrice.Click += handler;
            StoreDiscount.DiscountNumber.Click += handler;
            return StoreDiscount;
        }

        private void DiscountNumber_ControlAdded(object sender, ControlEventArgs e)
        {
            int index = Parent.Controls.IndexOf(this);
            DiscountNumber.Text = index.ToString();
        }

        private void DiscountNumber_ParentChanged(object sender, EventArgs e)
        {
            if(Parent != null)
            {
                int index = Parent.Controls.IndexOf(this);
                DiscountNumber.Text = (index+1).ToString();
            }
        }
    }
}
