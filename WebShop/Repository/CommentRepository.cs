using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Interface;
using WebShop.Models;

namespace WebShop.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly WebDbContext _context;
        public CommentRepository(WebDbContext context)
        {
            _context = context;
        }

        public Comment? GetCommentById(int commentId)
        {
            return _context.Comments.FirstOrDefault(c => c.CommentId == commentId);
        }

        public IEnumerable<Comment> GetCommentsByBlogId(int blogId)
        {
            return _context.Comments.Where(c => c.BlogId == blogId).ToList();
        }

        public void AddComment(Comment comment, int blogId)
        {
            Comment newComment = new Comment
            {
                UserName = comment.UserName,
                Text = comment.Text,
                Email = comment.Email,
                CreatedAt = DateTime.UtcNow,
                BlogId = blogId,
            };

            _context.Comments.Add(newComment);
            _context.SaveChanges();
        }


        public int GetTotalCommentsCountForBlog(int bBlogId)
        {
            int totalCommentsCount = _context.Comments.Count(c => c.BlogId == bBlogId);
            return totalCommentsCount;
        }
    }
}
