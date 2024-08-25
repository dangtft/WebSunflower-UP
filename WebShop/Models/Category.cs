using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required,MinLength(4,ErrorMessage ="Hãy nhập tên danh mục")]
        public string Name { get; set; }
        [Required, MinLength(4, ErrorMessage = "Hãy nhập mô tả danh mục")]
        public string Description { get; set; }
        public string Slug { get; set; }
        public string Status { get; set; }
    }

}
