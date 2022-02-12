using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SoundMachine
{
    public partial class MainForm : Form
    {
        public static MainForm _currentForm;
        public static bool _FILELOCK;
        private string _NEWLINES;
        private const int _MARGIN = 40;
        private int _MAXBUTTONS;
        private int _MAXROWS;
        private int _MAXCOLUMNS;
        private int _SCREENWIDTH;
        private int _SCREENHEIGHT;
        private static readonly Size _BUTTONSIZE = new Size(100, 100);
        private List<BindingButton> btnList;
        private ComboBox ProfileBox;

        public MainForm()
        {
            InitializeComponent();
            _FILELOCK = false;
            _NEWLINES = Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            btnList = new List<BindingButton>();
            _currentForm = this;
            KeyListener._hookID = KeyListener.SetHook(KeyListener._proc);
            KeyListener._listenerEnabled = true;

            Overlay overlay = new Overlay();
            overlay.UpdateBehaviorText();

            RenderCanvas();
            SoundSystem.PopulateOutputDevices();
            SettingsForm settingsForm = new SettingsForm();

            if (Config.CurrentConfig.InputPassthroughEnabled)
            {
                Thread t = new Thread(SoundSystem.ContinuousInputPlayback);
                t.Start();
            }

            SoundSystem.InitializeStoppables();
        }

        public void RenderCanvas()
        {
            SoundProfile.LoadSoundProfile(Config.CurrentConfig.Profiles[Config.CurrentConfig.CurrentProfile]);

            _MAXBUTTONS = Config.CurrentConfig.MaxSounds;
            _MAXCOLUMNS = 5;
            _MAXROWS = (int)Math.Ceiling((double)_MAXBUTTONS / _MAXCOLUMNS);
            _SCREENHEIGHT = (_MAXROWS * (_BUTTONSIZE.Height + _MARGIN)) + _MARGIN * 2;
            _SCREENWIDTH = (_MAXCOLUMNS * (_BUTTONSIZE.Width + _MARGIN)) + _MARGIN + 20;
            Size = new Size(_SCREENWIDTH, _SCREENHEIGHT);

            Controls.Clear();
            btnList.Clear();

            CreateControls();
            CreateButtons();
        }

        private void CreateControls()
        {
            ToolStrip toolStrip = new ToolStrip();
            toolStrip.Items.Add("Settings", null, ButtonSettings);
            toolStrip.Items.Add("New Profile", null, ButtonNew);
            toolStrip.Items.Add("Rename Profile", null, ButtonRename);
            toolStrip.Items.Add("Import Profile", null, ButtonImport);
            toolStrip.Items.Add("Delete Profile", null, ButtonDelete);

            //Profile combobox
            ProfileBox = new ComboBox();
            ProfileBox.Name = "ProfileBox";
            ProfileBox.Size = new Size(200, 20);
            ProfileBox.Location = new Point(_SCREENWIDTH - ProfileBox.Size.Width - 35, 3);
            ProfileBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ProfileBox.Items.AddRange(Config.CurrentConfig.Profiles.ToArray());
            ProfileBox.SelectedIndex = Config.CurrentConfig.CurrentProfile;
            ProfileBox.SelectedIndexChanged += new EventHandler(SetProfile);
            Controls.Add(ProfileBox);

            //"Profile" Label
            Label inputLbl3 = new Label();
            inputLbl3.Size = new Size(40, 15);
            inputLbl3.Location = new Point(ProfileBox.Location.X - inputLbl3.Size.Width, 6);
            inputLbl3.Text = "Profile:";

            //Bottom left tip label
            Label lblTip = new Label();
            lblTip.Name = "lblTip";
            lblTip.Text = "Hold " + ((Keys)Config.CurrentConfig.RecordBinding).ToString() + " + Binding to record";
            lblTip.Size = new Size(200, 15);
            lblTip.Location = new Point(15, _SCREENHEIGHT - 60);

            TrackBar tbVolume = new TrackBar();
            tbVolume.Name = "tbVolume";
            tbVolume.Size = new Size(300, 15);
            tbVolume.TickFrequency = 2;
            tbVolume.Location = new Point(_SCREENWIDTH - tbVolume.Size.Width - 20, _SCREENHEIGHT - tbVolume.Size.Height - 30);
            tbVolume.Value = Config.CurrentConfig.CurrentVolume;
            tbVolume.ValueChanged += new EventHandler(SetVolume);

            Label lblVol = new Label();
            lblVol.Text = "Volume:";
            lblVol.Size = new Size(45, 15);
            lblVol.Location = new Point(tbVolume.Location.X - lblVol.Size.Width, _SCREENHEIGHT - 70);

            Button btnToggleSystem = new Button();
            btnToggleSystem.Name = "btnToggleSystem";
            btnToggleSystem.Size = new Size(100, 30);
            btnToggleSystem.Location = new Point(_SCREENWIDTH - (_SCREENWIDTH / 2) + 25, 5);
            btnToggleSystem.Text = "Sound Enabled";
            btnToggleSystem.BackColor = Color.Green;
            btnToggleSystem.Click += new EventHandler(ButtonClickToggleSystem);
            btnToggleSystem.BringToFront();

            Controls.Add(btnToggleSystem);
            Controls.Add(toolStrip);
            Controls.Add(lblVol);
            Controls.Add(tbVolume);
            Controls.Add(inputLbl3);
            Controls.Add(lblTip);
        }

        //Dynamically Create Buttons
        private void CreateButtons()
        {
            for (int i = 0; i < _MAXBUTTONS; i++)
            {
                TextBox text = new TextBox();
                text.Name = "Text";
                text.Text = SoundProfile.CurrentSoundProfile.Texts[i];
                text.Size = new Size(text.Size.Width - 2, text.Size.Height);
                text.Location = new Point(1, Convert.ToInt16((_BUTTONSIZE.Height * 0.5) - (text.Size.Height * 0.5)));
                text.TextAlign = HorizontalAlignment.Center;
                text.LostFocus += new EventHandler(TextboxTextChanged);
                text.KeyDown += new KeyEventHandler(TextKeyDown);

                Button delBtn = new Button();
                delBtn.Text = "🗑";
                delBtn.Size = new Size(30, 30);
                delBtn.Location = new Point(_BUTTONSIZE.Width - 30, 0);
                delBtn.Click += new EventHandler(ButtonClickDelete);

                Button fileBtn = new Button();
                fileBtn.Text = "📁";
                fileBtn.Size = new Size(30, 30);
                fileBtn.Location = new Point(0, 0);
                fileBtn.Click += new EventHandler(ButtonSoundDialog);

                Button setBindingBtn = new Button();
                setBindingBtn.Size = new Size(53, 30);
                setBindingBtn.Location = new Point(Convert.ToInt16(_BUTTONSIZE.Width * 0.25), 0);
                setBindingBtn.Text = "Rebind";
                setBindingBtn.Click += new EventHandler(BindingDialog);

                BindingButton btn = new BindingButton();
                btn.Name = "btn" + i;
                btn.Size = _BUTTONSIZE;
                btn.Text = _NEWLINES + ((Keys)SoundProfile.CurrentSoundProfile.Bindings[i]).ToString();
                btn.Location = new Point(_MARGIN + ((i % _MAXCOLUMNS) * (_MARGIN + _BUTTONSIZE.Width)), _MARGIN + ((i / _MAXCOLUMNS) * (_BUTTONSIZE.Height + _MARGIN)));
                btn.Click += new EventHandler(ButtonClickPlay);
                if(SoundProfile.CurrentSoundProfile.Bindings.Length > i)
                    btn.BindingKeyCode = SoundProfile.CurrentSoundProfile.Bindings[i];

                btn.Controls.Add(fileBtn);
                btn.Controls.Add(text);
                btn.Controls.Add(delBtn);
                btn.Controls.Add(setBindingBtn);
                btnList.Add(btn);
                Controls.Add(btn);
            }
        }

        public void UpdateButtons()
        {
            ProfileBox.Items.Clear();
            ProfileBox.Items.AddRange(Config.CurrentConfig.Profiles.ToArray());
            ProfileBox.SelectedIndex = Config.CurrentConfig.CurrentProfile;
            int i = 0;
            foreach (BindingButton b in btnList)
            {
                TextBox tb = (TextBox)b.Controls.Find("Text", false).First();
                tb.Text = SoundProfile.CurrentSoundProfile.Texts[i];

                b.Text = _NEWLINES + ((Keys)SoundProfile.CurrentSoundProfile.Bindings[i]).ToString();
                if (SoundProfile.CurrentSoundProfile.Bindings.Length > i)
                    b.BindingKeyCode = SoundProfile.CurrentSoundProfile.Bindings[i];
                i++;
            }
        }

        private void SetProfile(object o, EventArgs e)
        {
            if (ProfileBox.SelectedIndex != Config.CurrentConfig.CurrentProfile)
            {
                Config.CurrentConfig.CurrentProfile = ((ComboBox)o).SelectedIndex;
                UpdateButtons();
            }
        }

        private void TextKeyDown(object o, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextboxTextChanged(o, e);
            }
        }

        //When textbox is changed with enter or losefocus, change name of sound-file too
        private void TextboxTextChanged(object o, EventArgs e)
        {
            TextBox tb = ((TextBox)o);
            Regex rgx = new Regex("[\\/:*\"<>|]");
            tb.Text = rgx.Replace(tb.Text, "");

            int macroNumber = Convert.ToInt16(tb.Parent.Name.Substring(3));
            if (SoundProfile.CurrentSoundProfile.Sounds[macroNumber] != null)
            {
                string newName = SoundProfile.CurrentSoundProfile.Sounds[macroNumber].Replace(SoundProfile.CurrentSoundProfile.Texts[macroNumber], tb.Text);
                if (((TextBox)o).Text != "")
                {
                    SoundProfile.CurrentSoundProfile.Texts[macroNumber] = ((TextBox)o).Text;

                    if (File.Exists(newName))
                    {
                        if (!File.Equals(SoundProfile.CurrentSoundProfile.Sounds[macroNumber], newName))
                            File.Delete(newName);
                    }

                    SoundSystem.KillSound(macroNumber);
                    if (!File.Equals(SoundProfile.CurrentSoundProfile.Sounds[macroNumber], newName))
                        File.Move(SoundProfile.CurrentSoundProfile.Sounds[macroNumber], newName); //refactor please, no need to move files that were just recorded

                    SoundProfile.CurrentSoundProfile.Sounds[macroNumber] = newName;
                    SoundProfile.CurrentSoundProfile.SaveSoundProfile();
                }
            }
        }

        //eventhandler to play the sound
        private void ButtonClickPlay(object o, EventArgs e)
        {
            SoundSystem.PlaySound(Convert.ToInt16(((Button)o).Name.Substring(3)));
        }

        private void ButtonSettings(object o, EventArgs e)
        {
            if (SettingsForm._currentForm == null || SettingsForm._currentForm.IsDisposed)
            {
                SettingsForm settingsForm = new SettingsForm();
            }
            SettingsForm._currentForm.Show();
            SettingsForm._currentForm.Activate();
        }

        private void ButtonNew(object o, EventArgs e)
        {
            ProfileForm profileForm = new ProfileForm();
            profileForm.ShowDialog();
            if (profileForm.NewProfile != "")
            {
                ((ComboBox)Controls.Find("ProfileBox", false).First()).Items.Add(profileForm.NewProfile);
                Config.CurrentConfig.CurrentProfile = Config.CurrentConfig.Profiles.Count - 1;
            }
            UpdateButtons();
            Config.CurrentConfig.SaveConfig();
        }

        private void ButtonRename(object o, EventArgs e)
        {
            ProfileForm profileForm = new ProfileForm();
            profileForm.Text = "Rename Profile";
            profileForm.IsRenaming = true;
            profileForm.ShowDialog();
            if (profileForm.NewProfile != "")
            {
                ((ComboBox)Controls.Find("ProfileBox", false).First()).Items[Config.CurrentConfig.CurrentProfile] = profileForm.NewProfile;
                Utilities.MoveProfile(Config.CurrentConfig.Profiles[Config.CurrentConfig.CurrentProfile], profileForm.NewProfile);
                Config.CurrentConfig.Profiles[Config.CurrentConfig.CurrentProfile] = profileForm.NewProfile;
            }
            UpdateButtons();
            Config.CurrentConfig.SaveConfig();
        }

        private void ButtonImport(object o, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = Config.WorkingDir;
            fd.Filter = "SoundSystem Profile |*.profile";
            fd.ShowDialog();

            if (fd.FileName != "")
            {
                if (Utilities.ImportProfile(fd.FileName))
                {
                    UpdateButtons();
                    Config.CurrentConfig.SaveConfig();
                }
            }
        }

        private void ButtonDelete(object o, EventArgs e)
        {
            try
            {
                Directory.Delete(Config.WorkingDir + Config.CurrentConfig.Profiles[Config.CurrentConfig.CurrentProfile], true);
            }
            catch (Exception x) { }
            
            Config.CurrentConfig.Profiles.RemoveAt(Config.CurrentConfig.CurrentProfile);

            if (Config.CurrentConfig.Profiles.Count == 0)
            {
                Config.CurrentConfig.Profiles.Add("Profile0");
            }

            if (Config.CurrentConfig.CurrentProfile > 0)
                Config.CurrentConfig.CurrentProfile--;
            else
                Config.CurrentConfig.CurrentProfile = -1;

            UpdateButtons();
            Config.CurrentConfig.SaveConfig();
        }


        //♫ button eventhandler, chooses sound file for button
        private void ButtonSoundDialog(object o, EventArgs e)
        {
            string savedir = SoundProfile.SoundDirectory;
            if (!Directory.Exists(savedir))
            {
                Directory.CreateDirectory(savedir);
            }

            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = savedir;
            fd.ShowDialog();
            int id = Convert.ToInt16(((Button)o).Parent.Name.Substring(3));


            if (fd.FileName != "")
            {
                string fullName = fd.FileName.Substring(fd.FileName.LastIndexOf('\\') + 1);
                string name = fullName.Substring(0, fullName.LastIndexOf('.'));
                RemoveDuplicateSound(savedir + fullName);

                SoundProfile.CurrentSoundProfile.Texts[id] = name;
                SoundProfile.CurrentSoundProfile.Sounds[id] = fullName;
                SoundProfile.CurrentSoundProfile.SaveSoundProfile();

                if (File.Exists(savedir + fullName))
                {
                    if (!File.Equals(savedir + fullName, fd.FileName))
                    {
                        File.Delete(savedir + fullName);
                    }
                }
                
                if(savedir + fullName != fd.FileName)
                    File.Copy(fd.FileName, savedir + fullName);

                TextBox tb = (TextBox)((Button)o).Parent.Controls.Find("Text", false).First();
                tb.Text = name;
            }
        }
        
        int RemoveDuplicateSound(string sound)
        {
            for(int i = 0; i < SoundProfile.CurrentSoundProfile.Sounds.Length; i++)
            {
                if (SoundProfile.CurrentSoundProfile.Sounds[i] == sound)
                {
                    Button btnSound = (Button)Controls.Find("btn" + i, false).First();
                    TextBox tb = (TextBox)btnSound.Controls.Find("Text", false).First();
                    tb.Text = "";
                    SoundProfile.CurrentSoundProfile.Sounds[i] = null;
                    SoundProfile.CurrentSoundProfile.Texts[i] = null;
                    SoundProfile.CurrentSoundProfile.SaveSoundProfile();
                    return i;
                }
            }

            return 0;
        }


        //Rebind button eventhandler, chooses keyboard binding for button
        private void BindingDialog(object o, EventArgs e)
        {
            int id = Convert.ToInt16(((Button)o).Parent.Name.Substring(3));

            SetBindingForm bindingForm = new SetBindingForm();
            bindingForm.CurrentButton = id;
            bindingForm.BindingType = KeyListener.KeyBinding.Sound;
            bindingForm.ShowDialog();

            if (bindingForm.NewBindingSet)
            {
                if (bindingForm.RemovedBinding != -1)
                {
                    Button btn = (Button)Controls.Find("btn" + bindingForm.RemovedBinding, false).First();
                    btn.Text = "";
                }

                ((Button)((Button)o).Parent).Text = _NEWLINES + ((Keys)SoundProfile.CurrentSoundProfile.Bindings[id]).ToString();
            }
        }

        //🗑 button eventhandler, deletes sound
        private void ButtonClickDelete(object o, EventArgs e)
        {
            int num = Convert.ToInt16(((Button)o).Parent.Name.Substring(3));
            if (File.Exists(SoundProfile.CurrentSoundProfile.Sounds[num]))
            {
                File.Delete(SoundProfile.CurrentSoundProfile.Sounds[num]);
                SoundProfile.CurrentSoundProfile.Sounds[num] = null;
                SoundProfile.CurrentSoundProfile.Texts[num] = null;
                SoundProfile.CurrentSoundProfile.SaveSoundProfile();
            }
            TextBox text = (TextBox)((Button)o).Parent.Controls.Find("Text", false).First();
            text.Text = "";
        }

        private void ButtonClickToggleSystem(object o, EventArgs e)
        {
            ToggleSystemEnabled((Button)o);
        }

        public void ToggleSystemEnabled(Button btn)
        {
            if (btn == null)
                btn = (Button)Controls.Find("btnToggleSystem", false).First();

            KeyListener._listenerEnabled = !KeyListener._listenerEnabled;
            btn.Text = KeyListener._listenerEnabled == true ? "Sound Enabled" : "Sound Muted";
            btn.BackColor = KeyListener._listenerEnabled == true ? Color.Green : Color.PaleVioletRed;
            if (KeyListener._listenerEnabled == false)
            {
                SoundSystem.KillAllSounds();
                if (Config.CurrentConfig.MuteInputWithSoundSystem)
                    SoundSystem.InputDisabled = true;
            }
            else
                SoundSystem.InputDisabled = false;
            //ToggleInputDevices(_systemEnabled);
        }

        //Save on close window
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SoundSystem.KillInputListener();
        }

        public void UpdateTextbox(int btnNum, string text)
        {
            Button btn = (Button)Controls.Find("btn" + btnNum, false).First();
            TextBox tb = (TextBox)btn.Controls.Find("Text", false).First();
            tb.Text = text;
        }

        public void UpdateTip()
        {
            Label lbl = (Label)Controls.Find("lblTip", false).First();
            lbl.Text = "Hold " + ((Keys)Config.CurrentConfig.RecordBinding).ToString() + " + Binding to record";
        }

        private void SetVolume(object o, EventArgs e)
        {
            TrackBar tbTemp = (TrackBar)Controls.Find("tbVolume", false).First();
            Config.CurrentConfig.CurrentVolume = tbTemp.Value;
        }
    }
}
