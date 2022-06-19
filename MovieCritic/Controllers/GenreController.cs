using Authorization.Data;
using Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly ApplicationDbContext _db;

        public GenreController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Genres);
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string genreName)
        {
            _db.Add(new Genre() { GenreName = genreName });
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var genre = _db.Genres.SingleOrDefault(x => x.Id == id);
            if(genre == null)
                return NotFound();

            return View(genre);
        }

        [HttpPost]
        public IActionResult Edit(Genre genre)
        {
            if (_db.Genres.SingleOrDefault(x => x.GenreName == genre.GenreName) == null)
            {
                _db.Genres.Update(genre);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        #endregion
        }
}
