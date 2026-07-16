using RightRule.Data;
using RightRule.Rules;
using RightRule.System.Common;

namespace RightRule
{
    public interface IRightRule
    {
        bool SilentErrors { get; }

        AppliedRule ApplyRule(string Context);
        AppliedRule ApplyRule<T>(string Context, string DatasetName, T Data);
        List<AppliedRule> ApplyRules(string Context);
        List<AppliedRule> ApplyRules(string Context, RulesToReturn rulesToReturn);
        List<AppliedRule> ApplyRules<T>(string Context, string DatasetName, T Data);
        List<AppliedRule> ApplyRules<T>(string Context, string DatasetName, T Data, RulesToReturn rulesToReturn);
        void DeRegisterData(string DatasetName);
        bool DeRegisterRules();
        bool DeRegisterRules(string Context);
        RegisteredDataset? GetRegisteredDataset(string DatasetName);
        RegisteredDatasets GetRegisteredDatasets();
        List<ParsedRule> GetRules();
        List<ParsedRule> GetRules(string Context);
        bool RegisterData(string DatasetName, Dictionary<string, object> Data);
        bool RegisterData<T>(string DatasetName, T Data);
        ParsedRule RegisterRule(RuleDefinition rule);
        List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules);
        bool UnloadOperator(string operatorName);
    }
}