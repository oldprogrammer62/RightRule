using RightRule.System.Common;

namespace RightRule.Operators;

internal interface IOperatorsService
{
    List<Operator> Operators { get; }
    Dictionary<string, Operator> OperatorAliases { get; }
    HashSet<string> OperatorNames { get; }
    bool DeRegisterOperator(string OperatorName);
}
