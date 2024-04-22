using Microsoft.AspNetCore.Mvc;
using MovieStore.DataAccess.Data;
using MovieStore.Models;

namespace MovieStore.Web.Controllers
{
	public class MovieController : Controller
	{
		private readonly ApplicationDbContext _db;

		public MovieController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<Movie> movies = _db.Movies.ToList();
			return View(movies);
		}
	}
}
