using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace SoundMachine
{
    [Serializable]
    class Config
    {
        public static string WorkingDir;
        public static Config CurrentConfig;
        
        private int _maxSounds;
        public int MaxSounds { 
            get { return _maxSounds; } 
            set { if (_maxSounds != value)
                {
                    _maxSounds = value;
                    SaveConfig();
                }
            } 
        }

        private List<string> _profiles;
        public List<string> Profiles { 
            get { return _profiles; }
            set
            {
                if (_profiles != value)
                {
                    _profiles = value;
                }
            }
        }

        private int _currentProfile;
        public int CurrentProfile { 
            get { return _currentProfile; }
            set
            {
                if (_currentProfile != value)
                {
                    if (value < Profiles.Count && value > 0)
                        _currentProfile = value;
                    else
                        _currentProfile = 0;
                    SoundProfile.LoadSoundProfile(Profiles[_currentProfile]);
                    SaveConfig();
                }
            }
        }

        private int _currentOutputDevice;
        public int CurrentOutputDevice { 
            get { return _currentOutputDevice; }
            set
            {
                if (_currentOutputDevice != value)
                {
                    _currentOutputDevice = value;
                    SaveConfig();
                }
            }
        }

        private int _currentInputDevice;
        public int CurrentInputDevice { 
            get { return _currentInputDevice; }
            set
            {
                if (_currentInputDevice != value)
                {
                    _currentInputDevice = value;
                    SaveConfig();
                }
            }
        }

        private int _soundPlaybackDevice;
        public int SoundPlaybackDevice {
            get { return _soundPlaybackDevice; }
            set
            {
                if (_soundPlaybackDevice != value)
                {
                    _soundPlaybackDevice = value;
                    SaveConfig();
                }
            }
        }

        private int _currentVolume;
        public int CurrentVolume { 
            get { return _currentVolume; }
            set
            {
                if (_currentVolume != value)
                {
                    _currentVolume = value;
                    SaveConfig();
                }
            }
        }

        private bool _inputPassthroughEnabled;
        public bool InputPassthroughEnabled { 
            get { return _inputPassthroughEnabled; }
            set
            {
                if (_inputPassthroughEnabled != value)
                {
                    _inputPassthroughEnabled = value;
                    SaveConfig();

                    if(value == true)
                    {
                        Thread t = new Thread(SoundSystem.ContinuousInputPlayback);
                        t.Start();
                    }
                }
            }
        }

        private bool _inputPlaybackEnabled;
        public bool InputPlaybackEnabled { 
            get { return _inputPlaybackEnabled; }
            set
            {
                if (_inputPlaybackEnabled != value)
                {
                    _inputPlaybackEnabled = value;
                    SaveConfig();
                }
            }
        }

        private int _inputChannels;
        public int InputChannels { 
            get { return _inputChannels; }
            set
            {
                if (_inputChannels != value)
                {
                    _inputChannels = value;
                    SaveConfig();
                }
            }
        }
        
        private int _inputSampleRate;
        public int InputSampleRate { 
            get { return _inputSampleRate; }
            set
            {
                if (_inputSampleRate != value)
                {
                    _inputSampleRate = value;
                    SaveConfig();
                }
            }
        }

        private bool _soundPlaybackEnabled;
        public bool SoundPlaybackEnabled { 
            get { return _soundPlaybackEnabled; }
            set
            {
                if (_soundPlaybackEnabled != value)
                {
                    _soundPlaybackEnabled = value;
                    SaveConfig();
                }
            }
        }

        private bool _muteInputWithSoundSystem;
        public bool MuteInputWithSoundSystem
        {
            get { return _muteInputWithSoundSystem; }
            set
            {
                if (_muteInputWithSoundSystem != value)
                {
                    _muteInputWithSoundSystem = value;
                    SaveConfig();
                }
            }
        }

        private bool _interruptKeys;
        public bool InterruptKeys { 
            get { return _interruptKeys; }
            set
            {
                if (_interruptKeys != value)
                {
                    _interruptKeys = value;
                    SaveConfig();
                }
            }
        }

        private int _toggleSystemBinding;
        public int ToggleSystemBinding { 
            get { return _toggleSystemBinding; }
            set
            {
                if (_toggleSystemBinding != value)
                {
                    _toggleSystemBinding = value;
                    SaveConfig();
                }
            }
        }

        private int _toggleModeBinding;
        public int ToggleModeBinding { 
            get { return _toggleModeBinding; }
            set
            {
                if (_toggleModeBinding != value)
                {
                    _toggleModeBinding = value;
                    SaveConfig();
                }
            }
        }

        private int _toggleOverlayBinding;
        public int ToggleOverlayBinding
        {
            get { return _toggleOverlayBinding; }
            set
            {
                if (_toggleOverlayBinding != value)
                {
                    _toggleOverlayBinding = value;
                    SaveConfig();
                }
            }
        }

        private int _toggleProfileBinding;
        public int ToggleProfileBinding
        {
            get { return _toggleProfileBinding; }
            set
            {
                if (_toggleProfileBinding != value)
                {
                    _toggleProfileBinding = value;
                    SaveConfig();
                }
            }
        }

        private int _recordBinding;
        public int RecordBinding
        {
            get { return _recordBinding; }
            set
            {
                if (_recordBinding != value)
                {
                    _recordBinding = value;
                    SaveConfig();
                }
            }
        }

        private SoundSystem.SoundMode _inputMode;
        public SoundSystem.SoundMode InputMode { 
            get { return _inputMode; }
            set
            {
                if (_inputMode != value)
                {
                    _inputMode = value;
                    SaveConfig();
                }
            }
        }

        public Config(int maxButtons)
        {
            _maxSounds = maxButtons;
            _profiles = new List<string>();
            _currentOutputDevice = 0;
            _currentInputDevice = 0;
            _soundPlaybackDevice = 0;
            _currentVolume = 10;
            _inputPassthroughEnabled = true;
            _inputPlaybackEnabled = true;
            _inputChannels = 1;
            _inputSampleRate = 44100;
            _soundPlaybackEnabled = true;
            _interruptKeys = false;
            _inputMode = SoundSystem.SoundMode.Interrupt;
            _currentProfile = 0;
            _recordBinding = (int)Keys.LControlKey;
            _profiles.Add("Profile 1");
        }

        public Config(Config blueprint)
        {
            _maxSounds = blueprint.MaxSounds < 10 ? 10 : blueprint.MaxSounds;
            _currentOutputDevice = blueprint.CurrentOutputDevice;
            _currentInputDevice = blueprint.CurrentInputDevice;
            _soundPlaybackDevice = blueprint.SoundPlaybackDevice;
            _currentVolume = blueprint.CurrentVolume;
            _inputPassthroughEnabled = blueprint.InputPassthroughEnabled;
            _inputPlaybackEnabled = blueprint.InputPlaybackEnabled;
            _soundPlaybackEnabled = blueprint.SoundPlaybackEnabled;
            _interruptKeys = blueprint.InterruptKeys;
            _inputChannels = blueprint.InputChannels == 0 ? 1 : blueprint.InputChannels;
            _inputSampleRate = blueprint.InputSampleRate == 0 ? 44100 : blueprint.InputSampleRate;
            _muteInputWithSoundSystem = blueprint.MuteInputWithSoundSystem;
            _toggleSystemBinding = blueprint.ToggleSystemBinding;
            _toggleModeBinding = blueprint.ToggleModeBinding;
            _toggleProfileBinding = blueprint.ToggleProfileBinding;
            _toggleOverlayBinding = blueprint.ToggleOverlayBinding;
            _recordBinding = blueprint.RecordBinding == 0 ? (int)Keys.LControlKey : blueprint.RecordBinding;
            _inputMode = blueprint.InputMode;
            _profiles = blueprint.Profiles == null ? new List<string>() : blueprint.Profiles;
            if (_profiles.Count == 0)
                _profiles.Add("Profile 1");
            _currentProfile = blueprint.CurrentProfile;
        }

        public void SaveConfig()
        {
            BinaryFormatter bf = new BinaryFormatter();

            using (StreamWriter sw = new StreamWriter(WorkingDir + "Config.cfg"))
            {
                bf.Serialize(sw.BaseStream, this);
                sw.Close();
            }
        }

        public static Config LoadConfig(int MaxButtons)
        {
            if (!Directory.Exists(WorkingDir))
            {
                Directory.CreateDirectory(WorkingDir);
            }

            if (File.Exists(WorkingDir + "Config.cfg"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(WorkingDir + "Config.cfg"))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        CurrentConfig = new Config((Config)bf.Deserialize(sr.BaseStream));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    File.Delete(WorkingDir + "Config.cfg");
                    CurrentConfig = new Config(MaxButtons);
                }
            }
            else
            {
                CurrentConfig = new Config(MaxButtons);
            }

            return CurrentConfig;
        }
    }
}
