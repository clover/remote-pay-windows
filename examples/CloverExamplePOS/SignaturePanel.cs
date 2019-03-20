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

using com.clover.remotepay.transport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public partial class SignaturePanel : Control
    {
        private Signature2 sig;
        public Signature2 Signature
        {
            get
            {
                return sig;
            }
            set
            {
                sig = value;
                if (value != null)
                {
                    SetBounds(0, 0, (int)(sig.width * scale), (int)(sig.height * scale));
                }
            }
        }
        float scale = 0.5f;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Signature != null)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // draw box
                Pen borderPen = new Pen(Color.FromArgb(255, 128, 128, 128));
                borderPen.Width = 1;
                Rectangle border = new Rectangle(new Point(0, 0), new Size((int)(Signature.width * scale), (int)(Signature.height * scale)));

                // draw X
                int xWidth = (int)(Signature.width * .1 * .6 * scale);
                int xHeight = (int)(xWidth * 1.5);
                e.Graphics.DrawLine(borderPen, new Point((int)(Signature.width * scale * .1 - xWidth), (int)(Signature.height * .6 * scale - xHeight)), new Point((int)(Signature.width * scale * .01) + xWidth, (int)(Signature.height * .6 * scale)));
                e.Graphics.DrawLine(borderPen, new Point((int)(Signature.width * scale * .1 - xWidth) + xWidth, (int)(Signature.height * .6 * scale - xHeight)), new Point((int)(Signature.width * scale * .01), (int)(Signature.height * .6 * scale)));
                // draw signature line
                e.Graphics.DrawLine(borderPen, new Point((int)(Signature.width * .1 * scale), (int)(Signature.height * .6 * scale)), new Point((int)(Signature.width * .9 * scale), (int)(Signature.height * .6 * scale)));

                Pen pen = new Pen(Color.FromArgb(255, 0, 0, 205));
                pen.Width = 2;

                foreach (Signature2.Stroke stroke in Signature.strokes)
                {
                    if (stroke.points.Count == 1)
                    {
                        Signature2.Point dot = stroke.points[0];
                        Rectangle rect = new Rectangle(new Point(dot.x, dot.y), new Size(2, 2));

                        e.Graphics.DrawEllipse(pen, rect);
                    }
                    else if (stroke.points.Count > 1)
                    {
                        for (int i = 1; i < stroke.points.Count; i++)
                        {
                            Signature2.Point strokePoint1 = stroke.points[i - 1];
                            Signature2.Point strokePoint2 = stroke.points[i];

                            Point pt1 = new Point();
                            pt1.X = (int)(strokePoint1.x * scale);
                            pt1.Y = (int)(strokePoint1.y * scale);
                            Point pt2 = new Point();
                            pt2.X = (int)(strokePoint2.x * scale);
                            pt2.Y = (int)(strokePoint2.y * scale);

                            e.Graphics.DrawLine(pen, pt1, pt2);
                        }
                    }
                }
            }
            else
            {
                // If there's no electronic signature object, this means signature is on the receipt
                Rectangle rect = new Rectangle(ClientRectangle.Location, ClientRectangle.Size);
                rect.Inflate(-5, -5);
                e.Graphics.DrawString("Verify signature on paper", Font, SystemBrushes.WindowText, rect, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });
            }
        }
    }
}
