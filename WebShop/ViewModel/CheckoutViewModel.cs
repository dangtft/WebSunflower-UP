using System.ComponentModel.DataAnnotations;

namespace WebShop.ViewModel
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Address is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }
    }
}
