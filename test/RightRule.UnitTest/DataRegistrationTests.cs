using RightRule.RuleModels;

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

        var rr = new BeSoft.RightRule();

        rr.RegisterData("Test", _Data);
        var registeredRule = rr.RegisterRule(rule1);

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

        var rr = new BeSoft.RightRule();

        rr.RegisterData("Test", _Data);
        var registeredRule = rr.RegisterRule(rule1);

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

        BeSoft.IRightRule rr = new BeSoft.RightRule();

        rr.RegisterData("Test", _Data);
        var newRule1 = rr.RegisterRule(rule1);

        var appliedRules = rr.EvaluateAll("AllContext", RightRule.System.Common.RulesToReturn.First);
        var appliedRule = appliedRules.FirstOrDefault();
        Assert.Equal(appliedRule.Decision, rule1.Decision);
    }

    [Fact]
    public void UniqueDataObjectRegsitration()
    {
        int x = 100;
        var r = ("r", x);

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var added1 = rr.RegisterData("r", r);
        Assert.True(added1.IsSuccess);

        var added2 = rr.RegisterData("r", r);
        Assert.False(added2.IsSuccess);
    }
}
