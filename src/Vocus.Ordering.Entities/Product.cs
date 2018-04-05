using Vocus.Common.Data.Nhibernate.Entities;

namespace Vocus.Ordering.Entities
{
    public class Product : IdentifiableEntity
    {
        public virtual string ProductKey { get; set; }
        public virtual decimal Price { get; set; }
    }
}