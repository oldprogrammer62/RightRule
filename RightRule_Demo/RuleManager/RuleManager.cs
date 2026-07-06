using RightRule.Extensions;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule_Demo.RuleManager
{
    internal class RuleManager
    {
        internal static List<AppliedRule> ApplyRules(Dictionary<string, object> Data, decimal HighValue, RulesToReturn rulesToReturn)
        {
            RightRule.RightRule rr = new RightRule.RightRule();

            string Context = "Demo";
            var rules = LoadRules(Context, HighValue);

            rr.RegisterRules(rules);
            rr.RegisterData(Context, Data);

            var appliedRules = rr.ApplyRules(Context, rulesToReturn);
            return appliedRules;
        }

        private static List<RuleDefinition> LoadRules(string Context, object HighValue)
        {
            return RuleLoader.RuleLoader.Load(Context, HighValue);
        }
    }
}
