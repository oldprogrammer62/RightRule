using RightRule.RuleModels;

namespace RightRuleTests;

public class LessThanTests
{
    private readonly Dictionary<string, object> _Data;

    public LessThanTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "intvalue", 100 },
            { "floatvalue", 100.01 },
            { "datevalue", new DateOnly(1970, 01, 01) },
            { "datetimevalue", new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 30, 30)) }
        };
    }

    [Fact]
    public void LessThan_Int()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Less Than Int Value Rule",
            Statement = @"Test.intvalue lessthan 1000",
            Context = "Context",
            Decision = "LESSTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void LessThan_Int_And_Decimal()
    {

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Less Than Int Value Rule",
            Statement = @"Test.intvalue lessthan 1000.00",
            Context = "Context",
            Decision = "LESSTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void NotLessThan()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Not Less Than Int Value Rule",
            Statement = @"Test.intvalue lessthan 5",
            Context = "Context",
            Decision = "NOT_LESSTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(AppliedRule.DefaultDecision, appliedRule.Decision);
    }

    [Fact]
    public void LessThan_Float()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Less Than Float Value Rule",
            Statement = @"Test.floatvalue lessthan 1000.001",
            Context = "Context",
            Decision = "LESSTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void LessThan_DateOnly()
    {
        var date = new DateOnly(1971, 01, 01);

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Less Than Date Only Value Rule",
            Statement = @$"Test.datevalue lessthan ""{date}""",
            Context = "Context",
            Decision = "LESSTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void LessThan_DateTime_DateDiff()
    {
        var date = new DateTime(new DateOnly(1971, 01, 01), new TimeOnly(12, 30, 30));

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Less Than Datetime Value Rule",
            Statement = @$"Test.datetimevalue lessthan ""{date}""",
            Context = "Context",
            Decision = "LESSTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void LessThan_DateTime_TimeDiff()
    {
        var date = new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 35, 30));

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Less Than Datetime Value Rule",
            Statement = @$"Test.datetimevalue lessthan ""{date}""",
            Context = "Context",
            Decision = "LESSTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.EvaluateFirstMatch("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
