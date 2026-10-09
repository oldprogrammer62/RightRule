using RightRule.Models;
using RightRule.System.Common.Operator.Models;

namespace RightRule.OperatorServices;

public interface IOperatorService
{
    List<Operator> Operators { get; }
    Dictionary<string, Operator> OperatorAliases { get; }

    OperatorRegistrationResult RegisterAllOperators();
    OperatorRegistrationResult RegisterAllOperators(RightRuleOptions options);
    OperatorRegistrationResult RegisterOperator(Operator Operator);

    List<Operator> GetOperators();
    Dictionary<string, Operator> GetRegisteredAliases();
    Dictionary<string, Operator> GetRegisteredAliases(Operator Operator);

    bool DeRegisterOperator(string OperatorName);
}
