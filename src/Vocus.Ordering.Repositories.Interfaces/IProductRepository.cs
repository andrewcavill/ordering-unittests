﻿using Vocus.Common.Data.Nhibernate.Repositories.Interfaces;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Repositories.Interfaces
{
    public interface IProductRepository : IIdentifiableEntityRepository<Product> { }
}