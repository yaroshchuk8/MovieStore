using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Repositories;

internal class MovieActorRepository(MovieStoreDbContext context)
    : BaseRepository<MovieActor>(context), IMovieActorRepository
{
    
}