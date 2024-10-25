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

        public RuleFilter()
        {

        }


        public bool Check(Log log)
        {
            SWALog.Write("DEBUG", $"Checking log: {log.Name}, {log.Appname}, {log.EventID}, {log.Severity}, {log.Message}");
            if (this.Log == log.Name)
            {
                SWALog.Write("DEBUG", $"Log match: {this.Log}");
                if (this.Source == log.Appname || this.Source == null)
                {
                    SWALog.Write("DEBUG", $"Source match: {this.Source}");
                    if (this.EventID == log.EventID || this.EventID == 0)
                    {
                        SWALog.Write("DEBUG", $"EventID match: {this.EventID}");
                        if (this.Severity == log.Severity || this.Severity == null)
                        {
                            SWALog.Write("DEBUG", $"Severity match: {this.Severity}");
                            if (this.Contains == null || log.Message.Contains(this.Contains))
                            {
                                SWALog.Write("DEBUG", "Pass contains check");
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

    }
}
