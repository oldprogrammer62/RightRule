using RightRule.OperatorServices.OperatorLoader.CustomOperators;

namespace RightRuleTests;

public class OperatorLoaderTests
{
    [Fact]
    public void TestItWorks_LoadOperator_Exists()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\Operator.exists.dll";
        var op = ExternalAssemblyLoader.Load(path);
        Assert.NotNull(op);
    }

    [Fact]
    public void TestItWorks_LoadOperator_DayOfWeek()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\Operator.dayofweek.dll";
        var op = ExternalAssemblyLoader.Load(path);
        Assert.NotNull(op);
    }
}
