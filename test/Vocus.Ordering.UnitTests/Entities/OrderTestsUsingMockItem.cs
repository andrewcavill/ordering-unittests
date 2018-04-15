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
            var mockItem = MockItemReturnsAmount(70.95M);
            order.AddItem(mockItem.Object);

            // act
            var amount = order.Amount();

            // assert
            Assert.AreEqual(70.95M,amount);
            mockItem.Verify(x => x.Amount(), Times.Once);
        }

        [Test]
        public void TestAmountWithTwoItems()
        {
            // arrange
            var order = new Order();
            var mockItem1 = MockItemReturnsAmount(70.95M);
            var mockItem2 = MockItemReturnsAmount(75.00M);
            order.AddItem(mockItem1.Object);
            order.AddItem(mockItem2.Object);

            // act
            var amount = order.Amount();

            // assert
            Assert.AreEqual(145.95M, amount);
            mockItem1.Verify(x => x.Amount(), Times.Once);
            mockItem2.Verify(x => x.Amount(), Times.Once);
        }

        [Test]
        public void TestAmountThrowsExceptionWhenItemThrowsException()
        {
            // arrange
            var order = new Order();
            var mockItem = MockItemThrowsException("A message");

            // act
            order.AddItem(mockItem.Object);

            // assert
            Assert.That(() => order.Amount(), Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("A message"));
            mockItem.Verify(x => x.Amount(), Times.Once);
        }

        private Mock<OrderItem> MockItemReturnsAmount(decimal amount)
        {
            var mockItem = new Mock<OrderItem>();
            mockItem.Setup(x => x.Amount()).Returns(amount);
            return mockItem;
        }

        private Mock<OrderItem> MockItemThrowsException(string message)
        {
            var mockItem = new Mock<OrderItem>();
            mockItem.Setup(x => x.Amount()).Throws(new BusinessLogicException(message));
            return mockItem;
        }
    }
}
