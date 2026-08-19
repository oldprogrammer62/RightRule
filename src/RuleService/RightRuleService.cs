using RightRule.Data;
using RightRule.Factories.RightFactory;
using RightRule.Operators;
using RightRule.RuleRegistration;
using RightRule.StatementProcessor.Evaluator;
using RightRule.System.Common;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IRightRuleFactory _factory;
    private readonly IEvaluator _evaluator;


    private readonly IOperatorsService _operatorsService;

    private readonly IRegisteredRuleService _registerRuleService;
    public IRegisteredRules RegisteredRules => _registerRuleService.RegisteredRules;

    private readonly IRegisteredDatasets? _registeredDataSets;
    public IRegisteredDatasets RegisteredDatasets => _registeredDataSets!;

    private readonly IRegisteredDataObjects? _registeredDataObjects;
    public IRegisteredDataObjects RegisteredDataObjects => _registeredDataObjects!;

    public RightRuleService(IServiceProvider ServiceProvider, IRightRuleFactory factory, IOperatorsService operatorsService, IRegisteredRuleService registeredRuleService, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects)
    {
        _serviceProvider = ServiceProvider;
        _factory = factory;

        _operatorsService = operatorsService;
        _registerRuleService = registeredRuleService;
        _registeredDataSets = registeredDatasets;
        _registeredDataObjects = registeredDataObjects;

        _evaluator = CreateEvaluator(_operatorsService.OperatorAliases);
    }

    private IEvaluator CreateEvaluator(Dictionary<string, Operator> Operators)
    {
        return _factory.CreateEvaluator(_serviceProvider, Operators);
    }
}
