using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public partial class StoreItem : UserControl
    {
        POSItem _item = new POSItem("abc", "Sample Item", 1099);
        public POSItem Item {
            set {
                _item = value;
                ItemPrice.Text = (_item.Price/100.0).ToString("C2");
                ItemButton.Text = _item.Name;
            }
            get
            {
                return _item;
            }
        }
        public StoreItem()
        {
            InitializeComponent();
        }

        private void StoreItem_Load(object sender, EventArgs e)
        {

        }

        public static StoreItem operator +(StoreItem StoreItem, EventHandler handler)
        {
            StoreItem.ItemButton.Click += handler;
            StoreItem.ItemPrice.Click += handler;
            StoreItem.ItemNumber.Click += handler;
            return StoreItem;
        }

        private void ItemNumber_ControlAdded(object sender, ControlEventArgs e)
        {
            int index = Parent.Controls.IndexOf(this);
            ItemNumber.Text = index.ToString();
        }

        private void ItemNumber_ParentChanged(object sender, EventArgs e)
        {
            if(Parent != null)
            {
                int index = Parent.Controls.IndexOf(this);
                ItemNumber.Text = (index+1).ToString();
            }
        }
    }
}
