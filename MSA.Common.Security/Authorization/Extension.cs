using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace MSA.Common.Security.Authorization;

public static class Extensions 
{
    public static IServiceCollection AddMSAAuthorization(
        this IServiceCollection services,
        Action<AuthorizationOptions> configure)
    {
        services.AddAuthorization(configure);

        return services;
    }
}