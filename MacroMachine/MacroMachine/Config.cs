using System;

namespace MacroMachine
{
    [Serializable]
    class Config
    {
        public static Config _currentConfig;
        public string[] Sounds { get; set; }
        public string[] Texts { get; set; }
        public int CurrentOutputDevice { get; set; }
        public DeviceType CurrentDeviceType { get; set; }

        public Config(int maxButtons)
        {
            Sounds = new string[maxButtons];
            Texts = new string[maxButtons];
            CurrentOutputDevice = 0;
            _currentConfig = this;
        }

        public Config(Config blueprint)
        {
            Sounds = blueprint.Sounds;
            Texts = blueprint.Texts;
            CurrentOutputDevice = blueprint.CurrentOutputDevice;
            _currentConfig = this;
            CurrentDeviceType = blueprint.CurrentDeviceType;
        }
    }
}
