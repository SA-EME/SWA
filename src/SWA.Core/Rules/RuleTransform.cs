using SWA.Core.Logs;
using System.Collections.Generic;
using System;

namespace SWA.Core.Rules
{

    public class RuleTransform
    {

        public string Severity { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }

        public RuleTransform() { }



        public void Apply(Log log, Dictionary<string, RuleProcess> processes)
        {
            if (!string.IsNullOrEmpty(Severity))
            {
                try
                {
                    log.Severity = (LogSeverity)Enum.Parse(typeof(LogSeverity), Severity);
                } catch (Exception e)
                {
                    SWALog.Write("ERROR", $"Error while apply severity in transform: {e.Message}");
                }
                
            }

            if (!string.IsNullOrEmpty(Message))
            {
                string CustomMessage = Message;

                foreach (var process in processes)
                {
                    CustomMessage = CustomMessage.Replace($"${process.Key}", process.Value.Output);
                }

                log.Message = CustomMessage;
            }
            if(!string.IsNullOrEmpty(Source))
            {
                log.Appname = Source;
            }
        }
    }

}
