using System;
using NUnit.Framework;
using Vocus.Common.Errors;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.UnitTests.Entities
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void TestAmountWithNoItems()
        {
            // arrange
            var order = new Order();

            // act
            var amount = order.Amount();

            // assert
            Assert.AreEqual(0, amount);
        }

        [Test]
        public void TestAmountWithOneItem()
        {
            // arrange
            var orderItemAmount = 70.95M;
            var order = new Order();
            order.AddItem(OrderItemReturnsAmount(orderItemAmount));

            // act
            var amount = order.Amount();

            // assert
            Assert.AreEqual(orderItemAmount, amount);
        }

        [Test]
        public void TestAmountWithTwoItems()
        {
            // arrange
            var orderItemAmount1 = 70.95M;
            var orderItemAmount2 = 75.00M;
            var order = new Order();
            order.AddItem(OrderItemReturnsAmount(orderItemAmount1));
            order.AddItem(OrderItemReturnsAmount(orderItemAmount2));

            // act
            var amount = order.Amount();

            // assert
            Assert.AreEqual(orderItemAmount1 + orderItemAmount2, amount);
        }

        [Test]
        public void TestAmountThrowsExceptionWhenItemThrowsException()
        {
            // arrange
            var order = new Order();
            order.AddItem(OrderItemWithNoProductThrowsException());

            // act, assert
            Assert.That(() => order.Amount(), Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Product must be set before calculating amount."));
        }

        

        private OrderItem OrderItemReturnsAmount(decimal amount)
        {
            return new OrderItem
            {
                Quantity = 1,
                Product = new Product
                {
                    Price = amount
                }
            };
        }

        private OrderItem OrderItemWithNoProductThrowsException()
        {
            return new OrderItem(); // No product so a call to Amount() will throw exception
        }
    }
}
