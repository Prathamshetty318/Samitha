    using DubaiChaRaja.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System.IO;

namespace DubaiChaRaja.Controllers
{
    public class ImageController : Controller
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _env;

        public ImageController(IConfiguration config, IWebHostEnvironment env)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Welcome", "Home");

            return View();
        }

        [HttpPost("Image/Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null || file == null || file.Length == 0)
                    return BadRequest();

                string uploads = Path.Combine(_env.WebRootPath, "uploads", userId.ToString());
                Directory.CreateDirectory(uploads);

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string path = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                    await file.CopyToAsync(stream);

                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new NpgsqlCommand("INSERT INTO UserImages (UserId, FileName) VALUES (@UserId, @FileName)", conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@FileName", fileName);
                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Upload Error: " + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        

        [HttpGet("Image/List")]
        public async Task<IActionResult> List()
        {
            try
            {
                var list = new List<object>();

                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    var cmd = new NpgsqlCommand("SELECT UserId, FileName, UploadDate FROM UserImages ORDER BY UploadDate DESC", conn);

                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int uploaderId = reader.GetInt32(0);
                        string fileName = reader.GetString(1);
                        DateTime uploadDate = reader.GetDateTime(2);

                        list.Add(new
                        {
                            url = $"/uploads/{uploaderId}/{fileName}",
                            name = fileName,
                            date = uploadDate,
                            userId = uploaderId
                        });
                    }
                }

                return Json(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR in /Image/ListAll → " + ex.Message);
                return StatusCode(500, "Something broke: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] ImageDeleteRequest model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized();

            var fileName = Path.GetFileName(model.ImageUrl);
            var fullPath = Path.Combine(_env.WebRootPath, "uploads", userId.ToString(), fileName);

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("DELETE FROM UserImages WHERE FileName = @fn", conn);
                cmd.Parameters.AddWithValue("@fn", fileName);
                await cmd.ExecuteNonQueryAsync();
            }

            return Ok();
        }

        public class ImageDeleteRequest
        {
            public string ImageUrl { get; set; }
        }
    }
}
