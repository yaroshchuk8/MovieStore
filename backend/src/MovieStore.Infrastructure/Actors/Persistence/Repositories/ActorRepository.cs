using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Entities;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Persistence.Repositories;
using MovieStore.Infrastructure.Persistence;
using MovieStore.Infrastructure.Persistence.Repositories;

namespace MovieStore.Infrastructure.Actors.Persistence.Repositories;

internal class ActorRepository(MovieStoreDbContext context) : BaseRepository<Actor>(context), IActorRepository
{
    
}