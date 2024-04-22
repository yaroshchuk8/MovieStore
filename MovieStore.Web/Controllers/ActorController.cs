using Microsoft.AspNetCore.Mvc;
using MovieStore.DataAccess.Repository.IRepository;
using MovieStore.Models;
using MovieStore.Models.ViewModels;

namespace MovieStore.Web.Controllers
{
	public class ActorController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ActorController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			List<Actor> actorList = _unitOfWork.Actor.GetAll().ToList();
			return View(actorList);
		}

		public IActionResult Upsert(int? id) //Update+Insert
		{
			Actor actor;

			//insert
			if (id == null || id == 0)
			{
				actor = new();
				return View(actor);
			}

			//update
			actor = _unitOfWork.Actor.Get(x => x.Id == id);

			if (actor == null)
			{
				return NotFound();
			}

			return View(actor);
		}

		[HttpPost]
		public IActionResult Upsert(Actor actor, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string moviePath = Path.Combine(wwwRootPath, @"images\actor");

					if (!string.IsNullOrEmpty(actor.ImageUrl))
					{
						//delete the old image
						var oldImagePath = Path.Combine(wwwRootPath, actor.ImageUrl.TrimStart('\\'));

						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					using (var fileStream = new FileStream(Path.Combine(moviePath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					actor.ImageUrl = @"\images\actor\" + fileName;
				}

				if (actor.Id == 0)
				{
					_unitOfWork.Actor.Add(actor);
					// TempData["success"] = "Product created successfuly";
				}
				else
				{
					_unitOfWork.Actor.Update(actor);
					// TempData["success"] = "Product updated successfuly";
				}
				_unitOfWork.Save();
				return RedirectToAction("Index");
			}
			else
			{
				return View(actor);
			}
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			
			Actor actorToDelete = _unitOfWork.Actor.Get(x => x.Id == id);
			
			if (actorToDelete == null)
			{
				return NotFound();
			}

			// delete respective image from /wwwroot/images/movie folder
			var imagePath =
				Path.Combine(_webHostEnvironment.WebRootPath,
				actorToDelete.ImageUrl.TrimStart('\\'));

			if (System.IO.File.Exists(imagePath))
			{
				System.IO.File.Delete(imagePath);
			}
			_unitOfWork.Actor.Remove(actorToDelete);
			_unitOfWork.Save();

			return RedirectToAction("Index");

		}
	}
}
