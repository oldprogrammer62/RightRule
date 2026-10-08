using RightRule.System.Common;

namespace RightRule.OperatorServices;

internal partial class OperatorService : IOperatorService, IDisposable
{
    public bool DeRegisterOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        var opToRemove = _operators.FirstOrDefault(op => op.Aliases.Contains(OperatorName, StringComparer.OrdinalIgnoreCase));
        if (opToRemove != null)
        {
            return DeRegisterOperator(OperatorName, opToRemove);
        }
        return false;
    }

    private bool DeRegisterOperator(string OperatorName, Operator Operator)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        if (Operator != null)
        {
            RemoveOperatorAliases(Operator);
            RemoveOperator(Operator);
            Operator.Dispose();
            return true;
        }
        return false;
    }

    private void RemoveOperator(Operator Operator)
    {
        _operators.Remove(Operator);
    }

    private void RemoveOperatorAliases(Operator Operator)
    {
        ArgumentNullException.ThrowIfNull(Operator, nameof(Operator));
        var keysToRemove = _operatorAliases
            .Where(opp => opp.Value.Equals(Operator))
            .Select(opp => opp.Key)
            .ToList();

        keysToRemove.ForEach(key => _operatorAliases.Remove(key));
    }
}
