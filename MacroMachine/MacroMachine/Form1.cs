﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MacroMachine
{
    public partial class Form1 : Form
    {
        public readonly string _WORKINGDIR;
        public static Form1 _currentForm;
        private ComboBox DeviceBox;
        private Config _currentConfig;
        private const int _MARGIN = 40;
        private const int _MAXROWS = 5;
        private const int _MAXCOLUMNS = 2;
        private const int _MAXBUTTONS = _MAXROWS * _MAXCOLUMNS;
        private int _SCREENWIDTH;
        private int _SCREENHEIGHT;
        private static readonly Size _BUTTONSIZE = new Size(100, 100);

        public Form1()
        {
            InitializeComponent();
            _currentForm = this;
            KeyLogger._hookID = KeyLogger.SetHook(KeyLogger._proc);

            _WORKINGDIR = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MacroMachine\\";

            if (!Directory.Exists(_WORKINGDIR))
            {
                Directory.CreateDirectory(_WORKINGDIR);
            }

            if (File.Exists(_WORKINGDIR + "Config.cfg"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(_WORKINGDIR + "Config.cfg"))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        _currentConfig = new Config((Config)bf.Deserialize(sr.BaseStream));
                    }
                }
                catch (Exception e)
                {
                    File.Delete(_WORKINGDIR + "Config.cfg");
                    _currentConfig = new Config(_MAXBUTTONS);
                }
            }
            else
            {
                _currentConfig = new Config(_MAXBUTTONS);
            }

            _SCREENHEIGHT = (_MAXBUTTONS * (_BUTTONSIZE.Height + _MARGIN) / _MAXROWS) + _MARGIN * 2;
            _SCREENWIDTH = (_MAXBUTTONS * (_BUTTONSIZE.Width + _MARGIN) / _MAXCOLUMNS) + _MARGIN + 20;
            this.Size = new Size(_SCREENWIDTH, _SCREENHEIGHT);

            CreateControls();
            CreateButtons();
        }

        private void CreateControls()
        {
            //Devices combobox
            DeviceBox = new ComboBox();
            DeviceBox.Size = new Size(200, 20);
            DeviceBox.Location = new Point((_SCREENWIDTH - DeviceBox.Size.Width - 15) - 10, 0);
            DeviceBox.SelectedIndexChanged += new EventHandler(SetOutputDeviceNumber);
            Controls.Add(DeviceBox);

            //"Device" Label
            Label inputLbl2 = new Label();
            inputLbl2.Size = new Size(100, 15);
            inputLbl2.Location = new Point(DeviceBox.Location.X - inputLbl2.Size.Width, 3);
            inputLbl2.Text = "Playback device:";

            Button btnOutput = new Button();
            btnOutput.Location = new Point(inputLbl2.Location.X - 450, 3);
            btnOutput.Size = new Size(200, 20);
            btnOutput.Text = "Load Output Devices";
            btnOutput.Click += new EventHandler(BtnLoadOutputDevices);

            Button btnInput = new Button();
            btnInput.Location = new Point(inputLbl2.Location.X - 250, 3);
            btnInput.Size = new Size(200, 20);
            btnInput.Text = "Load Input Devices";
            btnInput.Click += new EventHandler(BtnLoadInputDevices);

            //Bottom left tip label
            Label lblTip = new Label();
            lblTip.Text = "Hold CTRL + Numpad[n] to record";
            lblTip.Size = new Size(300, 15);
            lblTip.Location = new Point(15, _SCREENHEIGHT - 60);

            Controls.Add(inputLbl2);
            Controls.Add(lblTip);
            Controls.Add(btnInput);
            Controls.Add(btnOutput);

            DeviceBox.Items.AddRange(SoundSystem.GetDevices(_currentConfig.CurrentDeviceType));

            if (_currentConfig.CurrentOutputDevice >= DeviceBox.Items.Count)
                _currentConfig.CurrentOutputDevice = DeviceBox.Items.Count - 1;

            DeviceBox.SelectedIndex = _currentConfig.CurrentOutputDevice;
        }
        
        //Button event to load output devices
        private void BtnLoadOutputDevices(object o, EventArgs e)
        {
            DeviceBox.Items.Clear();
            DeviceBox.Items.AddRange(SoundSystem.GetDevices(DeviceType.OutputDevice));
            _currentConfig.CurrentDeviceType = DeviceType.OutputDevice;
            DeviceBox.SelectedIndex = 0;
        }

        //Button event to load input devices
        private void BtnLoadInputDevices(object o, EventArgs e)
        {
            DeviceBox.Items.Clear();
            DeviceBox.Items.AddRange(SoundSystem.GetDevices(DeviceType.InputDevice));
            _currentConfig.CurrentDeviceType = DeviceType.InputDevice;
            DeviceBox.SelectedIndex = 0;
        }

        //EventHandler for choosing a device in DeviceBox
        private void SetOutputDeviceNumber(object o, EventArgs e)
        {
            ComboBox cb = (ComboBox)o;
            _currentConfig.CurrentOutputDevice = cb.SelectedIndex;
        }

        //Dynamically Create Buttons
        private void CreateButtons()
        {
            for (int i = 0; i < _MAXBUTTONS; i++)
            {
                TextBox text = new TextBox();
                text.Name = "Text";
                text.Text = _currentConfig.Texts[i];
                text.Size = new Size(text.Size.Width - 2, text.Size.Height);
                text.Location = new Point(1, Convert.ToInt16((_BUTTONSIZE.Height * 0.5) - (text.Size.Height * 0.5)));
                text.TextAlign = HorizontalAlignment.Center;
                text.LostFocus += new EventHandler(TextboxTextChanged);
                text.KeyDown += new KeyEventHandler(TextKeyDown);

                Button delBtn = new Button();
                delBtn.Text = "x";
                delBtn.Size = new Size(30, 30);
                delBtn.Location = new Point(_BUTTONSIZE.Width - 30, 0);
                delBtn.Click += new EventHandler(ButtonDeleter);

                Button fileBtn = new Button();
                fileBtn.Text = "♫";
                fileBtn.Size = new Size(30, 30);
                fileBtn.Location = new Point(0, 0);
                fileBtn.Click += new EventHandler(ButtonSoundDialog);

                Label lblNumPad = new Label();
                lblNumPad.Text = "Numpad" + i;
                lblNumPad.Size = new Size(53, 15);
                lblNumPad.Location = new Point(Convert.ToInt16(_BUTTONSIZE.Width * 0.25), _BUTTONSIZE.Height - Convert.ToInt16((_BUTTONSIZE.Height * 0.35)));
                
                Button btn = new Button();
                btn.Name = "btn" + i;
                btn.Size = _BUTTONSIZE;
                btn.Location = new Point(_MARGIN + ((i % _MAXROWS) * (_MARGIN + _BUTTONSIZE.Width)), _MARGIN + ((i / _MAXROWS) * (_BUTTONSIZE.Height + _MARGIN)));
                btn.Click += new EventHandler(ButtonClick);
                
                btn.Controls.Add(fileBtn);
                btn.Controls.Add(text);
                btn.Controls.Add(delBtn);
                btn.Controls.Add(lblNumPad);
                Controls.Add(btn);
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

            int macroNumber = Convert.ToInt16(tb.Parent.Name.Substring(3));
            if (_currentConfig.Sounds[macroNumber] != null)
            {
                string newName = _currentConfig.Sounds[macroNumber].Replace(_currentConfig.Texts[macroNumber], tb.Text);
                if (((TextBox)o).Text != "")
                {
                    _currentConfig.Texts[macroNumber] = ((TextBox)o).Text;

                    if (File.Exists(newName))
                    {
                        if (!File.Equals(_currentConfig.Sounds[macroNumber], newName))
                            File.Delete(newName);
                    }

                    if (!File.Equals(_currentConfig.Sounds[macroNumber], newName))
                        File.Move(_currentConfig.Sounds[macroNumber], newName); //refactor please, no need to move files that were just recorded

                    _currentConfig.Sounds[macroNumber] = newName;
                }
            }
        }

        //eventhandler to play the sound
        private void ButtonClick(object o, EventArgs e)
        {
            SoundSystem.PlayMacro(Convert.ToInt16(((Button)o).Name.Substring(3)));
        }

        //♫ button eventhandler, chooses sound file for button
        private void ButtonSoundDialog(object o, EventArgs e)
        {
            string savedir = _WORKINGDIR + "Sounds\\";
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
                if (!_currentConfig.Sounds.Contains(savedir + fullName))
                {
                    _currentConfig.Texts[id] = name;
                    if (_currentConfig.Sounds[id] != null)
                    {
                        File.Delete(_currentConfig.Sounds[id]);
                    }
                    _currentConfig.Sounds[id] = savedir + fullName;

                    if (File.Exists(savedir + fullName))
                    {
                        if (!File.Equals(savedir + fullName, fd.FileName))
                        {
                            File.Delete(savedir + fullName);
                            File.Copy(fd.FileName, savedir + fullName);
                        }
                    }
                    
                    TextBox tb = (TextBox)((Button)o).Parent.Controls.Find("Text", true).First();
                    tb.Text = name;
                }
            }

        }

        //x button eventhandler, deletes sound
        private void ButtonDeleter(object o, EventArgs e)
        {
            int num = Convert.ToInt16(((Button)o).Parent.Name.Substring(3));
            File.Delete(_currentConfig.Sounds[num]);
            _currentConfig.Sounds[num] = null;
            TextBox text = (TextBox)((Button)o).Parent.Controls.Find("Text", true).First();
            text.Text = "";
        }

        //Save on close window
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            BinaryFormatter bf = new BinaryFormatter();

            using (StreamWriter sw = new StreamWriter(_WORKINGDIR + "Config.cfg"))
            {
                bf.Serialize(sw.BaseStream, _currentConfig);
            }
        }

        public void UpdateTextbox(int btnNum, string text)
        {
            Button btn = (Button)Controls.Find("btn" + btnNum, true).First();
            TextBox tb = (TextBox)btn.Controls.Find("Text", true).First();
            tb.Text = text;
        }
    }
}
