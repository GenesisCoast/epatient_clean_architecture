using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using ePatient.Application.Common;
using ePatient.Domain.Patients;
using ePatient.Infrastructure.Persistence.Patients;
using Microsoft.EntityFrameworkCore;

namespace ePatient.Infrastructure.Persistence;

/// <summary>
/// EF core database context for the application.
/// </summary>
/// <param name="options"></param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
{
    /// <inheritdoc/>
    public DbSet<Patient> Patients => Set<Patient>();

    /// <inheritdoc/>
    public IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : class => new BaseRepository<TEntity>(this);

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PatientConfiguration());
    }
}