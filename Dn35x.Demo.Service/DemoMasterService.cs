using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.IO;

namespace Dn35x.Demo.Service
{
    public partial class DemoMasterService : ServiceBase
    {
        public DemoMasterService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // ServiceUser.ChangeServiceAccountInfo("Dn35xDemoMasterService", "exert", "123456");
                /*
                string domain = @"\\远程服务器名";
                using (WindowsUserSession user = WindowsUserSession.NewSession("xcvasdfdsafasd", domain, ""))
                {
                    string text = File.ReadAllText(string.Format(@"{0}\test\t.txt", domain));
                    File.WriteAllText(string.Format(@"{0}\test\fffff.log", domain), text);
                }
                */
            }
            catch (Exception e)
            {
                File.WriteAllText("a.log", e.Message + e.StackTrace);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
