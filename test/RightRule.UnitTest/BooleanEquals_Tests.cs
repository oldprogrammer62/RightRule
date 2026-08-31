using RightRule.RuleModels;

namespace RightRuleTests;

public class BooleanEquals_Tests
{
    Dictionary<string, object> _data = new Dictionary<string, object>();
    BeSoft.IRightRule rr = new BeSoft.RightRule();

    public BooleanEquals_Tests()
    {
        _data.Add("IsTrue", true);
        _data.Add("IsFalse", false);

        var x = rr.RegisterData("Data", _data);
    }

    [Fact]
    public void BooleanEquals1()
    {
        DefinedRule rule1 = new DefinedRule
        {
            Name = "BooleanEquals1",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "Data.IsTrue = \"true\""
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateAll("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result[0].Decision);
    }

    [Fact]
    public void BooleanEquals2()
    {
        DefinedRule rule1 = new DefinedRule
        {
            Name = "BooleanEquals2",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "true = Data.IsTrue"
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result.Decision);
    }

    [Fact]
    public void BooleanEquals3()
    {
        DefinedRule rule1 = new DefinedRule
        {
            Name = "BooleanEquals3",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "true = true"
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result.Decision);
    }

    [Fact]
    public void BooleanEquals4()
    {
        DefinedRule rule1 = new DefinedRule
        {
            Name = "BooleanEquals4",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "false = false"
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result.Decision);
    }

    [Fact]
    public void BooleanEquals5()
    {
        DefinedRule rule1 = new DefinedRule
        {
            Name = "BooleanEquals5",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "FALSE = false"
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result.Decision);
    }

    [Fact]
    public void BooleanEquals6()
    {
        // This will return a false result
        // Boolean false does not equal string false

        DefinedRule rule1 = new DefinedRule
        {
            Name = "BooleanEquals6",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "FALSE = \"false\""
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstMatch("Context");

        Assert.NotNull(result);
        Assert.Equal(DefaultRule.Decision, result.Decision);
    }
}
