using RightRule.RuleServices.RuleModels;

namespace RightRule.RuleServices.RuleLoader
{
    internal interface IRuleLoaderService
    {
        List<DefinedRule>? LoadRulesFromJson(string JsonData);
    }
}