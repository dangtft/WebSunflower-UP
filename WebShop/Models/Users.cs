using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class Users 
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Hãy nhập Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Hãy nhập Email"),EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage = "Hãy nhập Password")]
        public string Password { get; set; }
    }
}
