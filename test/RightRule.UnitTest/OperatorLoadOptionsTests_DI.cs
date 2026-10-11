using Microsoft.Extensions.DependencyInjection;
using RightRule.ServiceRegister;

namespace RightRuleTests;

public class OperatorLoadOptionsTests_DI
{
    [Fact]
    public void RightRule_WithoutOptions()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddRightRule();
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var rr = scope.ServiceProvider.GetRequiredService<RightRule.IRightRule>();

        Assert.NotNull(rr);

        var operators = rr.GetOperators();
        Assert.NotNull(operators);
        Assert.Equal(10, operators.Count);

        var defaultOperators = operators.Where(op => op.IsCustom == false).ToList();
        Assert.NotNull(defaultOperators);
        Assert.Equal(7, defaultOperators.Count);

        var customOperators = operators.Where(op => op.IsCustom == true).ToList();
        Assert.NotNull(customOperators);
        Assert.Equal(3, customOperators.Count);
    }

    [Fact]
    public void RightRule_WithOptions_True()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddRightRule(options =>
        {
            options.LoadDefaultOperators = true;
            options.DataCheck = true;
            options.Diagnostics = true;
            options.CustomOperatorLibraryPath = @"C:\Development\BSoft\RightRule\src\Operators\CustomOperators\customoperatorconfig.json";
        });

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var rr = scope.ServiceProvider.GetRequiredService<RightRule.IRightRule>();

        Assert.NotNull(rr);

        var operators = rr.GetOperators();
        Assert.NotNull(operators);
        Assert.Equal(10, operators.Count);

        var defaultOperators = operators.Where(op => op.IsCustom == false).ToList();
        Assert.NotNull(defaultOperators);
        Assert.Equal(7, defaultOperators.Count);

        var customOperators = operators.Where(op => op.IsCustom == true).ToList();
        Assert.NotNull(customOperators);
        Assert.Equal(3, customOperators.Count);
    }

    [Fact]
    public void RightRule_WithOptions_False()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddRightRule(options =>
        {
            options.LoadDefaultOperators = false;
            options.DataCheck = false;
            options.Diagnostics = false;
            options.CustomOperatorLibraryPath = @"C:\Development\BSoft\RightRule\src\Operators\CustomOperators\customoperatorconfig.json";
        });

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var rr = scope.ServiceProvider.GetRequiredService<RightRule.IRightRule>();

        Assert.NotNull(rr);

        var operators = rr.GetOperators();
        Assert.NotNull(operators);
        Assert.Equal(3, operators.Count);

        var defaultOperators = operators.Where(op => op.IsCustom == false).ToList();
        Assert.NotNull(defaultOperators);
        Assert.Equal(0, defaultOperators?.Count ?? 0);

        var customOperators = operators.Where(op => op.IsCustom == true).ToList();
        Assert.NotNull(customOperators);
        Assert.Equal(3, customOperators.Count);
    }
}
