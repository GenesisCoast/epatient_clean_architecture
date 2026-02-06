using Microsoft.OpenApi;

namespace ePatient.Api.Common;

/// <summary>
/// Configuration for OpenAPI/Swagger.
/// </summary>
public static class ConfigureOpenApi
{
    /// <summary>
    /// Adds OpenAPI services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "ePatient API",
                    Version = "v1",
                    Description = "API for managing patient records",
                    Contact = new OpenApiContact
                    {
                        Name = "ePatient Team",
                        Email = "support@epatient.com"
                    }
                };
                
                return Task.CompletedTask;
            });
        });

        return services;
    }

    /// <summary>
    /// Configures OpenAPI/Swagger middleware.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication UseOpenApiConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "ePatient API v1");
                options.RoutePrefix = "swagger";
                options.DocumentTitle = "ePatient API Documentation";
                options.DisplayRequestDuration();
                options.EnableDeepLinking();
                options.EnableFilter();
                options.EnableTryItOutByDefault();
            });
        }

        return app;
    }
}
