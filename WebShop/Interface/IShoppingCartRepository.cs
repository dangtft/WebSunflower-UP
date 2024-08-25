using WebShop.Models;

namespace WebShop.Interface
{
    public interface IShoppingCartRepository
    {
        void AddToCart(Product product);
        void RemoveFromCart(Product product);
        List<ShoppingCartItem> GetAllShoppingCartItems();
        void ClearCart();
        decimal GetShoppingCartTotal();
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        void IncreaseQuantity(Product product);
        void DecreaseQuantity(Product product);
    }
}
