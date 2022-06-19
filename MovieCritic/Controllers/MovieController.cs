using Authorization.Managers;
using Authorization.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieManager _movieManager;

        public MovieController(MovieManager movieManager)
        {
            _movieManager = movieManager;
        }

        public IActionResult Index()
        {
            var movieList = _movieManager.GetMovieList();
            return View(movieList);
        }

        public IActionResult Details(int? id)
        {
            var movie = _movieManager.Details(id);
            if(movie == null)
                return NotFound();
            return View(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ModifyMovieMV());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(ModifyMovieMV movie)
        {
            _movieManager.Create(movie);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var movie = _movieManager.GetMovieById(id);

            if(movie == null)
                return NotFound();

            var mv = new ModifyMovieMV() { Id = movie.Id, MovieTitle = movie.MovieTitle, MovieDescription = movie.Description, MovieGenre = movie.GenreId };
            return View(mv);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(ModifyMovieMV movieMV)
        {
            _movieManager.Edit(movieMV);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _movieManager.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
