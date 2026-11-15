using ClothingStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Clothing> Clothes => Set<Clothing>();
    public DbSet<ClothingSize> ClothingSizes => Set<ClothingSize>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // enum'ы храним в БД строками ("TShirt", "Manager", "Created")
        b.Entity<Clothing>().Property(x => x.Type).HasConversion<string>();
        b.Entity<Clothing>().Property(x => x.Price).HasPrecision(10, 2);
        b.Entity<User>().Property(x => x.Role).HasConversion<string>();
        b.Entity<Order>().Property(x => x.Status).HasConversion<string>();

        // логин должен быть уникальным
        b.Entity<User>().HasIndex(x => x.Login).IsUnique();
    }
}
