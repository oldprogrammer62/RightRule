using RightRule.System.Common;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public Dictionary<string, Operator> GetOperatorAliases()
    {
        return OperatorService.OperatorAliases;
    }

    public List<Operator> GetOperatorDetails()
    {
        return OperatorService.Operators;
    }

    public bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        return OperatorService.DeRegisterOperator(OperatorName);
    }
}
