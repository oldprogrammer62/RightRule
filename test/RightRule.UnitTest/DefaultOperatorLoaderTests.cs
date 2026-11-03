using RightRule.OperatorServices.OperatorLoader.DefaultOperators;
using RightRule.Operators.DefaultOperators;
using Xunit;

namespace RightRuleTests;

public sealed class DefaultOperatorLoaderTests
{
    [Fact]
    public void Load_ReturnsDefaultOperatorsInExpectedOrder()
    {
        var operators = DefaultOperatorLoader.Load();

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
        var firstLoad = DefaultOperatorLoader.Load();
        var secondLoad = DefaultOperatorLoader.Load();

        Assert.NotSame(firstLoad, secondLoad);
        Assert.Equal(firstLoad.Count, secondLoad.Count);

        for (var index = 0; index < firstLoad.Count; index++)
        {
            Assert.NotSame(firstLoad[index], secondLoad[index]);
        }
    }
}