using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule
{
    public interface IRightRule
    {
        bool SilentErrors { get; }

        AppliedRule ApplyRule(string Context);
        AppliedRule ApplyRule(string Context, string DatasetName, Dictionary<string, object> Data);

        List<AppliedRule> ApplyRules(string Context);
        List<AppliedRule> ApplyRules(string Context, RulesToReturn rulesToReturn);
        List<AppliedRule> ApplyRules(string Context, string DatasetName, Dictionary<string, object> Data);
        List<AppliedRule> ApplyRules(string Context, string DatasetName, Dictionary<string, object> Data, RulesToReturn rulesToReturn);

        void DeRegisterData(string DatasetName);

        bool DeRegisterRules();
        bool DeRegisterRules(string Context);

        RegisteredDataset? GetRegisteredDataset(string DatasetName);
        RegisteredDatasets GetRegisteredDatasets();

        List<ParsedRule> GetRules();
        List<ParsedRule> GetRules(string Context);

        bool RegisterData(string DatasetName, Dictionary<string, object> Data);

        ParsedRule RegisterRule(RuleDefinition rule);
        List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules);

        bool UnloadOperator(string operatorName);
    }
}