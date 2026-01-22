using Microsoft.EntityFrameworkCore;
using URLShort.Models;

namespace URLShort.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<UrlShortener> UrlShorteners => Set<UrlShortener>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UrlShortener>()
                .HasIndex(u => u.ShortenedUrl)
                .IsUnique();

            modelBuilder.Entity<UrlShortener>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
