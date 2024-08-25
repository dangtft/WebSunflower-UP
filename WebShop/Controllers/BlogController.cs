using Microsoft.AspNetCore.Mvc;
using WebShop.Interface;
using WebShop.Models;
using WebShop.Repository;
using WebShop.ViewModel;

namespace WebShop.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        public BlogController(IBlogRepository blogRepository,ICommentRepository commentRepository)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
        }

        public IActionResult Index(int pg = 1)
        {
            var blogs = _blogRepository.GetAllBlog(pg);
            int recsCount = _blogRepository.GetAllBlog().Count();

            var pager = new Paginate(recsCount, pg, 10);
            ViewBag.Pager = pager;
            var model = new BlogVM
            {
                Blogs = blogs,
            };
            return View(model);
        }

        public IActionResult Detail(int bBlogId)
        {
            var blog = _blogRepository.GetBlogById(bBlogId);
            var comments = _blogRepository.GetCommentsForBlog(bBlogId);
            int totalCommentsCount = _commentRepository.GetTotalCommentsCountForBlog(bBlogId);

            var model = new BlogVM
            {
                Blog = blog,
                Comments = comments,
                Comment = new Comment { BlogId = bBlogId },
                TotalCommentsCount = totalCommentsCount
            };
            return View(model);
        }
    }
}
