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

public interface IRightRuleService
{
    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    List<Dataset>? GetData();
    Dataset? GetData(string DataName);

    List<ParsedRule>? RegisteredRules { get; }
    List<Dataset>? RegisteredDatasets { get; }

    Dictionary<string, Operator> GetOperatorAliases();
    List<Operator> GetOperatorDetails();
    bool UnloadOperator(string OperatorName);

    ParsedRule? GetRegisteredRule(string RuleName);
    ParsedRule? GetRegisteredRule(int RuleNumber);
    List<ParsedRule> GetRules(string Context);

    DefaultRule GetDefaultRule(string Context);
    DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule);

    List<RuleRegistrationResult> RegisterRulesJson(IRightRule RightRule, string Data);
    List<RuleRegistrationResult> RegisterRules(IEnumerable<DefinedRule> Rules);
    RuleRegistrationResult RegisterRule(DefinedRule Rule);

    bool DeRegisterRules(string Context);
    bool DeRegisterRules();
    bool DeRegisterRule(string RuleName);
    bool DeRegisterRule(int RuleNumber);

    DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems);
    DataRegistrationResult RegisterData(string DatasetName, ValueTuple<string, object> DataItem);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, object DataItemValue);

    bool DeRegisterData();
    bool DeRegisterData(string DataName);

    Task<RightRuleServiceResult<IProcessedRule>> ApplyRulesAsync(string Context);
    Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType);

    List<(int Number, string Name)>? GetRuleNumbers();
    List<string>? GetRuleNames();
    List<string>? GetRuleContexts();
}
