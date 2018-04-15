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
        private Mock<IOrderRepository> _orderRepository;

        [SetUp]
        public void SetUp()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepository.Object);
        }

        [Test]
        public void CommitIsSuccessful()
        {
            // arrange
            var order = SetupOrder();
            order.AddItem(SetupOrderItem());
            _orderRepository.Setup(x => x.GetById(order.Id)).Returns(order);

            // act
            _orderService.Commit(order.Id);

            // assert
            Assert.False(order.DateCommitted == null);
            _orderRepository.Verify(x => x.GetById(order.Id), Times.Once);
        }

        [Test]
        public void CommitThrowsBusinessLogicExceptionIfOrderAlreadyCommitted()
        {
            // arrange
            var order = SetupOrder();
            order.AddItem(SetupOrderItem());
            order.DateCommitted = DateTime.Now; // Set DateComitted to a non-null value
            _orderRepository.Setup(x => x.GetById(order.Id)).Returns(order);

            // act, assert
            Assert.That(() => _orderService.Commit(order.Id),
                Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order is already committed."));
            _orderRepository.Verify(x => x.GetById(order.Id), Times.Once);
        }

        [Test]
        public void CommitThrowsBusinessLogicExceptionIfOrderItemsIsEmpty()
        {
            // arrange
            var order = SetupOrder();
            _orderRepository.Setup(x => x.GetById(order.Id)).Returns(order);

            // act, assert
            Assert.That(() => _orderService.Commit(order.Id),
                Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order has no items."));
            _orderRepository.Verify(x => x.GetById(order.Id), Times.Once);
        }

        private Order SetupOrder()
        {
            var brand = new Brand { Id = 1, BrandKey = "Slingshot", DisplayName = "Slingshot" };
        
            return new Order
            {
                Id = 123,
                Brand = brand,
                DateCreated = DateTime.Now
            };
        }

        private OrderItem SetupOrderItem()
        {
            var product = new Product { Id = 1, Price = 75, ProductKey = "UFB_100" };

            return new OrderItem
            {
                Id = 456,
                Product = product,
                Quantity = 1
            };
        }
    }
}
