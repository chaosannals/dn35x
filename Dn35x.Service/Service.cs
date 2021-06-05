using System.Collections;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Dn35x.Service
{
    public static class Service
    {
        public static void Start(string name)
        {
            using (ServiceController controller = new ServiceController(name))
            {
                if (
                    controller.Status != ServiceControllerStatus.Running &&
                    controller.Status != ServiceControllerStatus.StartPending &&
                    controller.Status != ServiceControllerStatus.ContinuePending
                )
                {
                    controller.Start();
                }
            }
        }

        public static void Stop(string name)
        {
            using (ServiceController controller = new ServiceController(name))
            {
                if (
                    controller.Status != ServiceControllerStatus.Stopped &&
                    controller.Status != ServiceControllerStatus.StopPending
                )
                {
                    controller.Stop();
                }
            }
        }

        public static ServiceControllerStatus Stat(string name)
        {
            using (ServiceController controller = new ServiceController(name))
            {
                return controller.Status;
            }
        }

        public static void Install(string path)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = path;
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
            }
        }

        public static void Uninstall(string path)
        {
            using (TransactedInstaller transacted = new TransactedInstaller())
            {
                AssemblyInstaller installer = new AssemblyInstaller(path, new string[] { });
                transacted.Installers.Add(installer);
                transacted.Uninstall(null);
            }
        }
    }
}
