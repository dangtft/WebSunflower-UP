using Microsoft.AspNetCore.Mvc;
using WebShop.Data;
using WebShop.Interface;
using WebShop.Models;
using WebShop.Repository;
using WebShop.ViewModel;

namespace WebShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IShoppingCartRepository shoppingCartRepository;
        private IProductRepository productRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }
        public IActionResult Index()
        {
            var items = shoppingCartRepository.GetAllShoppingCartItems();
            shoppingCartRepository.ShoppingCartItems = items;
            ViewBag.TotalCart = shoppingCartRepository.GetShoppingCartTotal();
            return View(items);
        }
        public JsonResult AddToShoppingCart(int pId)
        {
            var product = productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);
            if (product != null)
            {
                shoppingCartRepository.AddToCart(product);
                int cartCount = shoppingCartRepository.GetAllShoppingCartItems().Count();
                HttpContext.Session.SetInt32("CartCount", cartCount);

                return Json(new { success = true, message = $"{product.Name} has been added to your cart." });
               
            }
            return Json(new { success = false, message = "Product not found." });
        }
        public RedirectToActionResult RemoveFromShoppingCart(int pId)
        {
            var product = productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);
            if (product != null)
            {
                shoppingCartRepository.RemoveFromCart(product);
                int cartCount = shoppingCartRepository.GetAllShoppingCartItems().Count();
                HttpContext.Session.SetInt32("CartCount", cartCount);

                TempData["message"] = $"{product.Name} has been removed from your cart.";
                TempData["messageType"] = "warning";
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult IncreaseQuantity(int pId)
        {
            var product = productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);
            if (product != null)
            {
                shoppingCartRepository.IncreaseQuantity(product);
                int cartCount = shoppingCartRepository.GetAllShoppingCartItems().Count();
                HttpContext.Session.SetInt32("CartCount", cartCount);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult DecreaseQuantity(int pId)
        {
            var product = productRepository.GetAllProducts().FirstOrDefault(p => p.Id == pId);
            if (product != null)
            {
                shoppingCartRepository.DecreaseQuantity(product);
                int cartCount = shoppingCartRepository.GetAllShoppingCartItems().Count();
                HttpContext.Session.SetInt32("CartCount", cartCount);
            }
            return RedirectToAction("Index");
        }

    }
}
