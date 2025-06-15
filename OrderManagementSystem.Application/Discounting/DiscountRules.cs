using OrderManagementSystem.Core;

namespace OrderManagementSystem.Application.Discounting;

public class DiscountRules
{
    public static readonly Dictionary<CustomerType, Dictionary<int, decimal>> DISCOUNTS_OVER_PURCHASES_HISTORY =
        new Dictionary<CustomerType, Dictionary<int, decimal>>
        {
            {
                //if new cutomer got 50 orders over
                CustomerType.New, new Dictionary<int, decimal>
                {
                    { 50, 5 },
                    { 100, 10 }
                }
            },
            {
                CustomerType.Regular, new Dictionary<int, decimal>
                {
                    { 100, 5 },
                    { 300, 10 },
                    { 500, 15 }
                }
            },
            {
                CustomerType.Loyal, new Dictionary<int, decimal>
                {
                    { 50, 5 },
                    { 150, 10 },
                    { 300, 15 },
                    { 500, 20 }
                }
            },
            {
                CustomerType.VIP, new Dictionary<int, decimal>
                {
                    { 25, 5 },
                    { 75, 10 },
                    { 150, 15 },
                    { 300, 20 },
                    { 500, 25 }
                }
            }
        };
}