using ErrorOr;

namespace MovieStore.Api.Common.Pipeline;

public interface IRequestHandler<in TRequest, TValue>
{
    Task<ErrorOr<TValue>> Handle(TRequest request, CancellationToken cancellationToken);
}