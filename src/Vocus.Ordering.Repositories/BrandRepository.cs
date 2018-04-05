using Vocus.Common.Data.Nhibernate;
using Vocus.Common.Data.Nhibernate.Repositories;
using Vocus.Ordering.Entities;
using Vocus.Ordering.Repositories.Interfaces;

namespace Vocus.Ordering.Repositories
{
    public class BrandRepository : IdentifiableEntityRepository<Brand>, IBrandRepository
    {
        public BrandRepository(INHibernateSessionAccessor nHibernateSessionAccessor) : base(nHibernateSessionAccessor) { }
    }
}