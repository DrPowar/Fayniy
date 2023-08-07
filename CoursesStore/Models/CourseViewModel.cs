using Microsoft.AspNetCore.Http;

namespace CoursesStore.Models
{
    public class CourseViewModel
    {
        public string Name { get; set; }
        public IFormFile? CourseImage { get; set; }
    }
}
