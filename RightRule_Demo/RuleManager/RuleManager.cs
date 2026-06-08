using RightRule.Rules;

namespace RightRule_Demo.RuleManager
{
    internal class RuleManager
    {
        internal static AppliedRule ApplyRules(Dictionary<string, object> Data)
        {
            string Context = "Demo";

            RightRule.RightRule rr = new RightRule.RightRule();
            var rules = LoadRules(Context);
            rr.RegisterRules(rules);
            rr.RegisterData(Context, Data);

            var appliedRule = rr.ApplyRules(Context, RightRule.System.Common.RulesToApply.First);
            return appliedRule; ;
        }

        private static List<RuleDefinition> LoadRules(string Context)
        {
            return RuleLoader.RuleLoader.Load(Context);
        }
    }
}
