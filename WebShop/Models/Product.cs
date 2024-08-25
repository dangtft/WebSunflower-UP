using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop.Repository.Validation;

namespace WebShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(4, ErrorMessage = "Hãy nhập tên sản phẩm")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Hãy nhập mô tả  sản phẩm")]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Rating { get; set; }
        public string Image { get; set; } = "noimage.jpg";
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Image is required.")]
        [FileExtension(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        public IFormFile ImageUpload { get; set; }
    }
}
