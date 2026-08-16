using RightRule.RuleModels;

namespace RightRuleTests;

public class RuleBuilder_Test
{
    [Fact]
    public void TestBuilder()
    {
        BeSoft.IRightRule rr = new BeSoft.RightRule();

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = b")
            .Decision("GOOD")
            .CheckData(false)
            .Build();
        var x1 = rr.RegisterRule(rule1);

        var rule2 = rr.BuildRule()
            .Number(2)
            .Name("Name2")
            .Context("Context")
            .Statement("a = b")
            .Decision("GOOD")
            .CheckData(false)
            .Build();
        var x2 = rr.RegisterRule(rule2);

        Assert.NotNull(x1);
        Assert.IsType<ParsedRule>(x1);

        Assert.NotNull(x2);
        Assert.IsType<ParsedRule>(x2);
    }
}
