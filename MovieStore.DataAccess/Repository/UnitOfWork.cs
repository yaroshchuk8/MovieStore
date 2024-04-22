using MovieStore.DataAccess.Data;
using MovieStore.DataAccess.Repository.IRepository;

namespace MovieStore.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		public MovieRepository Movie { get; private set; }
		public GenreRepository Genre { get; private set; }

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			Movie = new MovieRepository(_context);
			Genre = new GenreRepository(_context);
		}

		public void Save()
		{
			_context.SaveChanges();
		}
	}
}
