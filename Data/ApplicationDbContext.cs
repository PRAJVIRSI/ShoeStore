using Microsoft.EntityFrameworkCore;
using ShoeStore_Group9.Models;

namespace ShoeStore_Group9.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => oi.OrderItemID);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.User)
                .WithMany(u => u.ShoppingCarts)
                .HasForeignKey(sc => sc.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.Product)
                .WithMany(p => p.ShoppingCarts)
                .HasForeignKey(sc => sc.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShoppingCart>()
                .HasKey(sc => sc.CartItemID);

            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, Username = "testuser", PasswordHash = "password", Email = "testuser@example.com", Role = "User" }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Sneakers", Description = "Comfortable sports shoes" },
                new Category { CategoryID = 2, CategoryName = "Formal", Description = "Formal shoes for office wear" },
                new Category { CategoryID = 3, CategoryName = "Boots", Description = "Durable boots for all terrains" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductID = 1, Name = "Nike Air Max", Description = "High-quality sports shoes", Price = 150, StockQuantity = 50, CategoryID = 1, ImageURL = "/Images/nike_air_max.jpg" },
                new Product { ProductID = 2, Name = "Adidas Ultraboost", Description = "Running shoes with superior comfort", Price = 180, StockQuantity = 30, CategoryID = 1, ImageURL = "/Images/adidas_ultraboost.jpg" },
                new Product { ProductID = 3, Name = "Oxford Classic", Description = "Elegant formal shoes", Price = 120, StockQuantity = 20, CategoryID = 2, ImageURL = "/Images/oxford_classic.jpg" },
                new Product { ProductID = 4, Name = "Timberland Boots", Description = "Durable and rugged boots", Price = 200, StockQuantity = 15, CategoryID = 3, ImageURL = "/Images/timberland_boots.jpg" }
            );
        }
    }
}
