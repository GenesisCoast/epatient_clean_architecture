using Bogus;
using ePatient.Domain.Patients;

namespace ePatient.Infrastructure.Persistence.Patients;

/// <summary>
/// Generates fake <see cref="Patient"/>(s).
/// </summary>
public class PatientGenerator : Faker<Patient>
{
    /// <inheritdoc/>
    public PatientGenerator()
    {
        CustomInstantiator(faker => Patient.Create(
            faker.Person.FirstName,
            faker.Person.LastName,
            faker.Person.DateOfBirth,
            medicalRecordNumber: faker.Random.AlphaNumeric(10),
            faker.Person.Email,
            faker.Person.Phone
        ));
    }
}
