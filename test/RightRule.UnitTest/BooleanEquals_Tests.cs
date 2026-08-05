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

        var x = rr.RegisterDataset("Data", _data);
    }

    [Fact]
    public void BooleanEquals1()
    {
        RuleDefinition rule1 = new RuleDefinition
        {
            Name = "BooleanEquals1",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "Data.IsTrue = \"true\""
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateAllRules("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result[0].Decision);
    }

    [Fact]
    public void BooleanEquals2()
    {
        RuleDefinition rule1 = new RuleDefinition
        {
            Name = "BooleanEquals2",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "true = Data.IsTrue"
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstRule("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result.Decision);
    }

    [Fact]
    public void BooleanEquals3()
    {
        RuleDefinition rule1 = new RuleDefinition
        {
            Name = "BooleanEquals3",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "true = true"
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstRule("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result.Decision);
    }

    [Fact]
    public void BooleanEquals4()
    {
        RuleDefinition rule1 = new RuleDefinition
        {
            Name = "BooleanEquals4",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "false = false"
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstRule("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result.Decision);
    }

    [Fact]
    public void BooleanEquals5()
    {
        RuleDefinition rule1 = new RuleDefinition
        {
            Name = "BooleanEquals5",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "FALSE = false"
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstRule("Context");

        Assert.NotNull(result);
        Assert.Equal("Good", result.Decision);
    }

    [Fact]
    public void BooleanEquals6()
    {
        RuleDefinition rule1 = new RuleDefinition
        {
            Name = "BooleanEquals6",
            Number = 1,
            Context = "Context",
            Decision = "Good",
            Statement = "FALSE = \"false\""
        };

        var pr = rr.RegisterRule(rule1);
        var result = rr.EvaluateFirstRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule1.Decision, result.Decision);
    }
}
