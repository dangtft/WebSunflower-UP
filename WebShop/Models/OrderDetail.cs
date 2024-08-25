namespace WebShop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OrderCode { get; set; }
        public int OrderId { get; set; }
        public int ProductId {  get; set; }
        public decimal Price { get; set; }  
        public int Quantity { get; set; }
        public Product? Product { get; set; }
        public Order? Order { get; set; }

    }
}
