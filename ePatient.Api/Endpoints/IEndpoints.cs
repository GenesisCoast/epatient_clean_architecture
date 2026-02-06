namespace ePatient.Api.Endpoints;

/// <summary>
/// Interface for dynamically mapping endpoints.
/// </summary>
public interface IEndpoints
{
    /// <summary>
    /// Define the routes in the application.
    /// </summary>
    /// <param name="app">Application to define routes in.</param>
    public void DefineEndpoints(IEndpointRouteBuilder app);
}
