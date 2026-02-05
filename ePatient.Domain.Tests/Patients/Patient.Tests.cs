using Bogus;
using ePatient.Domain.Patients;
using FsCheck;
using FsCheck.Xunit;
using FsCheck.Fluent;
using ePatient.Infrastructure.Persistence.Patients;

namespace ePatient.Domain.Tests.Patients;

/// <summary>
/// Property-based tests for <see cref="Patient"/>.
/// </summary>
public class PatientTests : BaseEntityTests<Patient, PatientGenerator>
{
    /// <summary>
    /// Verifies that <see cref="Patient.Create"/> correctly initializes all properties with provided values.
    /// </summary>
    [Property]
    public Property Create_ShouldInitializeAllProperties_WhenProvided(
        NonEmptyString firstName,
        NonEmptyString lastName,
        DateTime dateOfBirth,
        NonEmptyString mrn,
        string? email,
        string? phone
    )
    {
        var patient = Patient.Create(
            firstName.Get,
            lastName.Get,
            dateOfBirth,
            mrn.Get,
            email,
            phone
        );

        return (patient.FirstName == firstName.Get)
            .And(() => patient.LastName == lastName.Get)
            .And(() => patient.DateOfBirth == dateOfBirth)
            .And(() => patient.MedicalRecordNumber == mrn.Get)
            .And(() => patient.Email == email)
            .And(() => patient.PhoneNumber == phone)
            .And(() => patient.Id == 0)
            .Label($"{nameof(Patient.Create)} should set all properties correctly");
    }

    /// <summary>
    /// Verifies that <see cref="Patient.FirstName"/> can be set and retrieved correctly.
    /// </summary>
    [Property]  
    public Property FirstName_Should_GetSet(NonEmptyString firstName)
    {
        var patient = SetProperty(nameof(Patient.FirstName), firstName.Get);

        return (patient.FirstName == firstName.Get).ToProperty();
    }

    /// <summary>
    /// Verifies that <see cref="Patient.LastName"/> can be set and retrieved correctly.
    /// </summary>
    [Property]
    public Property LastName_Should_GetSet(NonEmptyString lastName)
    {
        var patient = SetProperty(nameof(Patient.LastName), lastName.Get);

        return (patient.LastName == lastName.Get).ToProperty();
    }

    /// <summary>
    /// Verifies that <see cref="Patient.DateOfBirth"/> can be set and retrieved correctly.
    /// </summary>
    [Property]
    public Property DateOfBirth_Should_GetSet(DateTime dateOfBirth)
    {
        var patient = SetProperty(nameof(Patient.DateOfBirth), dateOfBirth);

        return (patient.DateOfBirth == dateOfBirth).ToProperty();
    }

    /// <summary>
    /// Verifies that <see cref="Patient.MedicalRecordNumber"/> can be set and retrieved correctly.
    /// </summary>
    [Property]
    public Property MedicalRecordNumber_Should_GetSet(NonEmptyString mrn)
    {
        var patient = SetProperty(nameof(Patient.MedicalRecordNumber), mrn.Get);

        return (patient.MedicalRecordNumber == mrn.Get).ToProperty();
    }

    /// <summary>
    /// Verifies that <see cref="Patient.Email"/> can be set and retrieved correctly.
    /// </summary>
    [Property]
    public Property Email_Should_GetSet(string? email)
    {
        var patient = SetProperty(nameof(Patient.Email), email);

        return (patient.Email == email).ToProperty();
    }

    /// <summary>
    /// Verifies that <see cref="Patient.PhoneNumber"/> can be set and retrieved correctly.
    /// </summary>
    [Property]
    public Property PhoneNumber_Should_GetSet(string? phone)
    {
        var patient = SetProperty(nameof(Patient.PhoneNumber), phone);

        return (patient.PhoneNumber == phone).ToProperty();
    }

    /// <summary>
    /// Verifies that <see cref="Patient.Create"/> always returns a non-null <see cref="Patient"/> instance.
    /// </summary>
    [Property]
    public Property Create_Should_ReturnNonNull()
    {
        var patient = Faker.Generate();

        return (patient != null).ToProperty();
    }

    /// <summary>
    /// Verifies that <see cref="Patient.Id"/> is zero when <see cref="Patient"/> is newly created via <see cref="Patient.Create"/>.
    /// </summary>
    [Property]
    public Property Id_Should_BeZero_WhenNewlyCreated()
    {
        var patient = Faker.Generate();

        return (patient.Id == 0).ToProperty();
    }
}
