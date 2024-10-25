using System.ServiceProcess;
using System.Timers;

namespace SWA.Core
{
    public partial class SWAService : ServiceBase
    {
        public SWAService()
        {
            InitializeComponent();
            this.ServiceName = "SWAService";
        }

        protected override void OnStart(string[] args)
        {

            SWALog.Start();
        }
    }
}
