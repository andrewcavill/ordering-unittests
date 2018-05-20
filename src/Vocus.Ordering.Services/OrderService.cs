using System;
using System.Collections.Generic;
using Vocus.Common.Errors;
using Vocus.Ordering.Entities;
using Vocus.Ordering.Repositories.Interfaces;
using Vocus.Ordering.Services.Interfaces;

namespace Vocus.Ordering.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order GetById(int orderId)
        {
            return _orderRepository.GetById(orderId);
        }

        public IList<Order> GetUncommittedOrdersByBrandKey(string brandKey)
        {
            return _orderRepository.GetUncommittedOrdersByBrandKey(brandKey);
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

            // Set shipping amount - free shipping for orders of $100 or more, otherwise a flat rate of $10
            if (order.SubTotal() < 100)
                order.Shipping = 10;
            else
                order.Shipping = 0;

            // Mark the order as having been committed
            order.DateCommitted = DateTime.Now;
        }
    }
}