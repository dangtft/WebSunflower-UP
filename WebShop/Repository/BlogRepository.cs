using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Interface;
using WebShop.Models;

namespace WebShop.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private WebDbContext _db;
        public BlogRepository(WebDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Blog> GetAllBlog(int pg = 1)
        {
            List<Blog> blogs = _db.Blogs.Include(b => b.Comments).ToList();

            const int pageSize = 10;

            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = blogs.Count();

            var pager = new Paginate(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = blogs.Skip(recSkip).Take(pager.PageSize).ToList();

            return data;
        }

        public IEnumerable<Comment> GetCommentsForBlog(int bBlogId)
        {
            return _db.Comments.Where(c => c.BlogId == bBlogId).ToList();
        }
        public Blog? GetBlogById(int bBlogId)
        {
            return _db.Blogs.Include(b => b.Comments).FirstOrDefault(b => b.BlogId == bBlogId);
        }

        public IEnumerable<Blog> GetHotBlog()
        {
            return _db.Blogs.Where(p => p.IsHot == true);
        }
        public IEnumerable<Blog> SearchBlogsByName(string blogName)
        {
            return _db.Blogs.Where(b => EF.Functions.Like(b.Title, $"%{blogName}%")).ToList();
        }

        public void AddBlog(Blog blog)
        {
            blog.CreatedAt = DateTime.Now;
            _db.Blogs.Add(blog);
            _db.SaveChanges();
        }
        public void UpdateBlog(Blog blog)
        {
            var existingBlog = _db.Blogs.Find(blog.BlogId);
            if (existingBlog != null)
            {
                existingBlog.ImgUrl = blog.ImgUrl;
                existingBlog.Title = blog.Title;
                existingBlog.Content = blog.Content;
                existingBlog.ContentDetail = blog.ContentDetail;
                existingBlog.Author = blog.Author;
                existingBlog.CreatedAt = blog.CreatedAt;
                existingBlog.IsHot = blog.IsHot;

                _db.SaveChanges();
            }
        }

        public void DeleteBlog(int id)
        {
            var blog = _db.Blogs.Find(id);
            if (blog != null)
            {
                _db.Blogs.Remove(blog);
                _db.SaveChanges();
            }
        }
    }
}

