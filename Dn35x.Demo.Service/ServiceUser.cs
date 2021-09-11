using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Dn35x.Demo.Service
{
    public class ServiceUser
    {
        private const int SC_MANAGER_ALL_ACCESS = 0x000F003F;
        private const uint SERVICE_NO_CHANGE = 0xffffffff;  //这个值可以在 winsvc.h 中找到
        private const uint SERVICE_QUERY_CONFIG = 0x00000001;
        private const uint SERVICE_CHANGE_CONFIG = 0x00000002;

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ChangeServiceConfig(
            IntPtr hService,
            uint nServiceType,
            uint nStartType,
            uint nErrorControl,
            string lpBinaryPathName,
            string lpLoadOrderGroup,
            IntPtr lpdwTagId,
            [In] char[] lpDependencies,
            string lpServiceStartName,
            string lpPassword,
            string lpDisplayName);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenSCManager(string machineName, string databaseName, uint dwAccess);

        public static void ChangeServiceAccountInfo(string serviceName, string username, string password)
        {
            IntPtr scm_Handle = OpenSCManager(null, null, SC_MANAGER_ALL_ACCESS);
            if (scm_Handle == IntPtr.Zero)
                throw new ExternalException("打开服务管理器错误");

            IntPtr service_Handle = OpenService(scm_Handle, serviceName, SERVICE_QUERY_CONFIG | SERVICE_CHANGE_CONFIG);
            if (service_Handle == IntPtr.Zero)
                throw new ExternalException("打开服务错误");

            //修改服务的账户用户名和密码
            if (!ChangeServiceConfig(service_Handle, SERVICE_NO_CHANGE, SERVICE_NO_CHANGE,
                 SERVICE_NO_CHANGE, null, null, IntPtr.Zero, null, username, password, null))
            {
                int nError = Marshal.GetLastWin32Error();
                string msg = string.Format("无法修改服务登录身份的用户名和密码：{0:X8}", nError);
                throw new ExternalException(msg);
            }
        }
    }
}
