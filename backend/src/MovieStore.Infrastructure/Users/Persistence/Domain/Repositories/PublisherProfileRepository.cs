using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Users;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Persistence.Repositories;

namespace MovieStore.Infrastructure.Users.Persistence.Domain.Repositories;

internal class PublisherProfileRepository(MovieStoreDbContext context)
    : BaseRepository<PublisherProfile>(context), IPublisherProfileRepository 
{
    
}