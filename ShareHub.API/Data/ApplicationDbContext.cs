using Microsoft.EntityFrameworkCore;
using ShareHub.Contracts.dtos;

namespace ShareHub.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ShareItem> ShareItems { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
