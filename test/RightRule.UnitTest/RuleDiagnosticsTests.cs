using BeSoft;
using RightRule.RuleModels;
using System.Data;
using System.Text;

namespace RightRuleTests;

public class RuleDiagnosticsTests
{
    [Fact]
    public void RuleBuilderTest_NoConfig()
    {
        IRightRule rr = new BeSoft.RightRule();

        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .Decision("SAME")
            .Build();

        Assert.NotNull(rule1.RuleConfiguration);
        Assert.Equal(RightRuleConfiguration.DEFAULT_DATACHECK, rule1.RuleConfiguration.DataCheck);
        Assert.Equal(RightRuleConfiguration.DEFAULT_DIAGNOSTICS, rule1.RuleConfiguration.Diagnostics);
    }

    [Fact]
    public void RuleBuilderTest_Config()
    {
        IRightRule rr = new BeSoft.RightRule();

        bool datacheck = true;
        bool diagnostics = true;

        var rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("100 = 100 and 100 <> 101")
            .Decision("SAME")
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
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = "Test.intvalue equals Test.intvalue",
            Context = "Context",
            Decision = "EQUALS",
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
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = "Test.intvalue equals Test.intvalue",
            Context = "Context",
            Decision = "EQUALS",
            RuleConfiguration = rc
        };

        Assert.NotNull(rule1.RuleConfiguration);
        Assert.True(rule1.RuleConfiguration.DataCheck);
        Assert.True(rule1.RuleConfiguration.Diagnostics);
    }
}
