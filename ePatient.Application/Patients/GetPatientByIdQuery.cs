using Ardalis.Result;
using ePatient.Application.Common;
using ePatient.Application.Specifications;
using ePatient.Domain.Patients;
using MediatR;

namespace ePatient.Application.Patients;

/// <summary>
/// Request to get a patient by an ID.
/// </summary>
/// <param name="PatientId">The patient ID.</param>
public record GetPatientByIdQuery(string? PatientId) : IRequest<Result<Patient>>;

/// <summary>
/// Handler for <see cref="GetPatientByIdQuery"/>.
/// </summary>
public class GetPatientByIdHandler(IUnitOfWork UnitOfWork) : IRequestHandler<GetPatientByIdQuery, Result<Patient>>
{
    /// <inheritdoc/>
    public async Task<Result<Patient>> Handle(
        GetPatientByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(UnitOfWork);

        var repository = UnitOfWork.Repository<Patient>();
        var specification = new GetPatientByIdSpecification(int.Parse(query.PatientId!));

        var result = await repository.FirstOrDefaultAsync(specification, cancellationToken);

        return result is not null
            ? Result<Patient>.Success(result)
            : Result<Patient>.NotFound();
    }
}