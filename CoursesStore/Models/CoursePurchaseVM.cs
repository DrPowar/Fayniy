using Duende.IdentityServer.Models;
using System.ComponentModel.DataAnnotations;

namespace CoursesStore.Models
{
    public class CoursePurchaseVM : Course
    {
        public string Nonce { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Wrong format")]
        public string Email { get; set; }
    }
}
