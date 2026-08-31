using RightRule.RuleModels;

namespace RightRuleTests;

public class ApplyAllRules
{
    [Fact]
    public void TestAllRules()
    {
        int x = 10;

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        rr.RegisterData("xdata", x);

        DefinedRule rule1 = new DefinedRule()
        {
            Context = "Test",
            Number = 1,
            Name = "Name",
            Decision = "BAD",
            Statement = "xdata.x = 20"
        };

        DefinedRule rule2 = new DefinedRule()
        {
            Context = "Test",
            Number = 2,
            Name = "Name2",
            Decision = "GOOD",
            Statement = "xdata.x = 10"
        };

        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);

        List<AppliedRule>? result = rr.EvaluateAll("Test");
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
