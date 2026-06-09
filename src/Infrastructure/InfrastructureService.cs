using Application.Contracts.Infrastructure;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureService
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        
        return services;
    }
}