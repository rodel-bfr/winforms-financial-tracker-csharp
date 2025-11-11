using System;
using System.Windows.Forms;
using System.Globalization; // Add this using statement
using System.Threading;     // Add this using statement

namespace client_app
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // --- Add these three lines to set the culture to Italian (Italy) ---
            CultureInfo ci = new CultureInfo("it-IT");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            // -------------------------------------------------------------------

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
