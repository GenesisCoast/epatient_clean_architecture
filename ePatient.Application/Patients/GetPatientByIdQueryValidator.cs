using FluentValidation;

namespace ePatient.Application.Patients;

/// <summary>
/// Validation handler for <see cref="GetPatientByIdQuery"/>.
/// </summary>
public class GetPatientByIdQueryValidator : AbstractValidator<GetPatientByIdQuery>
{
    /// <inheritdoc/>
    public GetPatientByIdQueryValidator()
    {
        RuleFor(r => r.PatientId)
            .NotNull()
            .NotEmpty()
            .Must(id => int.TryParse(id, out int val) && val > 0).WithMessage("{PropertyName} must be a valid positive number.");
    }
}
