using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(4, ErrorMessage = "Hãy nhập tên thương hiệu")]
        public string Name { get; set; }
        [Required, MinLength(4, ErrorMessage = "Hãy nhập mô tả thương hiệu")]
        public string Description { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
    }
}
