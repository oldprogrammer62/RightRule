using RightRule.Data;
using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.Models.BaseModels;
using RightRule.Models.RuleModels;
using RightRule.OperatorServices;
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

    private readonly IRegisteredRuleService RegisteredRuleService;
    public List<ParsedRule>? RegisteredRules => RegisteredRuleService?.GetRules();

    private readonly IDataService DataService;
    public List<Dataset>? RegisteredDatasets => DataService?.GetDatasets();

    public RightRuleService(IServiceProvider ServiceProvider, ILanguageFactory factory, IOperatorService operatorsService, IRegisteredRuleService registeredRuleService, IDefaultRuleRegister defaultRuleRegister, IDataService dataService)
    {
        this.ServiceProvider = ServiceProvider;
        LanguageFactory = factory;
        OperatorsService = operatorsService;
        RegisteredRuleService = registeredRuleService;
        DefaultRuleRegister = defaultRuleRegister;
        DataService = dataService;
        Interpreter = CreateInterpreter();
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

    private IInterpreter CreateInterpreter()
    {
        return LanguageFactory.CreateInterpreter(ServiceProvider);
    }
}
