using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSA.Common.Contracts.Settings;

namespace MSA.Common.Security.Authentication;

public static class Extensions 
{
    public static IServiceCollection AddMSAAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var srvProvider = services.BuildServiceProvider();
                var config = srvProvider.GetService<IConfiguration>();
                var srvUrlsSetting = config.GetSection(nameof(ServiceUrlsSetting)).Get<ServiceUrlsSetting>();

                options.Authority = srvUrlsSetting.IdentityServiceUrl;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuers = new List<string>
                    {
                        "https://identity-api:5001",
                        "https://localhost:5001",
                        "https://localhost:8080"
                    },
                    ValidAudiences = new List<string>
                    {
                        "https://identity-api:5001/resources",
                        "https://localhost:5001/resources",
                        "productapi",
                        "orderapi"
                    }
                };
            });
        return services;
    }
}