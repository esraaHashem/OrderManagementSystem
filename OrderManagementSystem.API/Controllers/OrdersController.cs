using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Application.Order;
using OrderManagementSystem.Core;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementSystem.API.Controllers
{
    /// <summary>
    /// Order controller to manage order service end points.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("/api/Orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="orderService"></param>
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// endpoint that returns orders analytics (e.g., average value, fulfillment time).
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/analytics/orders")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully returned orders analytics ", typeof(OrderAnalytics))]
        public async Task<IActionResult> GetOrderAnalytics(DateTime? startDate, DateTime? endDate)
        {
            var result = await _orderService.GetOrderAnalyticsAsync(startDate, endDate);

            return Ok(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{orderId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully updated order status ", typeof(bool))]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var succussed = await _orderService.UpdateOrderStatusAsync(orderId, status);

            return Ok(succussed);
        }
    }
}