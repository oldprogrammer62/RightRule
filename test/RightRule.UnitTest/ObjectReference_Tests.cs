using RightRule;
using RightRule.LanguageProcessor.Evaluator;
using RightRule.Models;
using RightRule.RuleServices.RuleModels;

namespace RightRuleTests;

public class ObjectReference_Tests
{
    [Fact]
    public void NewObjectRefAndData_ShouldBeEqual()
    {
        IRightRule rr = new RightRule.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "OrderCount", 5 }
        };
        rr.RegisterData("OrderData", data);

        var obj1 = new RefObject() { Name = "Bob", Age = 31 };
        var name = "Bob";
        var age = 31;

        rr.RegisterData("BobsName", name);
        rr.RegisterData("BobsAge", age);

        DefinedRule rd1 = new DefinedRule()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "BobsName = \"Bob\" and BobsAge = 31 and OrderData.OrderCount >= 5",
            Decision = "Approved",
            FalseDecision = "Denied",
            OptionalData = null
        };
        rr.RegisterRule(rd1);
        var result = rr.ApplyRule("Test");
        Assert.NotNull(result);
        Assert.Equal(rd1.Decision, result.Decision);
    }

    [Fact]
    public void ObjectRefAndData_ShouldBeEqual()
    {
        IRightRule rr = new RightRule.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "OrderCount", 5 }
        };
        rr.RegisterData(nameof(data), data);

        var obj1 = new RefObject() { Name = "Bob", Age = 31 };
        var name = "Bob";
        var age = 31;
        rr.RegisterData("BobsName", name);
        rr.RegisterData("BobsAge", age);

        DefinedRule rd1 = new DefinedRule()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "BobsName = \"Bob\" and BobsAge = 31 and Data.OrderCount >= 5",
            Decision = "Approved",
            FalseDecision = "Denied",
            OptionalData = null
        };

        rr.RegisterRule(rd1);
        var result = rr.ApplyRule("Test");
        Assert.NotNull(result);
        Assert.Equal(rd1.Decision, result.Decision);
    }

    [Fact]
    public void ObjectReference_ShouldBeEqual()
    {
        IRightRule rr = new RightRule.RightRule();

        var name1 = "Bob";
        var age1 = 31;
        var name2 = "Harry";
        var age2 = 21;

        rr.RegisterData("BobsName", name1);
        rr.RegisterData("BobsAge", age1);
        rr.RegisterData("HarrysName", name2);
        rr.RegisterData("HarrysAge", age2);

        DefinedRule rd1 = new DefinedRule()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "BobsName = \"Bob\" and BobsAge = 31 and HarrysName = \"Harry\" and HarrysAge = 21",
            Decision = "Approved",
            FalseDecision = "Denied",
            OptionalData = null
        };

        rr.RegisterRule(rd1);
        var result = rr.ApplyRules("Test", EvaluationTypeEnum.ANY);
        Assert.NotNull(result);
        Assert.Equal(rd1.Decision, result[0].Decision);
    }

    [Fact]
    public void ObjectReference_NotEqual()
    {
        IRightRule rr = new RightRule.RightRule();

        var name1 = "Bob";
        var age1 = 30;
        var name2 = "Harry";
        var age2 = 20;

        rr.RegisterData("BobsName", name1);
        rr.RegisterData("BobsAge", age1);
        rr.RegisterData("HarrysName", name2);
        rr.RegisterData("HarrysAge", age2);

        DefinedRule rd1 = new DefinedRule()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "BobsName = \"Bob\" and BobsAge = 31 and HarrysName = \"Harry\" and HarrysAge = 20",
            Decision = "Approved",
            FalseDecision = "Denied",
            OptionalData = null
        };

        rr.RegisterRule(rd1);
        var result = rr.ApplyRules("Test", EvaluationTypeEnum.ANY);
        Assert.NotNull(result);

        Assert.Equal(rd1.FalseDecision, result[0].Decision);
    }

    [Fact]
    public void ObjectRegisteredAsIntrinsicVariable()
    {
        int x = 100;

        IRightRule rr = new RightRule.RightRule();
        var added1 = rr.RegisterData(nameof(x), x);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added1.Status);

        DefinedRule rule = new DefinedRule()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Decision = "GOOD",
            FalseDecision = "BAD",
            Statement = "x.x = 100"
        };
        var registeredRule = rr.RegisterRule(rule);
        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Multi_ObjectRegisteredAsIntrinsicVariable()
    {
        int x = 100;
        int y = 200;
        string greeting = "Hello";

        IRightRule rr = new RightRule.RightRule();
        var added1 = rr.RegisterData(nameof(x), x);
        var added2 = rr.RegisterData(nameof(y), y);
        var added3 = rr.RegisterData(nameof(greeting), greeting);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added1.Status);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added2.Status);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added3.Status);

        DefinedRule rule = new DefinedRule()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Decision = "GOOD",
            FalseDecision = "BAD",
            Statement = "x = 100 and y = 200 and greeting = \"Hello\""
        };
        var registeredRule = rr.RegisterRule(rule);
        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Multi_ObjectRegistered_Mixed()
    {
        int x = 100;
        int y = 200;
        string greeting = "Hello";

        IRightRule rr = new RightRule.RightRule();
        var added1 = rr.RegisterData(nameof(x), x);
        var added2 = rr.RegisterData(nameof(y), y);
        var added3 = rr.RegisterData(nameof(greeting), greeting);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added1.Status);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added2.Status);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added3.Status);

        var obj1 = new RefObject() { Name = "Bob", Age = 30 };
        var name = "Bob";
        var age = 30;
        var added4 = rr.RegisterData(nameof(name), name);
        var added5 = rr.RegisterData("BobsAge", age);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, added4.Status);

        DefinedRule rule = new DefinedRule()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Decision = "GOOD",
            FalseDecision = "BAD",
            Statement = "x = 100 and y = 200 and greeting = \"Hello\" and BobsAge = 30"
        };
        var registeredRule = rr.RegisterRule(rule);
        var result = rr.ApplyRule("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }
}

public class RefObject
{
    public RefObject()
    {
        Name = "Bob";
        Age = 21;
    }
    public string? Name { get; set; }
    public int Age { get; set; }
}
