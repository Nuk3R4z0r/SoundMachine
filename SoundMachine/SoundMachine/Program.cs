using System;
using Microsoft.Win32;
using System.Windows.Forms;

namespace SoundMachine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                RegistryKey WorkingDirectory = Registry.CurrentUser.CreateSubKey("SoundMachine");
                if (WorkingDirectory.GetValue("WorkingDirectory") == null || WorkingDirectory.GetValue("WorkingDirectory").ToString() == "")
                    WorkingDirectory.SetValue("WorkingDirectory", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SoundMachine\\");

                Config.WorkingDir = WorkingDirectory.GetValue("WorkingDirectory").ToString();
                Config.LoadConfig(10);
                Application.Run(new Form1());
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
