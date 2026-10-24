using RightRule;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;

namespace RightRuleTests;

public class ExtensionTests
{
    [Fact]
    public void GetRules_ShouldReturnParsedRules()
    {
        IRightRule rightRule = new RightRule.RightRule();

        var rules = rightRule.GetRegisteredRules();

        Assert.NotNull(rules);
        Assert.IsType<List<ParsedRule>>(rules);
    }

    [Fact]
    public void GetSpecifiedRules_ShouldReturnParsedRules()
    {
        string specifiedContext = "TestContext";

        IRightRule rightRule = new RightRule.RightRule();

        var result = rightRule.GetRegisteredRules(specifiedContext);

        Assert.NotNull(result);
        Assert.IsType<List<ParsedRule>>(result);
    }


    [Fact]
    public void RegisterRules()
    {
        // Arrange
        IRightRule rightRule = new RightRule.RightRule();
        var rules = new List<DefinedRule>
        {
        };

        var result = rightRule.RegisterRules(rules);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<RuleRegistrationResult>>(result);
    }

    [Fact]
    public void RegisterRule()
    {
        IRightRule rightRule = new RightRule.RightRule();
        DefinedRule rule = new DefinedRule()
        {
            Context = "TestContext",
            TrueDecision = "TestDecision",
            FalseDecision = "TestFalseDecision",
            Name = "TestRule",
            Number = 1,
            Statement = "Test.hello equals \"hello\""
        };

        var result = rightRule.RegisterRule(rule);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RuleRegistrationResult>(result);
        Assert.IsType<ParsedRule>(result.RegisteredRule);
    }
}
