using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using RightRule;
using RightRule.ServiceRegister;
using RightRuleLogger;

namespace RightRuleTests;

public class LoggerTests
{
    [Fact]
    public void LogTest()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddRightRule();
        var serviceProvider = services.BuildServiceProvider();
        var rr = serviceProvider.GetRequiredService<RightRule.IRightRule>();
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
        var rr = serviceProvider.GetRequiredService<RightRule.IRightRule>();
        rr.RegisterData("a", 100);
        Assert.NotNull(rr);
    }

    [Fact]
    public void LogTest_RightRuleLogger()
    {
        var services = new ServiceCollection();
        
        services.AddLogging(builder => 
            builder.AddRightRuleLogger("logs\\wednesday\\logs2.txt"));
        
        services.AddRightRule();
        
        var serviceProvider = services.BuildServiceProvider();
        var rr = serviceProvider.GetRequiredService<RightRule.IRightRule>();
        
        rr.RegisterData("a", 100);
        
        Assert.NotNull(rr);
    }
}
