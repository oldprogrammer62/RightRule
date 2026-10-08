using RightRule.OperatorServices;
using RightRule.System.Common.Operator.Models;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public OperatorRegistrationResult RegisterOperator(Operator Operator)
    {
        ArgumentNullException.ThrowIfNull(Operator, nameof(Operator));
        if (OperatorService == null)
        {
            return OperatorRegistrationResult.Failed("OperatorService is not initialized.");
        }
        return OperatorService.RegisterOperator(Operator);
    }

    public Dictionary<string, Operator> GetOperatorAliases()
    {
        return OperatorService?.OperatorAliases ?? new Dictionary<string, Operator>(StringComparer.OrdinalIgnoreCase);
    }

    public List<Operator> GetOperators()
    {
        return OperatorService?.Operators ?? new List<Operator>();
    }

    public bool UnloadOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        return OperatorService?.DeRegisterOperator(OperatorName) ?? false;
    }
}
