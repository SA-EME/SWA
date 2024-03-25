using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
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
                SWALog.Write("INFO", "Ajout de la règle " + rule.Name + " à l'écouteur " + logname);
            }
            else
            {
                LogName = logname;
                ELog = new EventLog(logname);
                ELog.EntryWritten += new EntryWrittenEventHandler(Event_NewLogWritten);
                ELog.EnableRaisingEvents = true;
                Rules.Add(rule);
                SWALog.Write("INFO", "Ajout de la règle " + rule.Name + " à l'écouteur " + logname);
                List.Add(this);
            }
        }

        private void Event_NewLogWritten(object source, EntryWrittenEventArgs e)
        {
            String Message = e.Entry.Message.Replace("\r\n", " ").Replace("\t", "");

            Log NewLog = new Log(LogName, Environment.MachineName, e.Entry.Source, (LogSeverity)e.Entry.EntryType, (int)e.Entry.InstanceId, Message, e.Entry.TimeGenerated);

            bool finded = false;

            foreach(Rule rule in Rules)
            {
                if (rule.Filter.Check(NewLog))
                {
                    //rule.Processing.ToString();
                    //rule.Transform.ToString();
                    //NewLog.Send();
                    finded = true;
                    break;
                }
            }

            if (!finded)
            {
                SWALog.Write("DEBUG", "Aucune règle n'a été trouvée pour le message : " + NewLog.ToString());
            } else
            {
                SWALog.Write("INFO", NewLog.ToString());
            }

        }

    }
}
