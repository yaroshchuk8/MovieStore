using Microsoft.AspNetCore.Mvc;
using MovieStore.DataAccess.Repository.IRepository;
using MovieStore.Models;
using System.Diagnostics;

namespace MovieStore.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Movie> movies = _unitOfWork.Movie.GetAll(includeProperties: ("Genres,Actors"));
            return View(movies);
        }

        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Movie movie = _unitOfWork.Movie.Get(x =>  x.Id == id, includeProperties:"Genres,Actors");
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
