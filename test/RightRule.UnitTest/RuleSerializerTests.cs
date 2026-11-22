using RightRule;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class RuleSerializerTests
{
    [Fact]
    public void SerializeRules_ReturnsSerializedRules()
    {
        var rr = new RightRule.RightRule().CreateSession();

        DefinedRule rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        var result1 = rr.RuleService.RegisterRule(rule1);
        Assert.NotNull(result1);

        var serializedRules = rr.RuleService.SerializeRules();
        Assert.Contains("\"Number\":1", serializedRules);
    }

    [Fact]
    public void SerializeRules_ReturnsSerializedMultipleRules()
    {
        var rr = new RightRule.RightRule().CreateSession();

        DefinedRule rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("a = b")
            .Decision("Decision")
            .Build();
        var result1 = rr.RuleService.RegisterRule(rule1);
        Assert.NotNull(result1);

        DefinedRule rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context2")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        var result2 = rr.RuleService.RegisterRule(rule2);
        Assert.NotNull(result2);

        var serializedRules = rr.RuleService.SerializeRules();
        Assert.Contains("\"Number\":1", serializedRules);
        Assert.Contains("\"Number\":2", serializedRules);
    }

    [Fact]
    public void SerializeRules_ReturnsSerializedRulesByContext()
    {
        var rr = new RightRule.RightRule().CreateSession();

        DefinedRule rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("a = b")
            .Decision("Decision")
            .Build();
        var result1 = rr.RuleService.RegisterRule(rule1);
        Assert.NotNull(result1);

        DefinedRule rule2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context2")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        var result2 = rr.RuleService.RegisterRule(rule2);
        Assert.NotNull(result2);

        var serializedRules = rr.RuleService.SerializeRules("Context");
        Assert.Contains("\"Number\":1", serializedRules);
    }

    [Fact]
    public void SerializeRules_ThenLoadRules()
    {
        var rr = new RightRule.RightRule().CreateSession();

        DefinedRule rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        var result1 = rr.RuleService.RegisterRule(rule1);
        Assert.NotNull(result1);

        var serializedRules = rr.RuleService.SerializeRules();
        Assert.Contains("\"Number\":1", serializedRules);

        // Remove all rules
        var done = rr.RuleService.DeRegisterRules();

        var loadedRules = rr.RuleService.LoadRules(serializedRules);
        Assert.NotNull(loadedRules);
    }
}
