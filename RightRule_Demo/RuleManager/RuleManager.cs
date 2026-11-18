using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;
using RightRule.RightRuleSession;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRule_Demo.RuleManager
{
    internal class RuleManager
    {
        internal static List<RightRuleResult<IProcessedRule>> ApplyRules(Dictionary<string, object> Data, decimal HighValue, EvaluationTypeEnum EvaluationType)
        {
            RightRuleOptions options = new RightRuleOptions()
            {
                DataCheck = false,
                Diagnostics = true,
                LoadCustomOperators = true,
                LoadDefaultOperators = true
            };
            var rr = new RightRule.RightRule(options).CreateSession();

            string Context = "Demo";
            var rules = LoadRules(rr, Context);

            var registeredRules = rr.RuleService.RegisterRules(rules);
            var done = rr.DataService.RegisterData(Context, Data);

            var hvData = MakeHvData(HighValue);
            rr.DataService.RegisterData("hv", hvData);

            var x = rr.RuleService.ApplyRule(Context);

            var appliedRules = rr.RuleService.ApplyRules(Context, EvaluationType);
            return appliedRules;
        }

        private static List<DefinedRule> LoadRules(IRightRuleSession rightRule, string Context)
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
