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

        var orderItems = new List<OrderItem>
                {
                    new OrderItem { Id = 1,OrderId =1, ItemNumber = "ITEM-001", Name = "Laptop", Description = "15-inch, 16GB RAM, 512GB SSD", Price = 999.99m, Quantity = 1 },
                    new OrderItem { Id = 2,OrderId =1, ItemNumber = "ITEM-002", Name = "Mouse", Description = "Wireless Optical Mouse", Price = 19.99m, Quantity = 2 },
                    new OrderItem { Id = 3, OrderId = 2, ItemNumber = "ITEM-003", Name = "Keyboard", Description = "Mechanical Keyboard", Price = 89.99m, Quantity = 1 },
                    new OrderItem { Id = 4,OrderId = 2, ItemNumber = "ITEM-004", Name = "Monitor", Description = "27-inch 4K Monitor", Price = 399.99m, Quantity = 1 },
                    new OrderItem { Id = 5,OrderId = 3, ItemNumber = "ITEM-005", Name = "Headphones", Description = "Noise Cancelling Headphones", Price = 199.99m, Quantity = 1 }
                };

        var orders = new List<Order>
                {
                    new Order
                    {
                        Id = 1,
                        CustomerId = 1,
                        OrderStatus = OrderStatus.Delivered,
                        OrderDate = DateTime.Now.AddDays(-10),
                        Items = new List<OrderItem> { orderItems[0], orderItems[1] }
                    },
                    new Order
                    {
                        Id = 2,
                        CustomerId = 2,
                        OrderStatus = OrderStatus.Shipped,
                        OrderDate = DateTime.Now.AddDays(-5),
                        Items = new List<OrderItem> { orderItems[2], orderItems[3] }
                    },
                    new Order
                    {
                        Id = 3,
                        CustomerId = 3,
                        OrderDate = DateTime.Now.AddHours(-10),
                        OrderStatus = OrderStatus.Pending,
                        Items = new List<OrderItem> { orderItems[4] }
                    },

                    new Order
                    {
                        Id = 4,
                        CustomerId = 1,
                        OrderStatus = OrderStatus.Shipped,
                        OrderDate = DateTime.Now.AddDays(-1),
                        Items = new List<OrderItem> { orderItems[0], orderItems[1] }
                    }
                    ,
                    new Order
                    {
                        Id = 5,
                        CustomerId = 2,
                        OrderStatus = OrderStatus.Pending,
                        OrderDate = DateTime.Now.AddDays(-2),
                        Items = new List<OrderItem> { orderItems[2], orderItems[3] }
                    },
                    new Order
                    {
                        Id = 6,
                        OrderDate = DateTime.Now.AddHours(-2),
                        CustomerId = 3,
                        OrderStatus = OrderStatus.Created,
                        Items = new List<OrderItem> { orderItems[4] }
                    },
                };
    }
}