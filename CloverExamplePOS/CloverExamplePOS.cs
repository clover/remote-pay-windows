using System;
using System.Windows.Forms;

namespace CloverExamplePOS
{
    static class CloverExamplePOS
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CloverExamplePOSForm());
        }
    }
}
