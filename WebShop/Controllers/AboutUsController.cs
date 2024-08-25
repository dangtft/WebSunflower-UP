using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
