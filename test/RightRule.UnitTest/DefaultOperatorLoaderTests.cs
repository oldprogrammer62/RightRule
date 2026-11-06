using Microsoft.Extensions.Logging.Abstractions;
using RightRule.Operators.DefaultOperators;
using RightRule.OperatorServices.OperatorLoader.DefaultOperators;

namespace RightRuleTests;

public sealed class DefaultOperatorLoaderTests
{
    private static DefaultOperatorLoader CreateDefaultLoader() =>
        new(NullLogger<DefaultOperatorLoader>.Instance);

    [Fact]
    public void Load_ReturnsDefaultOperatorsInExpectedOrder()
    {
        var operators = CreateDefaultLoader().Load();

        var expectedTypes = new[]
        {
            typeof(EqualsOperator),
            typeof(NotEqualsOperator),
            typeof(ContainsOperator),
            typeof(DoesNotContainOperator),
            typeof(LessThanOperator),
            typeof(GreaterThanOrEqualsOperator),
            typeof(InOperator)
        };

        Assert.Equal(expectedTypes, operators.Select(op => op.GetType()));
    }

    [Fact]
    public void Load_ReturnsNewListAndOperatorInstancesOnEachCall()
    {
        var firstLoad = CreateDefaultLoader().Load();
        var secondLoad = CreateDefaultLoader().Load();

        Assert.NotSame(firstLoad, secondLoad);
        Assert.Equal(firstLoad.Count, secondLoad.Count);

        for (var index = 0; index < firstLoad.Count; index++)
        {
            Assert.NotSame(firstLoad[index], secondLoad[index]);
        }
    }
}