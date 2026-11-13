using RightRule.DataAdaptors;
using RightRule.DataServices;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.System.Common.DataAdaptor;

namespace RightRule.RightRuleService;

public interface IRightRuleService
{
    IRuleService? RuleService { get; }
    IDataService? DataService { get; }
    IOperatorService? OperatorService { get; }
    IDataAdaptorService? DataAdaptorService { get; }

    Task<CheckStatementResult> SyntaxCheckAsync(string Statement);

    List<ParsedRule>? RegisteredRules { get; }
    
    DefaultRule GetDefaultRule(string Context);
    DefaultRuleRegistrationResult RegisterDefaultRule(DefaultRule Rule);

    DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor dataAdaptor);

    Task<RightRuleServiceResult<IProcessedRule>> ApplyRules_SingleResultAsync(string Context, EvaluationTypeEnum EvaluationType);
    Task<List<RightRuleServiceResult<IProcessedRule>>> ApplyRules_MultipleResultAsync(string Context, EvaluationTypeEnum EvaluationType);
}
