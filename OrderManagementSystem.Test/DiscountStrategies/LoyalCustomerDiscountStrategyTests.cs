using OrderManagementSystem.Application.Discounting;
using OrderManagementSystem.Core;

namespace OrderManagementSystem.Test.DiscountStrategies;

[TestClass]
public class LoyalCustomerDiscountStrategyTests
{
    [TestMethod]
    public void WhenApplyDiscountForLoyalCustomerWithNoRecentOrdersThen15PercentageDiscountIsApplied()
    {
        // Arrange
        decimal amount = 1000M;
        List<Order> orderHistory = new List<Order>();
        var sut = new LoyalCustomerDiscountStrategy();

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert
        Assert.AreEqual(850, discountedAmount);
    }

    [TestMethod]
    public void WhenApplyDiscountForLoyalCustomerWith45RecentOrdersThenOnlyBaseDiscountIsStillApplied()
    {
        // Arrange
        var sut = new LoyalCustomerDiscountStrategy();
        decimal amount = 100M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 49, overHowManyMonths: 12);

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        Assert.AreEqual(85M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForLoyalCustomerWith50RecentOrdersThenExtra5PercentDiscountIsApplied()
    {
        // Arrange
        decimal amount = 100M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 50, overHowManyMonths: 12);
        var sut = new LoyalCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        // 100 * 0.85 = 85
        // 85 * (1 - 0.05) = 80.75
        Assert.AreEqual(80.75M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForLoyalCustomerWith50OrdersOverLast3YearsThenBaseDiscountIsOnlyApplied()
    {
        // Arrange
        decimal amount = 100M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 50, overHowManyMonths: 36);
        var sut = new LoyalCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        // only calcu : 100 * 0.85 = 85
        Assert.AreEqual(85M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForLoyalCustomerWith300OrdersOverLastYearThenExtraDiscount15PercentIsApplied()
    {
        // Arrange
        decimal amount = 2000M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 300, overHowManyMonths: 12);
        var sut = new LoyalCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        //  calcu base : 2000 * 0.85 = 1700
        //        extra 1700 * extra (.85) = 1445
        Assert.AreEqual(1445M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForLoyalCustomerWith300OrdersOverLast3YearsThenExtraDiscount5PercentIsApplied()
    {
        // Arrange
        decimal amount = 2000M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 300, overHowManyMonths: 36);
        var sut = new LoyalCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        //  calcu base : 2000 * 0.85 = 1700
        //        extra : 1700 * extra (.95) = 1615
        Assert.AreEqual(1615M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForLoyalCustomerWith1000OrdersOverLastYearThenMaxExtraDiscount20PercentIsApplied()
    {
        // Arrange
        decimal amount = 2000M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 1000, overHowManyMonths: 12);
        var sut = new LoyalCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        //  calcu base : 2000 * 0.85 = 1700
        //        extra 1700 * extra (.8) = 1360
        Assert.AreEqual(1360M, result);
    }
}