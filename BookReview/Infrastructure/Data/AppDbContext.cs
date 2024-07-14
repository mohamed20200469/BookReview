using BookReview.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        DbSet<Book> Books { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasData(new Book
            {
                Id = 1,
                Title = "The Book of Five Rings",
            }, new Book
            {
                Id = 2,
                Title = "The Art of War"
            });
        }
    }
}
