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
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.vaultedCardListPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // vaultedCardListPanel
            // 
            this.vaultedCardListPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.vaultedCardListPanel.BackColor = System.Drawing.Color.White;
            this.vaultedCardListPanel.Controls.Add(this.TitleTextBox);
            this.vaultedCardListPanel.Controls.Add(this.PreAuthsListView);
            this.vaultedCardListPanel.Controls.Add(this.CancelBtn);
            this.vaultedCardListPanel.Controls.Add(this.OkBtn);
            this.vaultedCardListPanel.Location = new System.Drawing.Point(157, 86);
            this.vaultedCardListPanel.Name = "vaultedCardListPanel";
            this.vaultedCardListPanel.Size = new System.Drawing.Size(633, 372);
            this.vaultedCardListPanel.TabIndex = 40;
            // 
            // TitleTextBox
            // 
            this.TitleTextBox.BackColor = System.Drawing.Color.SeaGreen;
            this.TitleTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleTextBox.ForeColor = System.Drawing.Color.White;
            this.TitleTextBox.Location = new System.Drawing.Point(0, 0);
            this.TitleTextBox.Name = "TitleTextBox";
            this.TitleTextBox.Size = new System.Drawing.Size(633, 24);
            this.TitleTextBox.TabIndex = 10;
            this.TitleTextBox.Text = "Choose PreAuth For Payment";
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
            this.PreAuthsListView.FullRowSelect = true;
            this.PreAuthsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.PreAuthsListView.HideSelection = false;
            this.PreAuthsListView.Location = new System.Drawing.Point(3, 27);
            this.PreAuthsListView.Margin = new System.Windows.Forms.Padding(4);
            this.PreAuthsListView.MultiSelect = false;
            this.PreAuthsListView.Name = "PreAuthsListView";
            this.PreAuthsListView.Size = new System.Drawing.Size(627, 254);
            this.PreAuthsListView.TabIndex = 38;
            this.PreAuthsListView.UseCompatibleStateImageBehavior = false;
            this.PreAuthsListView.View = System.Windows.Forms.View.Details;
            this.PreAuthsListView.SelectedIndexChanged += new System.EventHandler(this.PreAuthsListView_SelectedIndexChanged);
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
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CancelBtn.Location = new System.Drawing.Point(517, 315);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(99, 29);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OkBtn.Location = new System.Drawing.Point(396, 315);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(107, 29);
            this.OkBtn.TabIndex = 1;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // PreAuthListForm
            // 
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(977, 561);
            this.Controls.Add(this.vaultedCardListPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkBtn;
    }
}