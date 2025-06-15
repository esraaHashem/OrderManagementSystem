using OrderManagementSystem.Core;

namespace OrderManagementSystem.Application.Discounting;

public class LoyalCustomerDiscountStrategy : IDiscountStrategy
{
    public decimal BaseDiscount => 0.85M;
    public int DiscountEligibilityMonths => -12;

    public decimal ApplyDiscount(decimal amount, List<Core.Order> orderHistory)
    {
        decimal baseDiscountedAmount = amount * BaseDiscount;

        decimal extraDiscountInPercentage = ApplyExtraDiscount(orderHistory);
        baseDiscountedAmount *= (1 - extraDiscountInPercentage / 100M);

        return baseDiscountedAmount;
    }

    //extra discount implemetation would be different depends on business logic
    private decimal ApplyExtraDiscount(List<Core.Order> orderHistory)
    {
        var discountEligibilityCutoff = DateTime.UtcNow.AddMonths(DiscountEligibilityMonths);

        var recentOrders = orderHistory.Where(order => order.OrderDate >= discountEligibilityCutoff)
            .ToList();

        if (!DiscountRules.DISCOUNTS_OVER_PURCHASES_HISTORY.TryGetValue(CustomerType.Loyal, out var discountTiers))
        {
            return 0m;
        }

        decimal extraDiscount = discountTiers.Where(x => recentOrders.Count >= x.Key)
                                 .OrderByDescending(x => x.Key)
                                 .Select(x => x.Value)
                                 .DefaultIfEmpty(0m)
                                 .First();
        return extraDiscount;
    }
}