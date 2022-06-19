using Authorization.Managers;
using Authorization.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly MovieManager _movieManager;
        private readonly ReviewManager _reviewManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewController(MovieManager movieManager, ReviewManager reviewManager, UserManager<IdentityUser> userManager)
        {
            _movieManager = movieManager;
            _reviewManager = reviewManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("Create/{id}")]
        public IActionResult Create(int? id)
        {
            if (_movieManager.GetMovieById(id) == null)
                return NotFound();
            return View(new ModifyReviewMV() { MovieId = (int)id, UserId = _userManager.FindByNameAsync(User.Identity?.Name).Result.Id });
        }

        [HttpPost]
        [Route("Create/{id}")]
        public IActionResult Create(ModifyReviewMV reviewMV)
        {
            if (_reviewManager.Create(reviewMV))
                return RedirectToAction("Index", "Movie");
            return RedirectToAction("Index", "Movie");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var review = _reviewManager.GetReviewById(id);

            if (review == null)
                return NotFound();

            IdentityUser user = _userManager.FindByNameAsync(User.Identity?.Name).Result;
            bool isAdmin = _userManager.GetRolesAsync(user)?.Result?[0] == "Admin";

            if (!isAdmin || User.Identity?.Name != user.UserName)
                return NotFound();

            var reviewMV = new ModifyReviewMV() {
                UserId = review.UserId,
                MovieId = review.MovieId,
                Content = review.Content,
                MovieScore = review.MovieScore
            };

            return View(reviewMV);
        }

        [HttpPost]
        public IActionResult Edit(ModifyReviewMV reviewMV)
        {
            if(_reviewManager.Update(reviewMV))
                return RedirectToAction("Index", "Movie");
            return RedirectToAction("Index", "Movie");
        }

        public IActionResult Delete(int? Id)
        {
            _reviewManager.Delete(Id);
            return RedirectToAction("Index", "Movie");
        }
    }
}
