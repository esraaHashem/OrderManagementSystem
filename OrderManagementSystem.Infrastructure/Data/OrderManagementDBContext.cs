using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core;

namespace OrderManagementSystem.Infrastructure.Data;

public class OrderManagementDBContext : DbContext
{
    public OrderManagementDBContext(DbContextOptions<OrderManagementDBContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //add seed data
        modelBuilder.Entity<Order>().Property(b => b.Id).ValueGeneratedOnAdd();

        var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John Doe", CustomerType = CustomerType.Regular },
                new Customer { Id = 2, Name = "Jane Smith", CustomerType = CustomerType.Loyal },
                new Customer { Id = 3, Name = "Mike Johnson", CustomerType = CustomerType.VIP },
                new Customer { Id = 4, Name = "Sarah Williams", CustomerType = CustomerType.New }
            };
        var now = DateTime.Now;
        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderStatus = OrderStatus.Delivered,
                OrderDate = now.AddDays(-10),
                LastModified = now.AddDays(-9)
            },
            new Order
            {
                Id = 2,
                CustomerId = 2,
                OrderStatus = OrderStatus.Delivered,
                OrderDate = now.AddDays(-8),
                LastModified = now.AddDays(-6)
            },
            new Order
            {
                Id = 3,
                CustomerId = 3,
                OrderStatus = OrderStatus.Delivered,
                OrderDate = now.AddDays(-5),
                LastModified = now.AddDays(-4)
            },
            new Order
            {
                Id = 4,
                CustomerId = 1,
                OrderStatus = OrderStatus.Shipped,
                OrderDate = now.AddDays(-3),
                LastModified = now.AddDays(-2)
            },
            new Order
            {
                Id = 5,
                CustomerId = 2,
                OrderStatus = OrderStatus.Pending,
                OrderDate = now.AddDays(-2),
                LastModified = now.AddDays(-2)
            },
            new Order
            {
                Id = 6,
                CustomerId = 3,
                OrderStatus = OrderStatus.Created,
                OrderDate = now.AddHours(-2),
                LastModified = now.AddHours(-2)
            }
        );

        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { Id = 1, OrderId = 1, Name = "Laptop", Price = 999.99m, Quantity = 1 },
            new OrderItem { Id = 2, OrderId = 1, Name = "Mouse", Price = 19.99m, Quantity = 2 },
            new OrderItem { Id = 3, OrderId = 2, Name = "Keyboard", Price = 89.99m, Quantity = 1 },
            new OrderItem { Id = 4, OrderId = 2, Name = "Monitor", Price = 399.99m, Quantity = 1 },
            new OrderItem { Id = 5, OrderId = 3, Name = "Headphones", Price = 199.99m, Quantity = 1 },
            new OrderItem { Id = 6, OrderId = 4, Name = "USB Hub", Price = 50.00m, Quantity = 3 },
            new OrderItem { Id = 7, OrderId = 5, Name = "Webcam", Price = 75.50m, Quantity = 1 },
            new OrderItem { Id = 8, OrderId = 6, Name = "External SSD", Price = 100.00m, Quantity = 2 }
        );
    }
}