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
public record GetPatientByIdQuery(string? PatientId) : IRequest<Result<GetPatientByIdQueryResponse>>;

/// <summary>
/// Request to get a patient by an ID.
/// </summary>
/// <param name="PatientId">The patient ID.</param>
public record GetPatientByIdQueryResponse(
    int? Id,
    string? FirstName,
    string? LastName,
    DateTime? DateOfBirth,
    string? MedicalRecordNumber,
    string? Email = null,
    string? PhoneNumber = null
);

/// <summary>
/// Handler for <see cref="GetPatientByIdQuery"/>.
/// </summary>
public class GetPatientByIdHandler(IUnitOfWork UnitOfWork) : IRequestHandler<GetPatientByIdQuery, Result<GetPatientByIdQueryResponse>>
{
    /// <inheritdoc/>
    public async Task<Result<GetPatientByIdQueryResponse>> Handle(
        GetPatientByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(UnitOfWork);

        var repository = UnitOfWork.Repository<Patient>();
        var specification = new GetPatientByIdSpecification(int.Parse(query.PatientId!));

        var result = await repository.FirstOrDefaultAsync(specification, cancellationToken);

        return result is null
            ? Result<GetPatientByIdQueryResponse>.NotFound()
            : Result<GetPatientByIdQueryResponse>.Success(new GetPatientByIdQueryResponse(
                result.Id,
                result.FirstName,
                result.LastName,
                result.DateOfBirth,
                result.MedicalRecordNumber,
                result.Email,
                result.PhoneNumber
            ));
    }
}