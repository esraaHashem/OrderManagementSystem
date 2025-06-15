using OrderManagementSystem.Application.Discounting;
using OrderManagementSystem.Core;

namespace OrderManagementSystem.Test;

[TestClass]
public class VIPCustomerDiscountTests
{
    [TestMethod]
    public void WhenApplyDiscountForVIPCustomerThen20PercentageDiscountIsApplied()
    {
        // Arrange
        decimal amount = 200M;
        List<Order> orderHistory = new List<Order>();
        var sut = new VIPCustomerDiscountStrategy();

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert : its should be 160 as it equals to 200 * 0.80M
        Assert.AreEqual(160M, discountedAmount);
    }

    [TestMethod]
    public void WhenVIPPurchasesAre100OrdersOverLastYearThenNearestDiscountIs10Percentage()
    {
        // Arrange
        var sut = new VIPCustomerDiscountStrategy();
        decimal amount = 200M;

        List<Order> orderHistory = TestDataGenerator.GetOrders(howManyOrders: 100, overHowManyYears: 1);

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert
        Assert.AreEqual(144M, discountedAmount);
    }

    [TestMethod]
    public void WhenVIPPurchasesAre100OrdersOverLastTwoYearThenNearestDiscountIs5Percentage()
    {
        // Arrange
        var sut = new VIPCustomerDiscountStrategy();
        decimal amount = 200M;  //200*0.80 = 160 , 160 * 1-(5/100) = 152

        List<Order> orderHistory = TestDataGenerator.GetOrders(howManyOrders: 100, overHowManyYears: 2);

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert
        Assert.AreEqual(152M, discountedAmount);
    }

    [TestMethod]
    public void WhenVIPPurchasesAre100OrdersOverLastThreeYearThenNoExtraDiscountIsApplied()
    {
        // Arrange
        var sut = new VIPCustomerDiscountStrategy();
        decimal amount = 200M;  //200*0.80 = 160 , 160 * 1-(5/100) = 152

        List<Order> orderHistory = TestDataGenerator.GetOrders(howManyOrders: 100, overHowManyYears: 4);

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert
        Assert.AreEqual(160M, discountedAmount);
    }

    [TestMethod]
    public void WhenVIPPurchasesAre500OrdersOverLastYearThenDiscount25IsApplied()
    {
        // Arrange
        decimal amount = 1000M;
        List<Order> orderHistory = TestDataGenerator.GetOrders(500, 1);
        var sut = new VIPCustomerDiscountStrategy();

        // Act
        var discountedAmount = sut.ApplyDiscount(amount, orderHistory);

        // Assert : its should be 160 as it equals to 1000 * 0.80M = 800, extra discount is 25% then 800*0.75 = 600
        Assert.AreEqual(600M, discountedAmount);
    }
}