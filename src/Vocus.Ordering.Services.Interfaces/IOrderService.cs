using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Services.Interfaces
{
    public interface IOrderService
    {
        Order GetById(int orderId);

        void Create(Order order);

        void Commit(int orderId);
    }
}