using Microsoft.AspNetCore.Identity;

namespace Cial.Models
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
    }
}
