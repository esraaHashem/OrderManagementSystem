namespace OrderManagementSystem.Core;

/// <summary>
/// Represents the possible statuses of an order
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Order has been created but not processed
    /// </summary>
    Created,

    /// <summary>
    /// Order is pending.
    /// </summary>
    Pending,

    /// <summary>
    /// order is being processed, getting items from warehouse for example
    /// </summary>
    Processing,

    /// <summary>
    /// Order has been shipped
    /// </summary>
    Shipped,

    /// <summary>
    /// Order has been delivered
    /// </summary>
    Delivered,

    /// <summary>
    /// Order has been cancelled
    /// </summary>
    Cancelled
}