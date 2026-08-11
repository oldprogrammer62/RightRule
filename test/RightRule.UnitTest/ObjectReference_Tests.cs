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
