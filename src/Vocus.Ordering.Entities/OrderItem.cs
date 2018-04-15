using System;
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
            if (Product == null) throw new BusinessLogicException("Product must be set before calculating amount.");
            if (Quantity < 1) throw new BusinessLogicException("Quantity must be set to a positive integer before calculating amount.");

            return PriceOverride == null ? Quantity * Product.Price : Quantity * PriceOverride.Value;
        }
    }
}