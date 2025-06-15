using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSystem.Application.Discounting;
using OrderManagementSystem.Application.Exceptions;
using OrderManagementSystem.Core;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Application.Order
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly OrderManagementDBContext _orderManagementDBContext;

        public OrderService(ILogger<OrderService> logger, OrderManagementDBContext orderManagementDBContext)
        {
            _logger = logger;
            _orderManagementDBContext = orderManagementDBContext;
        }

        public decimal CalculateTotalAmount(Core.Order order)
        {
            _logger.LogInformation($"------------------ Starting call {nameof(CalculateTotalAmount)} ----------------");

            if (order?.Customer == null)
            {
                throw new ArgumentException($"Exception in {nameof(CalculateTotalAmount)} : Customer is null");
            }

            IDiscountStrategy discountStrategy = GetDiscountStrategy(order.Customer.CustomerType);
            DiscountContext discountContext = new DiscountContext(discountStrategy);
            var discountedAmount = discountContext.GetDiscountedPrice(order.TotalAmount, order.Customer.Orders.ToList());

            _logger.LogInformation($"------------------ Finished calling {nameof(CalculateTotalAmount)} ----------------");

            //to prevent negative
            return Math.Max(0, discountedAmount);
        }

        //
        public async Task<OrderAnalytics> GetOrderAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _orderManagementDBContext.Orders.Include(x => x.Items).AsNoTracking().AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate <= endDate.Value);
            }

            var orders = await query.ToListAsync();

            var deliveredOrders = orders.Where(o => o.OrderStatus == OrderStatus.Delivered).ToList();

            return new OrderAnalytics
            {
                AverageOrderValue = orders.Any() ? orders.Average(x => x.TotalAmount) : 0,
                AverageFulfillmentTime =
                deliveredOrders.Any() ? TimeSpan.FromMilliseconds(deliveredOrders.Average(o => (o.LastModified - o.OrderDate).TotalMilliseconds))
                                     : TimeSpan.Zero,
                TotalOrders = orders.Count,
                CompletedOrders = deliveredOrders.Count
            };
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            _logger.LogInformation($"----- Starting call of {nameof(UpdateOrderStatusAsync)}- {nameof(OrderService)}");
            var orderToUpdate = await _orderManagementDBContext.Orders.FindAsync(orderId);
            if (orderToUpdate is null)
            {
                _logger.LogError($"Error Occurred: Order with Id {orderId} couldnot be found");
                throw new OrderNotFoundException($"Error Occurred: Order with Id {orderId} couldnot be found");
            }

            if (!IsValidTransition(orderToUpdate.OrderStatus, newStatus))
            {
                _logger.LogError($"Error Occurred: Cannot change status from {orderToUpdate.OrderStatus} to {newStatus}");
                throw new InvalidOrderStatusTransitionException(
                    $"Cannot change status from {orderToUpdate.OrderStatus} to {newStatus}");
            }

            try
            {
                orderToUpdate.OrderStatus = newStatus;
                orderToUpdate.LastModified = DateTime.UtcNow;
                var result = await _orderManagementDBContext.SaveChangesAsync();

                _logger.LogInformation($"----- Finished calling {nameof(UpdateOrderStatusAsync)}- {nameof(OrderService)}");
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: {ex} - {nameof(UpdateOrderStatusAsync)}- {nameof(OrderService)}");
                throw;
            }
        }

        private bool IsValidTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            return newStatus switch
            {
                OrderStatus.Pending => currentStatus == OrderStatus.Created,
                OrderStatus.Processing => currentStatus == OrderStatus.Pending,
                OrderStatus.Shipped => currentStatus == OrderStatus.Processing,
                OrderStatus.Delivered => currentStatus == OrderStatus.Shipped,
                OrderStatus.Cancelled => currentStatus != OrderStatus.Delivered,
                _ => false
            };
        }

        private IDiscountStrategy GetDiscountStrategy(CustomerType type)
        {
            return type switch
            {
                CustomerType.New => new NewCustomerDiscountStrategy(),
                CustomerType.Loyal => new LoyalCustomerDiscountStrategy(),
                CustomerType.Regular => new RegularCustomerDiscountStrategy(),
                CustomerType.VIP => new VIPCustomerDiscountStrategy(),
                _ => throw new ArgumentException("Invalid customer type")
            };
        }
    }
}