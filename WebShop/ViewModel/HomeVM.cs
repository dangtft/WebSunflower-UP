using WebShop.Models;

namespace WebShop.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
    }
}
