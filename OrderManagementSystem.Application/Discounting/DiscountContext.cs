namespace OrderManagementSystem.Application.Discounting;

public class DiscountContext
{
    private readonly IDiscountStrategy _discountStrategy;

    public DiscountContext(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public decimal GetDiscountedPrice(decimal amount, List<Core.Order> orderHistory)
    {
        var discountedAmount = _discountStrategy.ApplyDiscount(amount, orderHistory);
        return discountedAmount;
    }
}