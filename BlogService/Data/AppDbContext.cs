using Microsoft.EntityFrameworkCore;
using BlogService.Models;

namespace BlogService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Blogs)
            .WithOne(u => u.User!)
            .HasForeignKey(u => u.UserId);

        modelBuilder
            .Entity<Blog>()
            .HasOne(b => b.User)
            .WithMany(b => b.Blogs)
            .HasForeignKey(b => b.Id);
    }
}
