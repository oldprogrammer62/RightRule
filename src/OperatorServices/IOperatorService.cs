using RightRule.System.Common;

namespace RightRule.OperatorServices;

public interface IOperatorService
{
    List<Operator> Operators { get; }
    Dictionary<string, Operator> OperatorAliases { get; }
    HashSet<string> OperatorNames { get; }
    bool DeRegisterOperator(string OperatorName);
}
