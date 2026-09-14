using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.LanguageProcessor.Interpreter;
using RightRule.Models.BaseModels;
using RightRule.OperatorServices;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.System.Common;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    private readonly ILanguageFactory LanguageFactory;
    private readonly IInterpreter LanguageInterpreter;
    private readonly IOperatorService OperatorsService;

    private readonly IServiceProvider _serviceProvider;

    private readonly IRegisteredRuleService _registeredRuleService;
    public IRegisteredRules RegisteredRules => _registeredRuleService.RegisteredRules;


    private readonly IDefaultRuleRegister _defaultRuleRegister;
    public IDefaultRuleRegister DefaultRuleRegister => _defaultRuleRegister;


    private readonly IRegisteredDatasets? _registeredDataSets;
    public IRegisteredDatasets RegisteredDatasets => _registeredDataSets!;


    private readonly IRegisteredDataObjects? _registeredDataObjects;
    public IRegisteredDataObjects RegisteredDataObjects => _registeredDataObjects!;

    public InewGetDataService NewDataService { get; }

    public RightRuleService(IServiceProvider ServiceProvider, ILanguageFactory factory, IOperatorService operatorsService, IRegisteredRuleService registeredRuleService, IDefaultRuleRegister defaultRuleRegister, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects, InewGetDataService newDataService)
    {
        this._serviceProvider = ServiceProvider;
        LanguageFactory = factory;
        OperatorsService = operatorsService;

        _registeredRuleService = registeredRuleService;
        _defaultRuleRegister = defaultRuleRegister;
        _registeredDataSets = registeredDatasets;
        _registeredDataObjects = registeredDataObjects;
        NewDataService = newDataService;

        LanguageInterpreter = CreateInterpreter(OperatorsService.OperatorAliases, registeredDatasets, registeredDataObjects, OperatorsService.OperatorAliases, NewDataService);
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

    private IInterpreter CreateInterpreter(Dictionary<string, Operator> Operators, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects, Dictionary<string, Operator> operators, InewGetDataService NewDataService)
    {
        return LanguageFactory.CreateInterpreter(_serviceProvider, Operators, registeredDatasets, registeredDataObjects, operators, NewDataService);
    }
}
