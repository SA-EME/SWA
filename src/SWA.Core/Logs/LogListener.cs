﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SWA.Core.Rules;

namespace SWA.Core.Logs
{
    public class LogListener
    {
        private static readonly LinkedList<Log> LastedLogsSended = new LinkedList<Log>();

        private readonly EventLog ELog;
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
                ELog = new EventLog(rule.Filter.Log);
                ELog.EntryWritten += new EntryWrittenEventHandler(Event_NewLogWritten);
                ELog.EnableRaisingEvents = true;
                Rules.Add(rule);
                SWALog.Write("INFO", "Ajout de la règle " + rule.Name + " à l'écouteur " + rule.Filter.Log);
                List.Add(this);
            }
        }

        private void Event_NewLogWritten(object source, EntryWrittenEventArgs e)
        {

            Log NewLog = new Log(LogName, Environment.MachineName, e.Entry.Source, (LogSeverity)e.Entry.EntryType, (int)e.Entry.InstanceId, e.Entry.Message, e.Entry.TimeGenerated);
            if (LastedLogsSended.Count != 0 && LastedLogsSended.Last() == NewLog)
            {
                SWALog.Write("INFO", "Le message a déjà été traitée");
                return;
            }

            if (LastedLogsSended.Count >= 10) // TODO Change by config variable
            {
                LastedLogsSended.RemoveFirst();
            }

            LastedLogsSended.AddLast(NewLog);
            bool finded = false;

            foreach (Rule rule in Rules)
            {
                if (rule.Filter.Check(NewLog))
                {
                    var processDict = new Dictionary<string, RuleProcess>();
                    foreach (RuleProcess process in rule.Process)
                    {
                        process.Execute(NewLog);
                        processDict[process.Name] = process;
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

    }
}
