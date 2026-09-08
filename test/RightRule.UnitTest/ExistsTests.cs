using RightRule.RuleModels.RuleModels;
using System.Collections;

namespace RightRuleTests;

public class ExistsTests
{
    private readonly Dictionary<string, object> _Data;

    public ExistsTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "boolvalue", true },
            { "bools", new List<bool>() { true, false } },
            { "intvalue", 100 },
            { "ints", new List<int>() { 100, 200 } },
            { "floatvalue", 100.01 },
            { "floats", new List<double>() { 100.01, 200.02 } },
            { "stringvalue", "Hello World" },
            { "strings", new List<string>() { "Hello World", "Goodbye World" } },
            { "datevalue", new DateOnly(1970, 01, 01) },
            { "dates", new List<DateOnly>() { new DateOnly(1970, 01, 01), new DateOnly(2000, 12, 31) } },
            { "datetimevalue", new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 30, 30)) },
            { "datetimes", new List<DateTime>() { new DateTime(new DateOnly(1970, 01, 01), new TimeOnly(12, 30, 30)), new DateTime(new DateOnly(2000, 12, 31), new TimeOnly(23, 59, 59)) } }
        };
    }

    [Fact]
    public void Exists_int_in_Array()
    {
        Dictionary<string, object> Arguments = new(StringComparer.Ordinal)
        {
            { "number" , 4 },
            { "numbers", new ArrayList() { 1, 2, 3, 100, 4, 5 } }
        };

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists Int Value Rule",
            Statement = @"Arguments.number in Arguments.numbers",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);
        rr.RegisterData("Arguments", Arguments);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Exists_string_in_Array()
    {
        Dictionary<string, object> Arguments = new(StringComparer.Ordinal)
        {
            { "strings", new ArrayList() { "aa", "bb", "cc", "dd" } },
            { "dd_string", "dd" }
        };

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists String Value Rule",
            Statement = @"""dd"" in Arguments.strings",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);
        rr.RegisterData("Arguments", Arguments);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Exists_int_in()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists Int Value Rule",
            Statement = @"Test.intvalue in Test.ints",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Exists_int_foundin()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists Int Value Rule",
            Statement = @"Test.intvalue foundin Test.ints",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Exists_bool()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists Bool Value Rule",
            Statement = @"Test.boolvalue exists Test.bools",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Exists_int()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists Int Value Rule",
            Statement = @"Test.intvalue exists Test.ints",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Exists_float()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists Float Value Rule",
            Statement = @"Test.floatvalue exists Test.floats",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Exists_string()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists String Value Rule",
            Statement = @"Test.stringvalue exists Test.strings",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Exists_dateonly()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists DateOnly Value Rule",
            Statement = @"Test.datevalue exists Test.dates",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Exists_datetime()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Exists DateTime Value Rule",
            Statement = @"Test.datetimevalue exists Test.datetimes",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
