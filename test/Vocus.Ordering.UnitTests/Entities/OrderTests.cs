using Moq;
using NUnit.Framework;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.UnitTests.Entities
{
    public class OrderTests
    {
        [Test]
        public void SubTotal_IsZero_WhenOrderHasNoItems()
        {
            // arrange
            var order = new Order();

            // act
            var subTotal = order.SubTotal();

            // assert
            Assert.That(subTotal, Is.Zero);
        }

        [Test]
        public void SubTotal_IsEqualToItemAmount_WhenOrderHasOneItem()
        {
            // arrange
            var order = new Order();
            var orderItemAmount = 9.95M;
            var mockOrderItem = SetupMockOrderItem(orderItemAmount);
            order.AddItem(mockOrderItem.Object);

            // act
            var subTotal = order.SubTotal();

            // assert
            Assert.That(subTotal, Is.EqualTo(orderItemAmount));
        }

        [Test]
        public void SubTotal_IsEqualToSumOfItemAmounts_WhenOrderHasTwoItems()
        {
            // arrange
            var orderItemAmount1 = 9.95M;
            var orderItemAmount2 = 8.95M;
            var order = new Order();
            order.AddItem(SetupMockOrderItem(orderItemAmount1).Object);
            order.AddItem(SetupMockOrderItem(orderItemAmount2).Object);

            // act
            var subTotal = order.SubTotal();

            // assert
            Assert.That(subTotal, Is.EqualTo(orderItemAmount1 + orderItemAmount2));
        }

        private Mock<OrderItem> SetupMockOrderItem(decimal amount)
        {
            var mockItem = new Mock<OrderItem>();
            mockItem.Setup(x => x.Amount()).Returns(amount);
            return mockItem;
        }
    }
}
