// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local


using System;
using System.Runtime.InteropServices;


namespace Shell32Interop {
    public static class Shell32 {
        [DllImport ("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx (ref SHELLEXECUTEINFO lpExecInfo);

        [StructLayout (LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHELLEXECUTEINFO {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs (UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs (UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs (UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs (UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs (UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;


        public static bool ShellExecuteEx (string lpFile, string lpVerb) {
            var shellexecuteinfo = new SHELLEXECUTEINFO ();
            shellexecuteinfo.cbSize = Marshal.SizeOf (shellexecuteinfo);
            shellexecuteinfo.lpVerb = lpVerb;
            shellexecuteinfo.lpFile = lpFile;
            shellexecuteinfo.nShow = SW_SHOW;
            shellexecuteinfo.fMask = SEE_MASK_INVOKEIDLIST;

            return ShellExecuteEx (ref shellexecuteinfo);
        }
    }
}
