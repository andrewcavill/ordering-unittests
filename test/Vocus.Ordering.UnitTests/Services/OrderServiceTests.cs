using System;
using System.Collections.Generic;
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
        private Order _order;
        private OrderService _orderService;
        private Mock<IOrderRepository> _orderRepository;

        [SetUp]
        public void SetUp()
        {
            var brand = new Brand {Id = 1, BrandKey = "Slingshot", DisplayName = "Slingshot"};

            _order = new Order
            {
                Id = 123,
                Brand = brand,
                DateCreated = DateTime.Now
            };

            var product = new Product {Id = 1, Price = 75, ProductKey = "UFB_100"};

            var orderItem = new OrderItem
            {
                Id = 456,
                Order = _order,
                Product = product,
                Quantity = 1
            };

            _order.AddItem(orderItem);

            _orderRepository = new Mock<IOrderRepository>();
            _orderRepository.Setup(x => x.GetById(_order.Id)).Returns(_order);

            _orderService = new OrderService(_orderRepository.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _orderRepository.Verify(x => x.GetById(_order.Id), Times.Once);
        }

        [Test]
        public void CommitIsSuccessful()
        {
            Assert.True(_order.DateCommitted == null);
            _orderService.Commit(_order.Id);
            Assert.False(_order.DateCommitted == null);
        }

        [Test]
        public void CommitThrowsBusinessLogicExceptionIfOrderAlreadyCommitted()
        {
            _order.DateCommitted = DateTime.Now;
            Assert.That(() => _orderService.Commit(_order.Id),
                Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order is already committed."));
        }

        [Test]
        public void CommitThrowsBusinessLogicExceptionIfOrderItemsIsEmpty()
        {
            _order.OrderItems = new List<OrderItem>();
            Assert.That(() => _orderService.Commit(_order.Id),
                Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order has no items."));
        }

        [Test]
        public void CommitThrowsBusinessLogicExceptionIfOrderItemsIsNull()
        {
            _order.OrderItems = null;
            Assert.That(() => _orderService.Commit(_order.Id),
                Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order has no items."));
        }
    }
}
