using Ardalis.Result;
using Ardalis.Specification;
using ePatient.Application.Common;
using ePatient.Application.Patients;
using ePatient.Application.Specifications;
using ePatient.Domain.Patients;
using ePatient.Infrastructure.Persistence.Patients;
using NSubstitute;

namespace ePatient.Application.Tests.Patients;

/// <summary>
/// Unit tests for <see cref="GetPatientByIdHandler"/>.
/// </summary>
public class GetPatientByIdHandlerTests
{
    private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IRepositoryBase<Patient> repository = Substitute.For<IRepositoryBase<Patient>>();
    private readonly GetPatientByIdHandler handler;
    private readonly PatientGenerator faker = new();

    public GetPatientByIdHandlerTests()
    {
        handler = new GetPatientByIdHandler(unitOfWork);
        unitOfWork.Repository<Patient>().Returns(repository);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdHandler.Handle"/> returns success with patient when patient exists.
    /// </summary>
    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenPatientExists()
    {
        // Arrange
        var query = new GetPatientByIdQuery("1");
        var expectedPatient = faker.Generate();

        repository.FirstOrDefaultAsync(
            Arg.Any<GetPatientByIdSpecification>(),
            Arg.Any<CancellationToken>()
        ).Returns(expectedPatient);

        // Act
        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedPatient, result.Value);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdHandler.Handle"/> returns NotFound when patient does not exist.
    /// </summary>
    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenPatientDoesNotExist()
    {
        // Arrange
        var query = new GetPatientByIdQuery("999");

        repository.FirstOrDefaultAsync(
            Arg.Any<GetPatientByIdSpecification>(),
            Arg.Any<CancellationToken>()
        ).Returns((Patient?)null);

        // Act
        var result = await handler.Handle(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdHandler.Handle"/> throws ArgumentNullException when query is null.
    /// </summary>
    [Fact]
    public async Task Handle_Should_ThrowArgumentNullException_WhenQueryIsNull()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            handler.Handle(null!, TestContext.Current.CancellationToken));
    }

    /// <summary>
    /// Verifies that <see cref="GetPatientByIdHandler"/> throws ArgumentNullException when UnitOfWork is null.
    /// </summary>
    [Fact]
    public async Task Constructor_Should_ThrowArgumentNullException_WhenUnitOfWorkIsNull()
    {
        // Arrange &
        var handler = new GetPatientByIdHandler(null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await handler.Handle(new GetPatientByIdQuery("test"), TestContext.Current.CancellationToken));

        Assert.Contains("UnitOfWork", exception.Message);
    }
}
