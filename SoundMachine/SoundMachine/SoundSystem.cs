using System;
using System.Collections.Generic;
using NAudio.Wave;
using NAudio;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SoundMachine
{
    class SoundSystem
    {
        private static WasapiLoopbackCapture _waveSource;
        private static WaveFileWriter _waveFile;
        private static bool _readyForRecording = true;
        private static bool _killSignal = false;
        private static bool _stopListening = false;
        //Lists so that mulitple sounds can be played on the same time
        private static List<WaveFileReader> readers = new List<WaveFileReader>();
        private static List<WaveOutEvent> players = new List<WaveOutEvent>();
        private static WaveInEvent continuousWi;
        private static WaveOutEvent continuousWo;
        private static BufferedWaveProvider provider;

        //For playing sounds, called from KeyLogger.cs
        public static bool PlaySound(int number)
        {
            if (number == -1)
                return false;

            string fi = Config._currentConfig.Sounds[number]; //gets location+filename of sound from config determined by numpad number 

            if (fi != null && fi != "")
            {
                WaveOutEvent wo = new WaveOutEvent();
                wo.DeviceNumber = Config._currentConfig.CurrentOutputDevice;
                wo.PlaybackStopped += new EventHandler<StoppedEventArgs>(PlayBackStopped); //To cleanup resources when done
                players.Add(wo); //To find the resources again

                string ext = fi.Substring(fi.LastIndexOf(".")).ToLower();

                if (ext == ".mp3" || ext == ".wav")
                {
                    WaveFileReader reader = new WaveFileReader(fi);
                    readers.Add(reader);

                    try
                    {
                        wo.Init(reader);
                        wo.Play();
                    }
                    catch (MmException e)
                    {
                        MessageBox.Show("Error!\n" + e.Message + "\n\nCan't play audio:\nSelected audio device is already in use!");
                    }
                }
            }

            return true;
        }

        //For the dropdown menu and NAudio
        public static string[] GetOutputDevices()
        {
            List<string> value = new List<string>();
            for (int deviceId = 0; deviceId < WaveOut.DeviceCount; deviceId++)
            {
                value.Add(WaveOut.GetCapabilities(deviceId).ProductName);
            }

            return value.ToArray();
        }

        //For the dropdown menu and NAudio
        public static string[] GetInputDevices()
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

            if (_readyForRecording)
            {
                //we only want one recordingsession at a time
                _readyForRecording = false;

                _waveSource = new WasapiLoopbackCapture();
                _waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                _waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

                string saveFile = Form1._currentForm._WORKINGDIR + "Sounds\\" + "Macro" + number + ".wav";
                //checks if file exists
                if (Config._currentConfig.Sounds[number] != null)
                {
                    if (File.Exists(Config._currentConfig.Sounds[number]))
                    {
                        File.Delete(Config._currentConfig.Sounds[number]);
                    }
                }
                if (File.Exists(saveFile))
                {
                    File.Delete(saveFile);
                }

                //Saves to config
                Config._currentConfig.Sounds[number] = saveFile;
                Config._currentConfig.Texts[number] = "Macro" + number;

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
            WaveOutEvent wo = (WaveOutEvent)sender;
            int index = players.FindIndex(a => a == wo);
            players[index].Dispose();
            players[index] = null;
            players.RemoveAt(index);
            readers[index].Dispose();
            readers[index] = null;
            readers.RemoveAt(index);
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
            while (_killSignal == false)
            {
                _stopListening = false;
                continuousWi = new WaveInEvent();
                continuousWi.WaveFormat = new WaveFormat(96000, 24, 1);

                continuousWo = new WaveOutEvent();
                continuousWi.DeviceNumber = Config._currentConfig.CurrentInputDevice;
                continuousWo.DeviceNumber = Config._currentConfig.CurrentOutputDevice;

                provider = new BufferedWaveProvider(continuousWi.WaveFormat);
                continuousWo.Init(provider);
                continuousWi.DataAvailable += new EventHandler<WaveInEventArgs>(waveInput_DataAvailable);
                continuousWi.StartRecording();

                while (_stopListening == false) { Thread.Sleep(1); }

                continuousWi.StopRecording();
                provider.ClearBuffer();
                continuousWo.Stop();
            }
        }

        //Event, writes data from buffer
        private static void waveInput_DataAvailable(object sender, WaveInEventArgs e)
        {
            provider.AddSamples(e.Buffer, 0, e.Buffer.Length);
            continuousWo.Volume = Config._currentConfig.CurrentVolume / 10.0f;
            continuousWo.Play();
        }

        public static void resetListener()
        {
            _stopListening = true;
        }

        public static void Kill()
        {
            _killSignal = true;
            _stopListening = true;
        }
    }
}
