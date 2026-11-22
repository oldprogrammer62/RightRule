using RightRule;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleRegistration;

namespace RightRuleTests;

public class ExtensionTests
{
    [Fact]
    public void GetRules_ShouldReturnParsedRules()
    {
        var rightRule = new RightRule.RightRule().CreateSession();

        var rules = rightRule.RuleService.GetRegisteredRules();

        Assert.NotNull(rules);
        Assert.IsType<List<ParsedRule>>(rules);
    }

    [Fact]
    public void GetSpecifiedRules_ShouldReturnParsedRules()
    {
        string specifiedContext = "TestContext";

        var rightRule = new RightRule.RightRule().CreateSession();

        var result = rightRule.RuleService.GetRegisteredRules(specifiedContext);

        Assert.NotNull(result);
        Assert.IsType<List<ParsedRule>>(result);
    }


    [Fact]
    public void RegisterRules()
    {
        // Arrange
        var rightRule = new RightRule.RightRule().CreateSession();
        var rules = new List<DefinedRule>
        {
        };

        var result = rightRule.RuleService.RegisterRules(rules);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<RuleRegistrationResult>>(result);
    }

    [Fact]
    public void RegisterRule()
    {
        var rightRule = new RightRule.RightRule().CreateSession();
        DefinedRule rule = new DefinedRule()
        {
            Context = "TestContext",
            TrueDecision = "TestDecision",
            Name = "TestRule",
            Number = 1,
            Statement = "Test.hello equals \"hello\""
        };

        var result = rightRule.RuleService.RegisterRule(rule);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<RuleRegistrationResult>(result);
        Assert.IsType<ParsedRule>(result.RegisteredRule);
    }
}
