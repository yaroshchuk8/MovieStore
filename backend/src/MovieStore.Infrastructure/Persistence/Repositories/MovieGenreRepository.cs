using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Repositories;

internal class MovieGenreRepository(MovieStoreDbContext context)
    : BaseRepository<MovieGenre>(context), IMovieGenreRepository
{
    
}