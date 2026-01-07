using MediatR;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Interfaces.Repositories;
using ErrorOr;
using MovieStore.Domain.Genres;

namespace MovieStore.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGenreCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var genre = new Genre(name: request.Name, description: request.Description);
            await genreRepository.AddAsync(genre);
            await unitOfWork.CommitChangesAsync();

            return Result.Success;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(
                code: "Infrastructure.RepositoryFailure",
                description: $"Database access failed: {ex.Message}");
        }
    }
}