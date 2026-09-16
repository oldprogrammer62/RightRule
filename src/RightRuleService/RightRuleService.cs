using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.LanguageProcessor.Interpreter;
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
    private readonly ILanguageFactory LanguageFactory;
    private readonly IInterpreter Interpreter;
    private readonly IOperatorService OperatorsService;
    private readonly IServiceProvider ServiceProvider;
    private readonly IDefaultRuleRegister DefaultRuleRegister;

    private readonly IRuleService RuleService;
    public List<ParsedRule>? RegisteredRules => RuleService?.GetRules();

    private readonly IDataService DataService;

    public List<Dataset>? RegisteredDatasets => DataService?.GetDatasets();

    public RightRuleService(IServiceProvider serviceProvider, ILanguageFactory factory, IOperatorService operatorsService, IRuleService ruleService, IDefaultRuleRegister defaultRuleRegister, IDataService dataService, IInterpreter interpreter)
    {
        ServiceProvider = serviceProvider;
        LanguageFactory = factory;
        OperatorsService = operatorsService;
        RuleService = ruleService;
        DefaultRuleRegister = defaultRuleRegister;
        DataService = dataService;
        Interpreter = interpreter;
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
