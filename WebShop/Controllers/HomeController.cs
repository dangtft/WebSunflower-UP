using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebShop.Interface;
using WebShop.Models;
using WebShop.ViewModel;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository productRepository;
        private IBlogRepository blogRepository;
        public HomeController(IProductRepository productRepository,IBlogRepository blogRepository) 
        { 
            this.productRepository = productRepository;
            this.blogRepository = blogRepository;
        }
        public IActionResult Index()
        {
            var viewModel = new HomeVM
            {
                Blogs = blogRepository.GetAllBlog(),
                Products = productRepository.GetAllProducts()
            };
            return View(viewModel);
        }

    }
}
