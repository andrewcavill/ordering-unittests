using System.Collections.Generic;
using Vocus.Common.Data.Nhibernate;
using Vocus.Common.Data.Nhibernate.Repositories;
using Vocus.Ordering.Entities;
using Vocus.Ordering.Repositories.Interfaces;

namespace Vocus.Ordering.Repositories
{
    public class OrderRepository : IdentifiableEntityRepository<Order>, IOrderRepository
    {
        public OrderRepository(INHibernateSessionAccessor nHibernateSessionAccessor) : base(nHibernateSessionAccessor) { }

        public IList<Order> GetUncommittedOrdersByBrandKey(string brandKey)
        {
            return Session().QueryOver<Order>()
                .Where(o => o.DateCommitted == null)
                .JoinQueryOver(o => o.Brand)
                .Where(b => b.BrandKey == brandKey)
                .List();
        }
    }
}