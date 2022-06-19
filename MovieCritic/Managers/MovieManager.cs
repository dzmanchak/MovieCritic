using Authorization.Data;
using Authorization.Models;
using Authorization.ModelViews;

namespace Authorization.Managers
{
    public class MovieManager
    {
        private readonly ApplicationDbContext _db;

        public MovieManager(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool Create(ModifyMovieMV movie)
        {
            if (_db.Genres.SingleOrDefault(x => movie.MovieGenre == x.Id) == null)
                return false;

            if (_db.Movies.SingleOrDefault(x => x.MovieTitle.Equals(movie.MovieTitle)) == null)
            {
                _db.Movies.Add(new Movie() { MovieTitle = movie.MovieTitle, Description = movie.MovieDescription, GenreId = movie.MovieGenre, AvarageUserScore = 0 });
                _db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Delete(int? id)
        {
            if (id == null || id <= 0)
                return false;

            var movie = _db.Movies.SingleOrDefault(x => x.Id == id);

            if(movie == null)
                return false;

            var reviews = _db.Reviews.Where(x => x.MovieId == id).ToList();
            if (reviews.Count > 0)
                _db.Reviews.RemoveRange(reviews);

            _db.Movies.Remove(movie);
            _db.SaveChanges();

            return true;
        }

        public bool Edit(ModifyMovieMV movieMV)
        {
            if (movieMV.Id <= 0)
                return false;

            if (_db.Genres.SingleOrDefault(x => movieMV.MovieGenre == x.Id) == null)
                return false;

            var movie = _db.Movies.SingleOrDefault(x => x.Id == movieMV.Id);

            if ( movie != null)
            {
                movie.MovieTitle = movieMV.MovieTitle;
                movie.Description = movieMV.MovieDescription;
                movie.GenreId = movieMV.MovieGenre;

                _db.Update(movie);
                _db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool CalculateAvarageScore(Movie movie)
        {
            var reviews = _db.Reviews.Where(x => x.MovieId == movie.Id);
            int ratingSum = 0;

            foreach (var review in reviews)
                ratingSum += review.MovieScore;

            if(reviews.Count() != 0)
                movie.AvarageUserScore = ratingSum / reviews.Count();
            else
                movie.AvarageUserScore = 0;


            _db.Update(movie);
            _db.SaveChanges();

            return true;
        }

        #region Get
        public Movie? GetMovieById(int? id) => _db.Movies.SingleOrDefault(x => x.Id == id);
        public Movie? GetMovieByTitle(string? title) => _db.Movies.SingleOrDefault(x => x.MovieTitle.Equals(title));
        public DetailedMovieMV? Details(int? id)
        {
            if (id == null || id <= 0)
                return null;

            var movie = _db.Movies.SingleOrDefault(x => x.Id == id);

            if (movie == null)
                return null;

            var reviews = ReviewManager.ToReviewMVs(_db.Reviews.Where(x => x.MovieId == id), _db); ;

            DetailedMovieMV detailedMovieMV = new DetailedMovieMV()
            {
                Id = (int)id,
                MovieTitle = movie.MovieTitle,
                MovieDescription = movie.Description,
                Genre = _db.Genres.Single(x => x.Id == movie.GenreId).GenreName,
                AvarageUserScore = movie.AvarageUserScore,
                Reviews = reviews
            };

            return detailedMovieMV;
        }
        public IEnumerable<MovieMV> GetMovieList()
        {
            var movies = _db.Movies.ToList();
            List<MovieMV> result = new List<MovieMV>();

            foreach (var movie in movies)
            {
                result.Add(new MovieMV() { Id = movie.Id, Title = movie.MovieTitle, Genre = _db.Genres.SingleOrDefault(x => x.Id == movie.GenreId).GenreName });
            }

            return result;
        }
        #endregion
    }
}
