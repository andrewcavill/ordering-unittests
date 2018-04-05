using Vocus.Common.Data.Nhibernate;
using Vocus.Common.Data.Nhibernate.Repositories;
using Vocus.Ordering.Entities;
using Vocus.Ordering.Repositories.Interfaces;

namespace Vocus.Ordering.Repositories
{
    public class OrderRepository : IdentifiableEntityRepository<Order>, IOrderRepository
    {
        public OrderRepository(INHibernateSessionAccessor nHibernateSessionAccessor) : base(nHibernateSessionAccessor) { }
    }
}