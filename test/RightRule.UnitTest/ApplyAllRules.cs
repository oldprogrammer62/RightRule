using RightRule.RuleModels.RuleModels;
using RightRule.System.Common;

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
            Statement = "xdata.x = 20",
            Decision = "BAD",
            FalseDecision = "BAD"
        };

        DefinedRule rule2 = new DefinedRule()
        {
            Context = "Test",
            Number = 2,
            Name = "Name2",
            Statement = "xdata.x = 10",
            Decision = "GOOD",
            FalseDecision = "BAD"
        };

        rr.RegisterRule(rule1);
        rr.RegisterRule(rule2);

        var result = rr.ApplyRules("Test", RulesToReturn.All);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
