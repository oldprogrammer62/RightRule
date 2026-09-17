using RightRule.LanguageProcessor.Interpreter;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DataRegistration.Data;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    private readonly IInterpreter Interpreter;
    private readonly IStatementSyntaxChecker StatementSyntaxChecker;
    private readonly IOperatorService OperatorsService;
    private readonly IServiceProvider ServiceProvider;
    private readonly IDefaultRuleRegister DefaultRuleRegister;

    private readonly IRuleService RuleService;
    public List<ParsedRule>? RegisteredRules => RuleService?.GetRules();

    private readonly IDataService DataService;

    public List<Dataset>? RegisteredDatasets => DataService?.GetDatasets();

    public RightRuleService(IServiceProvider serviceProvider, IOperatorService operatorsService, IRuleService ruleService, IDefaultRuleRegister defaultRuleRegister, IDataService dataService, IInterpreter interpreter, IStatementSyntaxChecker statementSyntaxChecker)
    {
        ServiceProvider = serviceProvider;
        OperatorsService = operatorsService;
        RuleService = ruleService;
        DefaultRuleRegister = defaultRuleRegister;
        DataService = dataService;
        Interpreter = interpreter;
        StatementSyntaxChecker = statementSyntaxChecker;
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
