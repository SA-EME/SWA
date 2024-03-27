using System.ServiceProcess;
using System.Timers;

namespace SWA.Core
{
    public partial class SWAService : ServiceBase
    {
        public SWAService()
        {
            InitializeComponent();

        }

        protected override void OnStart(string[] args)
        {
            Timer timer = new Timer
            {
                Interval = 1000 * 10,
                AutoReset = false
            };
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            timer.Start();

            SWALog.Start();
        }
    
        /**
         * Check every 10 seconds something
         */
        static void TimerElapsed(object sender, ElapsedEventArgs e)
        {

        }

        protected override void OnStop()
        {
        }
    }
}
