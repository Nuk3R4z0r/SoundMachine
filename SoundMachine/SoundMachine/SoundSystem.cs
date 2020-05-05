using System;
using System.Collections.Generic;
using NAudio.Wave;
using NAudio;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SoundMachine
{
    public class SoundSystem
    {
        public enum SoundMode
        {
            Stack,
            Interrupt,
            StartStop,
            Loop
        }
        private enum SoundState
        {
            Null,
            Started,
            Stopped
        }

        private static WasapiLoopbackCapture _waveSource;
        private static WaveFileWriter _waveFile;
        private static bool _readyForRecording = true;
        private static bool _killSignal;
        private static bool _stopListening;
        //Lists so that mulitple sounds can be played on the same time
        private static List<Guid> OutputGuids;
        private static List<WaveFileReader> readers = new List<WaveFileReader>();
        private static List<WaveOutModEvent> players = new List<WaveOutModEvent>();
        private static WaveFileReader[] stoppableReaders;
        private static WaveOutModEvent[] stoppablePlayers;
        private static SoundState[] stoppableState;
        private static WaveInEvent continuousWi;
        private static DirectSoundOut continuousWo;
        private static BufferedWaveProvider provider;
        public static bool InputDisabled;

        public static void InitializeStoppables()
        {
            stoppableReaders = new WaveFileReader[Config._currentConfig.MaxSounds * 2];
            stoppablePlayers = new WaveOutModEvent[Config._currentConfig.MaxSounds * 2];
            stoppableState = new SoundState[Config._currentConfig.MaxSounds];
        }

        private static void AddResourcesToPool(int number, WaveFileReader reader, WaveOutModEvent player, bool isPlayback)
        {
            if (Config._currentConfig.InputMode == SoundMode.StartStop || Config._currentConfig.InputMode == SoundMode.Loop)
            {
                player.SoundNumber = number;
                player.Stoppable = true;
                    
                stoppableState[number] = SoundState.Started;
                if (isPlayback)
                    number += Config._currentConfig.MaxSounds;

                stoppablePlayers[number] = player;
                stoppableReaders[number] = reader;
            }
            else
            {
                player.SoundNumber = number;
                readers.Add(reader);
                players.Add(player);//To find the resources again
            }
        }

        public static void PlaySoundToDefaultOutput(int number, string fi)
        {
            WaveOutModEvent listenWo = new WaveOutModEvent();
            listenWo.DeviceNumber = -1;
            listenWo.PlaybackStopped += new EventHandler<StoppedEventArgs>(PlayBackStopped);
            listenWo.Volume = Config._currentConfig.CurrentVolume / 10.0f;

            WaveFileReader reader = new WaveFileReader(fi);
            AddResourcesToPool(number, reader, listenWo, true);

            listenWo.Init(reader);
            listenWo.Play();
        }

        //For playing sounds, called from KeyLogger.cs
        public static bool PlaySound(int number)
        {
            if (number == -1)
                return false;
 
            switch(Config._currentConfig.InputMode)
            {
                case SoundMode.Stack:
                    StackSound(number);
                    break;
                case SoundMode.Interrupt:
                    KillAllSounds();
                    StackSound(number);
                    break;
                case SoundMode.StartStop:
                    StartStopSound(number);
                    break;
                case SoundMode.Loop:
                    StartStopSound(number);
                    break;
            }

            return true;
        }

        private static void StartStopSound(int number)
        {
            if (stoppableState[number] == SoundState.Started)
            {
                if (Config._currentConfig.SoundPlaybackEnabled)
                    if(stoppablePlayers[number + Config._currentConfig.MaxSounds] != null)
                        stoppablePlayers[number + Config._currentConfig.MaxSounds].Pause();
                stoppablePlayers[number].Pause();
                stoppableState[number] = SoundState.Stopped;
            }
            else if(stoppableState[number] == SoundState.Stopped)
            {
                stoppablePlayers[number].Volume = Config._currentConfig.CurrentVolume / 10.0f;

                if (Config._currentConfig.SoundPlaybackEnabled)
                    stoppablePlayers[number + Config._currentConfig.MaxSounds].Play();
                stoppablePlayers[number].Play();
                stoppableState[number] = SoundState.Started;
            }
            else
            {
                StackSound(number);
            }
        }
        
        private static void StackSound(int number)
        {
            string fi = SoundProfile.CurrentSoundProfile.Sounds[number]; //gets location+filename of sound from config determined by numpad number
            if (fi == null)
                return;

            WaveOutModEvent wo = new WaveOutModEvent();
            wo.DeviceNumber = Config._currentConfig.CurrentOutputDevice;
                wo.PlaybackStopped += new EventHandler<StoppedEventArgs>(PlayBackStopped); //To cleanup resources when done

            string ext = fi.Substring(fi.LastIndexOf(".")).ToLower();

            if (ext == ".mp3" || ext == ".wav")
            {
                WaveFileReader reader;
                try
                {
                    reader = new WaveFileReader(fi);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }

                try
                {
                    wo.Init(reader);
                }
                catch (MmException e)
                {
                    MessageBox.Show("Error!\n" + e.Message + "\n\nCan't play audio:\nSelected audio device is already in use!");
                    return;
                }


                wo.Volume = Config._currentConfig.CurrentVolume / 10.0f;
                wo.Play();

                if (Config._currentConfig.SoundPlaybackEnabled)
                    PlaySoundToDefaultOutput(number, fi);
                AddResourcesToPool(number, reader, wo, false);
            }
        }

        //For the dropdown menu and NAudio
        public static string[] PopulateOutputDevices()
        {
            OutputGuids = new List<Guid>();
            List<string> value = new List<string>();
            for (int deviceId = 0; deviceId < WaveOut.DeviceCount; deviceId++)
            {
                value.Add(WaveOut.GetCapabilities(deviceId).ProductName);
            }

            foreach (var device in DirectSoundOut.Devices)
            {
                OutputGuids.Add(device.Guid);
            }

            return value.ToArray();
        }

        //For the dropdown menu and NAudio
        public static string[] PopulateInputDevices()
        {
            List<string> value = new List<string>();
            for (int deviceId = 0; deviceId < WaveIn.DeviceCount; deviceId++)
            {
                value.Add(WaveIn.GetCapabilities(deviceId).ProductName);
            }

            return value.ToArray();
        }

        public static bool RecordSound(int number)
        {
            if (number == -1)
                return false;

            KillSound(number);

            if (_readyForRecording)
            {
                //we only want one recordingsession at a time
                _readyForRecording = false;

                _waveSource = new WasapiLoopbackCapture();
                _waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                _waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

                string saveFile = Config.WorkingDir + SoundProfile.CurrentSoundProfile.ProfileName + "\\Sounds\\" + "Macro" + number + ".wav";
                //checks if file exists
                if (File.Exists(saveFile))
                {
                    File.Delete(saveFile);
                }

                //Saves to config
                SoundProfile.CurrentSoundProfile.Sounds[number] = saveFile;
                SoundProfile.CurrentSoundProfile.Texts[number] = "Macro" + number;
                SoundProfile.CurrentSoundProfile.SaveSoundProfile();

                //Starts recording which starts DataAvailable event

                _waveFile = new WaveFileWriter(saveFile, _waveSource.WaveFormat);
                _waveSource.StartRecording();

                //For UI
                Form1._currentForm.UpdateTextbox(number, "Macro" + number);
            }

            return true;
        }

        public static void StopRecording()
        {
            if (!_readyForRecording)
            {
                //starts StopRecording event
                _waveSource.StopRecording();
                _readyForRecording = true;
            }
        }

        //Event, cleanup for playback
        static void PlayBackStopped(object sender, StoppedEventArgs e)
        {
            WaveOutModEvent wo = (WaveOutModEvent)sender;
            if (wo.Stoppable)
            {
                TimeSpan ts = new TimeSpan(0, 0, 0);
                float volume = Config._currentConfig.CurrentVolume / 10.0f;
                for (int i = 0; i < stoppablePlayers.Length; i++)
                {
                    if (stoppablePlayers[i] == wo)
                    {
                        if (Config._currentConfig.InputMode == SoundMode.Loop)
                        {
                            stoppableReaders[i].CurrentTime = ts;
                            stoppablePlayers[i].Volume = volume;
                            stoppablePlayers[i].Play();
                        }
                        else
                        {
                            DisposeSound(i);
                        }
                        break;
                    }
                }
            }
            else
            {
                int index = players.FindIndex(a => a == wo);
                if(index != -1)
                    DisposeSound(index);
            }
        }

        //Event, writes data from buffer
        private static void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (_waveFile != null)
            {
                _waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                _waveFile.Flush();
            }
        }

        //Event, cleanup for recording
        private static void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (_waveSource != null)
            {
                _waveSource.Dispose();
                _waveSource = null;
            }

            if (_waveFile != null)
            {
                _waveFile.Dispose();
                _waveFile = null;
            }
        }

        public static void ContinuousInputPlayback()
        {
            _killSignal = false;
            while (_killSignal == false)
            {
                _stopListening = false;
                continuousWi = new WaveInEvent();
                continuousWi.WaveFormat = new WaveFormat(Config._currentConfig.InputSampleRate, Config._currentConfig.InputChannels);
                continuousWi.DeviceNumber = Config._currentConfig.CurrentInputDevice;
                
                continuousWo = new DirectSoundOut(OutputGuids[Config._currentConfig.CurrentOutputDevice + 1], 40);

                provider = new BufferedWaveProvider(continuousWi.WaveFormat);
                VolumeWaveProvider16 vSampleProvider = new VolumeWaveProvider16(provider);
                continuousWo.Init(vSampleProvider);
                continuousWi.DataAvailable += new EventHandler<WaveInEventArgs>(waveInput_DataAvailable);
                continuousWi.StartRecording();
                continuousWo.Play();
                
                while (_stopListening == false)
                {
                    vSampleProvider.Volume = Config._currentConfig.CurrentVolume / 10.0f;
                    Thread.Sleep(25);
                }
                
                continuousWi.StopRecording();
                provider.ClearBuffer();
                continuousWo.Stop();
            }
        }

        //Event, writes data from buffer
        private static void waveInput_DataAvailable(object sender, WaveInEventArgs e)
        {
            if(!InputDisabled)
                provider.AddSamples(e.Buffer, 0, e.Buffer.Length);
        }
         
        public static void SwitchSoundMode()
        {
            if (Config._currentConfig.InputMode == SoundMode.StartStop)
                Config._currentConfig.InputMode = SoundMode.Stack;
            else if (Config._currentConfig.InputMode == SoundMode.Stack)
                Config._currentConfig.InputMode = SoundMode.Interrupt;
            else if(Config._currentConfig.InputMode == SoundMode.Interrupt)
                Config._currentConfig.InputMode = SoundMode.Loop;
            else
                Config._currentConfig.InputMode = SoundMode.StartStop;
            Overlay._currentOverlay.UpdateBehaviorText();
        }

        public static void KillSound(int number)
        {
            if (Config._currentConfig.InputMode == SoundMode.StartStop || Config._currentConfig.InputMode == SoundMode.Loop)
            {
                if (stoppablePlayers[number] != null)
                {
                    DisposeSound(number);
                }
                if (stoppablePlayers[number + Config._currentConfig.MaxSounds] != null)
                {
                    DisposeSound(number + Config._currentConfig.MaxSounds);
                }
            }
            else
                for (int i = 0; i < players.Count;)
                    if (players[i].SoundNumber == number)
                        DisposeSound(i);
                    else
                        i++;
        }

        public static void KillAllSounds()
        {
            if (Config._currentConfig.InputMode == SoundMode.StartStop || Config._currentConfig.InputMode == SoundMode.Loop)
            {
                for (int i = 0; i < stoppablePlayers.Length; i++)
                {
                    if (stoppablePlayers[i] != null)
                    {
                        DisposeSound(i);
                    }
                }
                InitializeStoppables();
            }
            else
            {
                while(players.Count != 0)
                {
                    if (readers[0] != null)
                    {
                        DisposeSound(0);
                    }
                }
            }
        }

        private static void DisposeSound(int number)
        {
            if (Config._currentConfig.InputMode == SoundMode.StartStop || Config._currentConfig.InputMode == SoundMode.Loop)
            {
                stoppablePlayers[number].Stop();
                stoppablePlayers[number].Dispose();
                stoppablePlayers[number] = null;
                stoppableReaders[number].Close();
                stoppableReaders[number].Dispose();
                stoppableReaders[number] = null;
                if(number < Config._currentConfig.MaxSounds)
                    stoppableState[number] = SoundState.Null;
            }
            else
            {
                players[number].Stop();
                players[number].Dispose();
                players.RemoveAt(number);
                readers[number].Close();
                readers[number].Dispose();
                readers.RemoveAt(number);
            }
        }

        public static void resetListener()
        {
            _stopListening = true;
        }

        public static void KillInputListener()
        {
            _killSignal = true;
            _stopListening = true;
        }
    }
}
