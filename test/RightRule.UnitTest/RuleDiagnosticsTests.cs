using RightRule;
using RightRule.ConfigurationService;
using RightRule.RuleServices.RuleModels;

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

        Assert.NotNull(rule1.RuleConfiguration);
        Assert.Equal(RightRuleConfiguration.DEFAULT_DATACHECK, rule1.RuleConfiguration.DataCheck);
        Assert.Equal(RightRuleConfiguration.DEFAULT_DIAGNOSTICS, rule1.RuleConfiguration.Diagnostics);
    }

    [Fact]
    public void RuleBuilderTest_Config()
    {
        IRightRule rr = new RightRule.RightRule();

        bool datacheck = true;
        bool diagnostics = true;

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .Decision("SAME")
            .FalseDecision("NOTSAME")
            .WithDataCheck(datacheck)
            .WithDiagnostics(diagnostics)
            .Build();

        Assert.NotNull(rule1.RuleConfiguration);
        Assert.True(rule1.RuleConfiguration.DataCheck);
        Assert.True(rule1.RuleConfiguration.Diagnostics);
    }

    [Fact]
    public void RuleDefnTest_NoConfig()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = "Test.intvalue equals Test.intvalue",
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT_EQUALS"
        };

        Assert.NotNull(rule1.RuleConfiguration);
        Assert.Equal(RightRuleConfiguration.DEFAULT_DATACHECK, rule1.RuleConfiguration.DataCheck);
        Assert.Equal(RightRuleConfiguration.DEFAULT_DIAGNOSTICS, rule1.RuleConfiguration.Diagnostics);
    }

    [Fact]
    public void RuleDefnTest_Config()
    {
        RightRuleConfiguration rc = new RightRuleConfiguration()
        {
            DataCheck = true,
            Diagnostics = true
        };

        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = "Test.intvalue equals Test.intvalue",
            Context = "Context",
            Decision = "EQUALS",
            FalseDecision = "NOT_EQUALS",
            RuleConfiguration = rc,
        };

        Assert.NotNull(rule1.RuleConfiguration);
        Assert.True(rule1.RuleConfiguration.DataCheck);
        Assert.True(rule1.RuleConfiguration.Diagnostics);
    }
}
