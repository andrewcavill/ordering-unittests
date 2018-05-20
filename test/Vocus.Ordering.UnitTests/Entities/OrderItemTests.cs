using NUnit.Framework;
using Vocus.Common.Errors;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.UnitTests.Entities
{
    public class OrderItemTests
    {
        [Test]
        public void Amount_EqualsZero_WhenNoProduct()
        {
            // arrange
            var orderItem = new OrderItem { Quantity = 1}; // Product is not set

            // act
            var amount = orderItem.Amount();

            // assert
            Assert.That(amount, Is.Zero);
        }

        [Test]
        public void Amount_ThrowsException_WhenQuantityIsNegative()
        {
            // arrange
            var orderItem = new OrderItem { Quantity = -1 }; // Product is not set

            // act
            var action = new TestDelegate(() => orderItem.Amount());

            // assert
            Assert.That(action, Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Quantity must be greater than or equal to zero."));
        }

        [TestCase(9.95, 2, 19.9, TestName = "Product price multiplied by quantity when price is positive and quantity is positive")]
        [TestCase(9.95, 0, 0, TestName = "Zero when price is positive and quantity is zero")]
        [TestCase(-9.95, 2, -19.9, TestName = "Product price multiplied by quantity when price is negative and quantity is positive")]
        [TestCase(-9.95, 0, 0, TestName = "Zero when price is negative and quantity is zero")]
        [TestCase(0, 2, 0, TestName = "Zero when price is zero and quantity is positive")]
        [TestCase(0, 0, 0, TestName = "Zero when price is zero and quantity is zero")]
        public void Amount_Equals(decimal price, int quantity, decimal expectedAmount)
        {
            // arrange
            var orderItem = new OrderItem
            {
                Product = new Product { Price = price },
                Quantity = quantity
            };

            // act
            var amount = orderItem.Amount();

            // assert
            Assert.That(amount, Is.EqualTo(expectedAmount));
        }

        [TestCase(9.95, 2, 19.9, TestName = "Product price override multiplied by quantity when price is positive and quantity is positive")]
        [TestCase(9.95, 0, 0, TestName = "Zero when price override is positive and quantity is zero")]
        [TestCase(-9.95, 2, -19.9, TestName = "Product price override multiplied by quantity when price is negative and quantity is positive")]
        [TestCase(-9.95, 0, 0, TestName = "Zero when price override is negative and quantity is zero")]
        [TestCase(0, 2, 0, TestName = "Zero when price override is zero and quantity is positive")]
        [TestCase(0, 0, 0, TestName = "Zero when price override is zero and quantity is zero")]
        public void Amount_EqualsProductPriceOverrideMultipliedByQuantity(decimal priceOverride, int quantity, decimal expectedAmount)
        {
            // arrange
            var orderItem = new OrderItem
            {
                Product = new Product { Price = 999.99M },
                PriceOverride = priceOverride,
                Quantity = quantity,
            };

            // act
            var amount = orderItem.Amount();

            // assert
            Assert.That(amount, Is.EqualTo(expectedAmount));
        }
    }
}
