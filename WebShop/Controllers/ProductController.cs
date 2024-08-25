using Microsoft.AspNetCore.Mvc;
using WebShop.Interface;
using WebShop.Models;
using WebShop.Repository;

namespace WebShop.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }
        public IActionResult Shop(int pg = 1)
        {
            var products = _productRepository.GetAllProducts(pg);
            int recsCount = _productRepository.GetAllProducts().Count(); 

            var pager = new Paginate(recsCount, pg, 10);
            ViewBag.Pager = pager;

            return View(products);
        }
        public IActionResult Details(int id)
        {
            var product = _productRepository.GetProductDetail(id);
            return View(product);
        }
    }
}
