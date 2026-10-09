using RightRule;
using RightRule.Models;

namespace RightRuleTests;

public class RuleDiagnosticsTests
{
    [Fact]
    public void RuleBuilderTest_NoConfig()
    {
        IRightRule rr = new RightRule.RightRule();

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .Decision("SAME")
            .FalseDecision("NOT_SAME")
            .Build();

        Assert.NotNull(rule1);

        var options = new RightRuleOptions();
        var configurations = rr.GetRightRuleOptions();

        Assert.NotNull(configurations);
        Assert.Equal(options.DataCheck, configurations.DataCheck);
        Assert.Equal(options.Diagnostics, configurations.Diagnostics);
        Assert.Equal(options.LoadDefaultOperators, configurations.LoadDefaultOperators);
    }

    [Fact]
    public void RuleBuilderTest_DefaultConfig()
    {
        RightRuleOptions options = new RightRuleOptions();

        IRightRule rr = new RightRule.RightRule(options);

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .Decision("SAME")
            .FalseDecision("NOTSAME")
            .Build();

        var configurations = rr.GetRightRuleOptions();
        Assert.NotNull(configurations);
        Assert.Equal(options.DataCheck, configurations.DataCheck);
        Assert.Equal(options.Diagnostics, configurations.Diagnostics);
        Assert.Equal(options.LoadDefaultOperators, configurations.LoadDefaultOperators);
    }

    [Fact]
    public void RuleBuilderTest_ModifiedConfig()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = false
        };

        IRightRule rr = new RightRule.RightRule(options);

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .Decision("SAME")
            .FalseDecision("NOTSAME")
            .Build();

        var configurations = rr.GetRightRuleOptions();
        Assert.NotNull(configurations);
        Assert.Equal(options.DataCheck, configurations.DataCheck);
        Assert.Equal(options.Diagnostics, configurations.Diagnostics);
        Assert.Equal(options.LoadDefaultOperators, configurations.LoadDefaultOperators);
    }
}
