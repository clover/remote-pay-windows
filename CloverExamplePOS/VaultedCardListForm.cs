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
    public partial class VaultedCardListForm : OverlayForm
    {
        private POSCard Card = null;
        private ListView cardsListView;
        public enum VaultedCardAction { PAY, AUTH};
        private VaultedCardAction CardAction; 
        public VaultedCardListForm(Form toCover) : base(toCover) 
        {
            InitializeComponent();
            OK_Button.Enabled = false;
            this.VaultedCardsListView.FullRowSelect = true;
            this.Text = "Vaulted Card List";
        }

        public void setCardsListView(ListView tempCardsListView)
        {
            cardsListView = tempCardsListView;
            foreach (ListViewItem item in cardsListView.Items)
            {
                this.VaultedCardsListView.Items.Add((ListViewItem)item.Clone());
            }
        }
        public void setCardAction(VaultedCardAction action)
        {
            CardAction = action;
        }

        public VaultedCardAction getCardAction()
        {
            return CardAction;
        }

        public POSCard getCard()
        {
            return Card;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            Card = (POSCard)this.VaultedCardsListView.SelectedItems[0].Tag;
            this.Close();
            this.Dispose();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            Card = null;
            this.Close();
            this.Dispose();
        }

        private void VaultedCardsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            OK_Button.Enabled = this.VaultedCardsListView.SelectedItems.Count == 1;
        }

        private void VaultedCardListForm_Load(object sender, EventArgs e)
        {

        }
    }
}
