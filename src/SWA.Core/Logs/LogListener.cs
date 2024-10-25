using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using SWA.Core.Rules;

namespace SWA.Core.Logs
{
    public class LogListener
    {
        private static readonly LinkedList<Log> LastedLogsSended = new LinkedList<Log>();
       public static readonly object lockObject = new object();
        public string LogName { get; set; }

        public List<LogListener> List = new List<LogListener>();

        public List<Rule> Rules = new List<Rule>();



        public LogListener(Rule rule)
        {
            if (List.Contains(this))
            {
                List.Find(x => x.LogName == rule.Filter.Log).Rules.Add(rule);
                SWALog.Write("INFO", "Ajout de la règle " + rule.Name + " à l'écouteur " + rule.Filter.Log);
            }
            else
            {
                LogName = rule.Filter.Log;

                EventLogQuery query = new EventLogQuery(rule.Filter.Log, PathType.LogName);
                EventLogWatcher watcher = new EventLogWatcher(query);
                watcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(Event_NewLogWritten);
                watcher.Enabled = true;

                Rules.Add(rule);
                SWALog.Write("INFO", "Ajout de la règle " + rule.Name + " à l'écouteur " + rule.Filter.Log);
                List.Add(this);
            }
        }

        private void Event_NewLogWritten(object sender, EventRecordWrittenEventArgs e)
        {
            if (e.EventRecord != null)
            {
                var NewLog = new Log(
                    e.EventRecord.LogName,
                    Environment.MachineName,
                    e.EventRecord.ProviderName,
                    (LogSeverity)e.EventRecord.Level,
                    (int)e.EventRecord.Id,
                    e.EventRecord.FormatDescription(),
                    e.EventRecord.TimeCreated.Value);

                lock (lockObject)
                {
                    if (LastedLogsSended.Count != 0 && LastedLogsSended.Last() == NewLog)
                    {
                        SWALog.Write("INFO", "Le message a déjà été traité");
                        return;
                    }

                    if (LastedLogsSended.Count >= 10) // TODO Change by config variable
                    {
                        LastedLogsSended.RemoveFirst();
                    }

                    LastedLogsSended.AddLast(NewLog);
                }

                LastedLogsSended.AddLast(NewLog);
                bool finded = false;

                foreach (Rule rule in Rules)
                {
                    if (rule.Filter.Check(NewLog))
                    {
                        var processDict = new Dictionary<string, RuleProcess>();
                        if (rule.Process != null) {
                            foreach (RuleProcess process in rule.Process)
                            {
                                process.Execute(NewLog);
                                processDict[process.Name] = process;
                            }
                        }
                        rule.Transform.Apply(NewLog, processDict);


                        NewLog.Format();
                        NewLog.Send();
                        finded = true;
                        break;
                    }
                }

                if (!finded)
                {
                    SWALog.Write("DEBUG", "Aucune règle n'a été trouvée pour le message : " + NewLog.ToString());
                }
                else
                {
                    SWALog.Write("INFO", NewLog.ToString());
                }
            }
            else
            {
                SWALog.Write("ERROR", "Erreur lors de la lecture de l'événement.");
            }

        }
    }

}
