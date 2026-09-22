using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DataRegistration.Data;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RuleApplication;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    private readonly IStatementSyntaxChecker StatementSyntaxChecker;
    private readonly IRuleApplicationService RuleApplicationService;
    private readonly IOperatorService OperatorsService;
    private readonly IDefaultRuleRegister DefaultRuleRegister;
    public List<DefaultRulePerContext>? DefaultRules => DefaultRuleRegister.GetDefaultRules();

    private readonly IRuleRegistrationService RuleRegistrationService;
    public List<ParsedRule>? RegisteredRules => RuleRegistrationService?.GetRules();

    private readonly IDataRegistrationService DataRegistrationService;
    public List<Dataset>? RegisteredDatasets => DataRegistrationService?.GetDatasets();

    public RightRuleService(
        IOperatorService operatorsService, 
        IRuleRegistrationService ruleService, 
        IDefaultRuleRegister defaultRuleRegister, 
        IDataRegistrationService dataService, 
        IStatementSyntaxChecker statementSyntaxChecker,
        IRuleApplicationService ruleApplicationService
        )
    {
        OperatorsService = operatorsService;
        RuleRegistrationService = ruleService;
        DefaultRuleRegister = defaultRuleRegister;
        DataRegistrationService = dataService;
        StatementSyntaxChecker = statementSyntaxChecker;
        RuleApplicationService = ruleApplicationService;
    }

    public Task<CheckStatementResult> SyntaxCheckAsync(string Statement)
    {
        return StatementSyntaxChecker.CheckAsync(Statement, false);
    }

    private static List<RightRuleServiceResult<IProcessedRule>> BuildResult(List<IProcessedRule> RulesApplied)
    {
        List<RightRuleServiceResult<IProcessedRule>> results = new List<RightRuleServiceResult<IProcessedRule>>();
        foreach (var ruleApplied in RulesApplied)
        {
            results.Add(BuildResult(ruleApplied));
        }
        return results;
    }

    private static RightRuleServiceResult<IProcessedRule> BuildResult(IProcessedRule RuleApplied)
    {
        var result = RightRuleServiceResult<IProcessedRule>.Build(RuleApplied);
        return result;
    }
}
