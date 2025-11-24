using Microsoft.EntityFrameworkCore;
using Lista5Zad5.Models;

namespace Lista5Zad5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;
    }
}