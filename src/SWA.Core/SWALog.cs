using System;
using System.Diagnostics;
using SWA.Core.Logs;
using SWA.Core.Rules;

namespace SWA.Core
{
    public class SWALog
    {

        private readonly EventLog log;

        public SWALog() {
        }

        public void Initialize()
        {
            log.EntryWritten += new EntryWrittenEventHandler(Event_NewLogWritten);
            log.EnableRaisingEvents = true;
        }

    }
}
