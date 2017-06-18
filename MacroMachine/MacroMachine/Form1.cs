using System;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MacroMachine
{
    public partial class Form1 : Form
    {
        readonly string _WORKINGDIR;
        public static Form1 _currentForm;

        Config _currentConfig;
        int _buttonCount = 0;
        const int _MARGIN = 40;
        const int _MAXROWS = 5;
        const int _MAXCOLUMNS = 2;
        const int _MAXBUTTONS = _MAXROWS * _MAXCOLUMNS;
        int _SCREENWIDTH;
        int _SCREENHEIGHT;
        readonly static Size _BUTTONSIZE = new Size(100, 100);

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


            /*ComboBox cb = new ComboBox();
            cb.Location = new Point(_SCREENWIDTH - cb.Size.Width - 15, 0);
            cb.SelectedIndexChanged += new EventHandler(SetInputDeviceNumber);
            Controls.Add(cb);

            Label inputLbl = new Label();
            inputLbl.Size = new Size(35, 15);
            inputLbl.Location = new Point(cb.Location.X - inputLbl.Size.Width, 3);
            inputLbl.Text = "Input:";

            Controls.Add(inputLbl);

            cb.Items.AddRange(SoundSystem.GetDevices());
            cb.SelectedIndex = _currentConfig.CurrentInputDevice;
            */

            ComboBox cb2 = new ComboBox();
            cb2.Size = new Size(200, 20);
            cb2.Location = new Point((_SCREENWIDTH - cb2.Size.Width - 15) /*- inputLbl.Width - cb.Width - 10*/, 0);
            cb2.SelectedIndexChanged += new EventHandler(SetOutputDeviceNumber);
            Controls.Add(cb2);

            Label inputLbl2 = new Label();
            inputLbl2.Size = new Size(45, 15);
            inputLbl2.Location = new Point(cb2.Location.X - inputLbl2.Size.Width, 3);
            inputLbl2.Text = "Output:";

            Controls.Add(inputLbl2);

            cb2.Items.AddRange(SoundSystem.GetDevices());
            cb2.SelectedIndex = _currentConfig.CurrentOutputDevice;

            for (int i = 0; i < _MAXBUTTONS; i++)
            {
                CreateButtons();
            }
        }

        /*void SetInputDeviceNumber(object o, EventArgs e)
        {
            ComboBox cb = (ComboBox)o;
            _currentConfig.CurrentInputDevice = cb.SelectedIndex;
        }*/

        void SetOutputDeviceNumber(object o, EventArgs e)
        {
            ComboBox cb = (ComboBox)o;
            _currentConfig.CurrentOutputDevice = cb.SelectedIndex;
        }

        void CreateButtons()
        {
            if (_buttonCount != _MAXBUTTONS)
            {
                TextBox text = new TextBox();
                text.Name = "Text";
                text.Text = _currentConfig.Texts[_buttonCount];
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

                /*
                Label macroLbl = new Label();
                macroLbl.Text = "Macro key:";
                macroLbl.Size = new Size(60, 15);
                macroLbl.Location = new Point(0, _BUTTONSIZE.Height - macroLbl.Size.Height);

                TextBox macroTxt = new TextBox();
                macroTxt.TextAlign = HorizontalAlignment.Center;
                macroTxt.MaxLength = 1;
                macroTxt.Size = new Size(30, 30);
                macroTxt.Text = _currentConfig.MacroKey[_buttonCount];
                macroTxt.Location = new Point(macroLbl.Width, _BUTTONSIZE.Height - macroTxt.Size.Height);
                macroTxt.TextChanged += new EventHandler(MacroTextChanged);
                */

                Button btn = new Button();
                btn.Name = "btn" + _buttonCount;
                btn.Size = _BUTTONSIZE;
                btn.Location = new Point(_MARGIN + ((_buttonCount % _MAXROWS) * (_MARGIN + _BUTTONSIZE.Width)), _MARGIN + ((_buttonCount / _MAXROWS) * (_BUTTONSIZE.Height + _MARGIN)));
                btn.Click += new EventHandler(ButtonClick);

                //btn.Controls.Add(macroLbl);
                //btn.Controls.Add(macroTxt);
                btn.Controls.Add(fileBtn);
                btn.Controls.Add(text);
                btn.Controls.Add(delBtn);
                Controls.Add(btn);

                _buttonCount++;
            }
        }

        /*void MacroTextChanged(object o, EventArgs e)
        {
            TextBox tb = ((TextBox)o);
            _currentConfig.MacroKey[Convert.ToInt16(tb.Parent.Name.Substring(3))] = ((TextBox)o).Text;
        }*/

        void TextKeyDown(object o, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextboxTextChanged(o, e);
            }
        }

        void TextboxTextChanged(object o, EventArgs e)
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

        void ButtonClick(object o, EventArgs e)
        {
            SoundSystem.PlayMacro(Convert.ToInt16(((Button)o).Name.Substring(3)));
        }

        void ButtonSoundDialog(object o, EventArgs e)
        {
            string savedir = _WORKINGDIR + "Sounds\\";
            if (!Directory.Exists(savedir))
            {
                Directory.CreateDirectory(savedir);
            }

            OpenFileDialog fd = new OpenFileDialog();
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
                        File.Delete(savedir + fullName);
                    }

                    File.Copy(fd.FileName, savedir + fullName);

                    TextBox tb = (TextBox)((Button)o).Parent.Controls.Find("Text", true).First();
                    tb.Text = name;
                }
            }

        }

        void ButtonDeleter(object o, EventArgs e)
        {
            int num = Convert.ToInt16(((Button)o).Parent.Name.Substring(3));
            File.Delete(_currentConfig.Sounds[num]);
            _currentConfig.Sounds[num] = null;
            TextBox text = (TextBox)((Button)o).Parent.Controls.Find("Text", true).First();
            text.Text = "";
        }

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
