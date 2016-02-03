namespace CloverExamplePOS
{
    partial class VaultedCardListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.VaultedCardsListView = new System.Windows.Forms.ListView();
            this.CardName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.First6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Last4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Exp_Month = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Exp_Year = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Token = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.vaultedCardListPanel = new System.Windows.Forms.Panel();
            this.TitleTextBox = new System.Windows.Forms.Label();
            this.vaultedCardListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK_Button
            // 
            this.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OK_Button.Location = new System.Drawing.Point(1138, 395);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(129, 57);
            this.OK_Button.TabIndex = 1;
            this.OK_Button.Text = "OK";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel_Button.Location = new System.Drawing.Point(1017, 395);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(115, 57);
            this.Cancel_Button.TabIndex = 2;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // VaultedCardsListView
            // 
            this.VaultedCardsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VaultedCardsListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.VaultedCardsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CardName,
            this.First6,
            this.Last4,
            this.Exp_Month,
            this.Exp_Year,
            this.Token});
            this.VaultedCardsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.VaultedCardsListView.HideSelection = false;
            this.VaultedCardsListView.Location = new System.Drawing.Point(0, 32);
            this.VaultedCardsListView.Margin = new System.Windows.Forms.Padding(6);
            this.VaultedCardsListView.MultiSelect = false;
            this.VaultedCardsListView.Name = "VaultedCardsListView";
            this.VaultedCardsListView.Size = new System.Drawing.Size(1270, 354);
            this.VaultedCardsListView.TabIndex = 38;
            this.VaultedCardsListView.UseCompatibleStateImageBehavior = false;
            this.VaultedCardsListView.View = System.Windows.Forms.View.Details;
            this.VaultedCardsListView.SelectedIndexChanged += new System.EventHandler(this.VaultedCardsListView_SelectedIndexChanged);
            // 
            // CardName
            // 
            this.CardName.Text = "Name";
            this.CardName.Width = 135;
            // 
            // First6
            // 
            this.First6.Text = "First6";
            this.First6.Width = 84;
            // 
            // Last4
            // 
            this.Last4.Text = "Last4";
            // 
            // Exp_Month
            // 
            this.Exp_Month.Text = "Exp. Month";
            // 
            // Exp_Year
            // 
            this.Exp_Year.Text = "Exp. Year";
            // 
            // Token
            // 
            this.Token.Text = "Token";
            this.Token.Width = 147;
            // 
            // vaultedCardListPanel
            // 
            this.vaultedCardListPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.vaultedCardListPanel.BackColor = System.Drawing.Color.White;
            this.vaultedCardListPanel.Controls.Add(this.TitleTextBox);
            this.vaultedCardListPanel.Controls.Add(this.VaultedCardsListView);
            this.vaultedCardListPanel.Controls.Add(this.Cancel_Button);
            this.vaultedCardListPanel.Controls.Add(this.OK_Button);
            this.vaultedCardListPanel.Location = new System.Drawing.Point(12, 130);
            this.vaultedCardListPanel.Name = "vaultedCardListPanel";
            this.vaultedCardListPanel.Size = new System.Drawing.Size(1270, 455);
            this.vaultedCardListPanel.TabIndex = 39;
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.BackColor = System.Drawing.Color.SeaGreen;
            this.TitleTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleTextBox.ForeColor = System.Drawing.Color.White;
            this.TitleTextBox.Location = new System.Drawing.Point(0, 0);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(1270, 36);
            this.TitleTextBox.TabIndex = 10;
            this.TitleTextBox.Text = "Select Vaulted Card";
            this.TitleTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VaultedCardListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 629);
            this.ControlBox = false;
            this.Controls.Add(this.vaultedCardListPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VaultedCardListForm";
            this.Load += new System.EventHandler(this.VaultedCardListForm_Load);
            this.vaultedCardListPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button OK_Button;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.ListView VaultedCardsListView;
        private System.Windows.Forms.ColumnHeader CardName;
        private System.Windows.Forms.ColumnHeader First6;
        private System.Windows.Forms.ColumnHeader Last4;
        private System.Windows.Forms.ColumnHeader Exp_Month;
        private System.Windows.Forms.ColumnHeader Exp_Year;
        private System.Windows.Forms.ColumnHeader Token;
        private System.Windows.Forms.Panel vaultedCardListPanel;
        private System.Windows.Forms.Label TitleTextBox;
    }
}