using Ardalis.Specification;

namespace ePatient.Application.Common;

/// <summary>
/// Defines a contract for coordinating changes across multiple repositories as a single unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Gets a repository instance for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity for which to retrieve the repository. Must be a reference type.</typeparam>
    /// <returns>An <see cref="IRepositoryBase{TEntity}"/> instance for the specified entity type.</returns>
    IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : class;

    /// <summary>
    /// Asynchronously saves all changes made in this context to the underlying database.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous save operation.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries
    /// written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}   