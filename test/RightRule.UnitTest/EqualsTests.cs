using RightRule.Extensions;
using RightRule.Rules;

namespace RightRuleTests;

public class EqualsTests
{
    private readonly Dictionary<string, object> _Data;

    public EqualsTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "boolvalue", true },
            { "intvalue", 100 },
            { "floatvalue", 100.01 },
            { "stringvalue", "Hello World" },
            { "datevalue", new DateOnly(1970, 01, 01) },
            { "datetimevalue", new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 30, 30)) }
        };
    }

    [Fact]
    public void Equals_Bool()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"boolvalue equals true",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int_And_Decimal()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = @"intvalue equals 100.00",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = @"intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Float()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Float Value Rule",
            Statement = @"floatvalue equals 100.01",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_String()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals String Value Rule",
            Statement = @"stringvalue equals ""Hello World""",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_DateOnly()
    {
        var date = new DateOnly(1970, 01, 01);

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Date Only Value Rule",
            Statement = @$"datevalue equals ""{date}""",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_DateTime()
    {
        var date = new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 30, 30));

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Date Time Value Rule",
            Statement = @$"datetimevalue equals ""{date}""",
            Context = "Context",
            Decision = "EQUALS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
