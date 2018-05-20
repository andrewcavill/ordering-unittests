using System.Collections.Generic;
using Vocus.Common.Data.Nhibernate.Repositories.Interfaces;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Repositories.Interfaces
{
    public interface IOrderRepository : IIdentifiableEntityRepository<Order>
    {
        IList<Order> GetUncommittedOrdersByBrandKey(string brandKey);
    }
}