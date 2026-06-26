using RightRule.System.Common;

namespace RightRule.Operators;

internal class OperatorsService
{
    private readonly List<IOperator> _operators;
    public List<IOperator> Operators => _operators;

    private readonly Dictionary<string, IOperator> _operatorAliases;
    public Dictionary<string, IOperator> OperatorAliases => _operatorAliases;

    private readonly HashSet<string> _operatorNames;
    public HashSet<string> OperatorNames => _operatorNames;

    internal OperatorsService()
    {
        _operators = RegisterOperators();
        _operatorAliases = RegisterOperatorAliases(_operators);
        _operatorNames = RegisterOperatorNames(_operatorAliases);
    }

    internal  bool DeRegisterOperator(string OperatorName)
    {
        var opToRemove = _operators.FirstOrDefault(op => op.Identifiers.Contains(OperatorName, StringComparer.OrdinalIgnoreCase));
        if (opToRemove != null)
        {
            return DeRegisterOperator(OperatorName, opToRemove);
        }
        return false;
    }

    internal bool DeRegisterOperator(string OperatorName, IOperator Operator)
    {
        if (Operator != null)
        {
            RemoveOperatorAliases(Operator);
            RemoveOperatorNames(OperatorName);

            Operator.Dispose();

            RemoveOperator(Operator);
            return true;
        }
        return false;
    }

    private void RemoveOperator(IOperator Operator)
    {
        _operators.Remove(Operator);
    }

    private void RemoveOperatorAliases(IOperator Operator)
    {
        foreach (var opp in _operatorAliases.ToList())
        {
            if (opp.Value.Equals(Operator))
            {
                _operatorAliases.Remove(opp.Key);
            }
        }
    }

    private void RemoveOperatorNames(string OperatorName)
    {
        foreach (var opp in _operatorNames.ToList())
        {
            if (opp.Equals(OperatorName, StringComparison.OrdinalIgnoreCase))
            {
                _operatorNames.Remove(opp);
                break;
            }
        }
    }

    private static List<IOperator> RegisterOperators()
    {
        var operatorNames = LoadAllOperators();
        if (operatorNames == null || operatorNames.Count == 0)
            throw new Exception("No operators found.");

        if (OperatorIdentifiersUnique(operatorNames) == false)
            throw new Exception("Operator identifiers must be unique.");

        return operatorNames;
    }

    private static bool OperatorIdentifiersUnique(List<IOperator> operators)
    {
        if (operators == null || operators.Count == 0)
            return true;

        // Flatten all identifiers, ignore null/whitespace, compare case-insensitively.
        var hasDuplicates = operators
            .Where(op => op?.Identifiers != null)
            .SelectMany(op => op.Identifiers)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .GroupBy(id => id, StringComparer.OrdinalIgnoreCase)
            .Any(g => g.Count() > 1);

        return !hasDuplicates;
    }

    private static List<IOperator> LoadAllOperators()
    {
        return
            [
            .. GetDefaultOperators(),
            .. RegisterCustomOperators()
            ];
    }

    private static List<IOperator> GetDefaultOperators()
    {
        return DefaultOperatorLoader.Load();
    }

    private static List<IOperator> RegisterCustomOperators()
    {
        return CustomOperatorLoader.Load();
    }

    private static Dictionary<string, IOperator> RegisterOperatorAliases(List<IOperator> operators)
    {
        var operatorAliases = new Dictionary<string, IOperator>(StringComparer.OrdinalIgnoreCase);
        foreach (var op in operators)
        {
            foreach (var alias in op.Identifiers)
            {
                if (string.IsNullOrWhiteSpace(alias))
                    continue;
                operatorAliases[alias] = op;
            }
        }
        return operatorAliases;
    }

    private static HashSet<string> RegisterOperatorNames(Dictionary<string, IOperator> OperatorAliases)
    {
        var operatorNames = new HashSet<string>(OperatorAliases.Values.SelectMany(op => op.Identifiers).Distinct(StringComparer.OrdinalIgnoreCase));
        return operatorNames;
    }
}
