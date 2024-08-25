using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Interface;
using WebShop.Models;
using WebShop.Repository;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {        
        private readonly WebDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductRepository _productRepository;
        public ProductController(WebDbContext db,IWebHostEnvironment webHostEnvironment, IProductRepository productRepository)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _productRepository = productRepository;
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
        [HttpGet]
        public IActionResult ProductDetail(int id)
        {
            var product = _db.Products
                                 .Include(p => p.Brand)
                                 .Include(p => p.Category)
                                 .FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                return View("ProductDetail", product);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_db.Brands, "Id", "Name");
            var product = _productRepository.GetProductDetail(id);
            if (product != null)
            {
                return View("EditProduct", product);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_db.Brands, "Id", "Name", product.BrandId);

            if (ModelState.IsValid)
            {
                var existingProduct = await _db.Products.FindAsync(product.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.BrandId = product.BrandId;
                existingProduct.Slug = product.Name.Replace(" ", "-");

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    // Delete the old image
                    if (!string.IsNullOrEmpty(existingProduct.Image))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, existingProduct.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpload.CopyToAsync(fs);
                    }
                    existingProduct.Image = imageName;
                }

                try
                {
                    _db.Update(existingProduct);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Sửa sản phẩm thành công";
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
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
            if (!string.IsNullOrEmpty(product.Image))
            {
                string oldImagePath = Path.Combine(uploadsDir, product.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            try
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                TempData["success"] = "Xóa sản phẩm thành công";
                return RedirectToAction("GetAllProducts");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi xóa dữ liệu: " + ex.Message);
                return BadRequest();
            }
        }
    }
}
