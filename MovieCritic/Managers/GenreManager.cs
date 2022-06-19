using Authorization.Data;
using Authorization.Models;

namespace Authorization.Managers
{
    public class GenreManager
    {
        private readonly ApplicationDbContext _db;
        public GenreManager(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(string genreName)
        {
            if (_db.Genres.SingleOrDefault(x => x.GenreName.Equals(genreName)) == null)
            {
                _db.Genres.Add(new Genre() { GenreName = genreName });
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public Genre? GetGenreById(int? id) => _db.Genres.SingleOrDefault(x => x.Id == id);
        public Genre? GetGenreByName(string? genreName) => _db.Genres.SingleOrDefault(x => x.GenreName.Equals(genreName));
        public IEnumerable<Genre> GetGenreList()
        {
            return _db.Genres;
        }
    }
}
