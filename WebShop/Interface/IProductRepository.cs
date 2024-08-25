using WebShop.Models;

namespace WebShop.Interface
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts(int pg = 1);
        Product GetProductDetail(int id);

    }
}
