using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ePatient.Application.Common;

/// <summary>
/// Configuration for MediatR services.
/// </summary>
public static class ConfigureMediatR
{
    /// <summary>
    /// Adds MediatR services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<IUnitOfWork>();
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });
        
        return services;
    }
}
