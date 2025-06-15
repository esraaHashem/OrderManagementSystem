using OrderManagementSystem.Core;

namespace OrderManagementSystem.Test
{
    public static class TestDataGenerator
    {
        public static List<Order> GetOrders(int howManyOrders, int overHowManyYears)
        {
            var orders = new List<Order>();
            var now = DateTime.UtcNow;
            var startDate = DateTime.UtcNow.AddYears(-overHowManyYears);
            var endDate = DateTime.UtcNow;

            var random = new Random();
            var timeSpan = endDate - startDate;

            for (int i = 0; i < howManyOrders; i++)
            {
                var randomDays = random.NextDouble() * timeSpan.TotalDays;
                var orderDate = startDate.AddDays(randomDays);

                orders.Add(new Order
                {
                    OrderDate = orderDate
                });
            }

            return orders;
        }
    }
}