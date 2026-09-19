using BeSoft;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.RuleApplication;
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

        var result1 = rr.ApplyRule("Context", "Data", data);
        Assert.NotNull(result1);

        var result2 = rr.ApplyRule("Context", "Data", data, DataRetentionEnum.KEEP);
        Assert.NotNull(result2);

        var result3 = rr.ApplyRules("Context", "Data", data, EvaluationTypeEnum.FIRST, DataRetentionEnum.KEEP);
        Assert.NotNull(result3);

        var result4 = await rr.ApplyRuleAsync("Context");
        Assert.NotNull(result4);

        var result5 = await rr.ApplyRulesAsync("Context", EvaluationTypeEnum.FIRST);
        Assert.NotNull(result5);

        var result6 = await rr.ApplyRulesAsync("Context", "Data", data, EvaluationTypeEnum.ANY, DataRetentionEnum.KEEP);
        Assert.NotNull(result6);
    }
}
