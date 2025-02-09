using Microsoft.EntityFrameworkCore;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence;

internal class MovieStoreDbContext : DbContext
{
    public MovieStoreDbContext(DbContextOptions<MovieStoreDbContext> options) : base(options) {}
    
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Genre> Genres { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieStoreDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}