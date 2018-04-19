using System;
using System.Linq;
using Vocus.Common.Errors;
using Vocus.Ordering.Entities;
using Vocus.Ordering.Repositories.Interfaces;
using Vocus.Ordering.Services.Interfaces;

namespace Vocus.Ordering.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailRepository _emailService;

        public OrderService(IOrderRepository orderRepository, IEmailRepository emailService)
        {
            _orderRepository = orderRepository;
            _emailService = emailService;
        }

        public Order GetById(int orderId)
        {
            return _orderRepository.GetById(orderId);
        }

        public void Create(Order order)
        {
            _orderRepository.Create(order);
        }

        public void Commit(int orderId)
        {
            var order = _orderRepository.GetById(orderId);

            // Check that the order hasn't already been committed
            if (order.IsCommitted())
                throw new BusinessLogicException("Order is already committed.");

            // Check that the order has at least one item
            if (order.OrderItems == null || !order.OrderItems.Any())
                throw new BusinessLogicException("Order has no items.");

            // Set shipping amount - free shipping for orders of $100 or more, otherwise a flat rate of $10
            if (order.Amount() < 100)
                order.Shipping = 10;

            // Mark the order as having been committed
            order.DateCommitted = DateTime.Now;

            _emailService.SendOrderCommitEmail(order);
        }
    }
}