using RightRule;
using RightRule.OperatorServices;
using RightRule.OperatorServices.OperatorLoader.CustomOperators;

namespace RightRuleTests;

public class OperatorLoaderTests
{
    [Fact]
    public void RegisterAll_StartUp()
    {
        IRightRule rr = new RightRule.RightRule();

        Assert.NotNull(rr);
        Assert.NotNull(rr.GetOperatorAliases());
    }

    [Fact]
    public void RegisterAll_Manually()
    {
        IOperatorService ops = new OperatorService();
        var x = ops.RegisterAllOperators();

        Assert.True(x);
        Assert.NotNull(ops.Operators);
        Assert.NotNull(ops.OperatorAliases);
    }

    [Fact]
    public void LoadAll_FromJsonConfig()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\customoperatorconfig.json";
        var op = CustomOperatorLoader.LoadOperators(path);
        Assert.NotNull(op);
    }

    [Fact]
    public void LoadCustomOperator_Exists()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\Operator.exists.dll";
        var op = CustomOperatorLoader.LoadOperator(path);
        Assert.NotNull(op);
        Assert.Equal("exists", op.Name.ToLower());
    }

    [Fact]
    public void LoadCustomOperator_DayOfWeek()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\Operator.dayofweek.dll";
        var op = CustomOperatorLoader.LoadOperator(path);
        Assert.NotNull(op);
        Assert.Equal("dayofweek", op.Name.ToLower());
    }
}
