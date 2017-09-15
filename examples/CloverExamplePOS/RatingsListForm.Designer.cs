using BrightIdeasSoftware;

namespace CloverExamplePOS
{
    partial class RatingsListForm
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
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.ratingsListPanel = new System.Windows.Forms.Panel();
            this.TitleTextBox = new System.Windows.Forms.Label();
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.Category = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Question = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Rating_Value = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ratingsListPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel_Button.Location = new System.Drawing.Point(916, 407);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(141, 47);
            this.Cancel_Button.TabIndex = 2;
            this.Cancel_Button.Text = "Cancel";
            this.Cancel_Button.UseVisualStyleBackColor = true;
            this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // ratingsListPanel
            // 
            this.ratingsListPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ratingsListPanel.BackColor = System.Drawing.Color.White;
            this.ratingsListPanel.Controls.Add(this.TitleTextBox);
            this.ratingsListPanel.Controls.Add(this.objectListView1);
            this.ratingsListPanel.Controls.Add(this.Cancel_Button);
            this.ratingsListPanel.Location = new System.Drawing.Point(12, 169);
            this.ratingsListPanel.Name = "ratingsListPanel";
            this.ratingsListPanel.Size = new System.Drawing.Size(1270, 491);
            this.ratingsListPanel.TabIndex = 39;
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
            this.TitleTextBox.Text = "Customer Ratings";
            this.TitleTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // objectListView1
            // 
 //           this.objectListView1.AllColumns.Add(this.Category);
            this.objectListView1.AllColumns.Add(this.Question);
            this.objectListView1.AllColumns.Add(this.Rating_Value);
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            //this.Category,
            this.Question,
            this.Rating_Value});
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.objectListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.objectListView1.Location = new System.Drawing.Point(3, 32);
            this.objectListView1.Margin = new System.Windows.Forms.Padding(6);
            this.objectListView1.TabIndex = 11;
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.Size = new System.Drawing.Size(1261, 330);
            this.objectListView1.TabIndex = 40;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // Category
            // 
            this.Category.AspectName = "id";
            this.Category.IsEditable = false;
            this.Category.Text = "Category";
            this.Category.Width = 186;
            // 
            // Question
            // 
            this.Question.AspectName = "question";
            this.Question.IsEditable = false;
            this.Question.Text = "Question";
            this.Question.Width = 216;
            // 
            // Rating_Value
            // 
            // C#
            // Gets a reference to the same assembly that 
            // contains the type that is creating the ResourceManager.
            System.Reflection.Assembly myAssembly;
            myAssembly = this.GetType().Assembly;

            // Creates the ResourceManager.
            System.Resources.ResourceManager myManager = new
               System.Resources.ResourceManager("CloverExamplePOS.RatingsListForm",
               myAssembly);

            // Retrieves String and Image resources.
            System.Drawing.Image star;
            star = (System.Drawing.Image)myManager.GetObject("star16");
            this.objectListView1.SmallImageList = new System.Windows.Forms.ImageList();
            this.objectListView1.SmallImageList.Images.Add(star);
            this.Rating_Value.AspectName = "value";
            this.Rating_Value.Renderer = new MultiImageRenderer(0, 5, 0, 6);
            this.Rating_Value.IsEditable = false;
            this.Rating_Value.Text = "Rating";
            this.Rating_Value.Width = 268;
            // 
            // RatingsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1294, 707);
            this.ControlBox = false;
            this.Controls.Add(this.ratingsListPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RatingsListForm";
            this.Load += new System.EventHandler(this.RatingsListForm_Load);
            this.ratingsListPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Cancel_Button;
        private System.Windows.Forms.Panel ratingsListPanel;
        private System.Windows.Forms.Label TitleTextBox;
        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn Category;
        private BrightIdeasSoftware.OLVColumn Question;
        private BrightIdeasSoftware.OLVColumn Rating_Value;

    }
}