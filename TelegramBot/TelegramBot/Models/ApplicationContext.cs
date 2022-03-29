using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<ImagesTags> ImagesTags { get; set; } = null!;

        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Image>()
                    .HasMany(c => c.Tags)
                    .WithMany(s => s.Images)
                    .UsingEntity<ImagesTags>(
                j => j
                        .HasOne(pt => pt.Tag)
                        .WithMany(t => t.ImagesTags)
                        .HasForeignKey(pt => pt.TagId),
                j => j
                        .HasOne(pt => pt.Image)
                        .WithMany(p => p.ImagesTags)
                        .HasForeignKey(pt => pt.ImageId));
        }
    }
}
