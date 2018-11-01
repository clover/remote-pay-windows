// Copyright (C) 2018 Clover Network, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
//
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Linq;
using System.Windows.Forms;
using CloverExamplePOS.CustomActivity;


namespace CloverExamplePOS
{
    public partial class RatingsListForm : OverlayForm
    {
        private Rating[] Ratings = null;
        public RatingsListForm(Form toCover) : base(toCover) 
        {
            InitializeComponent();
            this.Text = "Customer Rating List";
        }

        public void setRatings(Rating[] ratings)
        {
            objectListView1.SetObjects(ratings);
            Ratings = ratings;
        }

        public Rating getRating(int index)
        {
            return Ratings.ElementAt(index);
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }


        private void RatingsListForm_Load(object sender, EventArgs e)
        {

        }
    }
}
