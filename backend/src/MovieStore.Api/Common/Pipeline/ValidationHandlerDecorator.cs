using ErrorOr;
using FluentValidation;

namespace MovieStore.Api.Common.Pipeline;

public class ValidationHandlerDecorator<TRequest, TValue>(
    IRequestHandler<TRequest, TValue> inner,
    IEnumerable<IValidator<TRequest>> validators) 
    : IRequestHandler<TRequest, TValue>
{
    public async Task<ErrorOr<TValue>> Handle(
        TRequest request,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await inner.Handle(request, cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);
        var failures = (await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .SelectMany(r => r.Errors)
            .ToList();

        return failures.Count == 0
            ? await inner.Handle(request, cancellationToken)
            : failures.Select(f => Error.Validation(f.PropertyName, f.ErrorMessage)).ToList();
    }
}