using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Domain.Actors;
using MovieStore.Domain.Genres;
using MovieStore.Domain.Movies;
using MovieStore.Domain.Users;
using MovieStore.Infrastructure.Users.Persistence.Identity.Entities;

namespace MovieStore.Infrastructure.Common.Persistence;

public class MovieStoreDbContext(DbContextOptions<MovieStoreDbContext> options)
    : IdentityDbContext<
        IdentityUserEntity,
        IdentityRoleEntity,
        int,
        IdentityUserClaimEntity,
        IdentityUserRoleEntity,
        IdentityUserLoginEntity,
        IdentityRoleClaimEntity,
        IdentityUserTokenEntity
    >(options), IUnitOfWork
{
    // Domain
    public DbSet<Actor> Actor { get; set; }
    public DbSet<Genre> Genre { get; set; }
    public DbSet<Movie> Movie { get; set; }
    public DbSet<MovieActor> MovieActor { get; set; }
    public DbSet<MovieGenre> MovieGenre { get; set; }
    public DbSet<UserProfile> UserProfile { get; set; }
    public DbSet<Wallet> Wallet { get; set; }
    public DbSet<PublisherProfile> PublisherProfile { get; set; }
    
    // Identity
    public DbSet<RefreshToken> RefreshToken { get; set; }
    
    public async Task CommitChangesAsync()
    {
        await SaveChangesAsync();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieStoreDbContext).Assembly);
    }
}