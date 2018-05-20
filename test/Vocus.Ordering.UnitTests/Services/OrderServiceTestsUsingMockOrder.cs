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
    public class OrderServiceTestsUsingMockOrder
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
            _mockOrderRepository.Verify(x => x.GetById(OrderId), Times.Once);                       // Order is retrieved from order repo
            _order.VerifySet(x => x.DateCommitted = It.IsAny<DateTime>());                          // DateCommitted property is set
        }

        
    }
}
