using Microsoft.Extensions.Logging;
using RightRule.DataProvider.DataProviderLibrary;
using RightRule.DataServices;
using RightRule.DataServices.Data;
using RightRule.LanguageProcessor.SyntaxChecker;
using RightRule.OperatorServices;
using RightRule.RuleServices;
using RightRule.RuleServices.DefaultRuleRegistration;
using RightRule.RuleServices.RuleApplication;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;
using System.ComponentModel;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService, IDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private bool Disposed { get; set; } = false;

    private readonly IStatementSyntaxChecker StatementSyntaxChecker;
    private readonly IRuleApplicationService RuleApplicationService;
    private readonly IDataProviderLibraryManager DataProviderLibraryManager;
    private readonly ILogger<RightRuleService> Logger;
    private readonly IOperatorService OperatorService;
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
        IDataProviderLibraryManager dataProviderLibraryManager,
        ILogger<RightRuleService> logger
        )
    {
        OperatorService = operatorsService;
        RuleService = ruleService;
        DefaultRuleRegister = defaultRuleRegister;
        DataService = dataService;
        StatementSyntaxChecker = statementSyntaxChecker;
        RuleApplicationService = ruleApplicationService;
        DataProviderLibraryManager = dataProviderLibraryManager;
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
