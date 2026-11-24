using RightRule.Models;

namespace RightRuleTests;

public class OperatorLoadOptionsTests_Manual
{
    [Fact]
    public void RightRule_RunManually_WithoutOptions()
    {
        var rr = new RightRule.RightRule().CreateSession();
        Assert.NotNull(rr);
        Assert.NotNull(rr.OperatorService);

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
        RightRuleOptions options = new RightRuleOptions 
        { 
            LoadDefaultOperators = true,
            LoadCustomOperators = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();
        Assert.NotNull(rr);
        Assert.NotNull(rr.OperatorService);

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
        Assert.NotNull(rr.OperatorService);

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

    [Fact]
    public void RightRule_LoadsOnlySelectedDefaultOperators()
    {
        var options = new RightRuleOptions
        {
            LoadDefaultOperators = true,
            DefaultOperatorNames = ["EqualsOperator", "InOperator"],
            LoadCustomOperators = false,
            CustomOperatorNames = []
        };

        var rr = new RightRule.RightRule(options).CreateSession();
        Assert.NotNull(rr);
        Assert.NotNull(rr.OperatorService);

        var operators = rr.OperatorService.Operators;

        Assert.Equal(2, operators.Count);
        Assert.Equal(
            ["EqualsOperator", "InOperator"],
            operators.Select(op => op.Name).OrderBy(name => name).ToArray());
    }

    [Fact]
    public void RightRule_SpecifiesOperatorsButLoadFalse()
    {
        var options = new RightRuleOptions
        {
            LoadDefaultOperators = false,
            DefaultOperatorNames = ["EqualsOperator", "InOperator"],
            LoadCustomOperators = false,
            CustomOperatorNames = []
        };

        var rr = new RightRule.RightRule(options).CreateSession();
        Assert.NotNull(rr);
        Assert.NotNull(rr.OperatorService);

        var operators = rr.OperatorService.Operators;
        Assert.Empty(operators);
    }

    [Fact]
    public void RightRule_LoadSpecificCustomOperators()
    {
        var options = new RightRuleOptions
        {
            LoadDefaultOperators = false,
            DefaultOperatorNames = [],
            LoadCustomOperators = true,
            CustomOperatorNames = ["Exists"]
        };

        var rr = new RightRule.RightRule(options).CreateSession();
        Assert.NotNull(rr);
        Assert.NotNull(rr.OperatorService);

        var operators = rr.OperatorService.Operators;
        Assert.Single(operators);
        Assert.Equal("Exists", operators.Single().Name);
    }

    [Fact]
    public void RightRule_LoadSpecificCustomAndDefaultOperators()
    {
        var options = new RightRuleOptions
        {
            LoadDefaultOperators = true,
            DefaultOperatorNames = ["EqualsOperator"],
            LoadCustomOperators = true,
            CustomOperatorNames = ["Exists"]
        };

        var rr = new RightRule.RightRule(options).CreateSession();
        Assert.NotNull(rr);
        Assert.NotNull(rr.OperatorService);

        var operators = rr.OperatorService.Operators;
        Assert.Equal(2, operators.Count);
        Assert.Equal("EqualsOperator", operators[0].Name);
        Assert.Equal("Exists", operators[1].Name);
    }
}
