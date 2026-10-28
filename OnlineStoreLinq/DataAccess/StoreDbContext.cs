using Microsoft.EntityFrameworkCore;
using OnlineStoreLinq.Models;

namespace OnlineStoreLinq.DataAccess;

public class StoreDbContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
             @"Server=localhost;Database=OnlineStoreLinq;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}