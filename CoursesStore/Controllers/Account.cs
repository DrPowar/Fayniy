using Microsoft.AspNetCore.Mvc;

namespace CoursesStore.Controllers
{
    public class Account : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
