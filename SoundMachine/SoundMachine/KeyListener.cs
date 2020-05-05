using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SoundMachine
{
    //Inspired by https://null-byte.wonderhowto.com/how-to/create-simple-hidden-console-keylogger-c-sharp-0132757/
    public class KeyListener
    {
        public enum KeyBinding
        {
            Sound,
            ToggleSystem,
            ToggleMode,
            ToggleProfile,
            ToggleOverlay,
            Record
        }
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static bool isRecording = false;
        public static bool _listenerEnabled;
        public static bool changeBinding = false;
        public static LowLevelKeyboardProc _proc = HookCallback;
        public static IntPtr _hookID = IntPtr.Zero;
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            IntPtr hInstance = LoadLibrary("user32");
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, hInstance, 0);
        }

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            if (!_listenerEnabled && !changeBinding && vkCode != Config._currentConfig.ToggleSystemBinding)
            {
                return CallNextHookEx(_hookID, nCode, wParam, lParam);
            }

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                if (changeBinding)
                {
                    if (SetBindingForm._currentForm.BindingType == KeyBinding.ToggleOverlay)
                        Config._currentConfig.ToggleOverlayBinding = vkCode;
                    else if (SetBindingForm._currentForm.BindingType == KeyBinding.ToggleProfile)
                        Config._currentConfig.ToggleProfileBinding = vkCode;
                    else if (SetBindingForm._currentForm.BindingType == KeyBinding.ToggleSystem)
                        Config._currentConfig.ToggleSystemBinding = vkCode;
                    else if (SetBindingForm._currentForm.BindingType == KeyBinding.ToggleMode)
                        Config._currentConfig.ToggleModeBinding = vkCode;
                    else if (SetBindingForm._currentForm.BindingType == KeyBinding.Record)
                        Config._currentConfig.RecordBinding = vkCode;
                    else
                    {
                        for (int i = 0; i < SoundProfile.CurrentSoundProfile.Bindings.Length; i++)
                        {
                            int tempCode = SoundProfile.CurrentSoundProfile.Bindings[i];
                            if (tempCode == vkCode && SetBindingForm._currentForm.CurrentButton != i)
                            {
                                SoundProfile.CurrentSoundProfile.Bindings[i] = 0;
                                SetBindingForm._currentForm.RemovedBinding = i;
                                break;
                            }
                            SoundProfile.CurrentSoundProfile.Bindings[SetBindingForm._currentForm.CurrentButton] = vkCode;
                            SoundProfile.CurrentSoundProfile.SaveSoundProfile();
                        }
                    } //Set new binding
                    SetBindingForm._currentForm.NewBindingSet = true;
                    SetBindingForm._currentForm.Close();
                    return (System.IntPtr)1;
                }
                else
                {
                    if (vkCode == Config._currentConfig.ToggleOverlayBinding)
                    {
                        if (Overlay._currentOverlay.Visible)
                            Overlay._currentOverlay.Hide();
                        else
                            Overlay._currentOverlay.Show();
                        return (System.IntPtr)1;
                    }
                    else if (vkCode == Config._currentConfig.ToggleProfileBinding)
                    {
                        SoundProfile.Next();
                        return (System.IntPtr)1;
                    }
                    else if (vkCode == Config._currentConfig.ToggleSystemBinding)
                    {
                        Form1._currentForm.ToggleSystemEnabled(null);
                        Overlay._currentOverlay.UpdateStatusColor();
                        return (System.IntPtr)1;
                    }
                    else if (vkCode == Config._currentConfig.ToggleModeBinding)
                    {
                        SoundSystem.KillAllSounds();
                        SoundSystem.SwitchSoundMode();
                        if (SettingsForm._currentForm != null)
                            SettingsForm._currentForm.SetKeyPressBoxSelectedIndex();
                        return (System.IntPtr)1;
                    }
                    else if (vkCode == Config._currentConfig.RecordBinding) //Control button on keyboard
                        isRecording = true; //Enables recording
                    else if (!isRecording)
                    {
                        if (SoundSystem.PlaySound(Utilities.ConvertKeyCodeToButtonId(vkCode)))
                            if (Config._currentConfig.InterruptKeys)
                                return (System.IntPtr)1;
                    }
                    else
                    {
                        if (SoundSystem.RecordSound(Utilities.ConvertKeyCodeToButtonId(vkCode)))
                            if (Config._currentConfig.InterruptKeys)
                                return (System.IntPtr)1;
                    }
                }
            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                if (vkCode == Config._currentConfig.RecordBinding) //Control button on keyboard
                {
                    isRecording = false;
                    SoundSystem.StopRecording(); //if ctrl is released stop all recording
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
