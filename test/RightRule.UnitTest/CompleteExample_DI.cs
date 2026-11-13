using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RightRule.Models;
using RightRule.ServiceRegister;
using RightRuleLogger;

namespace RightRuleTests;

public class CompleteExample_DI
{
    [Fact]
    public void LogTest_DI_WithCustomLogger()
    {
        var services = new ServiceCollection();

        // Create and register the appsettings details
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        services.AddSingleton<IConfiguration>(configuration);

        // Configure logging to use Custom RightRuleLogger
        services.AddLogging(builder =>
        {
            builder.AddRightRuleLogger();
        });

        RightRuleOptions options = new RightRuleOptions
        {
            DataCheck = false,
            Diagnostics = true,
            LoadDefaultOperators = true,
            LoadCustomOperators = true,
            // CustomOperatorLibraryPath = "Hello/world" // Use default
        };

        // Register RightRule with specified options
        services.AddRightRule(options);

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var rr = scope.ServiceProvider.GetRequiredService<RightRule.IRightRule>();
        Assert.NotNull(rr);

        var dataResult = rr.DataService.RegisterData("a", 100);
        Assert.NotNull(dataResult);

        var loadedOptions = rr.GetRightRuleOptions();
        Assert.Same(options, loadedOptions);

        Assert.Equal(options.DataCheck, loadedOptions.DataCheck);

        Assert.Equal(options.Diagnostics, loadedOptions.Diagnostics);

        Assert.Equal(options.LoadDefaultOperators, loadedOptions.LoadDefaultOperators);

        Assert.Equal(options.LoadCustomOperators, loadedOptions.LoadCustomOperators);
    }

    [Fact]
    public void LogTest_DI_NoLogger()
    {
        var services = new ServiceCollection();

        // Create and register the appsettings details
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        services.AddSingleton<IConfiguration>(configuration);

        // Register RightRule with specified options
        services.AddRightRule(options =>
        {
            options.DataCheck = false;
            options.Diagnostics = true;
            options.LoadDefaultOperators = true;
            options.LoadCustomOperators = true;
            // options.CustomOperatorLibraryPath = "Hello/world"; // Use default
        });

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var rr = scope.ServiceProvider.GetRequiredService<RightRule.IRightRule>();

        rr.DataService.RegisterData("a", 100);
        Assert.NotNull(rr);

        var loadedOptions = rr.GetRightRuleOptions();
        Assert.False(loadedOptions.DataCheck);
        Assert.True(loadedOptions.Diagnostics);
        Assert.True(loadedOptions.LoadDefaultOperators);
        Assert.True(loadedOptions.LoadCustomOperators);
    }
}
