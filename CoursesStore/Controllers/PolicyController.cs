using Microsoft.AspNetCore.Mvc;

namespace CoursesStore.Controllers
{
    public class PolicyController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View("Privacy policy");
        }
        public IActionResult Refund()

        {
            return View("Refund policy");
        }
        public IActionResult TermsOfService()
        {
            return View("TermsOfService");
        }
    }
}
