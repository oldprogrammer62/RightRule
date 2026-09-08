using RightRule.System.Common;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public Dictionary<string, Operator> GetOperatorAliases()
    {
        return OperatorsService.OperatorAliases;
    }

    public List<Operator> GetOperatorDetails()
    {
        return OperatorsService.Operators;
    }

    public bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        return OperatorsService.DeRegisterOperator(OperatorName);
    }
}
