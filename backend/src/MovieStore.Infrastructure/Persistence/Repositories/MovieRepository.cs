using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Repositories;

internal class MovieRepository(MovieStoreDbContext context) : BaseRepository<Movie>(context), IMovieRepository
{
    
}