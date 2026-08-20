using BeSoft;
using RightRule.Data;
using RightRule.RuleModels;
using RightRule.RuleRegistration;
using RightRule.System.Common;

namespace RightRule.RuleService;

internal interface IRightRuleService
{
    IRegisteredRules RegisteredRules { get; }
    IRegisteredDatasets RegisteredDatasets { get; }
    IRegisteredDataObjects RegisteredDataObjects { get; }

    Dictionary<string, Operator> GetOperatorAliases();
    List<Operator> GetOperatorDetails();
    bool UnloadOperator(string OperatorName);
    ParsedRule? GetRegisteredRule(string Name);
    ParsedRule? GetRegisteredRule(int Number);
    List<ParsedRule> GetRules(string Context);

    List<ParsedRule>? RegisterRulesJson(IRightRule rightRule, string Data);
    List<ParsedRule> RegisterRules(IEnumerable<RuleDefinition> rules);
    ParsedRule RegisterRule(RuleDefinition rule);
    bool DeRegisterRules(string Context);
    bool DeRegisterRules();
    bool DeRegisterRule(string Name);
    bool DeRegisterRule(int Number);
    RegisteredDataset? GetDataSet(string DataName);
    RegisteredDataObject? GetDataObject(string DataName);
    bool RegisterData(string DataName, Dictionary<string, object> Data);
    bool RegisterData(string DataName, object VariableValue);
    bool DeRegisterData();
    bool DeRegisterData(string DataName);
    Task<List<AppliedRule>> EvaluateFirstRuleAsync(string Context);
    Task<List<AppliedRule>> EvaluateFirstRuleAsync(string Context, string DataName, Dictionary<string, object> Data);
    Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context);
    Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, RulesToReturn ReturnRules);
    Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data);
    Task<List<AppliedRule>> EvaluateAllRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn ReturnRules);

    List<(int Number, string Name)>? GetRuleNumbers();
    List<string>? GetRuleNames();
    List<string>? GetRuleContexts();
}
