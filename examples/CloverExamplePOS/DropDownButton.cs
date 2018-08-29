using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloverExamplePOS
{

    public class DropDownButton : Button
    {
        public new List<EventHandler> Click { get; set; }

        public DropDownButton()
        {
            Click = new List<EventHandler>();
            init();
        }

        private void init()
        {
            Padding = new Padding(Padding.Left, Padding.Top, Padding.Right + 15, Padding.Bottom);
            base.Click += Clicked;
        }

        private void Clicked(object sender, EventArgs e)
        {
            if (e is MouseEventArgs)
            {
                if (((MouseEventArgs)e).X < this.Size.Width - 15)
                {
                    foreach (EventHandler ch in Click)
                    {
                        if (ch != null)
                        {
                            ch(sender, e);
                        }
                    }
                }
                else
                {
                    //ContextMenu.Show(this, new Point(this.Location.X-this.Size.Width, this.Location.Y+this.Size.Height));
                    ContextMenu.Show(this, new Point(0, this.Size.Height - Margin.Bottom));
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // draw box

            Pen borderPen = new Pen(Color.FromArgb(16, 0, 0, 0)); // TODO: needs to be text color
            int ddAreaX = this.Size.Width - 15;
            int ddAreaHeight = this.Size.Height - Padding.Top - Padding.Bottom - 4; // 3 px space on top and bottom
            pevent.Graphics.DrawLine(borderPen, new Point(ddAreaX, 3), new Point(ddAreaX, ddAreaHeight));

            // draw triangle
            int triangleCenterX = ddAreaX + 7;
            int triangleCenterY = this.Size.Height / 2;

            Point[] triangle = new Point[4];
            triangle[0] = new Point(triangleCenterX - 3, triangleCenterY - 2);
            triangle[1] = new Point(triangleCenterX + 3, triangleCenterY - 2);
            triangle[2] = new Point(triangleCenterX, triangleCenterY + 2);
            triangle[3] = triangle[0];
            Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0)); // TODO: needs to be text color
            pevent.Graphics.FillPolygon(brush, triangle);
        }
    }
}
