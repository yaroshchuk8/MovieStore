using Microsoft.AspNetCore.Mvc;
using MovieStore.DataAccess.Data;
using MovieStore.Models;

namespace MovieStore.Web.Controllers
{
    public class GenreController : Controller
    {
        private ApplicationDbContext _db;

        public GenreController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Genre> genreList = _db.Genres.ToList();
            return View(genreList);
        }
    }
}
