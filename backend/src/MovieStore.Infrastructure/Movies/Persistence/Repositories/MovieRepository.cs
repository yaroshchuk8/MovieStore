using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Movies;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Persistence.Repositories;

namespace MovieStore.Infrastructure.Movies.Persistence.Repositories;

internal class MovieRepository(MovieStoreDbContext context) : BaseRepository<Movie>(context), IMovieRepository
{
    
}