using ePatient.Application.Patients;
using MediatR;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace ePatient.Api.Endpoints;

/// <summary>
/// Endpoints for patients.
/// </summary>
public class PatientEndpoints : IEndpoints
{
    /// <sinheritdoc/>
    public void DefineEndpoints(IEndpointRouteBuilder app)
    {
        var patients = app.MapGroup("v1").MapGroup("patients");

        MapGetPatientById(patients);
    }

    /// <summary>
    /// Maps the get patient by ID endpoint.
    /// </summary>
    /// <param name="group">The routing group.</param>
    public static void MapGetPatientById(IEndpointRouteBuilder group)
    {
        group
            .MapGet("{id}", async (IMediator mediator, string id) =>
            {
                var result = await mediator.Send(new GetPatientByIdQuery(id));

                return result.ToMinimalApiResult();
            })
            .Produces<GetPatientByIdQueryResponse>(200)
            .Produces<ProblemDetails>(400)
            .Produces<ProblemDetails>(404);
    }
}