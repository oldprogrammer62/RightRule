using RightRule.OperatorServices.OperatorLoader;
using RightRule.OperatorServices.OperatorLoader.CustomOperators;
using RightRule.OperatorServices.OperatorLoader.DefaultOperators;
using RightRule.System.Common;

namespace RightRule.OperatorServices;

internal partial class OperatorService : IOperatorService, IDisposable
{
    private List<Operator> _operators;
    public List<Operator> Operators { get { return _operators; } }

    private Dictionary<string, Operator> _operatorAliases;
    public Dictionary<string, Operator> OperatorAliases { get { return _operatorAliases; } }

    public OperatorService()
    {
        _operators = new List<Operator>();
        _operatorAliases = new Dictionary<string, Operator>(StringComparer.OrdinalIgnoreCase);

        RegisterAllOperators();
    }

    public bool RegisterAllOperators()
    {
        _operators = LoadStartUpOperators();
        _operatorAliases = MakeOperatorAliases(_operators);
        return true;
    }

    public bool RegisterOperator(Operator Operator)
    {
        if (!OperatorExists(Operator))
        {
            _operators.Add(Operator);
        }
        var aliases = Helpers.MakeCleanAliases(Operator);

        if (!OperatorAliasesUnique(_operatorAliases, aliases))
        {
            throw new Exception($"Operator aliases must be unique across all registered Operators. Operator '{Operator.Name}' has duplicate aliases.");
        }
        _operatorAliases = RegisterOperatorAliases(_operatorAliases, Operator);
        return true;
    }
    
    private List<Operator> LoadStartUpOperators()
    {
        var operators = LoadAllOperators();

        if (operators == null || operators.Count == 0)
            throw new Exception("No Operators found.");

        if (!OperatorAliasesUnique(operators))
            throw new Exception("Operator aliases must be unique across all registered Operators.");

        return operators;
    }

    private static List<Operator> LoadAllOperators()
    {
        return
            [
            .. LoadDefaultOperators(),
            .. LoadCustomOperators()
            ];
    }

    internal static List<Operator> LoadDefaultOperators()
    {
        return DefaultOperatorLoader.Load();
    }

    internal static List<Operator> LoadCustomOperators()
    {
        string configurationPath = Helpers.GetConfigurationPath();
        if (string.IsNullOrEmpty(configurationPath) || !File.Exists(configurationPath))
        {
            return new List<Operator>();
        }
        return CustomOperatorLoader.LoadOperators(configurationPath);
    }

    public List<Operator> GetOperators()
    {
        return [.. _operators];
    }

    private bool OperatorExists(Operator Operator)
    {
        return Helpers.OperatorNameExists(_operators, Operator);
    }

    private bool OperatorAliasesUnique(List<Operator> Operators)
    {
        return Helpers.OperatorAliasesUnique(Operators);
    }

    private bool OperatorAliasesUnique(Dictionary<string, Operator> RegisteredAliases, Dictionary<string, Operator> OperatorAliases)
    {
        string[] aliases = [.. OperatorAliases.Select(a => a.Key)];
        return Helpers.OperatorAliasesUnique(RegisteredAliases, aliases);
    }

    private Dictionary<string, Operator> MakeOperatorAliases(List<Operator> Operators)
    {
        var allAliases = new Dictionary<string, Operator>(StringComparer.OrdinalIgnoreCase);
        foreach (var op in Operators)
        {
            var operatorAliases = GetCleanAliases(op);
            if (!OperatorAliasesUnique(allAliases, operatorAliases))           {
                throw new Exception($"Operator aliases must be unique across all registered Operators. Operator '{op.Name}' has duplicate aliases.");
            }

            foreach (var alias in operatorAliases)
            {
                if (string.IsNullOrWhiteSpace(alias.Key))
                    continue;
                allAliases[alias.Key] = op;
            }
        }
        return allAliases;
    }

    private Dictionary<string, Operator> RegisterOperatorAliases(Dictionary<string, Operator> Aliases, Operator Op)
    {
        var opAliases = GetCleanAliases(Op);
        foreach (var alias in opAliases)
        {
            if (string.IsNullOrWhiteSpace(alias.Key))
                continue;
            Aliases[alias.Key] = Op;
        }
        return Aliases;
    }

    public void Dispose()
    {
        foreach(var op in _operators)
        {
            op.Dispose();
        }
    }
}
