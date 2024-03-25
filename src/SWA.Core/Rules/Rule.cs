using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWA.Core.Rules.Actions;

namespace SWA.Core.Rules
{
    public class Rule
    {

        public RuleFilter Filter { get; set; }
        public RuleProcessing Processing { get; set; }
        public RuleTransform Transform { get; set; }

        public static List<Rule> List = new List<Rule>();

        public Rule(RuleFilter Filter, RuleProcessing Processing, RuleTransform Transform)
        {
            this.Filter = Filter;
            this.Processing = Processing;
            this.Transform = Transform;

            List.Add(this);
        }

    }
}
