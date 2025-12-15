using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Entities;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Persistence.Repositories;

namespace MovieStore.Infrastructure.Users.Persistence.Repositories;

internal class UserProfileRepository(MovieStoreDbContext context)
    : BaseRepository<UserProfile>(context), IUserProfileRepository
{
    
}