using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        public string Content { get; set; }
        public int MovieScore { get; set; }
    }
}
