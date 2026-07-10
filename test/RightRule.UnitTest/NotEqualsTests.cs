using RightRule.Extensions;
using RightRule.Rules;

namespace RightRuleTests;

public class NotEqualsTests
{
    private readonly Dictionary<string, object> _Data;

    public NotEqualsTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "boolvalue", false },
            { "intvalue", 101 },
            { "floatvalue", 101.01 },
            { "stringvalue", "Hello Worldx" },
            { "datevalue", new DateOnly(1971, 01, 01) },
            { "datetimevalue", new DateTime(new DateOnly(1971, 01, 01), new TimeOnly(12, 30, 30)) }
        };
    }

    [Fact]
    public void NotEquals_Bool()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "NotEquals Bool Value Rule",
            Statement = @"Test.boolvalue notequals true",
            Context = "Context",
            Decision = "NOTEQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void NotEquals_Int()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "NotEquals Int Value Rule",
            Statement = @"Test.intvalue notequals 100",
            Context = "Context",
            Decision = "NOTEQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void NotEquals_Float()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "NotEquals Float Value Rule",
            Statement = @"Test.floatvalue notequals 100.01",
            Context = "Context",
            Decision = "NOTEQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void NotEquals_String()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "NotEquals String Value Rule",
            Statement = @"Test.stringvalue notequals ""Hello World""",
            Context = "Context",
            Decision = "NOTEQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void NotEquals_DateOnly()
    {
        var date = new DateOnly(1970, 01, 01);

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "NotEquals Date Only Value Rule",
            Statement = @$"Test.datevalue notequals ""{date}""",
            Context = "Context",
            Decision = "NOTEQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void NotEquals_DateTime()
    {
        var date = new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 30, 30));

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "NotEquals Date Time Value Rule",
            Statement = @$"Test.datetimevalue notequals ""{date}""",
            Context = "Context",
            Decision = "NOTEQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
