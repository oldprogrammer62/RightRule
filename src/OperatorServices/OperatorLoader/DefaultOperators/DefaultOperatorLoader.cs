using Microsoft.Extensions.Logging;
using RightRule.Operators.DefaultOperators;
using RightRule.System.Common.Operator.Models;

namespace RightRule.OperatorServices.OperatorLoader.DefaultOperators;

internal sealed class DefaultOperatorLoader : IDefaultOperatorLoader
{
    private readonly ILogger<DefaultOperatorLoader> _logger;

    public DefaultOperatorLoader(ILogger<DefaultOperatorLoader> logger)
    {
        _logger = logger;
    }

    public List<Operator> Load()
    {
        return new List<Operator>()
        {
            new EqualsOperator(),
            new NotEqualsOperator(),
            new ContainsOperator(),
            new DoesNotContainOperator(),
            new LessThanOperator(),
            new GreaterThanOrEqualsOperator(),
            new InOperator()
        };
    }
}
