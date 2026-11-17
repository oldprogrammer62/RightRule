using RightRule;
using RightRule.Models;

namespace RightRuleTests;

public class RuleDiagnosticsTests
{
    [Fact]
    public void RuleBuilderTest_NoConfig()
    {
        var rr = new RightRule.RightRule().CreateSession();

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .TrueDecision("SAME")
            .FalseDecision("NOT_SAME")
            .Build();

        Assert.NotNull(rule1);

        var options = new RightRuleOptions();
        var configurations = rr.RightRuleOptions;

        Assert.NotNull(configurations);
        Assert.Equal(options.DataCheck, configurations.DataCheck);
        Assert.Equal(options.Diagnostics, configurations.Diagnostics);
        Assert.Equal(options.LoadDefaultOperators, configurations.LoadDefaultOperators);
        Assert.Equal(options.CustomOperatorLibraryPath, configurations.CustomOperatorLibraryPath);
    }

    [Fact]
    public void RuleBuilderTest_DefaultConfig()
    {
        RightRuleOptions options = new RightRuleOptions();

        var rr = new RightRule.RightRule(options).CreateSession();

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .TrueDecision("SAME")
            .FalseDecision("NOTSAME")
            .Build();

        var configurations = rr.RightRuleOptions;
        Assert.NotNull(configurations);
        Assert.Equal(options.DataCheck, configurations.DataCheck);
        Assert.Equal(options.Diagnostics, configurations.Diagnostics);
        Assert.Equal(options.LoadDefaultOperators, configurations.LoadDefaultOperators);
        Assert.Equal(options.CustomOperatorLibraryPath, configurations.CustomOperatorLibraryPath);
    }

    [Fact]
    public void RuleBuilderTest_ModifiedConfig()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = false,
            CustomOperatorLibraryPath = string.Empty
        };

        var rr = new RightRule.RightRule(options).CreateSession();

        var rule1 = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .TrueDecision("SAME")
            .FalseDecision("NOTSAME")
            .Build();

        var configurations = rr.RightRuleOptions;
        Assert.NotNull(configurations);
        Assert.Equal(options.DataCheck, configurations.DataCheck);
        Assert.Equal(options.Diagnostics, configurations.Diagnostics);
        Assert.Equal(options.LoadDefaultOperators, configurations.LoadDefaultOperators);
    }
}
