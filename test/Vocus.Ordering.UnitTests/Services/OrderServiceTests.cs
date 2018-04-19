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
        private Mock<IEmailRepository> _mockEmailRepository;

        [SetUp]
        public void SetUp()
        {
            _order = new Order { Id = 12345 };

            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderRepository.Setup(x => x.GetById(_order.Id)).Returns(_order);

            _mockEmailRepository = new Mock<IEmailRepository>();

            _orderService = new OrderService(_mockOrderRepository.Object, _mockEmailRepository.Object);
        }

        [Test]
        public void TestCommitIsSuccessfulWhenOrderIsNotCommittedAndHasAtLeastOneItem()
        {
            // arrange
            _order.AddItem(SetUpOrderItem(100));    // Add one item to order

            // act
            _orderService.Commit(_order.Id);

            // assert
            _mockOrderRepository.Verify(x => x.GetById(_order.Id), Times.Once);
            _mockEmailRepository.Verify(x => x.SendOrderCommitEmail(_order), Times.Once);
            Assert.True(_order.IsCommitted());
        }

        [Test]
        public void TestShippingIsTenDollarsWhenOrderAmountLessThanOneHundredDollars()
        {
            // arrange
            _order.AddItem(SetUpOrderItem(99.99M));     // Order has single item with amount is less than $100

            // act
            _orderService.Commit(_order.Id);

            // assert
            Assert.AreEqual(10, _order.Shipping);
        }

        [Test]
        public void TestShippingIsFreeWhenOrderAmountEqualsOneHundredDollars()
        {
            // arrange
            _order.AddItem(SetUpOrderItem(100));     // Order has single item with amount of $100

            // act
            _orderService.Commit(_order.Id);

            // assert
            Assert.AreEqual(0, _order.Shipping);
        }

        [Test]
        public void TestCommitThrowsExceptionWhenOrderAlreadyCommitted()
        {
            // arrange
            _order.AddItem(SetUpOrderItem(100));
            _order.DateCommitted = DateTime.Now;     // Order is already committed

            // act, assert
            Assert.That(() => _orderService.Commit(_order.Id), Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order is already committed."));
            _mockEmailRepository.Verify(x => x.SendOrderCommitEmail(_order), Times.Never);
        }

        [Test]
        public void TestCommitThrowsExceptionWhenOrderHasNoItems()
        {
            // arrange
            // The order created in the SetUp() method does not have any items

            // act, assert
            Assert.That(() => _orderService.Commit(_order.Id), Throws.TypeOf<BusinessLogicException>().With.Message.EqualTo("Order has no items."));
            _mockEmailRepository.Verify(x => x.SendOrderCommitEmail(_order), Times.Never);      // Verify email not sent
            Assert.False(_order.IsCommitted());     // Verify order not set to Committed
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
