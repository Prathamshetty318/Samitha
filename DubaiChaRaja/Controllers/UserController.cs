using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace DubaiChaRaja.Controllers
{
    private readonly string _connectionString;
    public class UserController:ControllerBase
    {
        [HttpPost]
        public IActionResult RaiseAccessRequest()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("UPDATE Users SET access_requested = true WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", userId.Value);
            cmd.ExecuteNonQuery();

            return Ok(new { message = "Access request raised." });
        }

    }
}
