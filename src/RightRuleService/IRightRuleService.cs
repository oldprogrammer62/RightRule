using RightRule.DataServices;
using RightRule.DataServices.Data;
using RightRule.DataServices.DataRegistration;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.RuleServices.RuleRegistration;
using RightRule.System.Common;

namespace RightRule.RightRuleService;

public interface IRightRuleService
{
    IRuleService? RuleService { get; }
    IDataService? DataService { get; }

    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    List<Dataset>? GetDataset();
    Dataset? GetDataset(string DatasetName);

    List<ParsedRule>? RegisteredRules { get; }
    List<Dataset>? RegisteredData { get; }

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


    bool RegisterDataProvider(object dataProvider);
    DataRegistrationResult RegisterData<T>(string DatasetName, T Data) where T : notnull;


    DataRegistrationResult RegisterData(string DatasetName, (string, object) DataItem);
    List<DataRegistrationResult> RegisterData(string DatasetName, List<(string, object)> DataItems);
    DataRegistrationResult RegisterData(string DatasetName, Dictionary<string, object> DataItems);
    DataRegistrationResult RegisterData(string DatasetName, string DataItemName, object DataItemValue);

    bool DeRegisterData();
    bool DeRegisterData(string DatasetName);

    Task<RightRuleServiceResult<IProcessedRule>> ApplyRulesAsync(string Context);
    Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRulesAsync(string Context, EvaluationTypeEnum evaluationType);

    List<string>? GetRuleContexts();
    List<(int Number, string Name)>? GetRuleNumbers(string Context);
    List<string>? GetRuleNames(string Context);
}
