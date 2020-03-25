using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SoundMachine
{
    //Inspired by https://null-byte.wonderhowto.com/how-to/create-simple-hidden-console-keylogger-c-sharp-0132757/
    class KeyListener
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private static bool isRecording = false;

        public static LowLevelKeyboardProc _proc = HookCallback;
        public static IntPtr _hookID = IntPtr.Zero;
        public static bool changeBinding = false;


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (changeBinding)
                {
                    for (int i = 0; i < Config._currentConfig.Bindings.Length; i++)
                    {
                        int tempCode = Config._currentConfig.Bindings[i];
                        if (tempCode == vkCode)
                        {
                            Config._currentConfig.Bindings[i] = 0;
                            SetBindingForm.RemovedBinding = i;
                            break;
                        }
                    }
                    Config._currentConfig.Bindings[SetBindingForm.CurrentButton] = vkCode; //Set new binding
                    SetBindingForm.NewBindingSet = true;
                    SetBindingForm._currentForm.Close();
                }
                else
                {
                    if ((Keys)vkCode == Keys.LControlKey) //Control button on keyboard
                        isRecording = true; //Enables recording

                    else if (!isRecording)
                    {
                        if(SoundSystem.PlaySound(Utilities.ConvertKeyCodeToButtonId(vkCode)))
                            if (Config._currentConfig.InterruptKeys)
                                return (System.IntPtr)1;
                    }
                    else
                    {
                        if(SoundSystem.RecordSound(Utilities.ConvertKeyCodeToButtonId(vkCode)))
                            if (Config._currentConfig.InterruptKeys)
                                return (System.IntPtr)1;
                    }
                }
                
            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if ((Keys)vkCode == Keys.LControlKey) //Control button on keyboard
                {
                    isRecording = false;
                    SoundSystem.StopRecording(); //if ctrl is released stop all recording
                }
            }
            
            
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }
}
