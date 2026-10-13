using RightRule;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;
using System.Collections;

namespace RightRuleTests;

public class DataTypesTests_Array
{
    [Fact]
    public void Int_in_ArrayList()
    {
        int number = 4;
        ArrayList numbers = new ArrayList() { 1, 2, 3, 100, 4, 5 };

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Int In Value Rule",
            Statement = @"number in numbers",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };

        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };

        IRightRule rr = new RightRule.RightRule(options);

        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("number", number);
        rr.RegisterData("numbers", numbers);

        var appliedRule = rr.ApplyRule("Context");

        Assert.NotNull(appliedRule.StatementApplied);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Int_in_Array()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };
        IRightRule rr = new RightRule.RightRule(options);

        int number1 = 1;
        rr.RegisterData("number", number1);

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Int In Array Rule",
            Statement = "number in [1,2,3]",
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };
        var newRule = rr.RegisterRule(rule);

        var appliedRule = rr.ApplyRule("Context");

        Assert.NotNull(appliedRule.StatementApplied);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Int_in_ArrayWithReplacements()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };
        IRightRule rr = new RightRule.RightRule(options);

        int number1 = 1;
        int number2 = 2;
        int number3 = 3;
        int number4 = 4;
        rr.RegisterData("number", number1);

        var statement = string.Format("number in [{0},{1},{2},{3}]",number1,number2,number3,number4);

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "Int In Array Rule",
            Statement = statement,
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };
        var newRule = rr.RegisterRule(rule);

        var appliedRule = rr.ApplyRule("Context");

        Assert.NotNull(appliedRule.StatementApplied);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void String_in_ArrayWithReplacements()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };
        IRightRule rr = new RightRule.RightRule(options);

        string s1 = "my";
        string s2 = "name";
        string s3 = "is";
        string s4 = "mikey";
        rr.RegisterData("string", "name");

        var statement = string.Format("string in [\"{0}\",\"{1}\",\"{2}\",\"{3}\"]", s1, s2, s3, s4);

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "String In Array Rule",
            Statement = statement,
            Context = "Context",
            Decision = "EXISTS",
            FalseDecision = "NOT_EXISTS"
        };
        var newRule = rr.RegisterRule(rule);

        var appliedRule = rr.ApplyRule("Context");

        Assert.NotNull(appliedRule.StatementApplied);
        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
