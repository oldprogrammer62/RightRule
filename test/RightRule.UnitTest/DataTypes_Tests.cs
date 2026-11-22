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
        var rr = new RightRule.RightRule().CreateSession();

        int a = 100;
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_Dec()
    {
        var rr = new RightRule.RightRule().CreateSession();

        double a = 100.001;
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = 100.001")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_String()
    {
        var rr = new RightRule.RightRule().CreateSession();

        string a = "100";
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"100\"")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_Char()
    {
        var rr = new RightRule.RightRule().CreateSession();

        char a = 'C';
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \'C\'")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_Boolean()
    {
        var rr = new RightRule.RightRule().CreateSession();

        bool a = true;
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = true")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_BooleanString()
    {
        var rr = new RightRule.RightRule().CreateSession();

        bool a = true;
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"true\"")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_DateTime()
    {
        var rr = new RightRule.RightRule().CreateSession();

        DateTime a = new DateTime(1970, 01, 01, 12, 12, 12);
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"01/01/1970 12:12:12\"")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_DateOnly()
    {
        var rr = new RightRule.RightRule().CreateSession();

        DateOnly a = new DateOnly(1970, 01, 01);
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"01/01/1970\"")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void Equals_TimeOnly()
    {
        var rr = new RightRule.RightRule().CreateSession();

        TimeOnly a = new TimeOnly(12, 30, 30);
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a = \"12:30:30\"")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var result = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(result.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void In_ArrayIntegers()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();

        int a = 2;
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a in [1,2,3]")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var appliedRule = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(appliedRule);
        Assert.NotNull(appliedRule.StatementApplied);
        Assert.Equal(appliedRule.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void In_ArrayDoubles()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();

        double a = 2.2;
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a in [1.1,2.2,3.3]")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var appliedRule = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(appliedRule);
        Assert.NotNull(appliedRule.StatementApplied);
        Assert.Equal(appliedRule.Decision, rule1Reg?.RegisteredRule?.Decision);
    }

    [Fact]
    public void In_ArrayStrings()
    {
        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();

        string a = "2";
        var data1Reg = rr.DataService.RegisterData("a", a);

        var rule1Reg = rr.RuleService.BuildRule()
            .Number(1)
            .Name("Rule1")
            .Context("Context")
            .Statement("a in [\"1\",\"2\",\"3\"]")
            .Decision("SAME")
            .Build()
            .Register(rr.RuleService);

        var appliedRule = rr.RuleService.ApplyRule("Context");
        Assert.NotNull(appliedRule);
        Assert.NotNull(appliedRule.StatementApplied);
        Assert.Equal(appliedRule.Decision, rule1Reg?.RegisteredRule?.Decision);
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
        };

        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();

        var newRule = rr.RuleService.RegisterRule(rule);
        rr.DataService.RegisterData("number", number);
        rr.DataService.RegisterData("numbers", numbers);

        var appliedRule = rr.RuleService.ApplyRule("Context");

        Assert.NotNull(appliedRule.StatementApplied);
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
        };

        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();
        var newRule = rr.RuleService.RegisterRule(rule);
        rr.DataService.RegisterData("number", number);
        rr.DataService.RegisterData("numbers", numbers);

        var appliedRule = rr.RuleService.ApplyRule("Context");

        Assert.NotNull(appliedRule.StatementApplied);
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
            DecisionData = "Hello $name.name World",
        };

        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true
        };

        var rr = new RightRule.RightRule(options).CreateSession();

        string name = "Harry";
        rr.DataService.RegisterData("name", name);


        var newRule = rr.RuleService.RegisterRule(rule);
        rr.DataService.RegisterData("value", value);
        rr.DataService.RegisterData("values", values);

        var appliedRule = rr.RuleService.ApplyRule("Context");

        Assert.NotNull(appliedRule.StatementApplied);
        Assert.Equal(rule.Decision, appliedRule.Decision);
        Assert.DoesNotContain("$name.name", appliedRule.DecisionData.ToString());
    }
}
