using System;
using System.IO;
using System.Diagnostics;

namespace Dn35x.Firewall
{
    public static class Netsh
    {
        public static Process NewProcess(string exePath)
        {
            Process process = new Process();
            process.StartInfo.FileName = "netsh";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
            return process;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exePath"></param>
        /// <returns></returns>
        public static string AddRule(string name, string exePath)
        {
            string systag = "未知";
            Process process = NewProcess(exePath);
            if (Environment.OSVersion.Version.Major <= 5)
            {
                systag = "XP系统";
                process.StartInfo.Arguments = string.Format("firewall add allowedprogram \"{0}\" \"{1}\" ENABLE", exePath, name);
            }
            else
            {
                systag = "Win7及以上系统";
                process.StartInfo.Arguments = string.Format("advfirewall firewall add rule name=\"{1}\" dir=in action=allow program=\"{0}\" enable=yes", exePath, name);
            }
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return string.Format("[{0}]: {1}", systag, result);
        }

        /// <summary>
        /// 删除。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exePath"></param>
        /// <returns></returns>
        public static string DeleteRule(string name, string exePath)
        {
            string systag = "未知系统";
            Process process = NewProcess(exePath);
            if (Environment.OSVersion.Version.Major <= 5)
            {
                systag = "XP系统";
                process.StartInfo.Arguments = string.Format("firewall delete allowedprogram \"{0}\"", exePath);
            }
            else
            {
                systag = "Win7及以上系统";
                process.StartInfo.Arguments = string.Format("advfirewall firewall delete rule name=\"{0}\"", name);
            }
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return string.Format("[{0}]: {1}", systag, result);
        }
    }
}
