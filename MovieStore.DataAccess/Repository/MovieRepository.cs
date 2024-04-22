using MovieStore.DataAccess.Data;
using MovieStore.Models;

namespace MovieStore.DataAccess.Repository
{
    public class MovieRepository : Repository<Movie>
    {
        public MovieRepository(ApplicationDbContext context) : base(context) { }
    }
}
