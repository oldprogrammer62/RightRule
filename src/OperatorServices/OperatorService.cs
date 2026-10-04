using RightRule.OperatorServices.OperatorLoader.CustomOperators;
using RightRule.OperatorServices.OperatorLoader.DefaultOperators;
using RightRule.System.Common;

namespace RightRule.OperatorServices;

internal class OperatorService : IOperatorService, IDisposable
{
    private readonly List<Operator> _operators;
    public List<Operator> Operators { get { return _operators; } }

    private readonly Dictionary<string, Operator> _operatorAliases;
    public Dictionary<string, Operator> OperatorAliases { get { return _operatorAliases; } }

    private readonly HashSet<string> _operatorNames;
    public HashSet<string> OperatorNames { get { return _operatorNames; } }

    public OperatorService()
    {
        _operators = RegisterOperators();
        _operatorAliases = RegisterOperatorAliases(_operators);
        _operatorNames = RegisterOperatorNames(_operatorAliases);
    }

    public bool DeRegisterOperator(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        var opToRemove = _operators.FirstOrDefault(op => op.Identifiers.Contains(OperatorName, StringComparer.OrdinalIgnoreCase));
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
            RemoveOperatorNames(OperatorName);

            Operator.Dispose();

            RemoveOperator(Operator);
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

    private void RemoveOperatorNames(string OperatorName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(OperatorName, nameof(OperatorName));
        var keysToRemove = _operatorNames
            .Where(opp => opp.Equals(OperatorName, StringComparison.InvariantCultureIgnoreCase))
            .Select(opp => opp)
            .ToList();

        keysToRemove.ForEach(key => _operatorNames.Remove(key));
    }

    private static List<Operator> RegisterOperators()
    {
        var operatorNames = LoadAllOperators();
        if (operatorNames == null || operatorNames.Count == 0)
            throw new Exception("No operators found.");

        if (OperatorIdentifiersUnique(operatorNames) == false)
            throw new Exception("Operator identifiers must be unique.");

        return operatorNames;
    }

    private static bool OperatorIdentifiersUnique(List<Operator> Operators)
    {
        if (Operators == null || Operators.Count == 0)
            return true;

        // Flatten all identifiers, ignore null/whitespace, compare case-insensitively.
        var hasDuplicates = Operators
            .Where(op => op?.Identifiers != null)
            .SelectMany(op => op.Identifiers)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .GroupBy(id => id, StringComparer.OrdinalIgnoreCase)
            .Any(g => g.Count() > 1);

        return !hasDuplicates;
    }

    private static List<Operator> LoadAllOperators()
    {
        return
            [
            .. GetDefaultOperators(),
            .. RegisterCustomOperators()
            ];
    }

    private static List<Operator> GetDefaultOperators()
    {
        return DefaultOperatorLoader.Load();
    }

    private static List<Operator> RegisterCustomOperators()
    {
        return CustomOperatorLoader.Load();
    }

    private static Dictionary<string, Operator> RegisterOperatorAliases(List<Operator> Operators)
    {
        var operatorAliases = new Dictionary<string, Operator>(StringComparer.OrdinalIgnoreCase);
        foreach (var op in Operators)
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

    private static HashSet<string> RegisterOperatorNames(Dictionary<string, Operator> OperatorAliases)
    {
        var operatorNames = new HashSet<string>(OperatorAliases.Values.SelectMany(op => op.Identifiers).Distinct(StringComparer.OrdinalIgnoreCase));
        return operatorNames;
    }

    public void Dispose()
    {
        foreach(var op in _operators)
        {
            op.Dispose();
        }
    }
}
