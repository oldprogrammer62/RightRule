using RightRule.Rules;

namespace RightRuleTests;

public class ComplexStatementTests
{
    private readonly Dictionary<string, object> _Data;

    public ComplexStatementTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "A", 1 },
            { "B", 2 },
            { "C", 3 },
            { "D", 4 },
            { "E", 1 }
        };
    }

    [Fact]
    public void AllValid_Equals()
    {
        // Arrange
        var statement = "(A = 1 and B = 2) and (C = 3 and D = 4)";
        var rightRule = new RightRule.RightRule();

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Rule 1",
            Statement = statement,
            Context = "Context",
            Decision = "EQUALS",
        };

        // Register the data
        rightRule.RegisterData("TestDataset", _Data);
        rightRule.RegisterRule(rule);
        var appliedRule = rightRule.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllValid_NotEquals()
    {
        // Arrange
        var statement = "(A = 1 and B = 2) and (C = 3 and not D = 44)";
        var rightRule = new RightRule.RightRule();

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Rule 1",
            Statement = statement,
            Context = "Context",
            Decision = "EQUALS",
        };

        // Register the data
        rightRule.RegisterData("TestDataset", _Data);
        rightRule.RegisterRule(rule);
        var appliedRule = rightRule.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void AllValid_Try()
    {
        var _arguments = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "X", 1 },
        };

        // Arrange
        var statement = "A equals E and A = 1";
        var rightRule = new RightRule.RightRule();

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Rule 1",
            Statement = statement,
            Context = "Context",
            Decision = "EQUALS",
            Arguments = _arguments,
        };

        // Register the data
        rightRule.RegisterData("TestDataset", _Data);
        rightRule.RegisterRule(rule);
        var appliedRule = rightRule.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
