using ePatient.Api.Endpoints;

namespace ePatient.Api.Common;

/// <summary>
/// Configuration for endpoint registration.
/// </summary>
public static class ConfigureEndpoints
{
    /// <summary>
    /// Adds endpoints to the service collection using Scrutor.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<IEndpoints>()
            .AddClasses(classes => classes.AssignableTo<IEndpoints>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        return services;
    }

    /// <summary>
    /// Maps all registered endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<IEndpoints>();

        foreach (var endpoint in endpoints)
        {
            endpoint.DefineEndpoints(app);
        }

        return app;
    }
}

