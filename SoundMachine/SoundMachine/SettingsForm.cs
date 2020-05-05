using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace SoundMachine
{
    public partial class SettingsForm : Form
    {
        public static SettingsForm _currentForm;

        public SettingsForm()
        {
            InitializeComponent();
            interruptInputBox.Checked = Config._currentConfig.InterruptKeys;
            numSoundsBox.Value = Config._currentConfig.MaxSounds;
            inputCheckBox.Checked = Config._currentConfig.InputPassthroughEnabled;
            inputChannelsBox.SelectedIndex = Config._currentConfig.InputChannels - 1;
            inputSamplerateBox.SelectedIndex = inputSamplerateBox.FindStringExact(Config._currentConfig.InputSampleRate.ToString());
            playbackCheckBox.Checked = Config._currentConfig.SoundPlaybackEnabled;
            muteInputCheckBox.Checked = Config._currentConfig.MuteInputWithSoundSystem;
            btnToggleBinding.Text = ((Keys)Config._currentConfig.ToggleSystemBinding).ToString();
            btnBehaviorBinding.Text = ((Keys)Config._currentConfig.ToggleModeBinding).ToString();
            btnProfileBinding.Text = ((Keys)Config._currentConfig.ToggleProfileBinding).ToString();
            btnOverlayBinding.Text = ((Keys)Config._currentConfig.ToggleOverlayBinding).ToString();
            btnRecordBinding.Text = ((Keys)Config._currentConfig.RecordBinding).ToString();

            keyPressBox.SelectedItem = Config._currentConfig.InputMode.ToString();
            
            LoadDevices();
            if (!Config._currentConfig.InputPassthroughEnabled)
                DeviceInBox.Enabled = false;

            _currentForm = this;
        }

        //Button event to load output devices
        private void BtnLoadDevices(object o, EventArgs e)
        {
            LoadDevices();
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
            if (((NumericUpDown)sender).Value != Config._currentConfig.MaxSounds)
            {
                Config._currentConfig.MaxSounds = (int)numSoundsBox.Value;
                Form1._currentForm.RenderCanvas();
                SoundProfile.CurrentSoundProfile.SaveSoundProfile();
            }
        }

        private void inputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DeviceInBox.Enabled = inputCheckBox.Checked;
            Config._currentConfig.InputPassthroughEnabled = inputCheckBox.Checked;
        }

        private void muteInputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config._currentConfig.MuteInputWithSoundSystem = muteInputCheckBox.Checked;
            if (KeyListener._listenerEnabled == false)
                SoundSystem.InputDisabled = true;
            else
                SoundSystem.InputDisabled = false;
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
            if (Config._currentConfig.SoundPlaybackEnabled != playbackCheckBox.Checked)
            {
                SoundSystem.KillAllSounds();
                Config._currentConfig.SoundPlaybackEnabled = playbackCheckBox.Checked;
            }
        }

        private void btnToggleBinding_Click(object sender, EventArgs e)
        {
            SetBindingForm setBindingForm = new SetBindingForm();
            setBindingForm.BindingType = KeyListener.KeyBinding.ToggleSystem;
            setBindingForm.ShowDialog();

            if (setBindingForm.NewBindingSet)
            {
                btnToggleBinding.Text = ((Keys)Config._currentConfig.ToggleSystemBinding).ToString();
            }
        }

        private void keyPressBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SoundSystem.SoundMode selectedMode;

            switch(keyPressBox.SelectedIndex)
            {
                case 0:
                    selectedMode = SoundSystem.SoundMode.Stack;
                    break;
                case 1:
                    selectedMode = SoundSystem.SoundMode.Interrupt;
                    break;
                case 2:
                    selectedMode = SoundSystem.SoundMode.StartStop;
                    break;
                case 3:
                    selectedMode = SoundSystem.SoundMode.Loop;
                    break;
                default:
                    selectedMode = SoundSystem.SoundMode.Interrupt;
                    break;
            }

            if (Config._currentConfig.InputMode != selectedMode)
            {
                SoundSystem.KillAllSounds();
                Config._currentConfig.InputMode = selectedMode;
            }
        }

        public void SetKeyPressBoxSelectedIndex()
        {
            if(!IsDisposed)
                keyPressBox.SelectedItem = Config._currentConfig.InputMode.ToString();
        }

        private void btnBehaviorBinding_Click(object sender, EventArgs e)
        {
            SetBindingForm setBindingForm = new SetBindingForm();
            setBindingForm.BindingType = KeyListener.KeyBinding.ToggleMode;
            setBindingForm.ShowDialog();

            if (setBindingForm.NewBindingSet)
            {
                btnBehaviorBinding.Text = ((Keys)Config._currentConfig.ToggleModeBinding).ToString();
            }
        }

        public void LoadDevices()
        {
            DeviceOutBox.Items.Clear();
            DeviceOutBox.Items.AddRange(SoundSystem.PopulateOutputDevices());
            DeviceInBox.Items.Clear();
            DeviceInBox.Items.AddRange(SoundSystem.PopulateInputDevices());

            if (Config._currentConfig.CurrentOutputDevice >= DeviceOutBox.Items.Count)
                Config._currentConfig.CurrentOutputDevice = DeviceOutBox.Items.Count - 1;

            if (Config._currentConfig.CurrentInputDevice >= DeviceInBox.Items.Count)
                Config._currentConfig.CurrentInputDevice = DeviceInBox.Items.Count - 1;


            DeviceOutBox.SelectedIndex = Config._currentConfig.CurrentOutputDevice;
            DeviceInBox.SelectedIndex = Config._currentConfig.CurrentInputDevice;
            SoundSystem.resetListener();
        }

        //EventHandler for choosing a device in DeviceBox
        private void SetOutputDeviceNumber(object o, EventArgs e)
        {
            ComboBox cb = (ComboBox)o;
            if (Config._currentConfig.CurrentOutputDevice != cb.SelectedIndex)
            {
                Config._currentConfig.CurrentOutputDevice = cb.SelectedIndex;
                SoundSystem.resetListener();
                SoundSystem.KillAllSounds();
            }
        }

        //EventHandler for choosing a device in DeviceBox
        private void SetInputDeviceNumber(object o, EventArgs e)
        {
            ComboBox cb = (ComboBox)o;
            if (Config._currentConfig.CurrentInputDevice != cb.SelectedIndex)
            {
                Config._currentConfig.CurrentInputDevice = cb.SelectedIndex;
                SoundSystem.resetListener();
            }
        }

        private void btnProfileBinding_Click(object sender, EventArgs e)
        {
            SetBindingForm setBindingForm = new SetBindingForm();
            setBindingForm.BindingType = KeyListener.KeyBinding.ToggleProfile;
            setBindingForm.ShowDialog();

            if (setBindingForm.NewBindingSet)
            {
                btnProfileBinding.Text = ((Keys)Config._currentConfig.ToggleProfileBinding).ToString();
            }
        }

        private void btnOverlayBinding_Click(object sender, EventArgs e)
        {
            SetBindingForm setBindingForm = new SetBindingForm();
            setBindingForm.BindingType = KeyListener.KeyBinding.ToggleOverlay;
            setBindingForm.ShowDialog();

            if (setBindingForm.NewBindingSet)
            {
                btnOverlayBinding.Text = ((Keys)Config._currentConfig.ToggleOverlayBinding).ToString();
            }
        }

        private void btnHomeDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Config.WorkingDir + "\\";
            dialog.ShowDialog();

            if(dialog.SelectedPath != Config.WorkingDir + "\\")
            {
                Utilities.MoveHome(dialog.SelectedPath);
            }
        }

        private void btnRecordBinding_Click(object sender, EventArgs e)
        {
            SetBindingForm setBindingForm = new SetBindingForm();
            setBindingForm.BindingType = KeyListener.KeyBinding.Record;
            setBindingForm.ShowDialog();
            if (setBindingForm.NewBindingSet)
            {
                btnRecordBinding.Text = ((Keys)Config._currentConfig.RecordBinding).ToString();
            }
            Form1._currentForm.UpdateTip();
        }
    }
}
