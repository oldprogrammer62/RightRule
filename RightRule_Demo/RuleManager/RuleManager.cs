using BeSoft;
using RightRule.Models;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RuleApplication;
using RightRule.System.Common;

namespace RightRule_Demo.RuleManager
{
    internal class RuleManager
    {
        internal static List<RightRuleResult<IProcessedRule>> ApplyRules(Dictionary<string, object> Data, decimal HighValue, RulesToReturn ReturnRules)
        {
            BeSoft.RightRule rr = new BeSoft.RightRule();

            string Context = "Demo";
            var rules = LoadRules(rr, Context);

            var registeredRules = rr.RegisterRules(rules);
            var done = rr.RegisterData(Context, Data);

            var hvData = MakeHvData(HighValue);
            rr.RegisterData("hv", hvData);

            var x = rr.ApplyRules(Context);

            var appliedRules = rr.ApplyRules(Context, ReturnRules);
            return appliedRules;
        }

        private static List<DefinedRule> LoadRules(IRightRule rightRule, string Context)
        {
            return RuleLoader.RuleLoader.Load(rightRule, Context);
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
