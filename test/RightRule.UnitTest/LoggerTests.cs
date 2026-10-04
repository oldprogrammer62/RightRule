using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RightRule.ServiceRegister;
using RightRuleLogger;

namespace RightRuleTests;

public class LoggerTests
{
    [Fact]
    public void LogTest_New()
    {
        using var rr = new RightRule.RightRule();
        rr.RegisterData("a", 100);
        Assert.NotNull(rr);
    }

    [Fact]
    public void LogTest_DI()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddRightRule();
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var rr = scope.ServiceProvider.GetRequiredService<RightRule.IRightRule>();
        rr.RegisterData("a", 100);
        Assert.NotNull(rr);
    }

    [Fact]
    public void LogTest_FakeLogger()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddFakeLogging());
        services.AddRightRule();
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var rr = scope.ServiceProvider.GetRequiredService<RightRule.IRightRule>();
        rr.RegisterData("a", 100);
        Assert.NotNull(rr);
    }

    [Fact]
    public void LogTest_RightRuleLogger()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton(configuration);

        services.AddLogging(builder => 
            builder.AddRightRuleLogger());
        
        services.AddRightRule();
        
        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var rr = scope.ServiceProvider.GetRequiredService<RightRule.IRightRule>();
        
        rr.RegisterData("a", 100);
        
        Assert.NotNull(rr);
    }
}
