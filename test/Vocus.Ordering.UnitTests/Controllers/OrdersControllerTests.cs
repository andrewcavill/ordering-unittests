using AutoMapper;
using Moq;
using NUnit.Framework;
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
        public void Commit_CallsOrderService()
        {
            // arrange
            var orderId = 12345;

            // act
            _ordersController.Commit(orderId);

            // assert
            _mockOrderService.Verify(x => x.Commit(orderId), Times.Once);
        }
    }
}
