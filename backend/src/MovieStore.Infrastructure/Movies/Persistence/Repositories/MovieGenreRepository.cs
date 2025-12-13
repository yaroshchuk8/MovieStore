using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Entities;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Persistence.Repositories;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Persistence.Repositories;

namespace MovieStore.Infrastructure.Movies.Persistence.Repositories;

internal class MovieGenreRepository(MovieStoreDbContext context)
    : BaseRepository<MovieGenre>(context), IMovieGenreRepository
{
    
}