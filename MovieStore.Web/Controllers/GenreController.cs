using Microsoft.AspNetCore.Mvc;
using MovieStore.DataAccess.Data;
using MovieStore.Models;

namespace MovieStore.Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly ApplicationDbContext _db;

        public GenreController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Genre> genreList = _db.Genres.ToList();
            return View(genreList);
        }

        public IActionResult Upsert(int? id) //Update+Insert
        {
            Genre genre;

            //insert
            if (id == null || id == 0)
            {
                genre = new();
                return View(genre);
            }

            //update
            genre = _db.Genres.Find(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        public IActionResult Upsert(Genre genre)
        {
            if (ModelState.IsValid)
            {
                if (genre.Id == 0)
                {
                    _db.Genres.Add(genre);
                    // TempData["success"] = "Product created successfuly";
                }
                else
                {
                    _db.Genres.Update(genre);
                    // TempData["success"] = "Product updated successfuly";
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(genre);
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Genre genre = _db.Genres.Find(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        public IActionResult Delete(Genre genre)
        {
            _db.Genres.Remove(genre);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}