using RightRule.Models;
using RightRule.OperatorServices.OperatorLoader;
using RightRule.OperatorServices.OperatorLoader.CustomOperators;
using RightRule.OperatorServices.OperatorLoader.DefaultOperators;
using RightRule.System.Common.Operator.Models;
using System.ComponentModel;

namespace RightRule.OperatorServices;

internal sealed partial class OperatorService : IOperatorService, IDisposable
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    private List<Operator> _operators;
    public List<Operator> Operators { get { return _operators; } }

    [EditorBrowsable(EditorBrowsableState.Never)]
    private Dictionary<string, Operator> _operatorAliases;
    public Dictionary<string, Operator> OperatorAliases { get { return _operatorAliases; } }

    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly RightRuleOptions _options;


    public OperatorService() : this(new RightRuleOptions { LoadDefaultOperators = true })
    {
    }

    public OperatorService(RightRuleOptions options)
    {
        _operators = new List<Operator>();
        _operatorAliases = new Dictionary<string, Operator>(StringComparer.OrdinalIgnoreCase);

        _options = options;

        RegisterAllOperators(options);
    }

    public OperatorRegistrationResult RegisterAllOperators()
    {
        return RegisterAllOperators(new RightRuleOptions { LoadDefaultOperators = true });
    }

    public OperatorRegistrationResult RegisterAllOperators(RightRuleOptions options)
    {
        try
        {
            _operators = LoadStartUpOperators(options);
            if (_operators.Count == 0)
            {
                return OperatorRegistrationResult.Success("No Operators registered. RightRule Options expect no Operators to be loaded");
            }

            _operatorAliases = MakeOperatorAliases(_operators);
            return OperatorRegistrationResult.Success("All Operators registered successfully.");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public OperatorRegistrationResult RegisterOperator(Operator Operator)
    {
        if (!OperatorExists(Operator))
        {
            _operators.Add(Operator);
        }
        var aliases = Helpers.MakeCleanAliases(Operator);

        if (!OperatorAliasesUnique(_operatorAliases, aliases))
        {
            return OperatorRegistrationResult.Failed($"Operator aliases must be unique across all registered Operators. Operator '{Operator.Name}' has duplicate aliases.");
        }
        _operatorAliases = RegisterOperatorAliases(_operatorAliases, Operator);

        return OperatorRegistrationResult.Success($"Operator '{Operator.Name}' registered successfully.");
    }
    
    private List<Operator> LoadStartUpOperators(RightRuleOptions options)
    {
        var operators = LoadAllOperators(options);

        if (operators == null || operators.Count == 0)
            return [];

        if (!OperatorAliasesUnique(operators))
            throw new Exception("Operator aliases must be unique across all registered Operators.");

        return operators;
    }

    private List<Operator> LoadAllOperators(RightRuleOptions options)
    {
        var loadDefaultOperators = options.LoadDefaultOperators;
        var loadCustomOperators = options.LoadCustomOperators;

        if (loadDefaultOperators && loadCustomOperators)
        {
            return [
                .. LoadDefaultOperators(),
                .. LoadCustomOperators()
                ];
        }
        if (loadDefaultOperators)
        {
            return [
                .. LoadDefaultOperators()
                ];
        }
        if (loadCustomOperators)
        {
            return [
                .. LoadCustomOperators()
                ];
        }
        return [];
    }

    internal static List<Operator> LoadDefaultOperators()
    {
        return DefaultOperatorLoader.Load();
    }

    internal List<Operator> LoadCustomOperators()
    {
        string configurationPath = _options.CustomOperatorLibraryPath;
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
