using Authorization.Data;
using Authorization.Models;
using Authorization.ModelViews;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Managers
{
    public class ReviewManager
    {
        private readonly ApplicationDbContext _db;
        private readonly MovieManager _movieManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewManager(ApplicationDbContext db, MovieManager movieManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _movieManager = movieManager;
            _userManager = userManager;
        }

        public bool Create(ModifyReviewMV review)
        {
            var movie = _movieManager.GetMovieById(review.MovieId);

            if (movie == null || _userManager.FindByIdAsync(review.UserId).Result == null)
                return false;

            if (GetReviewsByMovieId(movie.Id)?.Where(x => x.UserId.Equals(review.UserId)).ToList().Count != 0)
                return false;

            _db.Reviews.Add(new Review() { MovieId = review.MovieId, UserId = review.UserId, Content = review.Content, MovieScore = review.MovieScore });
            _db.SaveChanges();

            _movieManager.CalculateAvarageScore(movie);

            return true;
        }

        public bool Update(ModifyReviewMV reviewMV)
        {
            var movie = _movieManager.GetMovieById(reviewMV.MovieId);

            if (movie == null || _userManager.FindByIdAsync(reviewMV.UserId).Result == null)
                return false;

            var review = GetReviewById(reviewMV.Id);

            review.Content = reviewMV.Content;
            review.MovieScore = reviewMV.MovieScore;

            _db.Update(review);
            _db.SaveChanges();

            _movieManager.CalculateAvarageScore(movie);

            return true;
        }

        public bool Delete(int? Id)
        {
            if (Id == null || Id <= 0)
                return false;

            var review = _db.Reviews.SingleOrDefault(x => x.Id == Id);

            if (review == null)
                return false;

            _db.Reviews.Remove(review);
            _db.SaveChanges();

            _movieManager.CalculateAvarageScore(_movieManager.GetMovieById(review.MovieId));

            return true;
        }

        public static IEnumerable<ReviewMV> ToReviewMVs(IEnumerable<Review> reviews, ApplicationDbContext db)
        {
            List<ReviewMV> reviewMVs = new List<ReviewMV>();

            foreach (var review in reviews)
            {
                reviewMVs.Add(new ReviewMV()
                {
                    Id = review.Id,
                    UserId = review.UserId,
                    UserName = db.Users.Single(x => x.Id == review.UserId).UserName,
                    Content = review.Content,
                    Rating = review.MovieScore
                });
            }

            return reviewMVs;
        }

        public Review? GetReviewById(int? id) => _db.Reviews.SingleOrDefault(x => x.Id == id);
        public IEnumerable<Review>? GetReviewsByMovieId(int? movieId) => _db.Reviews.Where(x => x.MovieId == movieId);
    }
}
