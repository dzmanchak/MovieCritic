using Authorization.Managers;
using Authorization.Models;
using Authorization.ModelViews;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Data
{
    public class Seeder
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GenreManager _genreManager;
        private readonly MovieManager _movieManager;
        private readonly ReviewManager _reviewManager;

        public Seeder(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, GenreManager genreManager, MovieManager movieManager, ReviewManager reviewManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _genreManager = genreManager;
            _movieManager = movieManager;
            _reviewManager = reviewManager;
        }

        public void Seed()
        {
            SeedRoles(_roleManager);
            SeedAdmin(_userManager);
            SeedUsers(_userManager);
            SeedGenres();
            SeedMovies();
            SeedReviews();
        }

        private void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                roleManager.CreateAsync(new IdentityRole() { Name = "Admin", NormalizedName = "Admin".ToUpper() }).Wait();
            }
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                roleManager.CreateAsync(new IdentityRole() { Name = "User", NormalizedName = "User".ToUpper() }).Wait();
            }
        }

        private void SeedAdmin(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("root").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "root",
                    Email = "root@xyz.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "8R*2T@kXuwW,`Xy>").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }

        private void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("User1").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "User1",
                    Email = "user1@xyz.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "8R*2T@kXuwW,`Xy>").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }

            if (userManager.FindByNameAsync("User2").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "User2",
                    Email = "user2@xyz.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "8R*2T@kXuwW,`Xy>").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }

            if (userManager.FindByNameAsync("User3").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "User3",
                    Email = "user3@xyz.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "8R*2T@kXuwW,`Xy>").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
        }

        private void SeedGenres()
        {
            _genreManager.Create("Action");
            _genreManager.Create("Horror");
            _genreManager.Create("Fantasy");
            _genreManager.Create("Comedy");
        }

        private void SeedMovies()
        {
            _movieManager.Create(new ModifyMovieMV()
            {
                MovieTitle = "The Terminator",
                MovieDescription = "A cyborg is sent from the future to assassinate a young woman who is destined to give birth to a son who will become the key to saving humanity. Directed by James Cameron.",
                MovieGenre = _genreManager.GetGenreByName("Action").Id
            });
            _movieManager.Create(new ModifyMovieMV()
            {
                MovieTitle = "It",
                MovieDescription = "When children in town begin to disappear, a group of young kids is faced with their biggest fears as they square off against evil clown, Pennywise. Based on the Stephen King novel.",
                MovieGenre = _genreManager.GetGenreByName("Horror").Id
            });
            _movieManager.Create(new ModifyMovieMV()
            {
                MovieTitle = "The Lord Of The Rings",
                MovieDescription = "In the first part, The Lord of the Rings, a shy young hobbit named Frodo Baggins inherits a simple gold ring that holds the secret to the survival--or enslavement--of the entire world.",
                MovieGenre = _genreManager.GetGenreByName("Fantasy").Id
            });
            _movieManager.Create(new ModifyMovieMV()
            {
                MovieTitle = "Home Alone",
                MovieDescription = "When little Kevin McCallister is accidentally left behind when his family dashes off on a Christmas trip, he is left to defend his family's home from two bumbling burglars until the relatives return.",
                MovieGenre = _genreManager.GetGenreByName("Comedy").Id
            });
        }

        private void SeedReviews()
        {
            var movie_id = _movieManager.GetMovieByTitle("The Terminator").Id;

            var user = _userManager.FindByNameAsync("User1").Result;
            _reviewManager.Create(new ModifyReviewMV() { MovieId = movie_id, UserId = user.Id, Content = "Good film", MovieScore = 5 });
            user = _userManager.FindByNameAsync("User2").Result;
            _reviewManager.Create(new ModifyReviewMV() { MovieId = movie_id, UserId = user.Id, Content = "Best film", MovieScore = 5 });
            user = _userManager.FindByNameAsync("User3").Result;
            _reviewManager.Create(new ModifyReviewMV() { MovieId = movie_id, UserId = user.Id, Content = "Not bad", MovieScore = 4 });

            movie_id = _movieManager.GetMovieByTitle("It").Id;
            user = _userManager.FindByNameAsync("User1").Result;
            _reviewManager.Create(new ModifyReviewMV() { MovieId = movie_id, UserId = user.Id, Content = "Scary", MovieScore = 1 });
            user = _userManager.FindByNameAsync("User2").Result;
            _reviewManager.Create(new ModifyReviewMV() { MovieId = movie_id, UserId = user.Id, Content = "Wooow", MovieScore = 4 });
        }
    }
}
