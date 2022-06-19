using System.ComponentModel.DataAnnotations;

namespace Authorization.ModelViews
{
    public class ModifyReviewMV
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Review is too long.")]
        public string Content { get; set; }
        [Required]
        [Range(0, 5, ErrorMessage = "{1}-{2}")]
        public int MovieScore { get; set; }
    }
}
