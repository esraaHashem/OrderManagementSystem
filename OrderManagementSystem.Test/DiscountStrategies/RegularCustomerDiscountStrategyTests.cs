using OrderManagementSystem.Application.Discounting;
using OrderManagementSystem.Core;

namespace OrderManagementSystem.Test.DiscountStrategies;

[TestClass]
public class RegularCustomerDiscountStrategyTests
{
    [TestMethod]
    public void WhenApplyDiscountForRegularlCustomerWithNoRecentOrdersThen5PercentageDiscountIsApplied()
    {
        // Arrange
        decimal amount = 1000M;
        List<Order> orderHistory = new List<Order>();
        var sut = new RegularCustomerDiscountStrategy();

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert
        //1000 * .95
        Assert.AreEqual(950, discountedAmount);
    }

    [TestMethod]
    public void WhenApplyDiscountForRegularlCustomerWith45RecentOrdersThenOnlyBaseDiscountIsStillApplied()
    {
        // Arrange
        var sut = new RegularCustomerDiscountStrategy();
        decimal amount = 1000M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 49, overHowManyMonths: 12);

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        Assert.AreEqual(950M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForRegularlCustomerWith100OrdersOverLastYearThenNoExtraDiscountIsApplied()
    {
        // Arrange
        decimal amount = 100M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 100, overHowManyMonths: 12);
        var sut = new RegularCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        // 100 * 0.95 = 95
        // 95 * (1 - 0.05) = 80.75
        Assert.AreEqual(95M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForRegularlCustomerWith200OrdersOver6MonthsThenExtraDiscount5PercentageIsApplied()
    {
        // Arrange
        decimal amount = 100M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 200, overHowManyMonths: 6);
        var sut = new RegularCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        // 100 * 0.95 = 95
        // 95 * (1 - 0.05) = 90.25
        Assert.AreEqual(90.25M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForRegularlCustomerWith300OrdersLast6MonthsThenExtraDiscount10PercentageIsApplied()
    {
        // Arrange
        decimal amount = 100M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 300, overHowManyMonths: 6);
        var sut = new RegularCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        // 100 * 0.95 = 95
        // 95 * (1 - 0.1) = 85.5
        Assert.AreEqual(85.5M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForRegularlCustomerWith500OrdersOverLast3MonthsThen15PercentageExtraDiscountIsApplied()
    {
        // Arrange
        decimal amount = 100M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 500, overHowManyMonths: 3);
        var sut = new RegularCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        // 100 * 0.95 = 95
        // 95 * .85
        Assert.AreEqual(80.75M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForRegularlCustomerWith500OrdersOverLastYearThen5PercentageExtraDiscountIsApplied()
    {
        // Arrange
        decimal amount = 100M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 500, overHowManyMonths: 12);
        var sut = new RegularCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        // 100 * 0.95 = 95
        // 95 * .95
        Assert.AreEqual(90.25M, result);
    }

    [TestMethod]
    public void WhenApplyDiscountForRegularlCustomerWith1000OrdersOver3MonthsThen15PercentageExtraDiscountIsApplied()
    {
        // Arrange
        decimal amount = 100M;
        var orders = TestDataGenerator.GetOrders(howManyOrders: 1000, overHowManyMonths: 6);
        var sut = new RegularCustomerDiscountStrategy();

        // Act
        var result = sut.ApplyDiscount(amount, orders);

        // Assert
        // 100 * 0.95 = 95
        // 95 * .85
        Assert.AreEqual(80.75M, result);
    }
}