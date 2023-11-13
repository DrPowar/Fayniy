using System.ComponentModel.DataAnnotations;

namespace CoursesStore.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
