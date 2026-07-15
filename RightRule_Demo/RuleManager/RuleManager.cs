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
            var rules = LoadRules(Context);

            rr.RegisterRules(rules);
            rr.RegisterData(Context, Data);

            var hvData = MakeHvData(HighValue);
            rr.RegisterData("hv", hvData);

            var appliedRules = rr.ApplyRules(Context, rulesToReturn);
            return appliedRules;
        }

        private static List<RuleDefinition> LoadRules(string Context)
        {
            return RuleLoader.RuleLoader.Load(Context);
        }

        private static Dictionary<string, object> MakeHvData(decimal HighValue)
        {
            var hvData = new Dictionary<string, object>
            {
                { "value", HighValue }
            };
            return hvData;
        }
    }
}
