namespace WebShop.Models
{
    public class ShoppingCartItem
    {
        public int ID { get; set; }
        public Product? Product { get; set; }
        public int Qty { get; set; }
        public string? ShoppingCartId { get; set; }
    }
}
