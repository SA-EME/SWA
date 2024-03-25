using SWA.Core.Logs;

namespace SWA.Core.Rules
{
    public class RuleFilter
    {

        public string Log { get; set; }
        public string Source { get; set; }
        public int? EventID { get; set; }
        public LogSeverity? Severity { get; set; }
        public string Contains { get; set; }

        public RuleFilter(string Log, string Source)
        {
            this.Log = Log;
            this.Source = Source;
        }

        public RuleFilter(string Log, string Source, int EventID, LogSeverity? severity, string Contains)
        {
            this.Log = Log;
            this.Source = Source;
            this.EventID = EventID;
            this.Severity = severity;
            this.Contains = Contains;

        }

        public bool Check(Log log)
        {
            if (this.Log == log.Name)
            {
                SWALog.Write("DEBUG", $"Log: {this.Source} - {log.Appname}");
                if (this.Source == log.Appname)
                {
                    SWALog.Write("DEBUG", $"EventID: {this.EventID} - {log.EventID}");
                    if (this.EventID == log.EventID || this.EventID == 0)
                    {
                        SWALog.Write("DEBUG", $"Sev: {this.Severity} - {log.Severity}");
                        if (this.Severity == log.Severity || this.Severity == null)
                        {
                            SWALog.Write("DEBUG", $"Contains: {this.Contains} - {log.Message}");
                            if (this.Contains == null || log.Message.Contains(this.Contains))
                                SWALog.Write("DEBUG", "Pass contains check");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
