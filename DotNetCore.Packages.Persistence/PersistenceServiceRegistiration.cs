using DotNetCore.Packages.Domain.Repositories;
using DotNetCore.Packages.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Packages.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DotNetCore.Packages.Persistence;

public static class PersistenceServiceRegistiration
{
    public static IServiceCollection AddPersistenceServices<TContext>
        (this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<Domain.UnitOfWork.IUnitOfWork, Persistence.UnitOfWork.UnitOfWork>();
        
        services.AddDbContext<TContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        return services;
    }
}