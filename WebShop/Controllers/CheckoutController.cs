using WebShop.Areas.Admin.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebShop.Data;
using WebShop.Models;
using WebShop.Interface;
using WebShop.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly WebDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CheckoutController(WebDbContext context,IEmailSender emailSender,IShoppingCartRepository shopping, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _emailSender = emailSender;
            _shoppingCartRepository = shopping;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var ordercode = Guid.NewGuid().ToString();
                // Tạo đối tượng Order
                var order = new Order
                {
                    OrderCode = ordercode,
                    UserName = userEmail,
                    Status = 1,
                    CreatedDate = DateTime.Now,
                    FullName = model.FullName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber
                };

                // Thêm đơn hàng vào cơ sở dữ liệu
                _context.Add(order);
                _context.SaveChanges();

                // Lấy danh sách sản phẩm trong giỏ hàng
                var cartItems = _shoppingCartRepository.GetAllShoppingCartItems();

                // Tạo chi tiết đơn hàng cho mỗi sản phẩm
                foreach (var item in cartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        OrderCode = ordercode,
                        UserName = userEmail,
                        ProductId = item.Product.Id,
                        Price = item.Product.Price,
                        Quantity = item.Qty
                    };

                    // Thêm chi tiết đơn hàng vào cơ sở dữ liệu
                    _context.Add(orderDetail);
                }

                // Lưu tất cả các thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                // Xóa giỏ hàng sau khi đặt hàng thành công
                _shoppingCartRepository.ClearCart();

                // Thực hiện gửi email
                // await _emailSender.SendEmailAsync(userEmail, "Order Confirmation", "Your order has been placed successfully.");
                return RedirectToAction("Index", "ShoppingCart");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllOrderCompleted()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = _context.Orders
                                 .Include(o => o.OrderDetails)
                                 .ThenInclude(od => od.Product)
                                 .Where(o => o.UserName == userEmail && o.Status == 1)
                                 .OrderByDescending(o => o.CreatedDate)
                                 .ToList();

            return View(orders);
        }
    }
}
