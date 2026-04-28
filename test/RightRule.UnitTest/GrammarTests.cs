using RightRule.Rules;

namespace RightRuleTests;

public class GrammarTests
{
    private readonly Dictionary<string, object> _Data;

    public GrammarTests()
    {
        // Data
        _Data = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "name", "John" },
            { "surname", "Doe" },
            { "fullname", "John D. Doe" },
            { "age", 28 },
            { "numberoforders", 10 },
            { "goodpayer", true },
            { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
        };
    }

    [Fact]
    public void TestItWorks_NoDecision()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28",
            Context = "AgeContext",
            Decision = "OLDENOUGH",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);  

        var appliedRule = rr.ApplyRules("AgeContext1", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_ThisAndThis()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28 and goodpayer equals true",
            Context = "AgeContext",
            Decision = "OK",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AgeContext", RightRule.System.Common.RulesToApply.First);

        Assert.NotNull(appliedRule);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_ThisOrThis()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 29 or goodpayer equals true",
            Context = "AgeContext",
            Decision = "OK",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AgeContext", RightRule.System.Common.RulesToApply.First);

        Assert.NotNull(appliedRule);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_ThisAndThisOrThis()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"(age equals 28 and goodpayer equals true) or (numberoforders equals 10 and goodpayer equals true)",
            Context = "AgeContext",
            Decision = "OK",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AgeContext", RightRule.System.Common.RulesToApply.First);

        Assert.NotNull(appliedRule);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_Equals()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = @"age equals 28",
            Context = "AgeContext",
            Decision = "OLDENOUGH",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AgeContext", RightRule.System.Common.RulesToApply.First);

        Assert.NotNull(appliedRule);
    }

    [Fact]
    public void TestItWorks_Number_NotEquals()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = "age notequals 18",
            Context = "AgeContext",
            Decision = "OLDENOUGH",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);   

        var appliedRule = rr.ApplyRules("AgeContext", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(appliedRule.Decision, rule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_NotEqualsSymbol1()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = "age != 18",
            Context = "AgeContext",
            Decision = "OLDENOUGH",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AgeContext", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(appliedRule.Decision, rule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_NotEqualsSymbol2()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = "age <> 18",
            Context = "AgeContext",
            Decision = "OLDENOUGH",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AgeContext", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(appliedRule.Decision, rule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_EqualsSymbol()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Age Rule",
            Statement = "age = 28",
            Context = "AgeContext",
            Decision = "OLDENOUGH",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("AgeContext", RightRule.System.Common.RulesToApply.First);

        Assert.NotNull(appliedRule);
    }

    [Fact]
    public void TestItWorks_String_Equals()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"name equals ""John""",
            Context = "NameContext",
            Decision = "NAMEFOUND",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NameContext", RightRule.System.Common.RulesToApply.First);

        Assert.NotNull(appliedRule);
    }

    [Fact]
    public void TestItWorks_String_Equals_String()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 3,
            Name = "Name Rule",
            Statement = "name equals fullname",
            Context = "NameContext",
            Decision = "NAMESAMEASFULLNAME",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NameContext", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_Equals_Boolean_True()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 4,
            Name = "Good Payer Rule",
            Statement = "goodpayer equals true",
            Context = "GoodPayerContext",
            Decision = "GOODPAYER",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("GoodPayerContext", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(newRule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_Equals_Boolean_False()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 4,
            Name = "Good Payer Rule",
            Statement = "goodpayer equals false",
            Context = "NotGoodPayerContext",
            Decision = "NOTGOODPAYER",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NotGoodPayerContext", RightRule.System.Common.RulesToApply.First);

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_String_Equals_MultipleContext1()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Name Rule",
            Statement = @"name equals ""John""",
            Context = "NAMECONTEXT1",
            Decision = "NAMEFOUND",
        };

        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"name equals ""John""",
            Context = "NAMECONTEXT2",
            Decision = "NAMEF2OUND",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NAMECONTEXT1", RightRule.System.Common.RulesToApply.First);

        Assert.NotNull(appliedRule);
        Assert.Equal(newRule1.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_String_Equals_MultipleContext2()
    {
        // Rule
        RuleDefinition rule1 = new()
        {
            Number = 1,
            Name = "Name Rule",
            Statement = @"name equals ""John""",
            Context = "NAMECONTEXT1",
            Decision = "NAMEFOUND",
        };

        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"name equals ""John""",
            Context = "NAMECONTEXT2",
            Decision = "NAMEFOUND2",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NAMECONTEXT2", RightRule.System.Common.RulesToApply.First);

        Assert.NotNull(appliedRule);
        Assert.Equal(newRule2.Decision, appliedRule.Decision);
    }
}