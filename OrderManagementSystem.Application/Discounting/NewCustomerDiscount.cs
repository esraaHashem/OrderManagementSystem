namespace OrderManagementSystem.Application.Discounting;

public class NewCustomerDiscount : IDiscountStrategy
{
    public decimal BaseDiscount => 0.90M;
    public int DiscountEligibilityMonths => -3;

    public decimal ApplyDiscount(decimal amount, List<Core.Order> orderHistory)
    {
        decimal baseDiscountedAmount = amount * BaseDiscount;
        decimal extraDiscountPercentage = ApplyExtraDiscount(orderHistory);
        decimal extraDiscountedAmount = amount * (extraDiscountPercentage / 100M);

        return Math.Max((baseDiscountedAmount - extraDiscountedAmount), amount - Constants.MAX_DISCOUNT);
    }

    //extra discount implemetation would be different depends on business logic
    private decimal ApplyExtraDiscount(List<Core.Order> orderHistory)
    {
        var discountEligibilityCutoff = DateTime.UtcNow.AddMonths(DiscountEligibilityMonths);

        var recentOrders = orderHistory.Where(order => order.OrderDate >= discountEligibilityCutoff)
            .ToList();

        decimal extraDiscount = DiscountRules.DISCOUNTS_OVER_PURCHASES_HISTORY
                                 .Where(x => recentOrders.Count >= x.Key)
                                 .OrderByDescending(x => x.Key)
                                 .Select(x => x.Value)
                                 .DefaultIfEmpty(0m)
                                 .First();
        return extraDiscount;
    }
}