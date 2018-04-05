using Vocus.Common.Data.Nhibernate;
using Vocus.Common.Data.Nhibernate.Repositories;
using Vocus.Ordering.Entities;
using Vocus.Ordering.Repositories.Interfaces;

namespace Vocus.Ordering.Repositories
{
    public class ProductRepository : IdentifiableEntityRepository<Product>, IProductRepository
    {
        public ProductRepository(INHibernateSessionAccessor nHibernateSessionAccessor) : base(nHibernateSessionAccessor) { }
    }
}