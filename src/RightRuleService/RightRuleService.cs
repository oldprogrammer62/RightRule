using RightRule.Factories.RightRuleLanguageFactory;
using RightRule.Operators;
using RightRule.RegistrationServices.DataRegistration;
using RightRule.RegistrationServices.DefaultRuleRegistration;
using RightRule.RegistrationServices.Rules;
using RightRule.RuleModels;
using RightRule.RuleModels.BaseModels;
using RightRule.StatementProcessor.Interpreter;
using RightRule.System.Common;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILanguageFactory _factory;
    private readonly IInterpreter _interpreter;


    private readonly IOperatorsService _operatorsService;


    private readonly IRegisteredRuleService _registeredRuleService;
    public IRegisteredRules RegisteredRules => _registeredRuleService.RegisteredRules;


    private readonly IDefaultRuleRegister _defaultRuleRegister;
    public IDefaultRuleRegister DefaultRuleRegister => _defaultRuleRegister;


    private readonly IRegisteredDatasets? _registeredDataSets;
    public IRegisteredDatasets RegisteredDatasets => _registeredDataSets!;


    private readonly IRegisteredDataObjects? _registeredDataObjects;
    public IRegisteredDataObjects RegisteredDataObjects => _registeredDataObjects!;

    public RightRuleService(IServiceProvider ServiceProvider, ILanguageFactory factory, IOperatorsService operatorsService, IRegisteredRuleService registeredRuleService, IDefaultRuleRegister defaultRuleRegister, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects)
    {
        _serviceProvider = ServiceProvider;
        _factory = factory;

        _operatorsService = operatorsService;
        _registeredRuleService = registeredRuleService;
        _defaultRuleRegister = defaultRuleRegister;
        _registeredDataSets = registeredDatasets;
        _registeredDataObjects = registeredDataObjects;

        _interpreter = CreateInterpreter(_operatorsService.OperatorAliases, registeredDatasets, registeredDataObjects, _operatorsService.OperatorAliases);
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
        var result = RightRuleServiceResult<IProcessedRule>.Build(RuleApplied, "");
        return result;
    }

    private IInterpreter CreateInterpreter(Dictionary<string, Operator> Operators, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects, Dictionary<string, Operator> operators)
    {
        return _factory.CreateInterpreter(_serviceProvider, Operators, registeredDatasets, registeredDataObjects, operators);
    }
}
