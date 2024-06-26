﻿using SWA.Core.Logs;
using System.Collections.Generic;
using System;

namespace SWA.Core.Rules
{

    public class RuleTransform
    {

        public string Severity { get; set; }
        public string Message { get; set; }

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
                foreach (var process in processes)
                {
                    Message = Message.Replace($"${process.Key}", process.Value.Output);
                }

                log.Message = Message;
            }
        }
    }

}
