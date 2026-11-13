using Microsoft.Extensions.Logging.Abstractions;
using RightRule;
using RightRule.Models;
using RightRule.OperatorServices;
using RightRule.OperatorServices.OperatorLoader.CustomOperators;
using RightRule.OperatorServices.OperatorLoader.DefaultOperators;

namespace RightRuleTests;

public class OperatorLoaderTests
{
    private static CustomOperatorLoader CreateCustomLoader() =>
        new(NullLogger<CustomOperatorLoader>.Instance);

    private static DefaultOperatorLoader CreateDefaultLoader() =>
        new(NullLogger<DefaultOperatorLoader>.Instance);

    [Fact]
    public void RegisterAll_StartUp()
    {
        IRightRule rr = new RightRule.RightRule();

        Assert.NotNull(rr);
        Assert.NotNull(rr.OperatorService.OperatorAliases);
    }

    [Fact]
    public void RegisterAll_Manually()
    {
        IOperatorService ops = new OperatorService(CreateDefaultLoader(), CreateCustomLoader());
        var x = ops.RegisterAllOperators();

        Assert.NotNull(x);
        Assert.Equal(RightRuleStatusEnum.SUCCESS, x.Status);

        Assert.NotNull(ops.Operators);
        Assert.NotNull(ops.OperatorAliases);
    }

    [Fact]
    public void LoadAll_FromJsonConfig()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\customoperatorconfig.json";
        var op = CreateCustomLoader().LoadOperators(path);
        Assert.NotNull(op);
    }

    [Fact]
    public void LoadCustomOperator_Exists()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\Operator.exists.dll";
        var op = CreateCustomLoader().LoadOperator(path);
        Assert.NotNull(op);
        Assert.Equal("exists", op.Name.ToLower());
    }

    [Fact]
    public void LoadCustomOperator_DayOfWeek()
    {
        var path = @"C:\\Development\\BSoft\\RightRule\\src\\Operators\\CustomOperators\\Operator.dayofweek.dll";
        var op = CreateCustomLoader().LoadOperator(path);
        Assert.NotNull(op);
        Assert.Equal("dayofweek", op.Name.ToLower());
    }
}
