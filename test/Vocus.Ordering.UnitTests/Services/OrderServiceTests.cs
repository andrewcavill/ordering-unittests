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
        private const int OrderId = 12345;
        private OrderService _orderService;
        private Mock<Order> _order;
        private Mock<IOrderRepository> _mockOrderRepository;

        [SetUp]
        public void SetUp()
        {
            _order = new Mock<Order>();

            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderRepository.Setup(x => x.GetById(OrderId)).Returns(_order.Object);

            _orderService = new OrderService(_mockOrderRepository.Object);
        }

        [Test]
        public void Commit_Succeeds_WhenOrderIsNotCommitted()
        {
            // arrange
            _order.Setup(x => x.IsCommitted()).Returns(false);
            _order.Setup(x => x.SubTotal()).Returns(50);

            // act
            _orderService.Commit(OrderId);

            // assert
            _order.VerifySet(x => x.DateCommitted = It.IsAny<DateTime>());                          // DateCommitted property is set
        }

        [Test]
        public void Commit_ThrowsException_WhenOrderIsAlreadyCommitted()
        {
            // arrange
            _order.Setup(x => x.IsCommitted()).Returns(true);

            // act
            var action = new TestDelegate(() => _orderService.Commit(OrderId));

            // assert
            Assert.That(action, Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order is already committed."));   // Exception is thrown
        }

        [TestCase(99.99, 10, TestName = "$10 when sub-total is less than $100")]
        [TestCase(100, 0, TestName = "free when sub-total is equal to $100")]
        [TestCase(100.01, 0, TestName = "free when sub-total is greater than $100")]
        public void Commit_ShippingIs(decimal orderAmount, decimal shippingAmount)
        {
            // arrange
            _order.Setup(x => x.IsCommitted()).Returns(false);
            _order.Setup(x => x.SubTotal()).Returns(orderAmount);

            // act
            _orderService.Commit(OrderId);

            // assert
            _order.VerifySet(x => x.Shipping = shippingAmount);
        }
    }
}
