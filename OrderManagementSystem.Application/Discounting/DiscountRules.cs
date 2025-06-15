namespace OrderManagementSystem.Application.Discounting;

public class DiscountRules
{
    public static readonly Dictionary<int, decimal> DISCOUNTS_OVER_PURCHASES_HISTORY = new Dictionary<int, decimal>
                    {
                        { 100 , 5 },
                        { 300 , 10 },
                        { 700 , 20 }
                    };
}