using MovieStore.DataAccess.Data;
using MovieStore.Models;

namespace MovieStore.DataAccess.Repository
{
    public class GenreRepository : Repository<Genre>
    {
        public GenreRepository(ApplicationDbContext context) : base(context) { }
    }
}
