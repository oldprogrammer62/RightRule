using RightRule.RuleModels;

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
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 11",
            Context = "NumberOfOrdersContext",
            Decision = "ENOUGHORDERS",
            FalseDecision = "NOTENOUGHORDERS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NumberOfOrdersContext");

        Assert.Equal(DefaultRule.DECISIONWORD, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_ThisAndThis()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10 and Test.goodpayer equals true",
            Context = "NumberOfOrdersContext",
            Decision = "OK",
            FalseDecision = "NOTOK"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NumberOfOrdersContext");

        Assert.NotNull(appliedRule);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_ThisOrThis()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10 or Test.goodpayer equals true",
            Context = "NumberOfOrdersContext",
            Decision = "OK",
            FalseDecision = "NOTOK"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NumberOfOrdersContext");

        Assert.NotNull(appliedRule);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_ThisAndThisOrThis()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"(Test.numberoforders equals 10 and Test.goodpayer equals true) or (Test.numberoforders equals 10 and Test.goodpayer equals true)",
            Context = "NumberOfOrdersContext",
            Decision = "OK",
            FalseDecision = "NOTOK"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NumberOfOrdersContext");

        Assert.NotNull(appliedRule);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_Equals()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = @"Test.numberoforders equals 10",
            Context = "NumberOfOrdersContext",
            Decision = "ENOUGHORDERS",
            FalseDecision = "NOTENOUGHORDERS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NumberOfOrdersContext");

        Assert.NotNull(appliedRule);
    }

    [Fact]
    public void TestItWorks_Number_NotEquals()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = "Test.numberoforders notequals 11",
            Context = "NumberOfOrdersContext",
            Decision = "ENOUGHORDERS",
            FalseDecision = "NOTENOUGHORDERS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NumberOfOrdersContext");

        Assert.Equal(appliedRule.Decision, rule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_NotEqualsSymbol1()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = "Test.numberoforders != 18",
            Context = "NumberOfOrdersContext",
            Decision = "CORRECTORDERCOUNT",
            FalseDecision = "INCORRECTORDERCOUNT"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NumberOfOrdersContext");

        Assert.Equal(appliedRule.Decision, rule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_NotEqualsSymbol2()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = "Test.numberoforders <> 11",
            Context = "NumberOfOrdersContext",
            Decision = "ENOUGHORDERS",
            FalseDecision = "NOTENOUGHORDERS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NumberOfOrdersContext");

        Assert.Equal(appliedRule.Decision, rule.Decision);
    }

    [Fact]
    public void TestItWorks_Number_EqualsSymbol()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Number of orders Rule",
            Statement = "Test.numberoforders = 10",
            Context = "NumberOfOrdersContext",
            Decision = "ENOUGHORDERS",
            FalseDecision = "NOTENOUGHORDERS"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NumberOfOrdersContext");
        Assert.NotNull(appliedRule);
    }

    [Fact]
    public void TestItWorks_String_Equals()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"Test.name equals ""John""",
            Context = "NameContext",
            Decision = "NAMEFOUND",
            FalseDecision = "NAMENOTFOUND"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NameContext");

        Assert.NotNull(appliedRule);
    }

    [Fact]
    public void TestItWorks_String_Equals_String()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 3,
            Name = "Name Rule",
            Statement = "Test.name equals Test.fullname",
            Context = "NameContext",
            Decision = "NAMESAMEASFULLNAME",
            FalseDecision = "NAMEDIFFERENTFROMFULLNAME"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NameContext");

        Assert.Equal(DefaultRule.DECISIONWORD, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_Equals_Boolean_True()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 4,
            Name = "Good Payer Rule",
            Statement = "Test.goodpayer equals true",
            Context = "GoodPayerContext",
            Decision = "GOODPAYER",
            FalseDecision = "NOTGOODPAYER"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("GoodPayerContext");

        Assert.Equal(newRule.RegisteredRule.RuleDefinition.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_Equals_Boolean_False()
    {
        // Rule
        DefinedRule rule = new()
        {
            Number = 4,
            Name = "Good Payer Rule",
            Statement = "Test.goodpayer equals false",
            Context = "NotGoodPayerContext",
            Decision = "GOODPAYER",
            FalseDecision = "NOTGOODPAYER"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NotGoodPayerContext");

        Assert.Equal(DefaultRule.DECISIONWORD, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_String_Equals_MultipleContext1()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Name Rule",
            Statement = @"Test.name equals ""John""",
            Context = "NAMECONTEXT1",
            Decision = "NAMEFOUND",
            FalseDecision = "NAMENOTFOUND"
        };

        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Name Rule",
            Statement = @"Test.name equals ""John""",
            Context = "NAMECONTEXT2",
            Decision = "NAMEFOUND2",
            FalseDecision = "NAMENOTFOUND2"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NAMECONTEXT1");

        Assert.NotNull(appliedRule);
        Assert.Equal(newRule1.RegisteredRule.RuleDefinition.Decision, appliedRule.Decision);
    }

    [Fact]
    public void TestItWorks_String_Equals_MultipleContext2()
    {
        // Rule
        DefinedRule rule1 = new()
        {
            Number = 1,
            Name = "Name Rule1",
            Statement = @"Test.name equals ""John""",
            Context = "NAMECONTEXT1",
            Decision = "NAMEFOUND",
            FalseDecision = "NAMENOTFOUND"
        };

        DefinedRule rule2 = new()
        {
            Number = 2,
            Name = "Name Rule2",
            Statement = @"Test.name equals ""John""",
            Context = "NAMECONTEXT2",
            Decision = "NAMEFOUND2",
            FalseDecision = "NAMENOTFOUND2"
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule1 = rr.RegisterRule(rule1);
        var newRule2 = rr.RegisterRule(rule2);
        rr.RegisterData("Test", _Data);

        var appliedRule = rr.ApplyRules("NAMECONTEXT2");

        Assert.NotNull(appliedRule);
        Assert.Equal(newRule2.RegisteredRule.RuleDefinition.Decision, appliedRule.Decision);
    }
}
