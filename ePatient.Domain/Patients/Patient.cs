using System.ComponentModel.DataAnnotations.Schema;

namespace ePatient.Domain.Patients;

/// <summary>
/// The patient entity.
/// </summary>
public class Patient
{
    /// <summary>
    /// Gets the unique identifier for the patient.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    /// <summary>
    /// Gets the first name of the person.
    /// </summary>
    public string FirstName { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets the last name of the person.
    /// </summary>
    public string LastName { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets the Date of Birth of the person.
    /// </summary>
    public DateTime DateOfBirth { get; private set; }
    
    /// <summary>
    /// Gets the unique medical record number assigned to the patient.
    /// </summary>
    public string MedicalRecordNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the optional email address of the patient.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Gets the optional phone number of the patient.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Creates an instance of the patient.
    /// </summary>
    /// <param name="firstName">The first name of the patient.</param>
    /// <param name="lastName">The last name of the patient.</param>
    /// <param name="dateOfBirth">The date of birth of the patient.</param>
    /// <param name="medicalRecordNumber">The unique medical record number assigned to the patient.</param>
    /// <param name="email">The optional email address of the patient.</param>
    /// <param name="phoneNumber">The optional phone number of the patient.</param>
    /// <returns>The patient.</returns>
    public static Patient Create(
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string medicalRecordNumber,
        string? email = null,
        string? phoneNumber = null
    ) => new()
    {
        FirstName = firstName,
        LastName = lastName,
        DateOfBirth = dateOfBirth,
        MedicalRecordNumber = medicalRecordNumber,
        Email = email,
        PhoneNumber = phoneNumber
    };
}
