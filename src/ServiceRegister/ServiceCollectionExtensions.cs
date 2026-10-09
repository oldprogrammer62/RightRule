using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RightRule.ServiceRegister;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRightRule(this IServiceCollection services)
    {
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
