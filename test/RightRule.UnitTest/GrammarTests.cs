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
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 11",
            Context = "NumberOfOrdersContext",
            Decision = "ENOUGHORDERS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NumberOfOrdersContext");

        Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_ThisAndThis()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10 and Test.goodpayer equals true",
            Context = "NumberOfOrdersContext",
            Decision = "OK",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NumberOfOrdersContext");

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
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10 or Test.goodpayer equals true",
            Context = "NumberOfOrdersContext",
            Decision = "OK",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NumberOfOrdersContext");

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
            Name = "Number of orders Rule",
            Statement = @"(Test.numberoforders equals 10 and Test.goodpayer equals true) or (Test.numberoforders equals 10 and Test.goodpayer equals true)",
            Context = "NumberOfOrdersContext",
            Decision = "OK",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NumberOfOrdersContext");

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
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10",
            Context = "NumberOfOrdersContext",
            Decision = "ENOUGHORDERS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NumberOfOrdersContext");

        Assert.NotNull(appliedRule);
    }

    [Fact]
    public void TestItWorks_Number_NotEquals()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = "Test.numberoforders notequals 11",
            Context = "NumberOfOrdersContext",
            Decision = "NOTENOUGHORDERS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NumberOfOrdersContext");

        Assert.Equal(appliedRule.Decision, rule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_NotEqualsSymbol1()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = "Test.numberoforders != 18",
            Context = "NumberOfOrdersContext",
            Decision = "INCORRECTORDERCOUNT",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NumberOfOrdersContext");

        Assert.Equal(appliedRule.Decision, rule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_NotEqualsSymbol2()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = "Test.numberoforders <> 11",
            Context = "NumberOfOrdersContext",
            Decision = "NOTENOUGHORDERS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NumberOfOrdersContext");

        Assert.Equal(appliedRule.Decision, rule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_EqualsSymbol()
    {
        // Rule
        RuleDefinition rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = "Test.numberoforders = 10",
            Context = "NumberOfOrdersContext",
            Decision = "ENOUGHORDERS",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NumberOfOrdersContext");
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
            Statement = @"Test.name equals ""John""",
            Context = "NameContext",
            Decision = "NAMEFOUND",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NameContext");

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
            Statement = "Test.name equals Test.fullname",
            Context = "NameContext",
            Decision = "NAMESAMEASFULLNAME",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NameContext");

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
            Statement = "Test.goodpayer equals true",
            Context = "GoodPayerContext",
            Decision = "GOODPAYER",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("GoodPayerContext");

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
            Statement = "Test.goodpayer equals false",
            Context = "NotGoodPayerContext",
            Decision = "NOTGOODPAYER",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NotGoodPayerContext");

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
            Statement = @"Test.name equals ""John""",
            Context = "NAMECONTEXT1",
            Decision = "NAMEFOUND",
        };

        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"Test.name equals ""John""",
            Context = "NAMECONTEXT2",
            Decision = "NAMEFOUND2",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NAMECONTEXT1");

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
            Statement = @"Test.name equals ""John""",
            Context = "NAMECONTEXT1",
            Decision = "NAMEFOUND",
        };

        RuleDefinition rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"Test.name equals ""John""",
            Context = "NAMECONTEXT2",
            Decision = "NAMEFOUND2",
        };

        RightRule.RightRule rr = new RightRule.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRule("NAMECONTEXT2");

        Assert.NotNull(appliedRule);
        Assert.Equal(newRule2.Decision, appliedRule.Decision);
    }
}
