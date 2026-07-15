using RightRule.Rules;

namespace RightRuleTests;

public class ExtensionTests
{
    [Fact]
    public void GetRules_ShouldReturnParsedRules()
    {
        RightRule.RightRule rightRule = new RightRule.RightRule();

        var result = rightRule.GetRules();

        Assert.NotNull(result);
        Assert.IsType<List<ParsedRule>>(result);
    }

    [Fact]
    public void GetSpecifiedRules_ShouldReturnParsedRules()
    {
        string specifiedContext = "TestContext";

        RightRule.RightRule rightRule = new RightRule.RightRule();

        var result = rightRule.GetRules(specifiedContext);

        Assert.NotNull(result);
        Assert.IsType<List<ParsedRule>>(result);
    }


    [Fact]
    public void RegisterRules()
    {
        // Arrange
        RightRule.RightRule rightRule = new RightRule.RightRule();
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
        RightRule.RightRule rightRule = new RightRule.RightRule();
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
