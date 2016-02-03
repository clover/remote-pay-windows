namespace CloverExamplePOS
{
    partial class PreAuthListForm
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
            this.vaultedCardListPanel = new System.Windows.Forms.Panel();
            this.TitleTextBox = new System.Windows.Forms.Label();
            this.PreAuthsListView = new System.Windows.Forms.ListView();
            this.PADate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PAAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.OK_Button = new System.Windows.Forms.Button();
            this.vaultedCardListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // vaultedCardListPanel
            // 
            this.vaultedCardListPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.vaultedCardListPanel.BackColor = System.Drawing.Color.White;
            this.vaultedCardListPanel.Controls.Add(this.TitleTextBox);
            this.vaultedCardListPanel.Controls.Add(this.PreAuthsListView);
            this.vaultedCardListPanel.Controls.Add(this.Cancel_Button);
            this.vaultedCardListPanel.Controls.Add(this.OK_Button);
            this.vaultedCardListPanel.Location = new System.Drawing.Point(118, 70);
            this.vaultedCardListPanel.Margin = new System.Windows.Forms.Padding(2);
            this.vaultedCardListPanel.Name = "vaultedCardListPanel";
            this.vaultedCardListPanel.Size = new System.Drawing.Size(475, 302);
            this.vaultedCardListPanel.TabIndex = 40;
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.BackColor = System.Drawing.Color.SeaGreen;
            this.TitleTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleTextBox.ForeColor = System.Drawing.Color.White;
            this.TitleTextBox.Location = new System.Drawing.Point(0, 0);
            this.TitleTextBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(475, 19);
            this.TitleTextBox.TabIndex = 10;
            this.TitleTextBox.Text = "Select Pre Auth";
            this.TitleTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PreAuthsListView
            // 
            this.PreAuthsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PreAuthsListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PreAuthsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PADate,
            this.PAAmount});
            this.PreAuthsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.PreAuthsListView.HideSelection = false;
            this.PreAuthsListView.Location = new System.Drawing.Point(104, 22);
            this.PreAuthsListView.MultiSelect = false;
            this.PreAuthsListView.Name = "PreAuthsListView";
            this.PreAuthsListView.Size = new System.Drawing.Size(285, 237);
            this.PreAuthsListView.TabIndex = 38;
            this.PreAuthsListView.UseCompatibleStateImageBehavior = false;
            this.PreAuthsListView.View = System.Windows.Forms.View.Details;
            // 
            // PADate
            // 
            this.PADate.Text = "Date";
            this.PADate.Width = 175;
            // 
            // PAAmount
            // 
            this.PAAmount.Text = "Amount";
            this.PAAmount.Width = 84;
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel_Button.Location = new System.Drawing.Point(341, 264);
            this.Cancel_Button.Margin = new System.Windows.Forms.Padding(2);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(58, 30);
            this.Cancel_Button.TabIndex = 2;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OK_Button.Location = new System.Drawing.Point(403, 264);
            this.OK_Button.Margin = new System.Windows.Forms.Padding(2);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(64, 30);
            this.OK_Button.TabIndex = 1;
            this.OK_Button.Text = "OK";
            this.OK_Button.UseVisualStyleBackColor = true;
            this.OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // PreAuthListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 456);
            this.Controls.Add(this.vaultedCardListPanel);
            this.Name = "PreAuthListForm";
            this.Text = "PreAuthListForm";
            this.Load += new System.EventHandler(this.PreAuthListForm_Load);
            this.vaultedCardListPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel vaultedCardListPanel;
        private System.Windows.Forms.Label TitleTextBox;
        private System.Windows.Forms.ListView PreAuthsListView;
        private System.Windows.Forms.ColumnHeader PADate;
        private System.Windows.Forms.ColumnHeader PAAmount;
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.Button OK_Button;
    }
}