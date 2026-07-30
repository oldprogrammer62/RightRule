using RightRule.RuleModels;

namespace RightRuleTests;

public class GreaterThanOrEqualsTests
{
    private readonly Dictionary<string, object> _Data;

    public GreaterThanOrEqualsTests()
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
    public void GreaterThan_Int_And_Decimal()
    {

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Int Value Rule",
            Statement = @"Test.intvalue greaterthanorequals 10.00",
            Context = "Context",
            Decision = "GREATERTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThan_Int()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Int Value Rule",
            Statement = @"Test.intvalue greaterthanorequals 10",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThanEquals_Int()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Equals Int Value Rule",
            Statement = @"Test.intvalue greaterthanorequals 100",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThan_Float()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Float Value Rule",
            Statement = @"Test.floatvalue greaterthanorequals 99.9",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThanEquals_Float()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Equals Float Value Rule",
            Statement = @"Test.floatvalue greaterthanorequals 100.01",
            Context = "Context",
            Decision = "GREATERTHANOREQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThan_DateOnly()
    {
        var date = new DateOnly(1969, 01, 01);

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Less Than Date Only Value Rule",
            Statement = @$"Test.datevalue greaterthanorequals ""{date}""",
            Context = "Context",
            Decision = "GREATERTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThan_DateOnly_Equals()
    {
        var date = new DateOnly(1970, 01, 01);

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Less Than Date Only Value Rule",
            Statement = @$"Test.datevalue greaterthanorequals ""{date}""",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThanOrEquals_DateTime_DateDiff()
    {
        var date = new DateTime(new DateOnly(1969, 01, 01), new TimeOnly(12, 30, 30));

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Datetime Value Rule",
            Statement = @$"Test.datetimevalue greaterthanorequals ""{date}""",
            Context = "Context",
            Decision = "GREATERTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThan_DateTime_TimeDiff()
    {
        var date = new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 25, 30));

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Datetime Value Rule",
            Statement = @$"Test.datetimevalue greaterthanorequals ""{date}""",
            Context = "Context",
            Decision = "GREATERTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void GreaterThanOrEquals_DateTime_Equals()
    {
        var date = new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 30, 30));

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Greater Than Datetime Value Rule",
            Statement = @$"Test.datetimevalue greaterthanorequals ""{date}""",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void NotGreaterThan()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Not Greater Than Value Rule",
            Statement = @"Test.intvalue greaterthanorequals 1010",
            Context = "Context",
            Decision = "NOT_GREATERTHAN",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(AppliedRule.DefaultDecision, appliedRule.Decision);
    }
}
