using RightRule.System.Common;

namespace RightRule.RuleService;

internal partial class RightRuleService : IRightRuleService
{
    public Dictionary<string, Operator> GetOperatorAliases()
    {
        return _operatorsService.OperatorAliases;
    }

    public List<Operator> GetOperatorDetails()
    {
        return _operatorsService.Operators;
    }

    public bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        return _operatorsService.DeRegisterOperator(OperatorName);
    }
}
