using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SoundMachine
{
    public partial class SettingsForm : Form
    {
        private bool ChangedMaxSounds;

        public SettingsForm()
        {
            InitializeComponent();
            interruptInputBox.Checked = Config._currentConfig.InterruptKeys;
            numSoundsBox.Value = Config._currentConfig.MaxSounds;
            ChangedMaxSounds = false;
        }

        //Button event to load output devices
        private void BtnLoadDevices(object o, EventArgs e)
        {
            Form1._currentForm.LoadDevices();
        }

        private void interruptInputBox_CheckedChanged(object sender, EventArgs e)
        {
            if (interruptInputBox.Checked)
                Config._currentConfig.InterruptKeys = true;
            else
                Config._currentConfig.InterruptKeys = false;
        }

        private void numSoundsBox_ValueChanged(object sender, EventArgs e)
        {
            Config._currentConfig.MaxSounds = (int)numSoundsBox.Value;
            ChangedMaxSounds = true;
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChangedMaxSounds)
            {
                ProcessStartInfo Info = new ProcessStartInfo();
                Info.Arguments = "/C ping 127.0.0.1 -n 1 && \"" + Application.ExecutablePath + "\"";
                Info.WindowStyle = ProcessWindowStyle.Hidden;
                Info.CreateNoWindow = true;
                Info.FileName = "cmd.exe";
                Process.Start(Info);
                Form1._currentForm.Close();
            }
        }
    }
}
