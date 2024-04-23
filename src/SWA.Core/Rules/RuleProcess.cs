using System.Collections.Generic;
using System.Data;
using System.Reflection;
using SWA.Core.Logs;

namespace SWA.Core.Rules
{
    public class RuleProcess
    {

        public string Name { get; set; }
        public string Function { get; set; }
        public List<string> Arguments { get; set; }
        public List<string> CurrentArguments { get; set; }
        public string Output { get; set; }

        public RuleProcess()
        {

        }

        private void ReplaceLogsArgs(Log log)
        {
            CurrentArguments = new List<string>(Arguments);
            try
            {
                for (int i = 0; i < Arguments.Count; i++)
                {
                    if (Arguments[i].StartsWith("$"))
                    {
                        string propertyName = Arguments[i].Substring(1);
                        PropertyInfo property = typeof(Log).GetProperty(propertyName);
                        if (property != null)
                        {
                            CurrentArguments[i] = (string)property.GetValue(log);
                        }
                    }
                }
            }
            catch (DataException e)
            {
                SWALog.Write("ERROR", $"Error while replace logs args in process {Name}: {e.Message}");
            }
        }

        public void Execute(Log log)
        {
            ReplaceLogsArgs(log);
            try
            {
                var result = typeof(Actions).GetMethod(Function).Invoke(null, CurrentArguments.ToArray());
                Output = (string)result;
                SWALog.Write("DEBUG", $"Process {Name} executed with success, output : {Output}");
            }
            catch (DataException e)
            {
                SWALog.Write("ERROR", $"Error while execute process {Name}: {e.Message}");
                Output = "N/A";
            }

        }

    }
}
