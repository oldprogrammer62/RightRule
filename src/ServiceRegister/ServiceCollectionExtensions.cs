using Microsoft.Extensions.DependencyInjection;

namespace RightRule.ServiceRegister;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRightRule(this IServiceCollection services)
    {
        // Register your library's components
        services.AddScoped<IRightRule, RightRule>();

        return services;
    }
}
