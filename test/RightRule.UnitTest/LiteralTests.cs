using RightRule.Rules;

namespace RightRuleTests;

public class LiteralTests
{
    private readonly Dictionary<string, object> _Data;

    public LiteralTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "intvalue", 100 },
            { "stringvalue", "A_String" }
        };
    }

    [Fact]
    public void Equals_Right_Literal_String()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Literal String Rule",
            Statement = @"stringvalue equals ""A_String""",
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
    public void Equals_Right_Literal_Number()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Literal Number Rule",
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
    public void Equals_Left_Literal_String()
    {
        Dictionary<string, object> _Arguments = new(StringComparer.Ordinal)
        {
            { "AString", "A_String" }
        };

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Literal String Rule",
            Statement = @"""A_String"" equals {AString}",
            Context = "Context",
            Decision = "EQUALS",
            Arguments = _Arguments
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Both_Literal_Number()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Literal Number Rule",
            Statement = @"100 equals 100",
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
    public void Equals_Both_Literal_String()
    {
        Dictionary<string, object> _Arguments = new(StringComparer.Ordinal)
        {
            { "AString", "A_String" }
        };

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Literal String Rule",
            Statement = @"stringvalue equals {AString}",
            Context = "Context",
            Decision = "EQUALS",
            Arguments = _Arguments
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
