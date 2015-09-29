namespace CloverExamplePOS
{
    partial class StoreItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ItemButton = new System.Windows.Forms.Button();
            this.ItemPrice = new System.Windows.Forms.Label();
            this.ItemNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ItemButton
            // 
            this.ItemButton.AutoEllipsis = true;
            this.ItemButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ItemButton.Location = new System.Drawing.Point(0, 5);
            this.ItemButton.Name = "ItemButton";
            this.ItemButton.Size = new System.Drawing.Size(130, 40);
            this.ItemButton.TabIndex = 0;
            this.ItemButton.Text = "Item Name";
            this.ItemButton.UseVisualStyleBackColor = true;
            // 
            // ItemPrice
            // 
            this.ItemPrice.BackColor = System.Drawing.Color.SlateGray;
            this.ItemPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemPrice.ForeColor = System.Drawing.Color.White;
            this.ItemPrice.Location = new System.Drawing.Point(8, 44);
            this.ItemPrice.Name = "ItemPrice";
            this.ItemPrice.Size = new System.Drawing.Size(122, 18);
            this.ItemPrice.TabIndex = 1;
            this.ItemPrice.Text = "$";
            this.ItemPrice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ItemNumber
            // 
            this.ItemNumber.BackColor = System.Drawing.Color.LightGray;
            this.ItemNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ItemNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ItemNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ItemNumber.ForeColor = System.Drawing.Color.DimGray;
            this.ItemNumber.Location = new System.Drawing.Point(0, 44);
            this.ItemNumber.Name = "ItemNumber";
            this.ItemNumber.Size = new System.Drawing.Size(21, 18);
            this.ItemNumber.TabIndex = 2;
            this.ItemNumber.Text = "99";
            this.ItemNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StoreItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ItemNumber);
            this.Controls.Add(this.ItemPrice);
            this.Controls.Add(this.ItemButton);
            this.Name = "StoreItem";
            this.Size = new System.Drawing.Size(133, 66);
            this.Load += new System.EventHandler(this.StoreItem_Load);
            this.ParentChanged += new System.EventHandler(this.ItemNumber_ParentChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ItemButton;
        private System.Windows.Forms.Label ItemPrice;
        private System.Windows.Forms.Label ItemNumber;
    }
}
