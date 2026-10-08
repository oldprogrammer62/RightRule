using Microsoft.Extensions.Logging;
using RightRule.DataProvider;
using RightRule.DataServices;
using RightRule.DataServices.Data;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleApplication;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using RightRule.System.Common.Operator.Models;
using System.ComponentModel;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService, IDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private bool Disposed { get; set; } = false;

    private readonly IStatementSyntaxChecker StatementSyntaxChecker;
    private readonly IRuleApplicationService RuleApplicationService;
    private readonly ILogger<RightRuleService> Logger;

    public IDataProviderService? DataProviderService { get; }
    public IEnumerable<Type>? RegisteredDataProviders => DataProviderService?.RegisteredDataProviders;

    public IOperatorService? OperatorService { get; }
    public List<Operator>? RegisteredOperators => OperatorService?.Operators;

    private readonly IDefaultRuleRegister DefaultRuleRegister;
    public List<DefaultRulePerContext>? DefaultRules => DefaultRuleRegister.GetDefaultRules();

    public IRuleService RuleService { get; }
    public List<ParsedRule>? RegisteredRules => RuleService?.GetRules();

    public IDataService DataService { get; }
    public List<Dataset>? RegisteredData => DataService?.GetDatasets();

    public RightRuleService(
        IOperatorService operatorsService, 
        IRuleService ruleService, 
        IDefaultRuleRegister defaultRuleRegister, 
        IDataService dataService, 
        IStatementSyntaxChecker statementSyntaxChecker,
        IRuleApplicationService ruleApplicationService,
        IDataProviderService dataProviderService,
        ILogger<RightRuleService> logger
        )
    {
        OperatorService = operatorsService;
        RuleService = ruleService;
        DefaultRuleRegister = defaultRuleRegister;
        DataService = dataService;
        StatementSyntaxChecker = statementSyntaxChecker;
        RuleApplicationService = ruleApplicationService;
        DataProviderService = dataProviderService;
        Logger = logger;
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

    public void Dispose()
    {
        if (Disposed) return;

        if (DataService is IDisposable disposableDataService)
        {
            disposableDataService.Dispose();
        }
        if (RuleService is IDisposable disposableRuleService)
        {
            disposableRuleService.Dispose();
        }
        if (OperatorService is IDisposable disposableOperatorService)
        {
            disposableOperatorService.Dispose();
        }
        Disposed = true;
    }
}
