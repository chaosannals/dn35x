using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Dn35x.Base
{
    public static class ProcessExtends
    {
        public static bool Is32bit()
        {
            return IntPtr.Size == 4;
        }

        public static bool Is64bit()
        {
            return IntPtr.Size == 8;
        }

        public static bool IsWow64(this Process process)
        {
            try
            {
                bool result;
                return IsWow64Process(process.Handle, out result) && result;
            }
            catch (EntryPointNotFoundException)
            {
                return false;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);
    }
}
