using Vocus.Common.Data.Nhibernate.Entities;
using Vocus.Common.Errors;

namespace Vocus.Ordering.Entities
{
    public class OrderItem : IdentifiableEntity
    {
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal? PriceOverride { get; set; }

        public virtual decimal Amount()
        {
            if (Quantity < 0) throw new BusinessLogicException("Quantity must be greater than or equal to zero.");

            if (Product == null) return 0;

            return PriceOverride == null ? Quantity * Product.Price : Quantity * PriceOverride.Value;
        }
    }
}