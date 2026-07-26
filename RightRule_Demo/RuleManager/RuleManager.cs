using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule_Demo.RuleManager
{
    internal class RuleManager
    {
        internal async static Task<List<AppliedRule>> ApplyRulesAsync(Dictionary<string, object> Data, decimal HighValue, RulesToReturn rulesToReturn)
        {
            BeSoft.RightRule rr = new BeSoft.RightRule();

            string Context = "Demo";
            var rules = LoadRules(Context);

            var registeredRules = await rr.RegisterRulesAsync(rules);
            var done = rr.RegisterDataset(Context, Data);

            var hvData = MakeHvData(HighValue);
            rr.RegisterDataset("hv", hvData);

            var x = rr.ApplyRule(Context);

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
