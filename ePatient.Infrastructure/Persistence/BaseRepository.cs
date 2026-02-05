using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ePatient.Infrastructure.Persistence;

/// <summary>
/// Base repository implementation using Ardalis.Specification.EntityFrameworkCore.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
/// <param name="dbContext">The EF core context.</param>
public class BaseRepository<T>(DbContext dbContext) : RepositoryBase<T>(dbContext) where T : class;