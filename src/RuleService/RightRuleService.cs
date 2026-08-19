using RightRule.Data;
using RightRule.Operators;
using RightRule.RuleRegistration;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    private readonly IOperatorsService _operatorsService;

    private readonly IRegisteredRuleService _registerRuleService;
    public IRegisteredRules RegisteredRules => _registerRuleService.RegisteredRules;

    private readonly IRegisteredDatasets? _registeredDataSets;
    public IRegisteredDatasets RegisteredDatasets => _registeredDataSets!;

    private readonly IRegisteredDataObjects? _registeredDataObjects;
    public IRegisteredDataObjects RegisteredDataObjects => _registeredDataObjects!;

    public RightRuleService(IOperatorsService operatorsService, IRegisteredRuleService registeredRuleService, IRegisteredDatasets registeredDatasets, IRegisteredDataObjects registeredDataObjects)
    {
        _operatorsService = operatorsService;
        _registerRuleService = registeredRuleService;
        _registeredDataSets = registeredDatasets;
        _registeredDataObjects = registeredDataObjects;
    }
}
