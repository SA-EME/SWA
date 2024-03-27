using System.Collections.Generic;

namespace SWA.Core.Rules
{
    public class Rule
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public RuleFilter Filter { get; set; }
        public List<RuleProcess> Process { get; set; }
        public RuleTransform Transform { get; set; }

        public static List<Rule> List = new List<Rule>();

        public Rule()
        {
            List.Add(this);
        }

    }
}
