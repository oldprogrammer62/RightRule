using BeSoft;
using RightRule.RuleModels;

namespace RightRuleTests;

public class RuleRegistrationTests
{
    [Fact]
    public void RegisterRule_ByNumber()
    {
        IRightRule rr = new BeSoft.RightRule();

        RuleDefinition rule1 = rr.BuildRuleDefinition()
            .Number(1)
            .Name("Name1")
            .Context("Context1")
            .Statement("a = b")
            .Decision("Decision")
            .Build();

        var result = rr.RegisterRule(rule1);
        Assert.NotNull(result);

    }
}
