namespace Authorization.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string Description { get; set; }

        public int GenreId { get; set; }
        public virtual Genre genre { get; set; }

        public float AvarageUserScore { get; set; }
    }
}
