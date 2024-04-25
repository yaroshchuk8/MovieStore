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

            if (id == null || id == 0)
            {
                //insert
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
                    Movie movie = _unitOfWork.Movie.Get(m => m.Id == movieVM.Movie.Id, "Genres,Actors");
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
                        movie.Genres.Add(_unitOfWork.Genre.Get(x => x.Id == Convert.ToInt32(s)));
                    }
                    foreach (string s in movieVM.SelectedActors)
                    {
                        movie.Actors.Add(_unitOfWork.Actor.Get(x => x.Id == Convert.ToInt32(s)));
                    }

                    _unitOfWork.Movie.Update(movie);
                }
                // adding new entry 
                else
                {
                    movieVM.Movie.Genres = new();
                    movieVM.Movie.Actors = new();

                    //adding selected connections to join tables
                    foreach (string s in movieVM.SelectedGenres)
                    {
                        movieVM.Movie.Genres.Add(_unitOfWork.Genre.Get(x => x.Id == Convert.ToInt32(s)));
                    }
                    foreach (string s in movieVM.SelectedActors)
                    {
                        movieVM.Movie.Actors.Add(_unitOfWork.Actor.Get(x => x.Id == Convert.ToInt32(s)));
                    }

                    _unitOfWork.Movie.Add(movieVM.Movie);
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
