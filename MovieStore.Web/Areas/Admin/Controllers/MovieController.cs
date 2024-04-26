using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStore.DataAccess.Repository.IRepository;
using MovieStore.Models;
using MovieStore.Models.ViewModels;

namespace MovieStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MovieController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Movie> movies = _unitOfWork.Movie.GetAll(includeProperties:"Genres,Actors").ToList();
            return View(movies);
        }

        public IActionResult Upsert(int? id)
        {
            MovieVM movieVM = new();

            movieVM.SelectedGenres = new();
            movieVM.GenreList = _unitOfWork.Genre.GetAll()
                .Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });

            movieVM.SelectedActors = new();
            movieVM.ActorList = _unitOfWork.Actor.GetAll()
                .Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });

            //insert
            if (id == null || id == 0)
            {
                movieVM.Movie = new();
                return View(movieVM);
            }

            //update
            movieVM.Movie = _unitOfWork.Movie.Get(m => m.Id == id, "Genres,Actors");

            if (movieVM.Movie == null)
            {
                return NotFound();
            }

            movieVM.Movie.Genres.ForEach(x => movieVM.SelectedGenres.Add(x.Id.ToString()));
            movieVM.Movie.Actors.ForEach(x => movieVM.SelectedActors.Add(x.Id.ToString()));

            return View(movieVM);
        }

        [HttpPost]
        public IActionResult Upsert(MovieVM movieVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                //upserting the image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string moviePath = Path.Combine(wwwRootPath, @"images\movie");

                    if (!string.IsNullOrEmpty(movieVM.Movie.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, movieVM.Movie.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(moviePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    movieVM.Movie.ImageUrl = @"\images\movie\" + fileName;
                }

                // updating the existing entry
                if (movieVM.Movie.Id != 0)
                {
                    _unitOfWork.Movie.Update(movieVM);
                }
                // adding new entry 
                else
                {
                    _unitOfWork.Movie.Add(movieVM);
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                movieVM.SelectedGenres = new();
                movieVM.GenreList = _unitOfWork.Genre.GetAll()
                    .Select(item => new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    });

                movieVM.SelectedActors = new();
                movieVM.ActorList = _unitOfWork.Actor.GetAll()
                    .Select(item => new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    });

                return View(movieVM);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Movie movieToDelete = _unitOfWork.Movie.Get(x => x.Id == id, "Genres,Actors");

            if (movieToDelete == null)
            {
                return NotFound();
            }

            // delete entries from join tables
            for (int i = movieToDelete.Genres.Count - 1; i >= 0; i--)
            {
                movieToDelete.Genres.Remove(movieToDelete.Genres[i]);
            }
            for (int i = movieToDelete.Actors.Count - 1; i >= 0; i--)
            {
                movieToDelete.Actors.Remove(movieToDelete.Actors[i]);
            }

            // delete image from /wwwroot/images/movie folder
            var imagePath =
                Path.Combine(_webHostEnvironment.WebRootPath,
                movieToDelete.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            _unitOfWork.Movie.Remove(movieToDelete);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
