using Microsoft.AspNetCore.Mvc;
using Dubaicharaja.Models;
using Dubaicharaja.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;


namespace Dubaicharaja.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.User.FirstOrDefault(u => u.Username == model.Username & u.Password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetString("username", user.Username);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");


        }
    }
}


