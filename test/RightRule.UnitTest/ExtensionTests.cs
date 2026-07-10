using RightRule.Extensions;
using RightRule.Rules;

namespace RightRuleTests;

public class ExtensionTests
{
    [Fact]
    public void GetRules_ShouldReturnParsedRules()
    {
        // Arrange
        var rightRule = new RightRule.RightRule();
        // Act
        var result = rightRule.GetRules();
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<ParsedRule>>(result);
    }

    [Fact]
    public void GetSpecifiedRules_ShouldReturnParsedRules()
    {
        string specifiedContext = "TestContext";

        // Arrange
        var rightRule = new RightRule.RightRule();
        // Act
        var result = rightRule.GetRules(specifiedContext);
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<ParsedRule>>(result);
    }


    [Fact]
    public void RegisterRules()
    {
        // Arrange
        var rightRule = new RightRule.RightRule();
        var rules = new List<RuleDefinition>
        {
        };

        var result = rightRule.RegisterRules(rules);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<ParsedRule>>(result);
    }

    [Fact]
    public void RegisterRule()
    {
        // Arrange
        var rightRule = new RightRule.RightRule();
        RuleDefinition rule = new RuleDefinition()
        {
            Context = "TestContext",
            Decision = "TestDecision",
            Name = "TestRule",
            Number = 1,
            Statement = "Test.hello equals \"hello\""
        };

        var result = rightRule.RegisterRule(rule);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ParsedRule>(result);
    }
}
