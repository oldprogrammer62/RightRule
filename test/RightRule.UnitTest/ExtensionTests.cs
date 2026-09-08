using RightRule.RegistrationServices.RuleRgistration;
using RightRule.RuleModels.RuleModels;

namespace RightRuleTests;

public class ExtensionTests
{
    [Fact]
    public void GetRules_ShouldReturnParsedRules()
    {
        BeSoft.RightRule rightRule = new BeSoft.RightRule();

        var rules = rightRule.GetRegisteredRules();

        Assert.NotNull(rules);
        Assert.IsType<List<ParsedRule>>(rules.Rules);
    }

    [Fact]
    public void GetSpecifiedRules_ShouldReturnParsedRules()
    {
        string specifiedContext = "TestContext";

        BeSoft.RightRule rightRule = new BeSoft.RightRule();

        var result = rightRule.GetRegisteredRules(specifiedContext);

        Assert.NotNull(result);
        Assert.IsType<List<ParsedRule>>(result);
    }


    [Fact]
    public void RegisterRules()
    {
        // Arrange
        BeSoft.RightRule rightRule = new BeSoft.RightRule();
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
        BeSoft.RightRule rightRule = new BeSoft.RightRule();
        DefinedRule rule = new DefinedRule()
        {
            Context = "TestContext",
            Decision = "TestDecision",
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
