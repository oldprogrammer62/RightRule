using BeSoft;
using RightRule.DataRegistration;
using RightRule.Models;
using RightRule.Models.DataRegistration;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
using RightRule.RuleRegistration.DefaultRuleRegistration;
using RightRule.RuleRegistration.LogicRules;
using RightRule.System.Common;

namespace RightRule.RightRuleService;

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

    DefaultRule GetDefaultRule(string Context);
    DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule rule);

    List<RuleRegistrationResult> RegisterRulesJson(IRightRule rightRule, string Data);
    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> rules);
    RuleRegistrationResult RegisterRule(DefinedRule rule);

    bool DeRegisterRules(string Context);
    bool DeRegisterRules();
    bool DeRegisterRule(string Name);
    bool DeRegisterRule(int Number);
    RegisteredDataset? GetDataSet(string DataName);
    RegisteredDataObject? GetDataObject(string DataName);
    DataRegistrationResult RegisterData(string DataName, Dictionary<string, object> Data);
    DataRegistrationResult RegisterData(string DataName, object VariableValue);
    bool DeRegisterData();
    bool DeRegisterData(string DataName);

    Task<RightRuleServiceResult<IProcessedRule>> ApplyRulesAsync(string Context);
    Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, RulesToReturn rulesToReturn);
    Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn, DataRetention dataRetention);

    List<(int Number, string Name)>? GetRuleNumbers();
    List<string>? GetRuleNames();
    List<string>? GetRuleContexts();
}
