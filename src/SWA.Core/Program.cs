using System.ServiceProcess;

namespace SWA.Core
{
    internal static class Program
    {

        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SWAService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
