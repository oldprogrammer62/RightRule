using Microsoft.Extensions.DependencyInjection;
using RightRule.Models;

namespace RightRule.ServiceRegister;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRightRule(this IServiceCollection services,
        Action<RightRuleOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = new RightRuleOptions();
        configure?.Invoke(options);

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
