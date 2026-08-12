using RightRule.RuleModels;
using RightRule.System.Common;

namespace RightRule_Demo.RuleManager
{
    internal class RuleManager
    {
        internal static List<AppliedRule> ApplyRules(Dictionary<string, object> Data, decimal HighValue, RulesToReturn ReturnRules)
        {
            BeSoft.RightRule rr = new BeSoft.RightRule();

            string Context = "Demo";
            var rules = LoadRules(Context);

            var registeredRules = rr.RegisterRules(rules);
            var done = rr.RegisterData(Context, Data);

            var hvData = MakeHvData(HighValue);
            rr.RegisterData("hv", hvData);

            var x = rr.EvaluateFirstRule(Context);

            var appliedRules = rr.EvaluateAllRules(Context, ReturnRules);
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
