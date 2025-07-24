using Microsoft.EntityFrameworkCore;
using Dubaicharaja.Models;

namespace Dubaicharaja.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<ExpenseRecord> ExpenseRecords { get; set; }
        public DbSet<User> User { get; set; }
    }
}
