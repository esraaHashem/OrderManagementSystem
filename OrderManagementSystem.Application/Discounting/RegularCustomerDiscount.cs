namespace OrderManagementSystem.Application.Discounting;

public class RegularCustomerDiscount : IDiscountStrategy
{
    public decimal BaseDiscount => 0.95M;
    public int DiscountEligibilityMonths => -6;

    public decimal ApplyDiscount(decimal amount, List<Core.Order> orderHistory)
    {
        decimal baseDiscountedAmount = amount * BaseDiscount;
        decimal extraDiscountPercentage = ApplyExtraDiscount(orderHistory);
        decimal extraDiscountedAmount = amount * (extraDiscountPercentage / 100M);

        return Math.Max((baseDiscountedAmount - extraDiscountedAmount), amount - Constants.MAX_DISCOUNT);
    }

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
        throw new NotImplementedException();
    }
}