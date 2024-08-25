namespace WebShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode {  get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
