
using RightRule.System.Common;

namespace Operator.strequals
{
    public class StrEquals : RightRule.System.Common.Operator
    {
        public override List<string> Identifiers => ["strequals", "=str"];

        protected override async Task<OperatorResult> DoOperatorProcessAsync(object left, object right, object? OptionalData, bool Diagnostics)
        {
            if (left is string leftStr && right is string rightStr)
            {
                return OperatorResult.SuccessResult(this.Name, Equals(leftStr, rightStr, OptionalData));
            }
            else
            {
                return OperatorResult.FailureResult(this.Name, "Both left and right operands must be strings.");
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
