using WebShop.Models;

namespace WebShop.ViewModel
{
    public class BlogVM
    {
        public IEnumerable<Blog> Blogs { get; set; }
        public Blog Blog { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public Comment Comment { get; set; }

        public int TotalCommentsCount { get; set; }
    }
}
