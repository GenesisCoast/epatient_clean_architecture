using Bogus;

namespace ePatient.Domain.Tests;

/// <summary>
/// Base tests for any entities.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
/// <typeparam name="TGenerator">Entity type generator.</typeparam>
public abstract class BaseEntityTests<T, TGenerator>
    where T : class
    where TGenerator : Faker<T>, new()
{
    /// <summary>
    /// The entity generator faker.
    /// </summary>
    protected virtual TGenerator Faker { get; init; } = new();

    /// <summary>
    /// Sets a private property in the specified entity.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="propertyName">Name of the property to set.</param>
    /// <param name="propertyValue">Value to use in the setting.</param>
    /// <returns></returns>
    protected T SetProperty<TValue>(string propertyName, TValue propertyValue)
    {
        var entity = Faker.Generate();

        typeof(T)
            .GetProperty(propertyName)!
            .SetValue(entity, propertyValue);

        return entity;
    }
}
