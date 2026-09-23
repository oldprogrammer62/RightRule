using BeSoft;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DataRegistration.Data;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.System.Common;

namespace RightRule.RightRuleService;

internal interface IRightRuleService
{
    IRuleRegistrationService RuleRegistrationService { get; }

    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    List<Dataset>? GetDataset();
    Dataset? GetDataset(string DatasetName);

    List<ParsedRule>? RegisteredRules { get; }
    List<Dataset>? RegisteredDatasets { get; }

    Dictionary<string, Operator> GetOperatorAliases();
    List<Operator> GetOperatorDetails();
    bool UnloadOperator(string OperatorName);

    List<ParsedRule> GetRegisteredRules(string Context);
    ParsedRule? GetRegisteredRule(string Context, string RuleName);
    ParsedRule? GetRegisteredRule(string Context, int RuleNumber);

    DefaultRule GetDefaultRule(string Context);
    DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule);

    List<RuleRegistrationResult> RegisterRulesJson(IRightRule RightRule, string Data);
    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules);
    RuleRegistrationResult RegisterRule(DefinedRule Rule);

    bool DeRegisterRules();
    bool DeRegisterRules(string Context);
    bool DeRegisterRule(string Context, string RuleName);
    bool DeRegisterRule(string Context, int RuleNumber);

    DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems);
    DataRegistrationResult RegisterData(string DatasetName, ValueTuple<string, object> DataItem);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, object DataItemValue);

    bool DeRegisterData();
    bool DeRegisterData(string DatasetName);

    Task<RightRuleServiceResult<IProcessedRule>> ApplyRulesAsync(string Context);
    Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType);

    List<string>? GetRuleContexts();
    List<(int Number, string Name)>? GetRuleNumbers(string Context);
    List<string>? GetRuleNames(string Context);
}
