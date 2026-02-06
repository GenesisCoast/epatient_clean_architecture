using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ePatient.Application.Common;

/// <summary>
/// Configuration for FluentValidation services.
/// </summary>
public static class ConfigureValidation
{
    /// <summary>
    /// Adds FluentValidation services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssemblyContaining<IUnitOfWork>(includeInternalTypes: true);
    }
}
