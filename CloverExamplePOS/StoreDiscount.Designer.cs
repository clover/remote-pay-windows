namespace CloverExamplePOS
{
    partial class StoreDiscount
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
            this.DiscountButton = new System.Windows.Forms.Button();
            this.DiscountPrice = new System.Windows.Forms.Label();
            this.DiscountNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DiscountButton
            // 
            this.DiscountButton.AutoEllipsis = true;
            this.DiscountButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DiscountButton.Location = new System.Drawing.Point(0, 5);
            this.DiscountButton.Name = "DiscountButton";
            this.DiscountButton.Size = new System.Drawing.Size(130, 40);
            this.DiscountButton.TabIndex = 0;
            this.DiscountButton.Text = "Discount Name";
            this.DiscountButton.UseVisualStyleBackColor = true;
            // 
            // DiscountPrice
            // 
            this.DiscountPrice.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.DiscountPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DiscountPrice.ForeColor = System.Drawing.Color.White;
            this.DiscountPrice.Location = new System.Drawing.Point(8, 44);
            this.DiscountPrice.Name = "DiscountPrice";
            this.DiscountPrice.Size = new System.Drawing.Size(122, 18);
            this.DiscountPrice.TabIndex = 1;
            this.DiscountPrice.Text = "Discount";
            this.DiscountPrice.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // DiscountNumber
            // 
            this.DiscountNumber.BackColor = System.Drawing.Color.LightGray;
            this.DiscountNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DiscountNumber.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DiscountNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DiscountNumber.ForeColor = System.Drawing.Color.DimGray;
            this.DiscountNumber.Location = new System.Drawing.Point(0, 44);
            this.DiscountNumber.Name = "DiscountNumber";
            this.DiscountNumber.Size = new System.Drawing.Size(21, 18);
            this.DiscountNumber.TabIndex = 2;
            this.DiscountNumber.Text = "99";
            this.DiscountNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StoreDiscount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DiscountNumber);
            this.Controls.Add(this.DiscountPrice);
            this.Controls.Add(this.DiscountButton);
            this.Name = "StoreDiscount";
            this.Size = new System.Drawing.Size(133, 66);
            this.Load += new System.EventHandler(this.StoreDiscount_Load);
            this.ParentChanged += new System.EventHandler(this.DiscountNumber_ParentChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DiscountButton;
        private System.Windows.Forms.Label DiscountPrice;
        private System.Windows.Forms.Label DiscountNumber;
    }
}
