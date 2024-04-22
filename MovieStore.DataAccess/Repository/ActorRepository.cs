using MovieStore.DataAccess.Data;
using MovieStore.Models;

namespace MovieStore.DataAccess.Repository
{
    public class ActorRepository : Repository<Actor>
    {
        public ActorRepository(ApplicationDbContext context) : base(context) { }
    }
}
