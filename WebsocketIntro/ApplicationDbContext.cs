using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users"); // Set table name explicitly
    }
}

public class User
{   
    [Key]
    public int id { get; set; }
    public string name { get; set; }
}