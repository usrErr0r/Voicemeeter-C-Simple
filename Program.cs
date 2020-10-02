using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace VoiceMeeterControl
{
    class Program
    {
        public enum VbLoginResponse
        {
            OK = 0,
            OkVoicemeeterNotRunning = 1,
            NoClient = -1,
            AlreadyLoggedIn = -2,
        }

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_Login")]
        public static extern VbLoginResponse Login();
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_Logout")]
        public static extern VbLoginResponse Logout();

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_SetParameterFloat")]
        public static extern int SetParameter(string szParamName, float value);
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_GetParameterFloat")]
        public static extern int GetParameter(string szParamName, ref float value);
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_GetParameterStringA")]
        public static extern int GetParameter(string szParamName, ref string value);

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_IsParametersDirty")]
        public static extern int IsParametersDirty();
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);
        private static IntPtr? _dllHandle;
        public static void LoadDll(string dllPath)
        {
            if (!_dllHandle.HasValue)
            {
                _dllHandle = LoadLibrary(dllPath);
            }
        }
        public static float gain;



        static void Main(string[] args)
        {
            float GetParam(string n)
            {
                float output = -1;
                GetParameter(n, ref output);
                return output;
            }
            var vmDir = ("C:\\Program Files (x86)\\VB\\Voicemeeter");
            LoadDll(System.IO.Path.Combine(vmDir, "VoicemeeterRemote.dll"));
            var lr = Login();
            gain = GetParam("Strip[5].gain");
            Console.WriteLine(gain);
            Logout();
        }

    }
}
