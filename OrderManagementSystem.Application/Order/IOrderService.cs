using OrderManagementSystem.Core;

namespace OrderManagementSystem.Application.Order
{
    public interface IOrderService
    {
        decimal CalculateTotalAmount(Core.Order order);

        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);

        Task<OrderAnalytics> GetOrderAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}