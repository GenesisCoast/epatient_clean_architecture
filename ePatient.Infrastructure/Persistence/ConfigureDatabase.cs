using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ePatient.Application.Common;
using Bogus;
using ePatient.Infrastructure.Persistence.Patients;

namespace ePatient.Infrastructure.Persistence;

/// <summary>
/// Extension methods for configuring the SQL database.
/// </summary>
public static class ConfigureDatabase
{
    /// <summary>
    /// Configures the database and setups EF core.
    /// </summary>
    /// <param name="services">Services to add dependencies to.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection ConfigureDatabaseServices(this IServiceCollection services)
    {
        services.AddDbContext<IUnitOfWork, ApplicationDbContext>(
            options => options.UseInMemoryDatabase("PatientLookupDb"));

        return services;
    }

    /// <summary>
    /// Populates the database with the relevant tables and data.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void PopulateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        db.Database.EnsureCreated();
        //db.Database.Migrate();

        var faker = new PatientGenerator();
        var patients = faker.GenerateBetween(3, 5).ToList();
        db.Patients.AddRange(patients);
        db.SaveChanges();
    }
}