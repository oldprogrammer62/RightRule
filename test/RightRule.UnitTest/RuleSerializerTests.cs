using RightRule;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class RuleSerializerTests
{
    [Fact]
    public void SerializeRules_ReturnsSerializedRules()
    {
        IRightRule rr = new RightRule.RightRule();

        DefinedRule rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .TrueDecision("Decision")
            .FalseDecision("FalseDecision")
            .Build();

        var result1 = rr.RegisterRule(rule1);
        Assert.NotNull(result1);

        var serializedRules = rr.SerializeRules();
        Assert.Contains("\"Number\":1", serializedRules);
    }

    [Fact]
    public void SerializeRules_ReturnsSerializedMultipleRules()
    {
        IRightRule rr = new RightRule.RightRule();

        DefinedRule rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("a = b")
            .TrueDecision("Decision")
            .FalseDecision("FalseDecision")
            .Build();
        var result1 = rr.RegisterRule(rule1);
        Assert.NotNull(result1);

        DefinedRule rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context2")
            .Statement("a = b")
            .TrueDecision("Decision")
            .FalseDecision("FalseDecision")
            .Build();

        var result2 = rr.RegisterRule(rule2);
        Assert.NotNull(result2);

        var serializedRules = rr.SerializeRules();
        Assert.Contains("\"Number\":1", serializedRules);
        Assert.Contains("\"Number\":2", serializedRules);
    }

    [Fact]
    public void SerializeRules_ReturnsSerializedRulesByContext()
    {
        IRightRule rr = new RightRule.RightRule();

        DefinedRule rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("a = b")
            .TrueDecision("Decision")
            .FalseDecision("FalseDecision")
            .Build();
        var result1 = rr.RegisterRule(rule1);
        Assert.NotNull(result1);

        DefinedRule rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context2")
            .Statement("a = b")
            .TrueDecision("Decision")
            .FalseDecision("FalseDecision")
            .Build();

        var result2 = rr.RegisterRule(rule2);
        Assert.NotNull(result2);

        var serializedRules = rr.SerializeRules("Context");
        Assert.Contains("\"Number\":1", serializedRules);
    }

    [Fact]
    public void SerializeRules_ThenLoadRules()
    {
        IRightRule rr = new RightRule.RightRule();

        DefinedRule rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .TrueDecision("Decision")
            .FalseDecision("FalseDecision")
            .Build();

        var result1 = rr.RegisterRule(rule1);
        Assert.NotNull(result1);

        var serializedRules = rr.SerializeRules();
        Assert.Contains("\"Number\":1", serializedRules);

        // Remove all rules
        var done = rr.DeRegisterRules();

        var loadedRules = rr.LoadRules(serializedRules);
        Assert.NotNull(loadedRules);
    }
}
