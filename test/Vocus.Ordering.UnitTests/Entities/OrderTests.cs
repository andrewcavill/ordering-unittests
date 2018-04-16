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

        [Test]
        public void TestCommitIsSuccessful_ShippingIsTenDollarsWhenOrderAmountLessThanOneHundredDollars()
        {
            // arrange
            var order = new Order();
            order.AddItem(OrderItemReturnsAmount(99.99M));

            // act
            order.Commit();

            // assert
            Assert.False(order.DateCommitted == null);
            Assert.AreEqual(10, order.Shipping);
        }

        [Test]
        public void TestCommitIsSuccessful_ShippingIsFreeWhenOrderAmountEqualsOneHundredDollars()
        {
            // arrange
            var order = new Order();
            order.AddItem(OrderItemReturnsAmount(100));

            // act
            order.Commit();

            // assert
            Assert.False(order.DateCommitted == null);
            Assert.AreEqual(0, order.Shipping);
        }

        [Test]
        public void TestCommitThrowsExceptionWhenOrderAlreadyCommitted()
        {
            // arrange
            var order = new Order();
            order.AddItem(OrderItemReturnsAmount(100));
            order.DateCommitted = DateTime.Now;

            // act, assert
            Assert.That(() => order.Commit(), Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order is already committed."));
        }

        [Test]
        public void TestCommitThrowsExceptionWhenOrderHasNoItems()
        {
            // arrange
            var order = new Order();

            // act, assert
            Assert.That(() => order.Commit(), Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order has no items."));
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
