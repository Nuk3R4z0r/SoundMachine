using Microsoft.Win32;
using System.IO;

namespace SoundMachine
{
    class Utilities
    {
        public static int ConvertKeyCodeToButtonId(int vkCode)
        {
            int id = -1;

            for(int i = 0; i < SoundProfile.CurrentSoundProfile.Bindings.Length; i++)
            {
                if (SoundProfile.CurrentSoundProfile.Bindings[i] == vkCode)
                {
                    id = i;
                    return id;
                }
            }

            return id;
        }


        public static void MoveProfile(string oldProfile, string newProfile)
        {
            if (Directory.Exists(Config.WorkingDir + newProfile))
                Directory.Delete(Config.WorkingDir + newProfile, true);

            File.Move(Config.WorkingDir + oldProfile + "\\" + oldProfile + ".profile", Config.WorkingDir + oldProfile + "\\" + newProfile + ".profile");
            Directory.Move(Config.WorkingDir + oldProfile, Config.WorkingDir + newProfile);
            SoundProfile.CurrentSoundProfile.ProfileName = newProfile;
        }

        public static bool ImportProfile(string fd)
        { 
            string filePath = fd.Substring(0, fd.LastIndexOf("\\") + 1);
            string fullFileName = fd.Substring(fd.LastIndexOf("\\") + 1);
            string fileName = fullFileName.Substring(0, fullFileName.LastIndexOf("."));

            if (!Config._currentConfig.Profiles.Contains(fileName) || fd != Config.WorkingDir + fileName + "\\" + fullFileName)
            {
                Directory.CreateDirectory(Config.WorkingDir + fileName);
                if (!File.Exists(Config.WorkingDir + fileName + "\\" + fullFileName))
                {
                    File.Copy(fd, Config.WorkingDir + fileName + "\\" + fullFileName);
                }

                Directory.CreateDirectory(Config.WorkingDir + fileName + "\\" + "Sounds");
                foreach (string oldPath in Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories))
                {
                    string newPath = oldPath.Replace(filePath, Config.WorkingDir + fileName + "\\");
                    if (!File.Exists(newPath))
                        File.Copy(oldPath, newPath, true);
                }

                Config._currentConfig.Profiles.Add(fileName);
                Config._currentConfig.CurrentProfile = Config._currentConfig.Profiles.Count - 1;

                return true;
            }
            else
                return false;
        }

        public static void MoveHome(string newHome)
        {
            if(!newHome.Contains("SoundMachine\\"))
                newHome = newHome + "SoundMachine\\";
            SoundSystem.KillAllSounds();
            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(Config.WorkingDir, newHome, true);
            Config.WorkingDir = newHome;

            RegistryKey WorkingDirectory = Registry.CurrentUser.CreateSubKey("SoundMachine");
            WorkingDirectory.SetValue("WorkingDirectory", newHome);
        }
    }
}