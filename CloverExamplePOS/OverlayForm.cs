using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    public class OverlayForm : Form
    {
        private Form tocover = null;
        public OverlayForm()
        {

        }
        public OverlayForm(Form formToCover) : base()
        {
            this.tocover = formToCover;
            this.Owner = formToCover;
            this.Shown += new System.EventHandler(FormShown);
            WindowState = FormWindowState.Normal;
            StartPosition = FormStartPosition.Manual;
            this.Visible = false;
        }

        private void FormShown(object sender, EventArgs e)
        {
            this.BackColor = Color.DarkGray;
            this.Opacity = 0.98;
            this.FormBorderStyle = FormBorderStyle.None;
            this.AutoScaleMode = AutoScaleMode.None;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;

            this.Location = tocover.PointToScreen(Point.Empty);
            this.ClientSize = tocover.ClientSize;
            tocover.LocationChanged += ParentLocationChanged;
            tocover.ClientSizeChanged += ParentSizeChanged;
            //this.Show(tocover);
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int value = 1;
                DwmSetWindowAttribute(tocover.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
            }
            this.Visible = true;
        }

        private void ParentLocationChanged(object sender, EventArgs e)
        {
            this.Location = tocover.PointToScreen(Point.Empty);
        }
        private void ParentSizeChanged(object sender, EventArgs e)
        {
            this.ClientSize = tocover.ClientSize;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Restore owner
            this.Owner.LocationChanged -= ParentLocationChanged;
            this.Owner.ClientSizeChanged -= ParentSizeChanged;
            if (!this.Owner.IsDisposed && Environment.OSVersion.Version.Major >= 6)
            {
                int value = 1;
                DwmSetWindowAttribute(this.Owner.Handle, DWMWA_TRANSITIONS_FORCEDISABLED, ref value, 4);
            }
            base.OnFormClosing(e);
            this.Dispose();
        }

        private const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int value, int attrLen);

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OverlayForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "OverlayForm";
            this.Load += new System.EventHandler(this.OverlayForm_Load);
            this.ResumeLayout(false);

        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {

        }
    }
}
