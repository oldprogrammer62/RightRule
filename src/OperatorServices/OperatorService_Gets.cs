using RightRule.OperatorServices.OperatorLoader;
using RightRule.System.Common.Operator.Models;

namespace RightRule.OperatorServices;

internal sealed partial class OperatorService : IOperatorService, IDisposable
{
    public Dictionary<string, Operator> GetRegisteredAliases()
    {
        return OperatorAliases;
    }

    public Dictionary<string, Operator> GetRegisteredAliases(Operator Operator)
    {
        return OperatorAliases.Where(kvp => kvp.Value == Operator).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private static Dictionary<string, Operator> GetCleanAliases(Operator Operator)
    {
        return Helpers.MakeCleanAliases(Operator);
    }
}
