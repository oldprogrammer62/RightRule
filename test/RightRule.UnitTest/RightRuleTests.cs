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

        using var sut = new RightRule.RightRule(options).CreateSession();

        Assert.Equal(options.LoadCustomOperators, sut.RightRuleOptions.LoadCustomOperators);
        Assert.Equal(options.LoadDefaultOperators, sut.RightRuleOptions.LoadDefaultOperators);

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
        var sut = new RightRule.RightRule(options).CreateSession();

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
        var sut = scope.ServiceProvider.GetRequiredService<RightRule.RightRuleSession.IRightRuleSession>();

        Assert.False(sut.RightRuleOptions.LoadCustomOperators);
        Assert.False(sut.RightRuleOptions.LoadDefaultOperators);
        Assert.NotNull(sut.RuleService);
        Assert.NotNull(sut.DataService);
        Assert.NotNull(sut.OperatorService);
        Assert.NotNull(sut.DataAdaptorService);
    }
}