using DotNetCore.Packages.Application.Common.Services.Auth;
using DotNetCore.Packages.Application.Common.Services.Cache;
using DotNetCore.Packages.Infrastructure.Services.Auth;
using DotNetCore.Packages.Infrastructure.Services.Cache;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.Packages.Infrastructure;

public static class InfraServiceRegistiration
{
    public static IServiceCollection AddAInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuhtenticationService, AuthenticationService>();
        services.AddRedisSettingsService(configuration);
        return services;
    }

    private static IServiceCollection AddRedisSettingsService(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection("Redis");

        var connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
                               ?? redisConfig["ConnectionString"];

        var instanceName = redisConfig["InstanceName"];
        var useSsl = redisConfig.GetValue<bool>("UseSsl", false);
        var password = redisConfig["Password"];
        
        if (useSsl)
        {
            connectionString += ",ssl=True,abortConnect=False";
        }

        services.AddSingleton<IRedisCacheService>(new RedisCacheService(connectionString, instanceName));
        services.AddSingleton<IRedisLockService>(new RedisLockService(connectionString));

        return services;
        
    }
}