using Microsoft.EntityFrameworkCore;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence;

internal class MovieStoreDbContext : DbContext
{
    public MovieStoreDbContext(DbContextOptions<MovieStoreDbContext> options) : base(options) {}
    
    public DbSet<Movie> Movie { get; set; }
    public DbSet<Actor> Actor { get; set; }
    public DbSet<Genre> Genre { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieStoreDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}