using Microsoft.EntityFrameworkCore;
using ShareHub.Contracts.dtos;

namespace ShareHub.API.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ShareItem> ShareItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
    }
}
