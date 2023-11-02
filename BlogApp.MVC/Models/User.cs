using Microsoft.AspNetCore.Identity;

namespace BlogApp.MVC.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
