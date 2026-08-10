using RightRule.System.Common;
using System.Text.Json;

namespace Operator.objequals;

public class ObjEquals : RightRule.System.Common.Operator
{
    public override List<string> Identifiers => ["=O="];

    protected override Task<EvaluationResult> DoOperatorProcessAsync(object left, object right, object? OperatorConfig)
    {
        if (left == null && right == null)
        {
            return Task.FromResult(EvaluationResult.Failure("Missing values"));
        }
        if (OperatorConfig == null)
        {
            return Task.FromResult(EvaluationResult.Failure("Missing OperatorConfig (Type details)"));
        }

        if (OperatorConfig is string typeName)
        {
            Type targetType = Type.GetType(typeName, throwOnError: true) ?? throw new Exception($"Could not find type '{OperatorConfig}'");
            var xx = Convert.ChangeType(left, targetType);

            string json = JsonSerializer.Serialize(left);
            var x = JsonSerializer.Deserialize(json, targetType);
        }

        var result = EvaluationResult.Success(true);
        return Task.FromResult(result);
    }
}
