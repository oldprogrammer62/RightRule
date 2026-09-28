using BeSoft;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace RightRule.ServiceRegister;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRightRule(this IServiceCollection services)
    {
        // Register your library's components
        services.AddTransient<BeSoft.RightRule>();

        return services;
    }
}
