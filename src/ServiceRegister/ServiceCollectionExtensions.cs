using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RightRule.Models;

namespace RightRule.ServiceRegister;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRightRule(this IServiceCollection services) =>
    AddRightRule(services, new RightRuleOptions());

    public static IServiceCollection AddRightRule
        (
        this IServiceCollection services,
        RightRuleOptions options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);

        return AddRightRuleServices(services, options);
    }

    public static IServiceCollection AddRightRule
        (
        this IServiceCollection services,
        Action<RightRuleOptions> options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);

        var rightRuleOptions = new RightRuleOptions();
        options?.Invoke(rightRuleOptions);

        return AddRightRuleServices(services, rightRuleOptions);
    }

    private static IServiceCollection AddRightRuleServices(IServiceCollection services, RightRuleOptions options)
    {
        // Make sure the Path to Custom Operators has a value
        if (string.IsNullOrWhiteSpace(options.CustomOperatorLibraryPath))
        {
            options.CustomOperatorLibraryPath =
                RightRuleLibraryDefaults.CustomOperatorLibraryPath;
        }
        services.AddSingleton(options);

        if (services is ServiceCollection sc)
        {
            var servicesCollection = ServiceRegister.RegisterServices(sc);
            return servicesCollection;
        }
        var temp = new ServiceCollection();
        ServiceRegister.RegisterServices(temp);
        foreach (var sd in temp.Except(services))
        {
            services.Add(sd);
        }
        return services;
    }
}
