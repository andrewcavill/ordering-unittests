using System;
using Moq;
using NUnit.Framework;
using Vocus.Common.Errors;
using Vocus.Ordering.Entities;
using Vocus.Ordering.Repositories.Interfaces;
using Vocus.Ordering.Services;

namespace Vocus.Ordering.UnitTests.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private Order _order;
        private Mock<IOrderRepository> _mockOrderRepository;

        [SetUp]
        public void SetUp()
        {
            _order = new Order { Id = 12345 };

            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderRepository.Setup(x => x.GetById(_order.Id)).Returns(_order);

            _orderService = new OrderService(_mockOrderRepository.Object);
        }

        [Test]
        public void Commit_Succeeds_WhenOrderIsNotCommitted()
        {
            // arrange
            _order.AddItem(SetUpOrderItem(100));    // Add one item to order

            // act
            _orderService.Commit(_order.Id);

            // assert
            Assert.That(_order.IsCommitted(), Is.True);
            _mockOrderRepository.Verify(x => x.GetById(_order.Id), Times.Once);
        }

        [Test]
        public void Commit_ThrowsException_WhenOrderIsAlreadyCommitted()
        {
            // arrange
            _order.DateCommitted = DateTime.Now;     // Order is already committed

            // act
            var action = new TestDelegate(() => _orderService.Commit(_order.Id));

            // assert
            Assert.That(action, Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order is already committed."));
        }

        [TestCase(99.99, 10, TestName = "$10 when sub-total is less than $100")]
        [TestCase(100, 0, TestName = "free when sub-total is equal to $100")]
        [TestCase(100.01, 0, TestName = "free when sub-total is greater than $100")]
        public void Commit_ShippingIs(decimal orderAmount, decimal shippingAmount)
        {
            // arrange
            _order.AddItem(SetUpOrderItem(orderAmount));

            // act
            _orderService.Commit(_order.Id);

            // assert
            Assert.That(_order.Shipping, Is.EqualTo(shippingAmount));
        }

        private OrderItem SetUpOrderItem(decimal amount)
        {
            return new OrderItem
            {
                Id = 1,
                Quantity = 1,
                Product = new Product { Price = amount}
            };
        }
    }
}
