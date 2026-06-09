using Cortex.Mediator.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddCortexMediator([typeof(ApplicationService)],
            opt => opt.AddDefaultBehaviors());
        
        return services;
    }
}