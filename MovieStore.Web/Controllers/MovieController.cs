using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieStore.DataAccess.Data;
using MovieStore.Models;
using MovieStore.Models.ViewModels;

namespace MovieStore.Web.Controllers
{
	public class MovieController : Controller
	{
		private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
		{
			_db = db;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			List<Movie> movies = _db.Movies.ToList();
			return View(movies);
		}

		public IActionResult Upsert(int? id)
		{
			MovieVM movieVM = new();
			movieVM.SelectedItems = new();
			movieVM.GenreList = _db.Genres
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
			movieVM.Movie = _db.Movies.Include(x => x.Genres).FirstOrDefault(m => m.Id == id);

			if (movieVM.Movie == null)
			{
				return NotFound();
			}

			movieVM.Movie.Genres.ForEach(x => movieVM.SelectedItems.Add(x.Id.ToString()));

			return View(movieVM);
		}

		[HttpPost]
		public IActionResult Upsert(MovieVM movieVM, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = Path.Combine(wwwRootPath, @"images\movie");

					if (!string.IsNullOrEmpty(movieVM.Movie.ImageUrl))
					{
						//delete the old image
						var oldImagePath = Path.Combine(wwwRootPath, movieVM.Movie.ImageUrl.TrimStart('\\'));

						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					movieVM.Movie.ImageUrl = @"\images\product\" + fileName;
				}

				// updating the existing entry
				if (movieVM.Movie.Id != 0)
				{
					Movie movie = _db.Movies.Include(x => x.Genres).FirstOrDefault(m => m.Id == movieVM.Movie.Id);
					for (int i = movie.Genres.Count - 1; i >= 0; i--)
					{
						movie.Genres.Remove(movie.Genres[i]);
					}
					//_db.SaveChanges();
					foreach (string s in movieVM.SelectedItems)
					{
						movie.Genres.Add(_db.Genres.Find(Convert.ToInt32(s)));
					}
					_db.Movies.Update(movie);
				}
				// adding new entry 
				else
				{
					movieVM.Movie.Genres = new();

					foreach (string s in movieVM.SelectedItems)
					{
						movieVM.Movie.Genres.Add(_db.Genres.Find(Convert.ToInt32(s)));
					}
					_db.Movies.Add(movieVM.Movie);
				}

				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				movieVM.SelectedItems = new();
				movieVM.GenreList = _db.Genres
					.Select(item => new SelectListItem
					{
						Text = item.Name,
						Value = item.Id.ToString()
					});

				return View(movieVM);
			}
		}
	}
}
