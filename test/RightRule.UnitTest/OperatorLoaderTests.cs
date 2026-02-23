namespace RightRuleTests;

public class OperatorLoaderTests
{
    [Fact]
    public void TestItWorks_LoadOperator()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\Operator.doesnotcontain.dll";
        var op = Operator.Loader.Loader.Load(path);
        Assert.NotNull(op);
    }
}
