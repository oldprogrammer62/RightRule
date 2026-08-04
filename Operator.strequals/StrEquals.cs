
namespace Operator.strequals
{
    public class StrEquals : RightRule.System.Common.Operator
    {
        public override List<string> Identifiers => ["strequals", "=str"];

        protected override Task<bool> DoOperatorProcessAsync(object left, object right, object? OperatorConfig)
        {
            if (left is string leftStr && right is string rightStr)
            {
                return Task.FromResult(Equals(leftStr, rightStr, OperatorConfig));
            }
            else
            {
                throw new ArgumentException("Both left and right operands must be strings.");
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
