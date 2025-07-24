using DubaiChaRaja.Service;
using DubaiChaRaja.Models;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.Security.Cryptography;
using System.Text;

namespace DubaiChaRaja.Service
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void RegisterUser(string username, string email, string password)
        {
            var passwordHash = HashPassword(password);

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("INSERT INTO Users(Username, Email, PasswordHash, IsAdmin) VALUES (@Username, @Email, @PasswordHash, FALSE)", conn);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
            cmd.ExecuteNonQuery();
        }

        public void UpdatePassword(string email, string newPassword)
        {
            var hash = HashPassword(newPassword);

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("UPDATE Users SET PasswordHash = @hash WHERE Email = @Email", conn);
            cmd.Parameters.AddWithValue("@hash", hash);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.ExecuteNonQuery();
        }

        public bool UserExists(string username)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", conn);
            cmd.Parameters.AddWithValue("@Username", username);
            return (long)cmd.ExecuteScalar() > 0; // PostgreSQL returns long for COUNT
        }

        public bool EmailExists(string email)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);
            return (long)cmd.ExecuteScalar() > 0;
        }

        public User? ValidateUser(string username, string password)
        {
            var hash = HashPassword(password);

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM Users WHERE Username = @username AND PasswordHash = @password", conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", hash);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    IsAdmin = reader.GetBoolean(reader.GetOrdinal("IsAdmin")),
                    hasaccess = reader.GetBoolean(reader.GetOrdinal("hasaccess"))
                };
            }

            return null;
        }


      

        public void SaveVerificationCode(string email, string code)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"
                INSERT INTO PendingVerifications (Email, Code, Expiry)
                VALUES (@Email, @Code, NOW() + INTERVAL '10 minutes')
                ON CONFLICT (Email)
                DO UPDATE SET Code = EXCLUDED.Code, Expiry = NOW() + INTERVAL '10 minutes';
            ", conn);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Code", code);
            cmd.ExecuteNonQuery();
        }

        public bool IsCodeValid(string email, string code)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM PendingVerifications WHERE Email = @Email AND Code = @Code AND Expiry > NOW()", conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Code", code);

            var isValid = (long)cmd.ExecuteScalar() > 0;

            if (isValid)
            {
                var deleteCmd = new NpgsqlCommand("DELETE FROM PendingVerifications WHERE Email = @Email", conn);
                deleteCmd.Parameters.AddWithValue("@Email", email);
                deleteCmd.ExecuteNonQuery();
            }

            return isValid;
        }


        public string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        public void SetUserAccess(int userId, bool hasAccess)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand("UPDATE Users SET hasaccess = @access WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@access", hasAccess);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }


       
    }
}
