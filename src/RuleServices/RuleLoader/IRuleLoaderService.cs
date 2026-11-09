using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleLoader
{
    internal interface IRuleLoaderService
    {
        List<DefinedRule>? LoadRules(string JsonData);
    }
}