using Ardalis.Specification.EntityFrameworkCore;
using ePatient.Application.Specifications;
using ePatient.Infrastructure.Persistence;
using ePatient.Infrastructure.Persistence.Patients;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ePatient.Application.Tests.Specifications;

/// <summary>
/// Unit tests for <see cref="GetPatientByIdSpecification"/>.
/// </summary>
public class GetPatientByIdSpecificationTests : IDisposable
{
    private readonly ApplicationDbContext context;
    private readonly PatientGenerator faker = new();

    public GetPatientByIdSpecificationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        context = new ApplicationDbContext(options);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdSpecification"/> returns the correct patient when ID matches.
    /// </summary>
    [Fact]
    public async Task GetPatientByIdSpecification_Should_ReturnPatient_WhenIdExists()
    {
        // Arrange
        var patient = faker.Generate();
        context.Patients.Add(patient);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var specification = new GetPatientByIdSpecification(patient.Id);
        var evaluator = SpecificationEvaluator.Default;

        // Act
        var query = evaluator.GetQuery(context.Patients, specification);
        var result = await query.FirstOrDefaultAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(patient.Id, result.Id);
        Assert.Equal(patient.FirstName, result.FirstName);
        Assert.Equal(patient.LastName, result.LastName);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdSpecification"/> returns null when ID does not exist.
    /// </summary>
    [Fact]
    public async Task GetPatientByIdSpecification_Should_ReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var nonExistentId = 999;
        var specification = new GetPatientByIdSpecification(nonExistentId);
        var evaluator = SpecificationEvaluator.Default;

        // Act
        var query = evaluator.GetQuery(context.Patients, specification);
        var result = await query.FirstOrDefaultAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdSpecification"/> filters correctly with multiple patients in database.
    /// </summary>
    [Fact]
    public async Task GetPatientByIdSpecification_Should_ReturnCorrectPatient_WithMultiplePatientsInDatabase()
    {
        // Arrange
        var patient1 = faker.Generate();
        var patient2 = faker.Generate();
        var patient3 = faker.Generate();

        context.Patients.AddRange(patient1, patient2, patient3);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var specification = new GetPatientByIdSpecification(patient2.Id);
        var evaluator = SpecificationEvaluator.Default;

        // Act
        var query = evaluator.GetQuery(context.Patients, specification);
        var result = await query.FirstOrDefaultAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(patient2.Id, result.Id);
        Assert.Equal(patient2.FirstName, result.FirstName);
        Assert.Equal(patient2.LastName, result.LastName);
        Assert.Equal(patient2.MedicalRecordNumber, result.MedicalRecordNumber);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdSpecification"/> uses AsNoTracking.
    /// </summary>
    [Fact]
    public async Task GetPatientByIdSpecification_Should_UseAsNoTracking()
    {
        // Arrange
        var patient = faker.Generate();
        context.Patients.Add(patient);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var specification = new GetPatientByIdSpecification(patient.Id);
        var evaluator = SpecificationEvaluator.Default;

        // Act
        var query = evaluator.GetQuery(context.Patients, specification);
        var result = await query.FirstOrDefaultAsync(TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(result);
        var entry = context.Entry(result);
        Assert.Equal(EntityState.Detached, entry.State);
    }

    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Its only a test.")]
    public void Dispose()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
