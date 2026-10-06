using RightRule.System.Common;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public bool RegisterOperator(Operator Operator)
    {
        ArgumentNullException.ThrowIfNull(Operator, nameof(Operator));
        return OperatorService.RegisterOperator(Operator);
    }

    public Dictionary<string, Operator> GetOperatorAliases()
    {
        return OperatorService.OperatorAliases;
    }

    public List<Operator> GetOperators()
    {
        return OperatorService.Operators;
    }

    public bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        return OperatorService.DeRegisterOperator(OperatorName);
    }
}
