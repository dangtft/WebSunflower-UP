using Microsoft.AspNetCore.Identity;

namespace WebShop.Models
{
    public class AppUser : IdentityUser
    {
        public string Occupation { get; set; }
       
    }
}
