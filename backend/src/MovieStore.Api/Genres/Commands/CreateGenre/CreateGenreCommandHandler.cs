using ErrorOr;
using MovieStore.Api.Common.Persistence;
using MovieStore.Api.Common.Pipeline;

namespace MovieStore.Api.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler(MovieStoreDbContext context)
    : IRequestHandler<CreateGenreCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = new Genre(name: request.Name, description: request.Description);
        await context.Genre.AddAsync(genre, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}