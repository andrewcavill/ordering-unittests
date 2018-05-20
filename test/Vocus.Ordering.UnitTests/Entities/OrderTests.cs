using NUnit.Framework;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.UnitTests.Entities
{
    [TestFixture]
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
            var orderItemAmount = 70.95M;
            var order = new Order();
            order.AddItem(OrderItemReturnsAmount(orderItemAmount));

            // act
            var subTotal = order.SubTotal();

            // assert
            Assert.That(subTotal, Is.EqualTo(orderItemAmount));
        }

        [Test]
        public void SubTotal_IsEqualToSumOfItemAmounts_WhenOrderHasTwoItems()
        {
            // arrange
            var orderItemAmount1 = 70.95M;
            var orderItemAmount2 = 75.00M;
            var order = new Order();
            order.AddItem(OrderItemReturnsAmount(orderItemAmount1));
            order.AddItem(OrderItemReturnsAmount(orderItemAmount2));

            // act
            var subTotal = order.SubTotal();

            // assert
            Assert.That(subTotal, Is.EqualTo(orderItemAmount1 + orderItemAmount2));
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
    }
}
