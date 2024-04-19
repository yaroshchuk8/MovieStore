using Microsoft.EntityFrameworkCore;
using MovieStore.Models;

namespace MovieStore.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Genre> Genres { get; set; }
    }
}
