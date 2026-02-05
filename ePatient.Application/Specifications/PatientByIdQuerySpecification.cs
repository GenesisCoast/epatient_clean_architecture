using Ardalis.Specification;
using ePatient.Domain.Patients;

namespace ePatient.Application.Specifications;

/// <summary>
/// Specification to get a patient by an ID.
/// </summary>
public class GetPatientByIdSpecification : Specification<Patient>
{
    /// <inheritdoc/>
    public GetPatientByIdSpecification(int patientId)
    {
        Query.Where(p => p.Id == patientId).AsNoTracking();
    }
}