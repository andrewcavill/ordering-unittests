using Vocus.Common.Data.Nhibernate.Entities;

namespace Vocus.Ordering.Entities
{
    public class Brand : IdentifiableEntity
    {
        public virtual string BrandKey { get; set; }
        public virtual string DisplayName { get; set; }
    }
}