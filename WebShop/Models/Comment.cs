using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace WebShop.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "The Message field is required.")]
        public string? Text { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
    }
}
