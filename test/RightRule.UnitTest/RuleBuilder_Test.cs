using RightRule.RuleModels;

namespace RightRuleTests;

public class RuleBuilder_Test
{
    [Fact]
    public void TestBuilder()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        RuleDefinition ruleDef = new RuleDefinition()
        {
            Context = "Context",
            Number = 1,
            Name = "RuleDef",
            Decision = "GOOD",
            Statement = "a = b"
        };

        var rule1 = rr.BuildRule(ruleDef).WithDataDiagnostics().WithDataDiagnostics().Build();
        var x = rr.RegisterRule(rule1);

        Assert.NotNull(x);
        Assert.IsType<ParsedRule>(x);
    }
}
