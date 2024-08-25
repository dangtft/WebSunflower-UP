using WebShop.Models;

namespace WebShop.Interface
{
    public interface ICommentRepository
    {
        Comment GetCommentById(int commentId);
        IEnumerable<Comment> GetCommentsByBlogId(int blogId);
        void AddComment(Comment comment, int blogId);

        int GetTotalCommentsCountForBlog(int blogId);
    }
}
