using MovieStore.Application.Users.Interfaces;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Repositories;

internal class UserProfileRepository(MovieStoreDbContext context)
    : BaseRepository<UserProfile>(context), IUserProfileRepository
{
    
}