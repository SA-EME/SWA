using SWA.Core.Logs;

namespace SWA.Core.Rules
{
    public class RuleFilter
    {

        public string Log { get; set; }
        public string Source { get; set; }
        public int? EventID { get; set; }
        public string Contains { get; set; }

        public RuleFilter(string Log, string Source)
        {
            this.Log = Log;
            this.Source = Source;
        }

        public RuleFilter(string Log, string Source, int EventID, string Contains)
        {
            this.Log = Log;
            this.Source = Source;
            this.EventID = EventID;
            this.Contains = Contains;
        }

        public bool Check(Log log)
        {
            if (this.Log == log.Name)
            {
                if (this.Source == log.Source)
                {
                    if (this.EventID == log.EventID || this.EventID == null)
                    {
                        if (this.Contains == null || log.Message.Contains(this.Contains))
                            return true;
                    }
                }
            }
            return false;
        }

    }
}
