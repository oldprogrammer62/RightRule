using BeSoft;
using RightRule.RuleModels;

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
        rr.RegisterData("BobData", obj1);

        RuleDefinition rd1 = new RuleDefinition()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "BobData.Name = \"Bob\" and BobData.Age = 31 and OrderData.OrderCount >= 5",
            Decision = "Approved",
            OptionalData = null
        };
        rr.RegisterRule(rd1);
        var result = rr.EvaluateFirstRule("Test");
        Assert.NotNull(result);
        Assert.True(result.Result);
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
        rr.RegisterData("obj1", obj1);

        RuleDefinition rd1 = new RuleDefinition()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "obj1.Name = \"Bob\" and obj1.Age = 31 and Data.OrderCount >= 5",
            Decision = "Approved",
            OptionalData = null
        };

        rr.RegisterRule(rd1);
        var result = rr.EvaluateFirstRule("Test");
        Assert.NotNull(result);
        Assert.True(result.Result);
        Assert.Equal(rd1.Decision, result.Decision);
    }

    [Fact]
    public void ObjectReference_ShouldBeEqual()
    {
        IRightRule rr = new BeSoft.RightRule();

        var obj1 = new RefObject() { Name = "Bob", Age = 31 };
        var obj2 = new RefObject() { Name = "Harry", Age = 21 };
        rr.RegisterData("obj1", obj1);
        rr.RegisterData("obj2", obj2);

        RuleDefinition rd1 = new RuleDefinition()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "obj1.Name = \"Bob\" and obj1.Age = 31 and obj2.Name = \"Harry\" and obj2.Age = 21",
            Decision = "Approved",
            OptionalData = null
        };

        rr.RegisterRule(rd1);
        var result = rr.EvaluateAllRules("Test");
        Assert.NotNull(result);
        Assert.True(result[0].Result);

        Assert.Equal(rd1.Decision, result[0].Decision);
    }

    [Fact]
    public void ObjectReference_NotEqual()
    {
        IRightRule rr = new BeSoft.RightRule();

        var obj1 = new RefObject() { Name = "Bob", Age = 30 };
        var obj2 = new RefObject() { Name = "Harry", Age = 20 };
        rr.RegisterData("obj1", obj1);
        rr.RegisterData("obj2", obj2);

        RuleDefinition rd1 = new RuleDefinition()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "obj1.Name = \"Bob\" and obj1.Age = 31 and obj2.Name = \"Harry\" and obj2.Age = 21",
            Decision = "Approved",
            OptionalData = null
        };

        rr.RegisterRule(rd1);
        var result = rr.EvaluateAllRules("Test");
        Assert.NotNull(result);
        Assert.False(result[0].Result);

        Assert.Equal(AppliedRule.DefaultDecision, result[0].Decision);
    }

    [Fact]
    public void ObjectRegisteredAsIntrinsicVariable()
    {
        int x = 100;

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var added1 = rr.RegisterData("x", x);
        Assert.True(added1);

        RuleDefinition rule = new RuleDefinition()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Decision = "GOOD",
            Statement = "x.x = 100"
        };
        var registeredRule = rr.RegisterRule(rule);
        var result = rr.EvaluateFirstRule("Context");
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
        var added1 = rr.RegisterData("datax", x);
        var added2 = rr.RegisterData("datay", y);
        var added3 = rr.RegisterData("datagreeting", greeting);
        Assert.True(added1);
        Assert.True(added2);
        Assert.True(added3);

        RuleDefinition rule = new RuleDefinition()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Decision = "GOOD",
            Statement = "datax.x = 100 and datay.y = 200 and datagreeting.greeting = \"Hello\""
        };
        var registeredRule = rr.RegisterRule(rule);
        var result = rr.EvaluateFirstRule("Context");
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
        var added1 = rr.RegisterData("datax", x);
        var added2 = rr.RegisterData("datay", y);
        var added3 = rr.RegisterData("datagreeting", greeting);
        Assert.True(added1);
        Assert.True(added2);
        Assert.True(added3);

        var obj1 = new RefObject() { Name = "Bob", Age = 30 };
        var added4 = rr.RegisterData("databob", obj1);
        Assert.True(added4);

        RuleDefinition rule = new RuleDefinition()
        {
            Context = "Context",
            Name = "Name",
            Number = 1,
            Decision = "GOOD",
            Statement = "datax.x = 100 and datay.y = 200 and datagreeting.greeting = \"Hello\" and databob.Age = 30"
        };
        var registeredRule = rr.RegisterRule(rule);
        var result = rr.EvaluateFirstRule("Context");
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
