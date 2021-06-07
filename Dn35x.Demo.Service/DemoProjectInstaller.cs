using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace Dn35x.Demo.Service
{
    [RunInstaller(true)]
    public partial class DemoProjectInstaller : System.Configuration.Install.Installer
    {
        public DemoProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void masterInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void slaveInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
