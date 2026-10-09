using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RightRule.ServiceRegister;
using RightRule.System.Common.DataProvider;
using RightRuleLogger;

namespace RightRuleTests;

public class DataProviderTests
{
    [Fact]
    public void RegisterDataProvider_Dictionary()
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

        var input = new Dictionary<string, object>
        {
            { "Id", "123" },
            { "Name", "Test" },
            { "Value", 42 }
        };
        var result = rr.RegisterData<Dictionary<string, object>>("DS_Name", input);

        Assert.NotNull(result);
    }

    [Fact]
    public void RegisterDataProvider_String()
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

        string input = "HelloWorld";
        var result = rr.RegisterData<string>("DS_Name", input);

        Assert.NotNull(result);
    }

    [Fact]
    public void RegisterDataProvider_Int()
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

        int input = 1023;
        var result = rr.RegisterData("DS_Name", input);

        Assert.NotNull(result);
    }

    [Fact]
    public void RegisterDataProvider_Bespoke()
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

        var dataProvider = new BespokeDataProvider();
        var dataProviderResult = rr.RegisterDataProvider(dataProvider);

        var input = new BespokeClass();
        var result = rr.RegisterData<BespokeClass>("DS_Name", input);

        Assert.NotNull(result);
    }

    [Fact]
    public void DictionaryDataProvider()
    {
        var input = new Dictionary<string, object>
        {
            { "Id", "123" },
            { "Name", "Test" },
            { "Value", 42 }
        };

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

        rr.RegisterData<Dictionary<string, object>>("DS_Name", input);
    }

    [Fact]
    public void BespokeClassDataProvider()
    {
        var input = new BespokeClass();
        input.Id = 123;
        input.Name = "Test";
        input.Value = 42;

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

        var DataProviderRegResult = rr.RegisterDataProvider(new BespokeDataProvider());

        var result = rr.RegisterData("DS_Name", input);
    }
}

internal class BespokeDataProvider : DataProviderBase<BespokeClass>
{
    public override ProviderDataStructure Process(string DS_Name, BespokeClass input)
    {
        var result = new ProviderDataStructure();
        result.DatasetName = DS_Name;
        result.Data.Add("Id", input.Id);
        result.Data.Add("Name", input.Name);
        result.Data.Add("Value", input.Value);
        return result;
    }
}

public class BespokeClass()
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
}
