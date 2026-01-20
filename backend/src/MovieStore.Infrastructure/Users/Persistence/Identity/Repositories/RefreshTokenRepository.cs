using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Persistence.Repositories;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;
using MovieStore.Infrastructure.Users.Persistence.Identity.Repositories.Interfaces;

namespace MovieStore.Infrastructure.Users.Persistence.Identity.Repositories;

internal class RefreshTokenRepository(MovieStoreDbContext context)
    : BaseRepository<RefreshToken>(context), IRefreshTokenRepository
{
    
}