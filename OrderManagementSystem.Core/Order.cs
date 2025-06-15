using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Core;

/// <summary>
/// represents order class in order management system.
/// </summary>
public class Order
{
    /// <summary>
    /// Unique identifier for the order.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// list of items in this order.
    /// </summary>
    [Required]
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    /// <summary>
    /// Current status of the order if its created,pending,shipped,delivered or cancelled.
    /// </summary>
    [Required]
    public OrderStatus OrderStatus { get; set; }

    /// <summary>
    /// Identifier of the customer who placed this order
    /// </summary>
    [Required]
    public int CustomerId { get; set; }

    /// <summary>
    /// Total amount of the order
    /// </summary>
    [Required]
    public decimal TotalAmount => Items.Sum(item => item.Price * item.Quantity);

    /// <summary>
    ///
    /// </summary>
    public Customer Customer { get; set; } = null!;

    /// <summary>
    /// the date the order placed.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// keep track of time when change happens to order.
    /// </summary>
    public DateTime LastModified { get; set; }
}