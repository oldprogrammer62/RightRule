
using RightRule.System.Common;

namespace Operator.strequals
{
    public class StrEquals : RightRule.System.Common.Operator
    {
        public override List<string> Identifiers => ["strequals", "=str"];

        protected override async Task<EvaluationResult> DoOperatorProcessAsync(object left, object right, object? OptionalData, object? DecisionData, bool Diagnostics)
        {
            if (left is string leftStr && right is string rightStr)
            {
                return EvaluationResult.Success(Equals(leftStr, rightStr, OptionalData), DecisionData);
            }
            else
            {
                return EvaluationResult.Failure("Both left and right operands must be strings.", DecisionData);
            }
        }

        private static bool Equals(string left, string right, object? Config)
        {
            var stringComparison = ResolveConfig(Config);
            return string.Equals(left, right, stringComparison);
        }

        private static StringComparison ResolveConfig(object? Config)
        {
            if (Config == null)
            {
                return StringComparison.CurrentCultureIgnoreCase;
            }
            return (StringComparison)Config;
        }
    }
}
