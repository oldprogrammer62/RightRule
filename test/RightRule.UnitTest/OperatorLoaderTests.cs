using RightRule.Loaders.OperatorLoader;

namespace RightRuleTests;

public class OperatorLoaderTests
{
    [Fact]
    public void TestItWorks_LoadOperator_Exists()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\Operator.exists.dll";
        var op = Loader.Load(path);
        Assert.NotNull(op);
    }

    [Fact]
    public void TestItWorks_LoadOperator_DayOfWeek()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\Operator.dayofweek.dll";
        var op = Loader.Load(path);
        Assert.NotNull(op);
    }

    //[Fact]
    //public void TestItWorks_LoadOperator_DoesNotContain()
    //{
    //    var path = @"C:\\Development\\RightRule\\RightRule\\src\\Operators\\CustomOperators\\Operator.doesnotcontain.dll";
    //    var op = Loader.Load(path);
    //    Assert.NotNull(op);
    //}

    //[Fact]
    //public void TestItWorks_LoadOperator_GreaterThanOrEquals()
    //{
    //    var path = @"C:\\Development\\RightRule\\RightRule\\src\\Operators\\CustomOperators\\Operator.greaterthanorequals.dll";
    //    var op = Loader.Load(path);
    //    Assert.NotNull(op);
    //}
}
