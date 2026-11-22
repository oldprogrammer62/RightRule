using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class DataRegistrationTests
{
    private readonly Dictionary<string, object> _Data;

    public DataRegistrationTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "John" },
            { "surname", "Doe" },
            { "fullname", "John D. Doe" },
            { "numberoforders", 10 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };
    }

    [Fact]
    public void RegisterDataTest()
    {
        DefinedRule rule1 = new DefinedRule
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10",
            Context = "AllContext",
            Decision = "ENOUGHORDERS",
        };

        var rr = new RightRule.RightRule().CreateSession();

        rr.DataService.RegisterData("Test", _Data);
        var registeredRule = rr.RuleService.RegisterRule(rule1);

        Assert.NotNull(registeredRule);
    }

    [Fact]
    public void RegisterListDataTest()
    {
        DefinedRule rule1 = new DefinedRule
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10",
            Context = "AllContext",
            Decision = "ENOUGHORDERS",
        };

        var rr = new RightRule.RightRule().CreateSession();

        rr.DataService.RegisterData("Test", _Data);
        var registeredRule = rr.RuleService.RegisterRule(rule1);

        Assert.NotNull(registeredRule);
    }

    [Fact]
    public void RegisterDataAndApplyRulesTest()
    {
        // Rule
        DefinedRule rule1 = new DefinedRule
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10",
            Context = "AllContext",
            Decision = "ENOUGHORDERS",
        };

        var rr = new RightRule.RightRule().CreateSession();

        rr.DataService.RegisterData("Test", _Data);
        var newRule1 = rr.RuleService.RegisterRule(rule1);

        var appliedRules = rr.RuleService.ApplyRules("AllContext", EvaluationTypeEnum.FIRST);
        var appliedRule = appliedRules.FirstOrDefault();
        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void addDataToExistingDataObjectRegsitration()
    {
        int x = 100;
        var r = ("r", x);

        var rr = new RightRule.RightRule().CreateSession();
        var added1 = rr.DataService.RegisterData("r", r);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added1.Status);

        var added2 = rr.DataService.RegisterData("r", r);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added2.Status);
    }
}
