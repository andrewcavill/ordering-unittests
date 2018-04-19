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
