using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Repositories;

internal class ActorRepository(MovieStoreDbContext context) : BaseRepository<Actor>(context), IActorRepository
{
    
}