using RightRule;
using RightRule.Models;

namespace RightRuleTests;

public class OperatorLoadOptionsTests_Manual
{
    [Fact]
    public void RightRule_RunManually_WithoutOptions()
    {
        var rr = new RightRule.RightRule().CreateSession();

        Assert.NotNull(rr);

        var operators = rr.OperatorService.Operators;
        Assert.NotNull(operators);
        Assert.Equal(10, operators.Count);

        var defaultOperators = operators.Where(op => op.IsCustom == false).ToList();
        Assert.NotNull(defaultOperators);
        Assert.Equal(7, defaultOperators.Count);

        var customOperators = operators.Where(op => op.IsCustom == true).ToList();
        Assert.NotNull(customOperators);
        Assert.Equal(3, customOperators.Count);
    }

    [Fact]
    public void RightRule_RunManually_WithOptions_True()
    {
        RightRuleOptions options = new RightRuleOptions { LoadDefaultOperators = true };
        var rr = new RightRule.RightRule(options).CreateSession();

        Assert.NotNull(rr);

        var operators = rr.OperatorService.Operators;
        Assert.NotNull(operators);
        Assert.Equal(10, operators.Count);

        var defaultOperators = operators.Where(op => op.IsCustom == false).ToList();
        Assert.NotNull(defaultOperators);
        Assert.Equal(7, defaultOperators.Count);

        var customOperators = operators.Where(op => op.IsCustom == true).ToList();
        Assert.NotNull(customOperators);
        Assert.Equal(3, customOperators.Count);
    }

    [Fact]
    public void RightRule_RunManually_WithOptions_False()
    {
        RightRuleOptions options = new RightRuleOptions { LoadDefaultOperators = false };
        var rr = new RightRule.RightRule(options).CreateSession();

        Assert.NotNull(rr);

        var operators = rr.OperatorService.Operators;
        Assert.NotNull(operators);
        Assert.Equal(3, operators.Count);

        var defaultOperators = operators.Where(op => op.IsCustom == false).ToList();
        Assert.NotNull(defaultOperators);
        Assert.Equal(0, defaultOperators?.Count ?? 0);

        var customOperators = operators.Where(op => op.IsCustom == true).ToList();
        Assert.NotNull(customOperators);
        Assert.Equal(3, customOperators.Count);
    }
}
