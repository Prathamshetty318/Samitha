using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DubaiChaRaja.Models;
using DubaiChaRaja.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using Npgsql;



namespace DubaiChaRaja.Controllers
{


        public class AuthController : Controller
        {
            private readonly IUserService _userService;

            private readonly IEmailService _emailService;


            public AuthController(IUserService userService, IEmailService emailService)
            {
                _userService = userService;

                _emailService = emailService;

            }

            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Login(string username, string password, string email)
            {
                var user = _userService.ValidateUser(username, password);


            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                HttpContext.Session.SetInt32("hasaccess", user.hasaccess ? 1 : 0);

                if (user.IsAdmin)
                    return Json(new { redirect = "/Admin/Index" });
                else
                    return Json(new { redirect = "/Home/Index" });
            }
            else
            {
                Console.WriteLine("Invalid credentials for: " + username);
            }

                return Unauthorized("Invalid Credentials");
            }







            [HttpPost]
            public IActionResult SendCode(string email)
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest();

                string code = new Random().Next(100000, 999999).ToString();

                _userService.SaveVerificationCode(email, code);

                _emailService.SendResetEmail(email, code);

                Console.WriteLine($" Code for {email} is {code}");


                return Ok();
            }

            [HttpPost]
            public IActionResult VerifyCode(string email, string code)
            {
                bool valid = _userService.IsCodeValid(email, code);
                if (!valid) return BadRequest();
                return Ok();
            }




            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Register(string username, string email, string password)
            {
                var errors = new List<string>();


                if (_userService.UserExists(username))
                {
                    errors.Add("User name already exists");
                }

                if (_userService.EmailExists(email))
                {
                    errors.Add("Email Already Exists");
                }

                if (errors.Any())
                {
                    return Conflict(string.Join("&", errors));
                }

                _userService.RegisterUser(username, email, password);
                return Ok();
            }

            [HttpPost]
            public IActionResult ForgotPassword(string email,string newPassword)
            {
                _userService.UpdatePassword(email, newPassword);
                    return View("~/Views/Home/ForgotPassword.cshtml");

            }

        [HttpPost]
        public IActionResult RaiseAccessRequest()
        {
            var UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null) return Unauthorized();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("UPDATE Users SET access_requested = true WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", userId.Value);
            cmd.ExecuteNonQuery();

            return Ok(new { message = "Access request raised." });
        }


        public IActionResult Logout()
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Welcome", "Home");
                }
        }

    }