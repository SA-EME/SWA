using System;
using System.Collections.Generic;
using System.Diagnostics;
using SWA.Core.Rules;

namespace SWA.Core.Logs
{
    public class LogListener
    {
        private readonly EventLog ELog;
        public string LogName { get; set; }

        public List<LogListener> List = new List<LogListener>();

        public List<Rule> Rules = new List<Rule>();



        public LogListener(string logname, Rule rule)
        {
            if (List.Contains(this))
            {
                List.Find(x => x.LogName == logname).Rules.Add(rule);
            }
            else
            {
                LogName = logname;
                ELog = new EventLog(logname);
                ELog.EntryWritten += new EntryWrittenEventHandler(Event_NewLogWritten);
                ELog.EnableRaisingEvents = true;
                List.Add(this);
            }
        }

        private void Event_NewLogWritten(object source, EntryWrittenEventArgs e)
        {
            String Message = e.Entry.Message.Replace("\r\n", " ").Replace("\t", "");

            Log NewLog = new Log()
            {
                LogName = LogName,
                Hostname = Environment.MachineName,
                Appname = e.Entry.Source,
                Severity = (LogSeverity)e.Entry.EntryType,
                EventID = (int)e.Entry.InstanceId,
                Message = Message,
                TimeGenerated = e.Entry.TimeGenerated
            };

            Rules.ForEach(rule =>
            {
                if (rule.Filter.Check(NewLog))
                {

                    rule.Processing.ToString();
                    rule.Transform.ToString();

                    NewLog.Send();
                }
            });

        }

    }
}
