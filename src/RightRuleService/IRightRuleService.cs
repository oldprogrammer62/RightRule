using BeSoft;
using RightRule.Data;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RuleApplication;
using RightRule.System.Common;

namespace RightRule.RightRuleService;

internal interface IRightRuleService
{
    List<Dataset>? GetData();

    Dataset? GetData(string DataName);

    List<ParsedRule> RegisteredRules { get; }
    List<Dataset>? RegisteredDatasets { get; }

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

    DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems);
    DataRegistrationResult RegisterData(string DatasetName, ValueTuple<string, object> DataItem);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, object DataItemValue);

    bool DeRegisterData();
    bool DeRegisterData(string DataName);

    Task<RightRuleServiceResult<IProcessedRule>> ApplyRulesAsync(string Context);
    Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, RulesToReturn rulesToReturn);
    Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, string DataName, Dictionary<string, object> Data, RulesToReturn rulesToReturn, DataRetention dataRetention);

    List<(int Number, string Name)>? GetRuleNumbers();
    List<string>? GetRuleNames();
    List<string>? GetRuleContexts();
}
