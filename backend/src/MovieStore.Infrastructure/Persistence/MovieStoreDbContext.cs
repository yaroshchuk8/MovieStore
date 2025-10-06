using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence;

internal class MovieStoreDbContext(DbContextOptions<MovieStoreDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Actor> Actor { get; set; }
    public DbSet<Genre> Genre { get; set; }
    public DbSet<Movie> Movie { get; set; }
    public DbSet<MovieActor> MovieActor { get; set; }
    public DbSet<MovieGenre> MovieGenre { get; set; }
    
    public async Task CommitChangesAsync()
    {
        await SaveChangesAsync();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieStoreDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}