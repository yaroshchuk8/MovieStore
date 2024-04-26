using MovieStore.DataAccess.Data;
using MovieStore.DataAccess.Repository.IRepository;
using MovieStore.Models;
using MovieStore.Models.ViewModels;

namespace MovieStore.DataAccess.Repository
{
    public class MovieRepository : Repository<Movie>
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public void Update(MovieVM movieVM)
        {
            Movie movie = base.Get(m => m.Id == movieVM.Movie.Id, "Genres,Actors");
            movie.Title = movieVM.Movie.Title;
            movie.Description = movieVM.Movie.Description;
            movie.Price = movieVM.Movie.Price;
            movie.ImageUrl = movieVM.Movie.ImageUrl;

            //removing existing connections from join tables
            for (int i = movie.Genres.Count - 1; i >= 0; i--)
            {
                movie.Genres.Remove(movie.Genres[i]);
            }
            for (int i = movie.Actors.Count - 1; i >= 0; i--)
            {
                movie.Actors.Remove(movie.Actors[i]);
            }

            //adding newly selected connections to join tables
            foreach (string s in movieVM.SelectedGenres)
            {
                movie.Genres.Add(_context.Genres.FirstOrDefault(x => x.Id == Convert.ToInt32(s)));
            }
            foreach (string s in movieVM.SelectedActors)
            {
                movie.Actors.Add(_context.Actors.FirstOrDefault(x => x.Id == Convert.ToInt32(s)));
            }

            base.Update(movie);
        }

        public void Add(MovieVM movieVM)
        {
            movieVM.Movie.Genres = new();
            movieVM.Movie.Actors = new();

            //adding selected connections to join tables
            foreach (string s in movieVM.SelectedGenres)
            {
                movieVM.Movie.Genres.Add(_context.Genres.FirstOrDefault(x => x.Id == Convert.ToInt32(s)));
            }
            foreach (string s in movieVM.SelectedActors)
            {
                movieVM.Movie.Actors.Add(_context.Actors.FirstOrDefault(x => x.Id == Convert.ToInt32(s)));
            }

            base.Add(movieVM.Movie);
        }
    }
}
