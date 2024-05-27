using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSA.Common.Contracts.Domain;
using MSA.Common.Contracts.Settings;

namespace MSA.Common.PostgresMassTransit.PostgresDB;

public static class Extensions
{
    public static IServiceCollection AddPostgres<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        var srvProvider = services.BuildServiceProvider();
        var config = srvProvider.GetService<IConfiguration>();
        var pgSettings = config.GetSection(nameof(PostgresDBSetting)).Get<PostgresDBSetting>();

        services
            .AddEntityFrameworkNpgsql()
            .AddDbContext<TDbContext>(opt => {
                opt.UseNpgsql(pgSettings.ConnectionString, pgOpt => {
                    pgOpt.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name);
                });
            });

        return services;
    }

    public static IServiceCollection AddPostgresRepositories<TDbContext, TEntity>(
        this IServiceCollection services) 
        where TDbContext : AppDbContextBase
        where TEntity : class, IEntity 
    {
        // services.AddSingleton<IRepository<TEntity>>(srvProvider => {
        //     var dbContext = srvProvider.GetRequiredService<TDbContext>();
        //     return new PostgresRepository<TDbContext, TEntity>(dbContext);
        // });
        services.AddScoped<IRepository<TEntity>, PostgresRepository<TDbContext,TEntity>>();
        return services;
    }

        public static IServiceCollection AddPostgresUnitofWork<TDbContext>(
        this IServiceCollection services) 
        where TDbContext : AppDbContextBase
    {
        services.AddScoped<PostgresUnitOfWork<TDbContext>, PostgresUnitOfWork<TDbContext>>();
        return services;
    }
}