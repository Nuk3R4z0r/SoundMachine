using System;
using System.Windows.Forms;

namespace SoundMachine
{
    [Serializable]
    class Config
    {
        public static Config _currentConfig;
        public int MaxSounds { get; set; }
        public string[] Sounds { get; set; }
        public string[] Texts { get; set; }
        public int[] Bindings { get; set; }
        public int CurrentOutputDevice { get; set; }
        public int CurrentInputDevice { get; set; }
        public int SoundPlaybackDevice { get; set; }
        public int CurrentVolume { get; set; }
        public bool DisableKeys { get; set; }
        public bool InputPlaybackEnabled { get; set; }
        public bool SoundPlaybackEnabled { get; set; }
        public bool InterruptKeys { get; set; }


        public Config(int maxButtons)
        {
            MaxSounds = maxButtons;
            Sounds = new string[MaxSounds];
            Texts = new string[MaxSounds];
            Bindings = new int[MaxSounds];
            AddDefaultBindings();

            CurrentOutputDevice = 0;
            CurrentInputDevice = 0;
            SoundPlaybackDevice = 0;
            CurrentVolume = 0;
            InputPlaybackEnabled = true;
            SoundPlaybackEnabled = true;
            InterruptKeys = false;
            _currentConfig = this;
        }

        public Config(Config blueprint)
        {
            MaxSounds = blueprint.MaxSounds;
            if (MaxSounds < 10)
                MaxSounds = 10;

            Sounds = blueprint.Sounds ?? new string[MaxSounds];
            Texts = blueprint.Texts ?? new string[MaxSounds];
            if (blueprint.Bindings == null)
            {
                Bindings = new int[MaxSounds];
                AddDefaultBindings();
            }
            else
                Bindings = blueprint.Bindings;

            if(MaxSounds > Sounds.Length)
            {
                string[] tempSoundsArray = new string[MaxSounds];
                string[] tempTextsArray = new string[MaxSounds];
                int[] tempBindingsArray = new int[MaxSounds];

                for(int i = 0; i < Sounds.Length; i++)
                {
                    tempSoundsArray[i] = Sounds[i];
                    tempTextsArray[i] = Texts[i];
                    tempBindingsArray[i] = Bindings[i];
                }

                Sounds = tempSoundsArray;
                Texts = tempTextsArray;
                Bindings = tempBindingsArray;
            }

            CurrentOutputDevice = blueprint.CurrentOutputDevice;
            CurrentInputDevice = blueprint.CurrentInputDevice;
            SoundPlaybackDevice = blueprint.SoundPlaybackDevice;
            CurrentVolume = blueprint.CurrentVolume;
            InputPlaybackEnabled = blueprint.InputPlaybackEnabled;
            SoundPlaybackEnabled = blueprint.SoundPlaybackEnabled;
            InterruptKeys = blueprint.InterruptKeys;
            _currentConfig = this;
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
    }
}
