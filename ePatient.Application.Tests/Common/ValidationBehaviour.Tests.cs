using Ardalis.Result;
using ePatient.Application.Common;
using FluentValidation;
using MediatR;

namespace ePatient.Application.Tests.Common;

/// <summary>
/// Unit tests for <see cref="ValidationBehaviour{TRequest, TResponse}"/>.
/// </summary>
public class ValidationBehaviourTests
{
    /// <summary>
    /// Test request type.
    /// </summary>
    private record TestRequest(string Value) : IRequest<Result<string>>;

    /// <summary>
    /// Test validator that always passes.
    /// </summary>
    private class PassingValidator : AbstractValidator<TestRequest>
    {
        public PassingValidator()
        {
            RuleFor(x => x.Value).NotEmpty();
        }
    }

    /// <summary>
    /// Test validator that always fails.
    /// </summary>
    private class FailingValidator : AbstractValidator<TestRequest>
    {
        public FailingValidator()
        {
            RuleFor(x => x.Value).Must(_ => false).WithMessage("Validation failed");
        }
    }

    /// <summary>
    /// Verifies that <see cref="ValidationBehaviour{TRequest, TResponse}.Handle"/> calls next when no validators are provided.
    /// </summary>
    [Fact]
    public async Task Handle_Should_CallNext_WhenNoValidators()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behaviour = new ValidationBehaviour<TestRequest, Result<string>>(validators);
        var request = new TestRequest("test");
        var expectedResult = Result<string>.Success("success");
        var nextCalled = false;

        Task<Result<string>> Next(CancellationToken ct)
        {
            nextCalled = true;
            return Task.FromResult(expectedResult);
        }

        // Act
        var result = await behaviour.Handle(request, Next, CancellationToken.None);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal(expectedResult, result);
    }

    /// <summary>
    /// Verifies that <see cref="ValidationBehaviour{TRequest, TResponse}.Handle"/> calls next when all validators pass.
    /// </summary>
    [Fact]
    public async Task Handle_Should_CallNext_WhenValidationPasses()
    {
        // Arrange
        var validators = new List<IValidator<TestRequest>> { new PassingValidator() };
        var behaviour = new ValidationBehaviour<TestRequest, Result<string>>(validators);
        var request = new TestRequest("test");
        var expectedResult = Result<string>.Success("success");
        var nextCalled = false;

        Task<Result<string>> Next(CancellationToken ct)
        {
            nextCalled = true;
            return Task.FromResult(expectedResult);
        }

        // Act
        var result = await behaviour.Handle(request, Next, CancellationToken.None);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal(expectedResult, result);
    }

    /// <summary>
    /// Verifies that <see cref="ValidationBehaviour{TRequest, TResponse}.Handle"/> returns invalid result when validation fails.
    /// </summary>
    [Fact]
    public async Task Handle_Should_ReturnInvalid_WhenValidationFails()
    {
        // Arrange
        var validators = new List<IValidator<TestRequest>> { new FailingValidator() };
        var behaviour = new ValidationBehaviour<TestRequest, Result<string>>(validators);
        var request = new TestRequest("test");
        var nextCalled = false;

        Task<Result<string>> Next(CancellationToken ct)
        {
            nextCalled = true;
            return Task.FromResult(Result<string>.Success("success"));
        }

        // Act
        var result = await behaviour.Handle(request, Next, CancellationToken.None);

        // Assert
        Assert.False(nextCalled);
        Assert.Equal(ResultStatus.Invalid, result.Status);
        Assert.NotEmpty(result.ValidationErrors);
    }

    /// <summary>
    /// Verifies that <see cref="ValidationBehaviour{TRequest, TResponse}.Handle"/> throws when next delegate is null.
    /// </summary>
    [Fact]
    public async Task Handle_Should_ThrowArgumentNullException_WhenNextIsNull()
    {
        // Arrange
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behaviour = new ValidationBehaviour<TestRequest, Result<string>>(validators);
        var request = new TestRequest("test");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            behaviour.Handle(request, null!, CancellationToken.None));
    }

    /// <summary>
    /// Verifies that <see cref="ValidationBehaviour{TRequest, TResponse}.Handle"/> respects cancellation token.
    /// </summary>
    [Fact]
    public async Task Handle_Should_PassCancellationToken_ToValidators()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var tokenPassed = false;

        var validator = new InlineValidator<TestRequest>();
        validator.RuleFor(x => x.Value)
            .MustAsync(async (value, ct) =>
            {
                tokenPassed = ct == cts.Token;
                await Task.Yield();
                return true;
            });

        var validators = new List<IValidator<TestRequest>> { validator };
        var behaviour = new ValidationBehaviour<TestRequest, Result<string>>(validators);
        var request = new TestRequest("test");

        static Task<Result<string>> Next(CancellationToken ct) =>
            Task.FromResult(Result<string>.Success("success"));

        // Act
        await behaviour.Handle(request, Next, cts.Token);

        // Assert
        Assert.True(tokenPassed);
    }

    /// <summary>
    /// Verifies that <see cref="ValidationBehaviour{TRequest, TResponse}.Handle"/> passes cancellation token to next delegate.
    /// </summary>
    [Fact]
    public async Task Handle_Should_PassCancellationToken_ToNext()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behaviour = new ValidationBehaviour<TestRequest, Result<string>>(validators);
        var request = new TestRequest("test");
        CancellationToken? receivedToken = null;

        Task<Result<string>> Next(CancellationToken ct)
        {
            receivedToken = ct;
            return Task.FromResult(Result<string>.Success("success"));
        }

        // Act
        await behaviour.Handle(request, Next, cts.Token);

        // Assert
        Assert.NotNull(receivedToken);
        Assert.Equal(cts.Token, receivedToken.Value);
    }

    /// <summary>
    /// Verifies that method cache works correctly for multiple calls with same type.
    /// </summary>
    [Fact]
    public async Task Handle_Should_UseCachedMethod_ForSubsequentCalls()
    {
        // Arrange
        var validator = new FailingValidator();
        var validators = new List<IValidator<TestRequest>> { validator };
        var behaviour1 = new ValidationBehaviour<TestRequest, Result<string>>(validators);
        var behaviour2 = new ValidationBehaviour<TestRequest, Result<string>>(validators);
        var request = new TestRequest("test");

        static Task<Result<string>> Next(CancellationToken ct) =>
            Task.FromResult(Result<string>.Success("success"));

        // Act
        var result1 = await behaviour1.Handle(request, Next, CancellationToken.None);

        // Assert
        Assert.NotNull(behaviour1.GetInvalidMethod(typeof(Result<string>)));
        Assert.NotNull(behaviour2.GetInvalidMethod(typeof(Result<string>)));
    }
}
