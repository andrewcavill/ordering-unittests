using NUnit.Framework;
using Vocus.Common.Errors;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.UnitTests.Entities
{
    [TestFixture]
    public class OrderItemsTests
    {
        [TestCase(1, 75, null, 75, TestName = "WhenNoPriceOverrideAndQuantityIs1")]
        [TestCase(2, 75, null, 150, TestName = "WhenNoPriceOverrideAndQuantityIs2")]
        [TestCase(1, 75, 70.95, 70.95, TestName = "WhenHasPriceOverrideAndQuantityIs1")]
        [TestCase(2, 75, 70.95, 141.9, TestName = "WhenHasPriceOverrideAndQuantityIs2")]
        public void Amount(int quantity, decimal price, decimal? priceOverride, decimal expectedAmount)
        {
            var orderItem = new OrderItem
            {
                Product = new Product { Price = price },
                Quantity = quantity,
                PriceOverride = priceOverride
            };

            var orderItemAmount = orderItem.Amount();

            Assert.AreEqual(expectedAmount, orderItemAmount);
        }

        [Test]
        public void AmountThrowsBusinessLogicExceptionIfProductNotSet()
        {
            var orderItem = new OrderItem { Quantity = 1}; // Product is not set

            Assert.That(() => orderItem.Amount(), Throws.TypeOf<BusinessLogicException>()
                .With.Message.EqualTo("Product must be set before calculating amount."));
        }

        [TestCase(0, TestName = "QuantityIsZero")]
        [TestCase(-1, TestName = "QuantityIsNegative")]
        public void AmountThrowsBusinessLogicExceptionIfQuantityNotAPositiveInteger(int quantity)
        {
            var orderItem = new OrderItem
            {
                Product = new Product { Price = 75 },
                Quantity = quantity
            };

            Assert.That(() => orderItem.Amount(), Throws.TypeOf<BusinessLogicException>()
                .With.Message.EqualTo("Quantity must be set to a positive number before calculating amount."));
        }
    }
}
