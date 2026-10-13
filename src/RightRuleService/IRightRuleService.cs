using RightRule.DataProvider;
using RightRule.DataServices;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleRegistration;
using RightRule.System.Common.DataProvider;
using RightRule.System.Common.Operator.Models;

namespace RightRule.RightRuleService;

public interface IRightRuleService
{
    IRuleService? RuleService { get; }
    IDataService? DataService { get; }
    IOperatorService? OperatorService { get; }

    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    List<Dataset>? GetDataset();
    Dataset? GetDataset(string DatasetName);

    List<ParsedRule>? RegisteredRules { get; }
    List<Dataset>? RegisteredData { get; }
    List<Operator>? RegisteredOperators { get; }

    OperatorRegistrationResult RegisterOperator(Operator Operator);
    List<Operator> GetOperators();
    Dictionary<string, Operator> GetOperatorAliases();
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


    DataProviderRegistrationResult RegisterDataProvider(IDataProvider dataProvider);

    DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull;

    bool DeRegisterData();
    bool DeRegisterData(string DatasetName);

    Task<RightRuleServiceResult<IProcessedRule>> ApplyRules_SingleResultAsync(string Context, EvaluationTypeEnum EvaluationType);
    Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRules_MultipleResultAsync(string Context, EvaluationTypeEnum EvaluationType);

    List<string>? GetRuleContexts();
    List<(int Number, string Name)>? GetRuleNumbers(string Context);
    List<string>? GetRuleNames(string Context);
}
