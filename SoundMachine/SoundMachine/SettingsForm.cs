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
            interruptInputBox.Checked = Config.CurrentConfig.InterruptKeys;
            numSoundsBox.Value = Config.CurrentConfig.MaxSounds;
            inputCheckBox.Checked = Config.CurrentConfig.InputPassthroughEnabled;
            inputChannelsBox.SelectedIndex = Config.CurrentConfig.InputChannels - 1;
            inputSamplerateBox.SelectedIndex = inputSamplerateBox.FindStringExact(Config.CurrentConfig.InputSampleRate.ToString());
            playbackCheckBox.Checked = Config.CurrentConfig.SoundPlaybackEnabled;
            muteInputCheckBox.Checked = Config.CurrentConfig.MuteInputWithSoundSystem;
            btnToggleBinding.Text = ((Keys)Config.CurrentConfig.ToggleSystemBinding).ToString();
            btnBehaviorBinding.Text = ((Keys)Config.CurrentConfig.ToggleModeBinding).ToString();
            btnProfileBinding.Text = ((Keys)Config.CurrentConfig.ToggleProfileBinding).ToString();
            btnOverlayBinding.Text = ((Keys)Config.CurrentConfig.ToggleOverlayBinding).ToString();
            btnRecordBinding.Text = ((Keys)Config.CurrentConfig.RecordBinding).ToString();

            keyPressBox.SelectedItem = Config.CurrentConfig.InputMode.ToString();
            
            LoadDevices();
            if (!Config.CurrentConfig.InputPassthroughEnabled)
                DeviceInBox.Enabled = false;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            lblVersion.Text = "Version " + fvi.FileVersion;

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
                Config.CurrentConfig.InterruptKeys = true;
            else
                Config.CurrentConfig.InterruptKeys = false;
        }

        private void numSoundsBox_ValueChanged(object sender, EventArgs e)
        {
            if (((NumericUpDown)sender).Value != Config.CurrentConfig.MaxSounds)
            {
                Config.CurrentConfig.MaxSounds = (int)numSoundsBox.Value;
                MainForm._currentForm.RenderCanvas();
                SoundProfile.CurrentSoundProfile.SaveSoundProfile();
            }
        }

        private void inputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DeviceInBox.Enabled = inputCheckBox.Checked;
            Config.CurrentConfig.InputPassthroughEnabled = inputCheckBox.Checked;
        }

        public void UnckeckInputCheckBox()
        {
            if (IsHandleCreated)
            {
                Invoke((MethodInvoker)delegate
                {

                    inputCheckBox.Checked = false;

                });
            }

        }

        private void muteInputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.CurrentConfig.MuteInputWithSoundSystem = muteInputCheckBox.Checked;
            if (KeyListener._listenerEnabled == false)
                SoundSystem.InputDisabled = true;
            else
                SoundSystem.InputDisabled = false;
        }

        private void inputChannelsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Config.CurrentConfig.InputChannels != inputChannelsBox.SelectedIndex + 1)
            {
                Config.CurrentConfig.InputChannels = inputChannelsBox.SelectedIndex + 1;
                SoundSystem.resetListener();
            }
        }

        private void inputSamplerateBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Config.CurrentConfig.InputSampleRate.ToString() != inputSamplerateBox.Items[inputSamplerateBox.SelectedIndex].ToString())
            {
                Config.CurrentConfig.InputSampleRate = Convert.ToInt32(inputSamplerateBox.Items[inputSamplerateBox.SelectedIndex]);
                SoundSystem.resetListener();
            }
        }

        private void playbackCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Config.CurrentConfig.SoundPlaybackEnabled != playbackCheckBox.Checked)
            {
                SoundSystem.KillAllSounds();
                Config.CurrentConfig.SoundPlaybackEnabled = playbackCheckBox.Checked;
            }
        }

        private void btnToggleBinding_Click(object sender, EventArgs e)
        {
            SetBindingForm setBindingForm = new SetBindingForm();
            setBindingForm.BindingType = KeyListener.KeyBinding.ToggleSystem;
            setBindingForm.ShowDialog();

            if (setBindingForm.NewBindingSet)
            {
                btnToggleBinding.Text = ((Keys)Config.CurrentConfig.ToggleSystemBinding).ToString();
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

            if (Config.CurrentConfig.InputMode != selectedMode)
            {
                SoundSystem.KillAllSounds();
                Config.CurrentConfig.InputMode = selectedMode;
            }
        }

        public void SetKeyPressBoxSelectedIndex()
        {
            if(!IsDisposed)
                keyPressBox.SelectedItem = Config.CurrentConfig.InputMode.ToString();
        }

        private void btnBehaviorBinding_Click(object sender, EventArgs e)
        {
            SetBindingForm setBindingForm = new SetBindingForm();
            setBindingForm.BindingType = KeyListener.KeyBinding.ToggleMode;
            setBindingForm.ShowDialog();

            if (setBindingForm.NewBindingSet)
            {
                btnBehaviorBinding.Text = ((Keys)Config.CurrentConfig.ToggleModeBinding).ToString();
            }
        }

        public void LoadDevices()
        {
            DeviceOutBox.Items.Clear();
            DeviceOutBox.Items.AddRange(SoundSystem.PopulateOutputDevices());
            DeviceInBox.Items.Clear();
            DeviceInBox.Items.AddRange(SoundSystem.PopulateInputDevices());

            if (Config.CurrentConfig.CurrentOutputDevice >= DeviceOutBox.Items.Count)
                Config.CurrentConfig.CurrentOutputDevice = DeviceOutBox.Items.Count - 1;

            if (Config.CurrentConfig.CurrentInputDevice >= DeviceInBox.Items.Count)
                Config.CurrentConfig.CurrentInputDevice = DeviceInBox.Items.Count - 1;


            DeviceOutBox.SelectedIndex = Config.CurrentConfig.CurrentOutputDevice;
            DeviceInBox.SelectedIndex = Config.CurrentConfig.CurrentInputDevice;
            SoundSystem.resetListener();
        }

        //EventHandler for choosing a device in DeviceBox
        private void SetOutputDeviceNumber(object o, EventArgs e)
        {
            ComboBox cb = (ComboBox)o;
            if (Config.CurrentConfig.CurrentOutputDevice != cb.SelectedIndex)
            {
                Config.CurrentConfig.CurrentOutputDevice = cb.SelectedIndex;
                SoundSystem.resetListener();
                SoundSystem.KillAllSounds();
            }
        }

        //EventHandler for choosing a device in DeviceBox
        private void SetInputDeviceNumber(object o, EventArgs e)
        {
            ComboBox cb = (ComboBox)o;
            if (Config.CurrentConfig.CurrentInputDevice != cb.SelectedIndex)
            {
                Config.CurrentConfig.CurrentInputDevice = cb.SelectedIndex;
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
                btnProfileBinding.Text = ((Keys)Config.CurrentConfig.ToggleProfileBinding).ToString();
            }
        }

        private void btnOverlayBinding_Click(object sender, EventArgs e)
        {
            SetBindingForm setBindingForm = new SetBindingForm();
            setBindingForm.BindingType = KeyListener.KeyBinding.ToggleOverlay;
            setBindingForm.ShowDialog();

            if (setBindingForm.NewBindingSet)
            {
                btnOverlayBinding.Text = ((Keys)Config.CurrentConfig.ToggleOverlayBinding).ToString();
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
                btnRecordBinding.Text = ((Keys)Config.CurrentConfig.RecordBinding).ToString();
            }
            MainForm._currentForm.UpdateTip();
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
