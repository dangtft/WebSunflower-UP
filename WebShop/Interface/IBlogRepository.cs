using WebShop.Models;

namespace WebShop.Interface
{
    public interface IBlogRepository
    {
        IEnumerable<Blog> GetAllBlog(int pg = 1);
        IEnumerable<Blog> GetHotBlog();
        Blog GetBlogById(int bBlogId);
        void AddBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void DeleteBlog(int bBlogId);
        IEnumerable<Comment> GetCommentsForBlog(int bBlogId);
        IEnumerable<Blog> SearchBlogsByName(string blogName);
    }
}
