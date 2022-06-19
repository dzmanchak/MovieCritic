using System.ComponentModel.DataAnnotations;

namespace Authorization.ModelViews
{
    public class ModifyMovieMV
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string MovieTitle { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "Description is too long")]
        [Display(Name = "Description")]
        public string MovieDescription { get; set; }
        [Required]
        public int MovieGenre { get; set; }
    }
}
