using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Core;

/// <summary>
/// class represent order item in order management system.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Unique identifier for the order item
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// related order id.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// how many items has customer ordered.
    /// </summary>
    [Required]
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// how much that item costs.
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// The identifier of the order item.
    /// </summary>
    [Required]
    public string ItemNumber { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public string Description { get; set; } = string.Empty;
}