using RightRule.Operator.Loader;

namespace RightRuleTests;

public class OperatorLoaderTests
{
    [Fact]
    public void TestItWorks_LoadOperator()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\Operator.doesnotcontain.dll";
        var op = Loader.Load(path);
        Assert.NotNull(op);
    }
}
