using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroMachine
{
    [Serializable]
    class Config
    {
        public static Config _currentConfig;
        public string[] Sounds { get; set; }
        public string[] Texts { get; set; }
        //public string[] MacroKey { get; set; }
        public int CurrentOutputDevice { get; set; }
        //public int CurrentInputDevice { get; set; }

        public Config(int maxButtons)
        {
            Sounds = new string[maxButtons];
            Texts = new string[maxButtons];
            //MacroKey = new string[maxButtons];
            CurrentOutputDevice = 0;
            //CurrentInputDevice = 0;
            _currentConfig = this;
        }

        public Config(Config blueprint)
        {
            Sounds = blueprint.Sounds;
            Texts = blueprint.Texts;
            //MacroKey = blueprint.MacroKey;
            CurrentOutputDevice = blueprint.CurrentOutputDevice;
            //CurrentInputDevice = blueprint.CurrentInputDevice;
            _currentConfig = this;
        }
    }
}
