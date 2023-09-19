using Braintree;
using CoursesStore.Data;
using CoursesStore.Models;
using CoursesStore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace CoursesStore.Controllers
{
    public class CoursesController : Controller
    {
        private readonly CoursesStoreContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IBraintreeService _braintreeService;

        public CoursesController(CoursesStoreContext context, IWebHostEnvironment appEnvironment, IBraintreeService braintreeService)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _braintreeService = braintreeService;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var courses = from m in _context.Course
                          select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name!.Contains(searchString));
            }

            return View(await courses.ToListAsync());
        }

        public async Task<IActionResult> PurchaseCreate(int id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var gateway = _braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate();
            ViewBag.ClientToken = clientToken;

            var course = await _context.Course.FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var coursePurchaseVM = new CoursePurchaseVM
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                EffectCount = course.EffectCount,
                Nonce = "",
                Email = ""
            };

            return View(coursePurchaseVM);
        }

        [HttpPost]
        public IActionResult OrderProcessing(CoursePurchaseVM model)
        {
            var gateway = _braintreeService.GetGateway();
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(Convert.ToInt32(model.Price)),
                PaymentMethodNonce = model.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                EmailNewsletter.SendEmail("fayniystore16@gmail.com", "dgdd zwxv vkdj aeot", model.Email, model).GetAwaiter();

                return View("PurchaseSuccess");
            }
            else
            {
                return View("PurchaseFailure");
            }
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [Authorize]
        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                var graphics = new IFormFile[] { 
                    course.PreviewImage, 
                    course.MainVideo, 
                    course.CardVideo, 
                    course.BeforeExampleImage, 
                    course.AfterExampleImage};

                foreach (var item in graphics)
                {
                    if (item != null)
                    {
                        string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "Graphics", "Course", course.Name);
                        string uniqueFileName = item.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }
                    }
                }
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [Authorize]
        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Duration")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'CoursesStoreContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool CourseExists(int id)
        {
            return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private class EmailNewsletter
        {
            public static async Task SendEmail(string fromString, string password, string toString, CoursePurchaseVM coursePurchaseVM)
            {
                MailAddress from = new MailAddress(fromString, "FayniyStore");
                MailAddress to = new MailAddress(toString);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Посилання на товар";
                m.Body = "Письмо-тест 2 работы smtp-клиента";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(fromString, password);
                smtp.EnableSsl = true;
                try
                {
                    smtp.SendMailAsync(m);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
