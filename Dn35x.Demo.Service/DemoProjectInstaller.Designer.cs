
namespace Dn35x.Demo.Service
{
    partial class DemoProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.masterInstaller = new System.ServiceProcess.ServiceInstaller();
            this.slaveInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller
            // 
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;
            this.serviceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller_AfterInstall);
            // 
            // masterInstaller
            // 
            this.masterInstaller.Description = "Dn35x Demo Master Service.";
            this.masterInstaller.DisplayName = "Dn35x Demo Master Service";
            this.masterInstaller.ServiceName = "Dn35xDemoMasterService";
            this.masterInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.masterInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.masterInstaller_AfterInstall);
            // 
            // slaveInstaller
            // 
            this.slaveInstaller.Description = "Dn35x Demo Slave Service.";
            this.slaveInstaller.DisplayName = "Dn35x Demo Slave Service";
            this.slaveInstaller.ServiceName = "Dn35xDemoSlaveService";
            this.slaveInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.slaveInstaller_AfterInstall);
            // 
            // DemoProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller,
            this.masterInstaller,
            this.slaveInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller masterInstaller;
        private System.ServiceProcess.ServiceInstaller slaveInstaller;
    }
}