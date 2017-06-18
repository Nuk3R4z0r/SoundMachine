using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MacroMachine
{
    class KeyLogger
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        public static LowLevelKeyboardProc _proc = HookCallback;
        public static IntPtr _hookID = IntPtr.Zero;
        private static bool isCtrl = false;

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

                if (vkCode == 162) //Control
                    isCtrl = true;
                else if (!isCtrl)
                    MacroCheck(vkCode);
                else
                    RecordCheck(vkCode);
            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (vkCode == 162) //Control
                {
                    isCtrl = false;
                    SoundSystem.StopRecording();
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public static void MacroCheck(int vkCode)
        {
            if ((Keys)vkCode == Keys.NumPad0)
            {
                SoundSystem.PlayMacro(0);
            }
            else if ((Keys)vkCode == Keys.NumPad1)
            {
                SoundSystem.PlayMacro(1);
            }
            else if ((Keys)vkCode == Keys.NumPad2)
            {
                SoundSystem.PlayMacro(2);
            }
            else if ((Keys)vkCode == Keys.NumPad3)
            {
                SoundSystem.PlayMacro(3);
            }
            else if ((Keys)vkCode == Keys.NumPad4)
            {
                SoundSystem.PlayMacro(4);
            }
            else if ((Keys)vkCode == Keys.NumPad5)
            {
                SoundSystem.PlayMacro(5);
            }
            else if ((Keys)vkCode == Keys.NumPad6)
            {
                SoundSystem.PlayMacro(6);
            }
            else if ((Keys)vkCode == Keys.NumPad7)
            {
                SoundSystem.PlayMacro(7);
            }
            else if ((Keys)vkCode == Keys.NumPad8)
            {
                SoundSystem.PlayMacro(8);
            }
            else if ((Keys)vkCode == Keys.NumPad9)
            {
                SoundSystem.PlayMacro(9);
            }
        }

        public static void RecordCheck(int vkCode)
        {
            if ((Keys)vkCode == Keys.NumPad0)
            {
                SoundSystem.StartRecording(0);
            }
            else if ((Keys)vkCode == Keys.NumPad1)
            {
                SoundSystem.StartRecording(1);
            }
            else if ((Keys)vkCode == Keys.NumPad2)
            {
                SoundSystem.StartRecording(2);
            }
            else if ((Keys)vkCode == Keys.NumPad3)
            {
                SoundSystem.StartRecording(3);
            }
            else if ((Keys)vkCode == Keys.NumPad4)
            {
                SoundSystem.StartRecording(4);
            }
            else if ((Keys)vkCode == Keys.NumPad5)
            {
                SoundSystem.StartRecording(5);
            }
            else if ((Keys)vkCode == Keys.NumPad6)
            {
                SoundSystem.StartRecording(6);
            }
            else if ((Keys)vkCode == Keys.NumPad7)
            {
                SoundSystem.StartRecording(7);
            }
            else if ((Keys)vkCode == Keys.NumPad8)
            {
                SoundSystem.StartRecording(8);
            }
            else if ((Keys)vkCode == Keys.NumPad9)
            {
                SoundSystem.StartRecording(9);
            }
        }
    }
}
