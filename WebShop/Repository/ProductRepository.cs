using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebShop.Data;
using WebShop.Interface;
using WebShop.Models;

namespace WebShop.Repository
{
    public class ProductRepository:IProductRepository
    {
        private WebDbContext _db;
        public ProductRepository(WebDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Product> GetAllProducts(int pg = 1)
        {
            List<Product> products = _db.Products.ToList();

            const int pageSize = 10;

            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = products.Count();

            var pager = new Paginate(recsCount,pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = products.Skip(recSkip).Take(pager.PageSize).ToList();

            return data;
        }

        public Product? GetProductDetail(int id)
        {
            return _db.Products.FirstOrDefault(p => p.Id == id);
        }

       
    }
}
