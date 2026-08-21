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

        RuleDefinition rule1 = new RuleDefinition()
        {
            Context = "Test",
            Number = 1,
            Name = "Name",
            Decision = "BAD",
            Statement = "xdata.x = 20"
        };

        RuleDefinition rule2 = new RuleDefinition()
        {
            Context = "Test",
            Number = 2,
            Name = "Name2",
            Decision = "GOOD",
            Statement = "xdata.x = 10"
        };

        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);

        var result = rr.EvaluateAllRules("Test");
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
