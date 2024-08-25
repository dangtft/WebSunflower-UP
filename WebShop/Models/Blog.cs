using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.Repository.Validation;

namespace WebShop.Models
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string? ImgUrl { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ContentDetail { get; set; }
        public string? Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsHot { get; set; }
        public List<Comment>? Comments { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Image is required.")]
        [FileExtension(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        public IFormFile ImageUpload { get; set; }
    }
}
