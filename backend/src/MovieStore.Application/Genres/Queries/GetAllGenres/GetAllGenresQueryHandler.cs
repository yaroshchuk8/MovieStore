using ErrorOr;
using MediatR;
using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Contracts.Genres.Responses;

namespace MovieStore.Application.Genres.Queries.GetAllGenres;

public class GetAllGenresQueryHandler(IGenreRepository genreRepository)
    : IRequestHandler<GetAllGenresQuery, ErrorOr<List<GenreResponse>>>
{
    public async Task<ErrorOr<List<GenreResponse>>> Handle(
        GetAllGenresQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var genres = await genreRepository.GetAllAsync();
        
            var result = genres
                .Select(genre => new GenreResponse(genre.Id, genre.Name, genre.Description))
                .ToList();
        
            return result;
        }
        catch (Exception ex)
        {
            return Error.Unexpected(
                code: "Infrastructure.RepositoryFailure",
                description: $"Database access failed: {ex.Message}");
        }
    }
}