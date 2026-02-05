using ePatient.Application.Common;
using ePatient.Domain.Patients;
using ePatient.Infrastructure.Persistence;
using NetArchTest.Rules;
using System.Reflection;

namespace ePatient.Tests.Architecture;

/// <summary>
/// Architecture tests to enforce the Clean Architecture principles in the project structure.
/// </summary>
public class ArchitectureTests
{
    private const string DomainNamespace = "PAT.Domain";
    private const string ApplicationNamespace = "PAT.Application";
    private const string InfrastructureNamespace = "PAT.Infrastructure";
    private const string ApiNamespace = "PAT.Api";

    private static readonly Assembly DomainAssembly = typeof(Patient).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(IUnitOfWork).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;

    /// <summary>
    /// Verifies the domain dependencies.
    /// </summary>
    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange & Act
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationNamespace)
            .And()
            .NotHaveDependencyOn(InfrastructureNamespace)
            .And()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, 
            $"Domain layer should not depend on other layers. Failures: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    /// <summary>
    /// Verifies the application only depends on domain.
    /// </summary>
    [Fact]
    public void Application_Should_Only_DependOn_Domain()
    {
        // Arrange & Act
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureNamespace)
            .And()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful,
            $"Application layer should only depend on Domain. Failures: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    /// <summary>
    /// Verifies infrastructure does not depend on API.
    /// </summary>
    [Fact]
    public void Infrastructure_Should_Not_DependOn_Api()
    {
        // Arrange & Act
        var result = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful,
            $"Infrastructure layer should not depend on API/Presentation layer. Failures: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    /// <summary>
    /// Verifies the domain does not contain repository implementations, only interfaces.
    /// </summary>
    [Fact]
    public void Domain_Should_Not_ContainRepositoryImplementations()
    {
        // Arrange & Act
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("Repository")
            .Should()
            .BeInterfaces()
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful,
            $"Domain should only contain repository interfaces, not implementations. Failures: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    /// <summary>
    /// Verifies infrastructure contains repositorry implementations and not interfaces.
    /// </summary>
    [Fact]
    public void Infrastructure_Should_ContainRepositoryImplementations()
    {
        // Arrange & Act
        var result = Types.InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("Repository")
            .And()
            .DoNotHaveNameEndingWith("IRepository")
            .Should()
            .NotBeInterfaces()
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful,
            $"Infrastructure should contain repository implementations (concrete classes). Failures: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    /// <summary>
    /// Verifies that all interfaces defined in the domain assembly have at least one implementation in either the
    /// infrastructure or application assemblies.
    /// </summary>
    [Fact]
    public void Domain_Interfaces_Should_HaveImplementationsInInfrastructureOrApplication()
    {
        // Arrange
        var domainInterfaces = Types.InAssembly(DomainAssembly)
            .That()
            .AreInterfaces()
            .GetTypes()
            .ToList();

        // Act
        var interfacesWithoutImplementations = new List<string>();

        foreach (var domainInterface in domainInterfaces)
        {
            var hasImplementation = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ImplementInterface(domainInterface)
                .GetTypes()
                .Any();

            if (!hasImplementation)
            {
                hasImplementation = Types.InAssembly(ApplicationAssembly)
                    .That()
                    .ImplementInterface(domainInterface)
                    .GetTypes()
                    .Any();
            }

            if (!hasImplementation)
            {
                interfacesWithoutImplementations.Add(domainInterface.Name);
            }
        }

        // Assert
        Assert.Empty(interfacesWithoutImplementations);
    }
}
