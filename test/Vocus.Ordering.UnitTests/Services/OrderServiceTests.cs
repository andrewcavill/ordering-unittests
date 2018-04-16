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
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<IEmailRepository> _mockEmailRepository;

        [SetUp]
        public void SetUp()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockEmailRepository = new Mock<IEmailRepository>();
            _orderService = new OrderService(_mockOrderRepository.Object, _mockEmailRepository.Object);
        }

        [Test]
        public void TestCommitIsSuccessful()
        {
            // arrange
            var orderId = 12345;
            var mockOrder = new Mock<Order>();
            _mockOrderRepository.Setup(x => x.GetById(orderId)).Returns(mockOrder.Object);

            // act
            _orderService.Commit(orderId);

            // assert
            _mockOrderRepository.Verify(x => x.GetById(orderId), Times.Once);
            mockOrder.Verify(x => x.Commit(), Times.Once);
            _mockEmailRepository.Verify(x => x.SendOrderCommitEmail(mockOrder.Object), Times.Once);
        }

        [Test]
        public void TestCommitThrowsExceptionIfOrderThrowsException()
        {
            // arrange
            var orderId = 12345;
            var mockOrder = new Mock<Order>();
            mockOrder.Setup(x => x.Commit()).Throws(new BusinessLogicException("A message"));   // Order.Commit() throws an exception
            _mockOrderRepository.Setup(x => x.GetById(orderId)).Returns(mockOrder.Object);

            // act, assert
            Assert.That(() => _orderService.Commit(orderId), Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("A message"));
            _mockOrderRepository.Verify(x => x.GetById(orderId), Times.Once);
            mockOrder.Verify(x => x.Commit(), Times.Once);
            _mockEmailRepository.Verify(x => x.SendOrderCommitEmail(mockOrder.Object), Times.Never);
        }
    }
}
