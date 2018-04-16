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
            order.Commit();
            _emailService.SendOrderCommitEmail(order);
        }
    }
}