using ePatient.Application.Patients;
using FluentValidation.TestHelper;

namespace ePatient.Application.Tests.Patients;

/// <summary>
/// Unit tests for <see cref="GetPatientByIdQueryValidator"/>.
/// </summary>
public class GetPatientByIdQueryValidatorTests
{
    private readonly GetPatientByIdQueryValidator validator = new();

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdQueryValidator"/> passes validation when patient ID is a valid positive integer.
    /// </summary>
    [Theory]
    [InlineData("1")]
    [InlineData("10")]
    [InlineData("100")]
    [InlineData("999999")]
    public async Task Validate_Should_Pass_WhenPatientIdIsValidPositiveNumber(string patientId)
    {
        // Arrange
        var query = new GetPatientByIdQuery(patientId);

        // Act
        var result = await validator.TestValidateAsync(query, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdQueryValidator"/> passes validation when patient ID is zero.
    /// </summary>
    [Fact]
    public async Task Validate_Should_Fail_WhenPatientIdIsZero()
    {
        // Arrange
        var query = new GetPatientByIdQuery("0");

        // Act
        var result = await validator.TestValidateAsync(query, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PatientId);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdQueryValidator"/> fails validation when patient ID is null.
    /// </summary>
    [Fact]
    public async Task Validate_Should_Fail_WhenPatientIdIsNull()
    {
        // Arrange
        var query = new GetPatientByIdQuery(null);

        // Act
        var result = await validator.TestValidateAsync(query, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PatientId);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdQueryValidator"/> fails validation when patient ID is empty.
    /// </summary>
    [Fact]
    public async Task Validate_Should_Fail_WhenPatientIdIsEmpty()
    {
        // Arrange
        var query = new GetPatientByIdQuery(string.Empty);

        // Act
        var result = await validator.TestValidateAsync(query, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PatientId);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdQueryValidator"/> fails validation when patient ID is whitespace.
    /// </summary>
    [Theory]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public async Task Validate_Should_Fail_WhenPatientIdIsWhitespace(string patientId)
    {
        // Arrange
        var query = new GetPatientByIdQuery(patientId);

        // Act
        var result = await validator.TestValidateAsync(query, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PatientId);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdQueryValidator"/> fails validation when patient ID is not a number.
    /// </summary>
    [Theory]
    [InlineData("abc")]
    [InlineData("12abc")]
    [InlineData("abc12")]
    [InlineData("12.34")]
    [InlineData("1e5")]
    public async Task Validate_Should_Fail_WhenPatientIdIsNotANumber(string patientId)
    {
        // Arrange
        var query = new GetPatientByIdQuery(patientId);

        // Act
        var result = await validator.TestValidateAsync(query, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PatientId)
            .WithErrorMessage("Patient Id must be a valid positive number.");
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdQueryValidator"/> fails validation when patient ID is negative.
    /// </summary>
    [Theory]
    [InlineData("-1")]
    [InlineData("-100")]
    [InlineData("-999")]
    public async Task Validate_Should_Fail_WhenPatientIdIsNegative(string patientId)
    {
        // Arrange
        var query = new GetPatientByIdQuery(patientId);

        // Act
        var result = await validator.TestValidateAsync(query, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.PatientId)
            .WithErrorMessage("Patient Id must be a valid positive number.");
    }
}
