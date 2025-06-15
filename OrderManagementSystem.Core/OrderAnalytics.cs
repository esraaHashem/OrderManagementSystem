namespace OrderManagementSystem.Core
{
    public class OrderAnalytics
    {
        public decimal AverageOrderValue { get; set; }
        public TimeSpan AverageFulfillmentTime { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
    }
}