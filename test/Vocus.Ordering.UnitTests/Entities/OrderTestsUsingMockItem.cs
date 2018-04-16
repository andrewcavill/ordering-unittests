using System;
using Moq;
using NUnit.Framework;
using Vocus.Common.Errors;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.UnitTests.Entities
{
    [TestFixture]
    public class OrderTestsUsingMockItem
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
            var order = new Order();
            order.AddItem(MockOrderItemReturnsAmount(70.95M).Object);

            // act
            var amount = order.Amount();

            // assert
            Assert.AreEqual(70.95M,amount);
        }

        [Test]
        public void TestAmountWithTwoItems()
        {
            // arrange
            var order = new Order();
            order.AddItem(MockOrderItemReturnsAmount(70.95M).Object);
            order.AddItem(MockOrderItemReturnsAmount(75.00M).Object);

            // act
            var amount = order.Amount();

            // assert
            Assert.AreEqual(145.95M, amount);
        }

        [Test]
        public void TestAmountThrowsExceptionWhenItemThrowsException()
        {
            // arrange
            var order = new Order();
            order.AddItem(MockOrderItemThrowsException("A message").Object);

            // act, assert
            Assert.That(() => order.Amount(), Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("A message"));
        }

        [Test]
        public void TestCommitIsSuccessful_ShippingIsTenDollarsWhenOrderAmountLessThanOneHundredDollars()
        {
            // arrange
            var order = new Order();
            order.AddItem(MockOrderItemReturnsAmount(99.99M).Object);

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
            order.AddItem(MockOrderItemReturnsAmount(100).Object);

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
            order.AddItem(MockOrderItemReturnsAmount(100).Object);
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

        private Mock<OrderItem> MockOrderItemReturnsAmount(decimal amount)
        {
            var mockItem = new Mock<OrderItem>();
            mockItem.Setup(x => x.Amount()).Returns(amount);
            return mockItem;
        }

        private Mock<OrderItem> MockOrderItemThrowsException(string message)
        {
            var mockItem = new Mock<OrderItem>();
            mockItem.Setup(x => x.Amount()).Throws(new BusinessLogicException(message));
            return mockItem;
        }
    }
}
