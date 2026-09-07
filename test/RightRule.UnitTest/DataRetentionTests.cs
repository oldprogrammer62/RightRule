using BeSoft;
using RightRule.System.Common;
using System.Runtime.InteropServices.ObjectiveC;

namespace RightRuleTests;

public class DataRetentionTests
{
    [Fact]
    public async Task NoRetention()
    {
        IRightRule rr = new BeSoft.RightRule();

        var x = 100;

        var data = new Dictionary<string, object>
        {
            {"x", x }
        };

        var rule1 = rr.BuildRule()
            .Number(1)
            .Name("Name1")
            .Context("Context")
            .Statement("Data.x = 100")
            .Decision("GOOD")
            .FalseDecision("BAD")
            .Build();
        var ruleDef = rr.RegisterRule(rule1);

        var result1 = rr.ApplyRules("Context", "Data", data);
        Assert.NotNull(result1);

        var result2 = rr.ApplyRules("Context", "Data", data, DataRetention.Keep);
        Assert.NotNull(result2);

        var result3 = rr.ApplyRules("Context", "Data", data, RulesToReturn.First, DataRetention.Keep);
        Assert.NotNull(result3);

        var result4 = await rr.ApplyRulesAsync("Context");
        Assert.NotNull(result4);

        var result5 = await rr.ApplyRulesAsync("Context", RulesToReturn.First);
        Assert.NotNull(result5);

        var result6 = await rr.ApplyRulesAsync("Context", "Data", data, RulesToReturn.All, DataRetention.Keep);
        Assert.NotNull(result6);
    }
}
