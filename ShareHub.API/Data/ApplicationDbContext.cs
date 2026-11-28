using Microsoft.EntityFrameworkCore;
using ShareHub.Contracts.dtos;

namespace ShareHub.API.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ShareItem> ShareItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER CONFIGURATION
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasIndex(u => u.UserId)
                    .IsUnique();

                entity.Property(u => u.UserId)
                    .IsRequired();

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.PhoneNumber)
                    .IsRequired();

            });

            // CATEGORY CONFIGURATION
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Description)
                    .HasMaxLength(300);
            });

            // SHARE ITEM CONFIGURATION
            modelBuilder.Entity<ShareItem>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Description)
                    .HasMaxLength(300);

                // One User owns many ShareItems
                entity.HasOne(s => s.Owner)
                    .WithMany()
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                // One Category has many ShareItems
                entity.HasOne(s => s.Category)
                    .WithMany()
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // EXCHANGE CONFIGURATION
            modelBuilder.Entity<Exchange>(entity =>
            {
                entity.HasKey(e => e.Id);

                // One ShareItem can have many Exchanges
                entity.HasOne(e => e.Item)
                    .WithMany()
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                // One User can receive many Exchanges
                entity.HasOne(e => e.Receiver)
                    .WithMany()
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.ExchangeDate)
                    .IsRequired();

                entity.Property(e => e.IsCancelled)
                    .HasDefaultValue(false);
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastUpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
