using OrderManagementSystem.Application.Discounting;
using OrderManagementSystem.Core;

namespace OrderManagementSystem.Test;

[TestClass]
public class NewCustomerDicountTests
{
    [TestMethod]
    public void WhenApplyDiscountForNewCustomerWithNoRecentOrdersThen10PercentageDiscountIsApplied()
    {
        // Arrange
        decimal amount = 1000M;
        List<Order> orderHistory = new List<Order>();
        var sut = new NewCustomerDiscountStrategy();

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert
        Assert.AreEqual(900, discountedAmount);
    }

    [TestMethod]
    public void WhenNewCustomerPurchasesAre60OrdersOverLastYearThenNearestDiscountIs10Percentage()
    {
        // Arrange
        var sut = new NewCustomerDiscountStrategy();
        decimal amount = 200M;

        List<Order> orderHistory = TestDataGenerator.GetOrders(howManyOrders: 60, overHowManyYears: 0);

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert
        Assert.AreEqual(171M, discountedAmount);
    }

    [TestMethod]
    public void WhenVIPPurchasesAre100OrdersWithin3MonthsThenExtraDiscount20PercentageIsAdded()
    {
        // Arrange
        var sut = new NewCustomerDiscountStrategy();
        decimal amount = 200M;

        List<Order> orderHistory = TestDataGenerator.GetOrders(howManyOrders: 100, overHowManyYears: 0);

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert
        Assert.AreEqual(162M, discountedAmount);
    }

    [TestMethod]
    public void WhenNewCustomerPurchasesAre40OrdersRecentlyThenNoExtraDiscountIsApplied()
    {
        // Arrange
        var sut = new NewCustomerDiscountStrategy();
        decimal amount = 200M;

        List<Order> orderHistory = TestDataGenerator.GetOrders(howManyOrders: 40, overHowManyYears: 0);

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert
        Assert.AreEqual(180M, discountedAmount);
    }
}