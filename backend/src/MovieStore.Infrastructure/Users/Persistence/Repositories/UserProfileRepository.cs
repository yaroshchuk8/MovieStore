using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Entities;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Persistence.Repositories;

namespace MovieStore.Infrastructure.Persistence.Repositories;

internal class UserProfileRepository(MovieStoreDbContext context)
    : BaseRepository<UserProfile>(context), IUserProfileRepository
{
    
}