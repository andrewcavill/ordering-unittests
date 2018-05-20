using System.Collections.Generic;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Services.Interfaces
{
    public interface IOrderService
    {
        Order GetById(int orderId);

        IList<Order> GetUncommittedOrdersByBrandKey(string brandKey);

        void Create(Order order);

        void Commit(int orderId);
    }
}