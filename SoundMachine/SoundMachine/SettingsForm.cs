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
            inputCheckBox.Checked = Config._currentConfig.InputPassthroughEnabled;
            inputChannelsBox.SelectedIndex = Config._currentConfig.InputChannels - 1;
            inputSamplerateBox.SelectedIndex = inputSamplerateBox.FindStringExact(Config._currentConfig.InputSampleRate.ToString());
            playbackCheckBox.Checked = Config._currentConfig.SoundPlaybackEnabled;
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

        private void inputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (inputCheckBox.Checked == true)
            {
                Form1._currentForm.ToggleInputDevices(true);
            }
            else
            {
                Form1._currentForm.ToggleInputDevices(false);
            }
        }

        private void inputChannelsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Config._currentConfig.InputChannels != inputChannelsBox.SelectedIndex + 1)
            {
                Config._currentConfig.InputChannels = inputChannelsBox.SelectedIndex + 1;
                SoundSystem.resetListener();
            }
        }

        private void inputSamplerateBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Config._currentConfig.InputSampleRate.ToString() != inputSamplerateBox.Items[inputSamplerateBox.SelectedIndex].ToString())
            {
                Config._currentConfig.InputSampleRate = Convert.ToInt32(inputSamplerateBox.Items[inputSamplerateBox.SelectedIndex]);
                SoundSystem.resetListener();
            }
        }

        private void playbackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config._currentConfig.SoundPlaybackEnabled = playbackCheckBox.Checked;
        }
    }
}
