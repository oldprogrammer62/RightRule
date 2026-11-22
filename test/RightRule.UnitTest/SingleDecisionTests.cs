using RightRule.Models;

namespace RightRuleTests;

public class SingleDecisionTests
{
    [Fact]
    public void TestSingleDecision()
    {
        // Path to Custom Operators left as default
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = false,
            LoadDefaultOperators = true,
            LoadCustomOperators = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();

        var dataReg1 = rr.DataService.RegisterData("a", 10);
        var dataReg2 = rr.DataService.RegisterData("b", 10);

        var ruleReg1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("rule1")
            .Context("test")
            .Statement("a = b")
            .TrueDecision("PositivePass")
            .Build()
            .Register(rr.RuleService);

        var ruleReg2 = rr.RuleService.BuildRule()
            .Number(2)
            .Name("rule2")
            .Context("test")
            .Statement("a <> b")
            .TrueDecision("NegativePass")
            .Build()
            .Register(rr.RuleService);

        var applied = rr.RuleService.ApplyRule("test");

        Assert.NotNull(applied);
    }
}
