using Microsoft.AspNetCore.Mvc;
using MovieStore.DataAccess.Repository.IRepository;
using MovieStore.Models;

namespace MovieStore.Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Genre> genreList = _unitOfWork.Genre.GetAll().ToList();
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
            genre = _unitOfWork.Genre.Get(x => x.Id == id);

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
                    _unitOfWork.Genre.Add(genre);
                    // TempData["success"] = "Product created successfuly";
                }
                else
                {
                    _unitOfWork.Genre.Update(genre);
                    // TempData["success"] = "Product updated successfuly";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(genre);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Genre genreToDelete = _unitOfWork.Genre.Get(x => x.Id == id);
            if (genreToDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.Genre.Remove(genreToDelete);
            _unitOfWork.Save();

            return RedirectToAction("Index");

        }
    }
}