using BeSoft;
using RightRule.Models.RuleModels;
using RightRule.RegistrationServices.RuleRegistration;
using RightRule.RuleApplication;
using RightRule.System.Common;

namespace RightRuleTests;

public class ObjectReference_Tests
{
    [Fact]
    public void NewObjectRefAndData_ShouldBeEqual()
    {
        IRightRule rr = new BeSoft.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "OrderCount", 5 }
        };
        rr.RegisterData("OrderData", data);

        var obj1 = new RefObject() { Name = "Bob", Age = 31 };
        var name = "Bob";
        var age = 31;

        rr.RegisterData("BobData", nameof(name), name);
        rr.RegisterData("BobData", nameof(age), age);

        DefinedRule rd1 = new DefinedRule()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "BobData.name = \"Bob\" and BobData.age = 31 and OrderData.OrderCount >= 5",
            Decision = "Approved",
            FalseDecision = "Denied",
            OptionalData = null
        };
        rr.RegisterRule(rd1);
        var result = rr.ApplyRules("Test");
        Assert.NotNull(result);
        Assert.Equal(rd1.Decision, result.Decision);
    }

    [Fact]
    public void ObjectRefAndData_ShouldBeEqual()
    {
        IRightRule rr = new BeSoft.RightRule();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "OrderCount", 5 }
        };
        rr.RegisterData("Data", data);

        var obj1 = new RefObject() { Name = "Bob", Age = 31 };
        var name = "Bob";
        var age = 31;
        rr.RegisterData("obj1", nameof(name), name);
        rr.RegisterData("obj1", nameof(age), age);

        DefinedRule rd1 = new DefinedRule()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "obj1.name = \"Bob\" and obj1.age = 31 and Data.OrderCount >= 5",
            Decision = "Approved",
            FalseDecision = "Denied",
            OptionalData = null
        };

        rr.RegisterRule(rd1);
        var result = rr.ApplyRules("Test");
        Assert.NotNull(result);
        Assert.Equal(rd1.Decision, result.Decision);
    }

    [Fact]
    public void ObjectReference_ShouldBeEqual()
    {
        IRightRule rr = new BeSoft.RightRule();

        var name1 = "Bob";
        var age1 = 31;
        var name2 = "Harry";
        var age2 = 21;

        rr.RegisterData("obj1", nameof(name1), name1);
        rr.RegisterData("obj1", nameof(age1), age1);
        rr.RegisterData("obj2", nameof(name2), name2);
        rr.RegisterData("obj2", nameof(age2), age2);

        DefinedRule rd1 = new DefinedRule()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "obj1.name1 = \"Bob\" and obj1.age1 = 31 and obj2.name2 = \"Harry\" and obj2.age2 = 21",
            Decision = "Approved",
            FalseDecision = "Denied",
            OptionalData = null
        };

        rr.RegisterRule(rd1);
        var result = rr.ApplyRules("Test", RulesToReturnEnum.ALL);
        Assert.NotNull(result);
        Assert.Equal(rd1.Decision, result[0].Decision);
    }

    [Fact]
    public void ObjectReference_NotEqual()
    {
        IRightRule rr = new BeSoft.RightRule();

        var name1 = "Bob";
        var age1 = 30;
        var name2 = "Harry";
        var age2 = 20;

        rr.RegisterData("obj1", nameof(name1), name1);
        rr.RegisterData("obj1", nameof(age1), age1);
        rr.RegisterData("obj2", nameof(name2), name2);
        rr.RegisterData("obj2", nameof(age2), age2);

        DefinedRule rd1 = new DefinedRule()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "obj1.name1 = \"Bob\" and obj1.age1 = 31 and obj2.name2 = \"Harry\" and obj2.age2 = 20",
            Decision = "Approved",
            FalseDecision = "Denied",
            OptionalData = null
        };

        rr.RegisterRule(rd1);
        var result = rr.ApplyRules("Test", RulesToReturnEnum.ALL);
        Assert.NotNull(result);

        Assert.Equal(rd1.FalseDecision, result[0].Decision);
    }

    [Fact]
    public void ObjectRegisteredAsIntrinsicVariable()
    {
        int x = 100;

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var added1 = rr.RegisterData("x", nameof(x), x);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, added1.Status);

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
        var result = rr.ApplyRules("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Multi_ObjectRegisteredAsIntrinsicVariable()
    {
        int x = 100;
        int y = 200;
        string greeting = "Hello";

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var added1 = rr.RegisterData("datax", nameof(x), x);
        var added2 = rr.RegisterData("datay", nameof(y), y);
        var added3 = rr.RegisterData("datagreeting", nameof(greeting), greeting);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, added1.Status);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, added2.Status);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, added3.Status);

        DefinedRule rule = new DefinedRule()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Decision = "GOOD",
            FalseDecision = "BAD",
            Statement = "datax.x = 100 and datay.y = 200 and datagreeting.greeting = \"Hello\""
        };
        var registeredRule = rr.RegisterRule(rule);
        var result = rr.ApplyRules("Context");
        Assert.NotNull(result);
        Assert.Equal(rule.Decision, result.Decision);
    }

    [Fact]
    public void Multi_ObjectRegistered_Mixed()
    {
        int x = 100;
        int y = 200;
        string greeting = "Hello";

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var added1 = rr.RegisterData("datax", nameof(x), x);
        var added2 = rr.RegisterData("datay", nameof(y), y);
        var added3 = rr.RegisterData("datagreeting", nameof(greeting), greeting);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, added1.Status);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, added2.Status);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, added3.Status);

        var obj1 = new RefObject() { Name = "Bob", Age = 30 };
        var name = "Bob";
        var age = 30;
        var added4 = rr.RegisterData("databob", nameof(name), name);
        var added5 = rr.RegisterData("databob", nameof(age), age);
        Assert.Equal(RegistrationStatusEnum.SUCCESS, added4.Status);

        DefinedRule rule = new DefinedRule()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Decision = "GOOD",
            FalseDecision = "BAD",
            Statement = "datax.x = 100 and datay.y = 200 and datagreeting.greeting = \"Hello\" and databob.age = 30"
        };
        var registeredRule = rr.RegisterRule(rule);
        var result = rr.ApplyRules("Context");
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
