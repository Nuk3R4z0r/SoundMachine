using System;
using System.Collections.Generic;
using NAudio.Wave;
using NAudio;
using System.IO;
using System.Windows.Forms;

namespace MacroMachine
{
    class SoundSystem
    {
        private static WasapiLoopbackCapture _waveSource;
        private static WaveFileWriter _waveFile;
        private static bool _readyForRecording = true;
        //Lists so that mulitple sounds can be played on the same time
        private static List<WaveFileReader> readers = new List<WaveFileReader>();
        private static List<WaveOutEvent> players = new List<WaveOutEvent>();

        //For playing sounds, called from KeyLogger.cs
        public static void PlayMacro(int number)
        {
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
                    catch(MmException e)
                    {
                        MessageBox.Show("Error!\n" + e.Message + "\n\nCan't play audio:\nSelected audio device is already in use!");
                    }
                }
            }
        }

        //For the dropdown menu and NAudio
        public static string[] GetDevices()
        {
            List<string> value = new List<string>();
            for (int deviceId = 0; deviceId < WaveOut.DeviceCount; deviceId++)
            {
                value.Add(WaveOut.GetCapabilities(deviceId).ProductName);
            }

            return value.ToArray();
        }

        public static void StartRecording(int macroSpot)
        {
            if (_readyForRecording)
            {
                //we only want one recordingsession at a time
                _readyForRecording = false;

                _waveSource = new WasapiLoopbackCapture();
                _waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                _waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

                string saveFile = Form1._currentForm._WORKINGDIR + "Sounds\\" + "Macro" + macroSpot + ".wav";
                //checks if file exists
                if (Config._currentConfig.Sounds[macroSpot] != null)
                {
                    if (File.Exists(Config._currentConfig.Sounds[macroSpot]))
                    {
                        File.Delete(Config._currentConfig.Sounds[macroSpot]);
                    }
                }
                if (File.Exists(saveFile))
                {
                    File.Delete(saveFile);
                }

                //Saves to config
                Config._currentConfig.Sounds[macroSpot] = saveFile;
                Config._currentConfig.Texts[macroSpot] = "Macro" + macroSpot;

                //Starts recording which starts DataAvailable event
                _waveFile = new WaveFileWriter(saveFile, _waveSource.WaveFormat);
                _waveSource.StartRecording();

                //For UI
                Form1._currentForm.UpdateTextbox(macroSpot, "Macro" + macroSpot);
            }
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
            readers[index].Close();
            readers[index].Dispose();
            wo.Dispose();
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
    }
}
