using System.Reflection.Metadata;
using DotNetCore.Packages.Application.Common.Services.User;
using DotNetCore.Packages.Application.Features.Auth.Register;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.Packages.Application;

public static class ApplicationServiceRegistiration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(RegisterCommand).Assembly);
        services.AddTransient<IMediator, Mediator>();

        services.AddTransient<IUserService, UserService>();
        
        return services;
    } 
}