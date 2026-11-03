using Microsoft.Extensions.DependencyInjection;
using RightRule.Models;
using RightRule.ServiceRegister;

namespace RightRuleTests;

public class RightRuleConstructionTests
{
    [Fact]
    public void OptionsConstructor_ExposesOptionsAndServices()
    {
        var options = new RightRuleOptions
        {
            LoadCustomOperators = false,
            LoadDefaultOperators = false
        };

        using var sut = new RightRule.RightRule(options);

        Assert.Same(options, sut.Options);
        Assert.Same(options, sut.GetRightRuleOptions());
        Assert.NotNull(sut.RuleService);
        Assert.NotNull(sut.DataService);
        Assert.NotNull(sut.OperatorService);
        Assert.NotNull(sut.DataAdaptorService);
    }

    [Fact]
    public void OptionsConstructor_ThrowsWhenOptionsAreNull()
    {
        Assert.Throws<ArgumentNullException>(
            () => new RightRule.RightRule((RightRuleOptions)null!));
    }

    [Fact]
    public void Dispose_CanBeCalledMoreThanOnce()
    {
        var options = new RightRuleOptions
        {
            LoadCustomOperators = false,
            LoadDefaultOperators = false
        };
        var sut = new RightRule.RightRule(options);

        sut.Dispose();
        sut.Dispose();
    }

    [Fact]
    public void DependencyInjectionConstructor_UsesRegisteredOptionsAndServices()
    {
        var services = new ServiceCollection();
        services.AddRightRule(options =>
        {
            options.LoadCustomOperators = false;
            options.LoadDefaultOperators = false;
        });

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var sut = scope.ServiceProvider.GetRequiredService<RightRule.IRightRule>();

        Assert.Same(sut.Options, sut.GetRightRuleOptions());
        Assert.False(sut.Options.LoadCustomOperators);
        Assert.False(sut.Options.LoadDefaultOperators);
        Assert.NotNull(sut.RuleService);
        Assert.NotNull(sut.DataService);
        Assert.NotNull(sut.OperatorService);
        Assert.NotNull(sut.DataAdaptorService);
    }
}