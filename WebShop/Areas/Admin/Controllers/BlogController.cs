using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly WebDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BlogController(WebDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> GetAllBlogs()
        {
            return View(await _db.Blogs.OrderByDescending(p => p.BlogId).ToListAsync());
        }

        public IActionResult CreateBlog()
        {
            return View(new Blog());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(Blog blog)
        {
            if (ModelState.IsValid)
            {
                if (blog.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/blogs");
                    string imageName = Guid.NewGuid().ToString() + "_" + blog.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await blog.ImageUpload.CopyToAsync(fs);
                    }
                    blog.ImgUrl = imageName;
                }
                else
                {
                    ModelState.AddModelError("ImageUpload", "Image is required.");
                    return View(blog);
                }

                try
                {
                    _db.Add(blog);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Thêm blog thành công";
                    return RedirectToAction("GetAllBlos");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu: " + ex.Message);
                    return View(blog);
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

        public async Task<IActionResult> BlogDetail(int id)
        {
            var blog = await _db.Blogs
                .Include(b => b.Comments)
                .FirstOrDefaultAsync(b => b.BlogId == id);

            if (blog != null)
            {
                return View(blog);
            }
            return NotFound();
        }

        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (blog != null)
            {
                return View(blog);
            }
            return NotFound();
        }

        [HttpPost, ActionName("DeleteBlog")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (blog != null)
            {
                // Optionally delete the image file from the server
                if (!string.IsNullOrEmpty(blog.ImgUrl))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "media/blogs", blog.ImgUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _db.Blogs.Remove(blog);
                await _db.SaveChangesAsync();
                TempData["success"] = "Blog deleted successfully";
                return RedirectToAction("GetAllBlos");
            }
            return NotFound();
        }
        public async Task<IActionResult> EditBlog(int id)
        {
            var blog = await _db.Blogs.FindAsync(id);
            if (blog != null)
            {
                return View(blog);
            }
            return NotFound();
        }

        
        [HttpPost]
        public async Task<IActionResult> EditBlog(int id, Blog blog)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingBlog = await _db.Blogs.FindAsync(id);
                    if (existingBlog == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existingBlog.Title = blog.Title;
                    existingBlog.Content = blog.Content;
                    existingBlog.ContentDetail = blog.ContentDetail;
                    existingBlog.Author = blog.Author;
                    existingBlog.IsHot = blog.IsHot;

                    if (blog.ImageUpload != null)
                    {
                        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/blogs");
                        string imageName = Guid.NewGuid().ToString() + "_" + blog.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await blog.ImageUpload.CopyToAsync(fs);
                        }
                        existingBlog.ImgUrl = imageName;
                    }

                    _db.Update(existingBlog);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Blog updated successfully";
                    return RedirectToAction("GetAllBlogs");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(blog);
        }

        private bool BlogExists(int id)
        {
            return _db.Blogs.Any(e => e.BlogId == id);
        }
    }
}
