using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System.Threading.Tasks;

namespace DubaiChaRaja.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _connectionString;

        public AdminController(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
                return RedirectToAction("Welcome", "Home");

            var Users = new List<UserSummary>();
            int totalImages = 0;
            int totalRecords = 0;

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var cmd = new NpgsqlCommand("SELECT Id, Username, Email,hasaccess FROM Users", conn);
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Users.Add(new UserSummary
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Email = reader.GetString(2),
                        hasaccess=reader.GetBoolean(3),
                        ImageCount = 0
                    });
                }
                reader.Close();

                foreach (var u in Users)
                {
                    var cmdImg = new NpgsqlCommand("SELECT COUNT(*) FROM UserImages WHERE UserId = @Id", conn);
                    cmdImg.Parameters.AddWithValue("@Id", u.Id);
                    u.ImageCount = Convert.ToInt32(await cmdImg.ExecuteScalarAsync());
                    totalImages += u.ImageCount;
                }

                var cmdRec = new NpgsqlCommand("SELECT COUNT(*) FROM UserImages", conn);
                totalRecords = Convert.ToInt32(await cmdRec.ExecuteScalarAsync());
            }

            ViewBag.Users = Users;
            ViewBag.TotalCount = Users.Count;
            ViewBag.TotalImages = totalImages;
            ViewBag.TotalRecords = totalRecords;

            return View();
        }

        public async Task<IActionResult> UserImages(int UserId)
        {
            Console.WriteLine("UserImages called for UserId: " + UserId);

            var list = new List<string>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT FileName FROM UserImages WHERE UserId = @UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add($"/uploads/{UserId}/{reader.GetString(0)}");
                }
            }

            ViewBag.Images = list;
            return View();
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                var cmd1 = new NpgsqlCommand("DELETE FROM UserImages WHERE UserId = @id", conn);
                cmd1.Parameters.AddWithValue("@id", id);
                await cmd1.ExecuteNonQueryAsync();

                var cmd2 = new NpgsqlCommand("DELETE FROM FestivalRecords WHERE UserId = @id", conn);
                cmd2.Parameters.AddWithValue("@id", id);
                await cmd2.ExecuteNonQueryAsync();

                var cmd3 = new NpgsqlCommand("DELETE FROM Users WHERE Id = @id", conn);
                cmd3.Parameters.AddWithValue("@id", id);
                await cmd3.ExecuteNonQueryAsync();
            }

            return RedirectToAction("Index", "Admin");
        }


        [HttpPost]
        public async Task<IActionResult> ToggleAccess(int id, bool access)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand("UPDATE Users SET hasaccess = @access WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@access", access);
                int rows = await cmd.ExecuteNonQueryAsync();

                if (rows > 0)
                    return Ok();
            }

            return NotFound();
        }




        public class UserSummary
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public int ImageCount { get; set; }
            public bool hasaccess { get; set; }
        }
    }
}
