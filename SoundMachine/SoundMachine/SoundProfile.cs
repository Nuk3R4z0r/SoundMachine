using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SoundMachine
{
    [Serializable]
    class SoundProfile
    {
        private static SoundProfile _currentSoundProfile;
        public static string SoundDirectory
        {
            get { return Config.WorkingDir + SoundProfile.CurrentSoundProfile.ProfileName + "\\Sounds\\"; }
        }

        public static SoundProfile CurrentSoundProfile
        {
            get { return _currentSoundProfile; }
            set {
                CurrentSoundProfile = value;
            }
        }

        private string _profileName;
        public string ProfileName {
            get { return _profileName; } 
            set { 
                _profileName = value;
                SaveSoundProfile(); 
            } 
        }

        public string[] Sounds { get; set; }
        public string[] Texts { get; set; }
        public int[] Bindings { get; set; }

        public SoundProfile(string profileName)
        {
            _profileName = profileName;
            Sounds = new string[Config.CurrentConfig.MaxSounds];
            Texts = new string[Config.CurrentConfig.MaxSounds];
            Bindings = new int[Config.CurrentConfig.MaxSounds];
            AddDefaultBindings();
            _currentSoundProfile = this;

            SaveSoundProfile();
        }

        public SoundProfile(SoundProfile blueprint)
        {
            _profileName = blueprint.ProfileName;
            Sounds = blueprint.Sounds ?? new string[Config.CurrentConfig.MaxSounds];
            Texts = blueprint.Texts ?? new string[Config.CurrentConfig.MaxSounds];
            if (blueprint.Bindings == null)
            { 
                Bindings = new int[Config.CurrentConfig.MaxSounds];
                AddDefaultBindings();
            }
            else
                Bindings = blueprint.Bindings;

            if (Config.CurrentConfig.MaxSounds > Sounds.Length)
            {
                string[] tempSoundsArray = new string[Config.CurrentConfig.MaxSounds];
                string[] tempTextsArray = new string[Config.CurrentConfig.MaxSounds];
                int[] tempBindingsArray = new int[Config.CurrentConfig.MaxSounds];

                for (int i = 0; i < Sounds.Length; i++)
                {
                    tempSoundsArray[i] = Sounds[i];
                    tempTextsArray[i] = Texts[i];
                    tempBindingsArray[i] = Bindings[i];
                }

                Sounds = tempSoundsArray;
                Texts = tempTextsArray;
                Bindings = tempBindingsArray;

            }

            _currentSoundProfile = this;
        }

        private void AddDefaultBindings()
        {
            Bindings[0] = (int)Keys.NumPad0;
            Bindings[1] = (int)Keys.NumPad1;
            Bindings[2] = (int)Keys.NumPad2;
            Bindings[3] = (int)Keys.NumPad3;
            Bindings[4] = (int)Keys.NumPad4;
            Bindings[5] = (int)Keys.NumPad5;
            Bindings[6] = (int)Keys.NumPad6;
            Bindings[7] = (int)Keys.NumPad7;
            Bindings[8] = (int)Keys.NumPad8;
            Bindings[9] = (int)Keys.NumPad9;
        }

        //move folder on namechange please
        public void SaveSoundProfile(bool nameChange = false)
        {
            BinaryFormatter bf = new BinaryFormatter();

            using (StreamWriter sw = new StreamWriter(Config.WorkingDir + _profileName + "\\" + _profileName + ".profile"))
            {
                bf.Serialize(sw.BaseStream, this);
                sw.Close();
            }
        }

        public static SoundProfile LoadSoundProfile(string profile)
        {
            if (!Directory.Exists(Config.WorkingDir + profile + "\\"))
            {
                Directory.CreateDirectory(Config.WorkingDir + profile + "\\");
                Directory.CreateDirectory(Config.WorkingDir + profile + "\\Sounds\\");
            }

            if (File.Exists(Config.WorkingDir + profile + "\\" + profile + ".profile"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Config.WorkingDir + profile + "\\" + profile + ".profile"))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        new SoundProfile((SoundProfile)bf.Deserialize(sr.BaseStream));
                    }
                }
                catch (Exception e)
                {
                    File.Delete(Config.WorkingDir + profile + "\\" + profile + ".profile");
                    new SoundProfile(profile);
                }
            }
            else
            {
                new SoundProfile(profile);
            }
            Overlay._currentOverlay.UpdateProfileText();

            return _currentSoundProfile;
        }

        public static void Next()
        {
            Config.CurrentConfig.CurrentProfile++;
            MainForm._currentForm.UpdateButtons();
        }
    }
}
