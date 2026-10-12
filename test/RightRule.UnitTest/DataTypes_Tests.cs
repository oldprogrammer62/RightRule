using Microsoft.Testing.Platform.Extensions.Messages;
using RightRule;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;
using System.Collections;
using System.Net.Security;

namespace RightRuleTests;

public class DataTypes_Tests
{
    [Fact]
    public void Equals_Int()
    {
        IRightRule rr = new RightRule.RightRule();

        int a = 100;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_Dec()
    {
        IRightRule rr = new RightRule.RightRule();

        double a = 100.001;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100.001")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_String()
    {
        IRightRule rr = new RightRule.RightRule();

        string a = "100";
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"100\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_Char()
    {
        IRightRule rr = new RightRule.RightRule();

        char a = 'C';
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \'C\'")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_Boolean()
    {
        IRightRule rr = new RightRule.RightRule();

        bool a = true;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = true")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_BooleanString()
    {
        IRightRule rr = new RightRule.RightRule();

        bool a = true;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"true\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_DateTime()
    {
        IRightRule rr = new RightRule.RightRule();

        DateTime a = new DateTime(1970, 01, 01, 12, 12, 12);
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"01/01/1970 12:12:12\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_DateOnly()
    {
        IRightRule rr = new RightRule.RightRule();

        DateOnly a = new DateOnly(1970, 01, 01);
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"01/01/1970\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_TimeOnly()
    {
        IRightRule rr = new RightRule.RightRule();

        TimeOnly a = new TimeOnly(12, 30, 30);
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"12:30:30\"")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_ArrayIntegers()
    {
        IRightRule rr = new RightRule.RightRule();

        int a = 2;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a in [1,2,3]")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_ArrayDoubles()
    {
        IRightRule rr = new RightRule.RightRule();

        double a = 2.2;
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a in [1.1,2.2,3.3]")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_ArrayStrings()
    {
        IRightRule rr = new RightRule.RightRule();

        string a = "2";
        var data1Reg = rr.RegisterData("a", a);

        var rule1Reg = rr.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a in [\"1\",\"2\",\"3\"]")
            .Decision("SAME")
            .FalseDecision("DIFF")
            .Build()
            .Register(rr.RuleService);

        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

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

        IRightRule rr = new RightRule.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("number", number);
        rr.RegisterData("numbers", numbers);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }

    [Fact]
    public void Int_in_Array()
    {
        int number = 4;
        int[] numbers = [1,2,3,4,5];

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

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }


    [Fact]
    public void String_in_Array()
    {
        string value = "Hello";
        string[] values = ["Not", "Here", "Yet", "Hello", "World"];

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "String In Value Rule",
            Statement = @"value in values",
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
        rr.RegisterData("value", value);
        rr.RegisterData("values", values);

        var appliedRule = rr.ApplyRule("Context");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
