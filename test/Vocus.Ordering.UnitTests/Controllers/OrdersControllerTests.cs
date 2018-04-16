using AutoMapper;
using Moq;
using NUnit.Framework;
using Vocus.Common.Errors;
using Vocus.Ordering.Api.Controllers;
using Vocus.Ordering.Services.Interfaces;

namespace Vocus.Ordering.UnitTests.Controllers
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private OrdersController _ordersController;
        private Mock<IMapper> _mockMapper;
        private Mock<IOrderService> _mockOrderService;
        private Mock<IBrandService> _mockBrandService;
        private Mock<IProductService> _mockProductService;

        [SetUp]
        public void SetUp()
        {
            _mockMapper = new Mock<IMapper>();
            _mockOrderService = new Mock<IOrderService>();
            _mockBrandService = new Mock<IBrandService>();
            _mockProductService = new Mock<IProductService>();
            _ordersController = new OrdersController(_mockMapper.Object, _mockBrandService.Object, _mockOrderService.Object, _mockProductService.Object);
        }

        [Test]
        public void TestCommitIsSuccessful()
        {
            // arrange
            var orderId = 12345;

            // act
            _ordersController.Commit(orderId);

            // assert
            _mockOrderService.Verify(x => x.Commit(orderId), Times.Once);
        }

        [Test]
        public void TestCommitThrowsExceptionWhenServiceThrowsException()
        {
            // arrange
            var orderId = 12345;
            _mockOrderService.Setup(x => x.Commit(orderId)).Throws(new BusinessLogicException("A message"));

            // act, assert
            Assert.That(() => _ordersController.Commit(orderId), Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("A message"));
        }
    }
}
