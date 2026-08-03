using RightRule.RuleModels;

namespace RightRuleTests;

public class EqualsTests
{
    private readonly Dictionary<string, object> _Data;

    public EqualsTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "boolvalue", true },
            { "intvalue", 100 },
            { "floatvalue", 100.01 },
            { "stringvalue", "Hello World" },
            { "datevalue", new DateOnly(1970, 01, 01) },
            { "datetimevalue", new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 30, 30)) }
        };
    }

    [Fact]
    public void Equals_Bool()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Bool Value Rule",
            Statement = @"Test.boolvalue equals true",
            Context = "Context",
            Decision = "EQUALS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int_And_Decimal()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = @"Test.intvalue equals 100.00",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int_And_Int()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = "Test.intvalue equals Test.intvalue",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Float()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Float Value Rule",
            Statement = @"Test.floatvalue equals 100.01",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_String()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals String Value Rule",
            Statement = @"Test.stringvalue equals ""Hello World""",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_DateOnly()
    {
        var date = new DateOnly(1970, 01, 01);

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Date Only Value Rule",
            Statement = @$"Test.datevalue equals ""{date}""",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_DateTime()
    {
        var date = new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 30, 30));

        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Equals Date Time Value Rule",
            Statement = @$"Test.datetimevalue equals ""{date}""",
            Context = "Context",
            Decision = "EQUALS",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterDataset("Test", _Data);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int_MultiRules_Rule1()
    {
        // Data
        var _IntData = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "intvalue", 100 }
        };

        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };
        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Int Value Rule",
            Statement = @"Test.intvalue equals 200",
            Context = "Context",
            Decision = "EQUALS200",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterDataset("Test", _IntData);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule1.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int_MultiRules_Rule2()
    {
        // Data
        var _IntData = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "intvalue", 200 }
        };

        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };
        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Int Value Rule",
            Statement = @"Test.intvalue equals 200",
            Context = "Context",
            Decision = "EQUALS200",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterDataset("Test", _IntData);

        var appliedRule = rr.EvaluateFirstRule("Context");

        Assert.Equal(rule2.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Equals_Int_MultiRules_Rules()
    {
        // Data
        var _IntData = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "intvalue", 200 }
        };

        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Equals Int Value Rule",
            Statement = @"Test.intvalue equals 100",
            Context = "Context",
            Decision = "EQUALS100",
        };
        // Rule
        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Equals Int Value Rule",
            Statement = @"Test.intvalue equals 200",
            Context = "Context",
            Decision = "EQUALS200",
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterDataset("Test", _IntData);

        var appliedRule = rr.EvaluateAllRules("Context");

        Assert.NotNull(appliedRule);
    }
}
