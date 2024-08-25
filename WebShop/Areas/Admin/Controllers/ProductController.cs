using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Interface;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {        
        private readonly WebDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(WebDbContext db,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllProducts()
        {
            return View(await _db.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }

        public IActionResult CreateProduct()
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_db.Brands, "Id", "Name");
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name",product.CategoryId);
            ViewBag.Brands = new SelectList(_db.Brands, "Id", "Name",product.BrandId);

            if(ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ","-");
                var slug = await _db.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if(slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã tồn tại");
                    return View(product);
                }


                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }
                else
                {
                    ModelState.AddModelError("ImageUpload", "Image is required.");
                    return View(product);
                }


                try
                {
                    _db.Add(product);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Thêm sản phẩm thành công";
                    return RedirectToAction("GetAllProducts");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu: " + ex.Message);
                    return View(product);
                }
            }
            else
            {
                TempData["error"] = "Dữ liệu có thể bị lỗi ở một vài chỗ";
                List<string> errors = new List<string>();
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors); 
                return BadRequest(errorMessage);
            }

        }
    }
}
