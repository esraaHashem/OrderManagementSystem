namespace OrderManagementSystem.Application.Discounting;

public interface IDiscountStrategy
{
    decimal BaseDiscount { get; }
    int DiscountEligibilityMonths { get; }

    decimal ApplyDiscount(decimal amount, List<Core.Order> orderHistory);
}