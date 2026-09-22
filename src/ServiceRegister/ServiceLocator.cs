using Microsoft.Extensions.DependencyInjection;

namespace RightRule.ServiceRegister;

public static class ServiceLocator
{
    private static IServiceProvider? _provider;

    public static void SetProvider(IServiceProvider provider) => _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    public static IServiceProvider Provider => _provider ?? throw new InvalidOperationException("ServiceProvider not configured.");

    public static T? GetRequiredService<T>()
    {
        if (_provider == null)
        {
            return default;
        }
        try
        {
            return _provider.GetRequiredService<T>();
        }
        catch
        {
            return default;
        }
    }
}
