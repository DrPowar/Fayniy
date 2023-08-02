using Microsoft.AspNetCore.Identity;

namespace CoursesStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string name { get; set; }
        public string password { get; set; }
    }
}
