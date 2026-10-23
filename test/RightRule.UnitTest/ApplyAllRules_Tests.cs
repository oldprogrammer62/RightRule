using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;
using RightRule.RuleServices.RuleModels.BaseModels;

namespace RightRuleTests;

public class ApplyAllRules_Tests
{
    [Fact]
    public void TestAllRules()
    {
        int x = 10;

        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            LoadCustomOperators = true,
            LoadDefaultOperators = true,
            Diagnostics = true
        };
        using var rr = new RightRule.RightRule(options);
        rr.RegisterData(nameof(x), x);

        DefinedRule rule1 = new DefinedRule()
        {
            Context = "Test",
            Number = 1,
            Name = "Name",
            Statement = "x = 20",
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

        List<RightRuleResult<IProcessedRule>> result = rr.ApplyRules("Test", EvaluationTypeEnum.ANY);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
