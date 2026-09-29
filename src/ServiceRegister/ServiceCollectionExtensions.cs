using Microsoft.Extensions.DependencyInjection;

namespace RightRule.ServiceRegister;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRightRule(this ServiceCollection services)
    {
        var servicesCollection = ServiceRegister.RegisterServices(services);
        return servicesCollection;
    }
}
