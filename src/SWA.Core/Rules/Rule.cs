using System.Collections.Generic;
using SWA.Core.Rules.Actions;

namespace SWA.Core.Rules
{
    public class Rule
    {

        public string Name { get; set; }

        public RuleFilter Filter { get; set; }
        public RuleProcessing Processing { get; set; }
        public RuleTransform Transform { get; set; }

        public static List<Rule> List = new List<Rule>();

        public Rule(string name, RuleFilter Filter, RuleProcessing Processing, RuleTransform Transform)
        {
            this.Name = name;
            this.Filter = Filter;
            this.Processing = Processing;
            this.Transform = Transform;

            List.Add(this);
        }

    }
}
