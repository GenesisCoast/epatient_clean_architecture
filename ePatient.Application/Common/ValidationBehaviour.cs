using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using System.Collections.Concurrent;
using System.Reflection;

namespace ePatient.Application.Common;

/// <summary>
/// Defines a pipeline behavior that applies validation to incoming requests before passing them to the next handler
/// in the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request message to be validated. Must be a reference type.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
/// <param name="validators">A collection of validators to apply to the request. Each validator checks the request for specific validation
/// rules.</param>
public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class, IResult
{
    /// <summary>
    /// Cache the invalid method to avoid repeat reflection hits.
    /// </summary>
    public static ConcurrentDictionary<Type, MethodInfo> InvalidMethodCache { get; } = new();

    /// <summary>
    /// Gets the invalid method.
    /// </summary>
    /// <param name="responseType">Type to get the metod for.</param>
    /// <returns>The invalid method.</returns>
    public MethodInfo? GetInvalidMethod(Type responseType) => InvalidMethodCache.GetValueOrDefault(responseType);

    /// <summary>
    /// Get the cached method or use reflection to find it.
    /// </summary>
    /// <param name="responseType">Type to get the method for.</param>
    /// <returns>The invalid method.</returns>
    private static MethodInfo GetOrAddInvalidMethod(Type responseType)
    {
        return InvalidMethodCache.GetOrAdd(responseType, type =>
            type.GetMethod(
                nameof(Result.Invalid),
                BindingFlags.Static | BindingFlags.Public,
                [typeof(IEnumerable<ValidationError>)])!);
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle
        (TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(next);

        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

            var failures = validationResults
                .Where(r => r.Errors.Count > 0)
                .SelectMany(r => r.AsErrors())
                .ToList();

            if (failures.Count > 0)
            {
                return (TResponse)GetOrAddInvalidMethod(typeof(TResponse)).Invoke(null, [failures])!;
            }
        }

        return await next(cancellationToken).ConfigureAwait(false);
    }
}
