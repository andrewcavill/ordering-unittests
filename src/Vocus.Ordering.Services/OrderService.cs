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

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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

            if (order.DateCommitted.HasValue)
                throw new BusinessLogicException("Order is already committed.");

            if (order.OrderItems == null || !order.OrderItems.Any())
                throw new BusinessLogicException("Order has no items.");

            order.DateCommitted = DateTime.Now;
        }
    }
}