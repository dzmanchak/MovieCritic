using Authorization.Models;

namespace Authorization.ModelViews
{
    public class DetailedMovieMV
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string MovieDescription { get; set; }
        public string Genre { get; set; }
        public float AvarageUserScore { get; set; }
        public IEnumerable<ReviewMV> Reviews { get; set; }
    }
}
