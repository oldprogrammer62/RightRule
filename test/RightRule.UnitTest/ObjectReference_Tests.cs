using BeSoft;
using RightRule.RuleModels;

namespace RightRuleTests;

public class ObjectReference_Tests
{
    [Fact]
    public void ObjectReference_ShouldBeEqual()
    {
        // Arrange
        var obj1 = new RefObject() { Name = "Bob", Age = 31 };
        var obj2 = new RefObject() { Name = "Harry", Age = 21 };

        IRightRule rr = new BeSoft.RightRule();
        RuleDefinition rd1 = new RuleDefinition()
        {
            Number = 1,
            Name = "TestRule",
            Context = "Test",
            Statement = "obj1.Name = \"Bob\" and obj1.Age = 31",
            Decision = "Approved",
            OperatorConfig = obj1
        };
        RuleDefinition rd2 = new RuleDefinition()
        {
            Number = 2,
            Name = "TestRule",
            Context = "Test",
            Statement = "obj2.Name = \"Harry\" and obj2.Age = 21",
            Decision = "Good",
            OperatorConfig = obj2
        };
        rr.RegisterRule(rd1);
        rr.RegisterRule(rd2);
        var result = rr.EvaluateAllRules("Test");
        Assert.NotNull(result);
        Assert.True(result[0].Result);
        Assert.True(result[1].Result);

        Assert.Equal(rd1.Decision, result[0].Decision);
        Assert.Equal(rd2.Decision, result[1].Decision);
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
