using NUnit.Framework;
using Vocus.Common.Errors;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.UnitTests.Entities
{
    [TestFixture]
    public class OrderItemTests
    {
        [TestCase(1, 75, null, 75, TestName = "WhenNoPriceOverrideAndQuantityIs1")]
        [TestCase(2, 75, null, 150, TestName = "WhenNoPriceOverrideAndQuantityIs2")]
        [TestCase(1, 75, 70.95, 70.95, TestName = "WhenHasPriceOverrideAndQuantityIs1")]
        [TestCase(2, 75, 70.95, 141.9, TestName = "WhenHasPriceOverrideAndQuantityIs2")]
        public void TestAmount(int quantity, decimal price, decimal? priceOverride, decimal expectedAmount)
        {
            // arrange
            var orderItem = new OrderItem
            {
                Product = new Product { Price = price },
                Quantity = quantity,
                PriceOverride = priceOverride
            };

            // act
            var orderItemAmount = orderItem.Amount();

            // assert
            Assert.AreEqual(expectedAmount, orderItemAmount);
        }

        [Test]
        public void TestAmountThrowsBusinessLogicExceptionIfProductNotSet()
        {
            // arrange
            var orderItem = new OrderItem { Quantity = 1}; // Product is not set

            // act, assert
            Assert.That(() => orderItem.Amount(), Throws.TypeOf<BusinessLogicException>()
                .With.Message.EqualTo("Product must be set before calculating amount."));
        }
    }
}
