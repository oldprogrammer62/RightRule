using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class BooleanEquals_Tests
{
    Dictionary<string, object> _data = new Dictionary<string, object>();
    IRightRule rr = new RightRule.RightRule();

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
            Statement = "Data.IsTrue = \"true\"",
            TrueDecision = "Good",
            FalseDecision = "NotGood"
        };

        var pr = rr.RegisterRule(rule1);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, pr.Status);

        var result = rr.ApplyRules("Context", EvaluationTypeEnum.ANY);
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
            Statement = "true = Data.IsTrue",
            TrueDecision = "Good",
            FalseDecision = "NotGood"
        };

        var pr = rr.RegisterRule(rule1);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, pr.Status);

        var result = rr.ApplyRule("Context");
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
            Statement = "true = true",
            TrueDecision = "Good",
            FalseDecision = "NotGood"
        };

        var pr = rr.RegisterRule(rule1);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, pr.Status);

        var result = rr.ApplyRule("Context");
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
            Statement = "false = false",
            TrueDecision = "Good",
            FalseDecision = "NotGood"
        };

        var pr = rr.RegisterRule(rule1);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, pr.Status);

        var result = rr.ApplyRule("Context");
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
            Statement = "False = false",
            TrueDecision = "Good",
            FalseDecision = "NotGood"
        };

        var pr = rr.RegisterRule(rule1);
        Assert.Equal(RightRuleStatusEnum.ERROR, pr.Status);
    }

    [Fact]
    public void BooleanEquals6()
    {
        // Boolean false is case sensitive - lower case
        // Boolean as a string is nopt case sensitive
        // Result will be true
        // Decision will be returned

        DefinedRule rule1 = new DefinedRule
        {
            Name = "BooleanEquals6",
            Number = 1,
            Context = "Context",
            TrueDecision = "Good",
            FalseDecision = "NotGood",
            Statement = "false = \"False\""
        };

        var pr = rr.RegisterRule(rule1);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, pr.Status);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule1.TrueDecision, result.Decision);
    }

    [Fact]
    public void BooleanEquals7()
    {
        // This will return a true result

        DefinedRule rule1 = new DefinedRule
        {
            Name = "BooleanEquals6",
            Number = 1,
            Context = "Context",
            TrueDecision = "Good",
            FalseDecision = "NotGood",
            Statement = "false = \"false\""
        };

        var pr = rr.RegisterRule(rule1);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, pr.Status);

        var result = rr.ApplyRule("Context");

        Assert.NotNull(result);
        Assert.Equal(rule1.TrueDecision, result.Decision);
    }
}
