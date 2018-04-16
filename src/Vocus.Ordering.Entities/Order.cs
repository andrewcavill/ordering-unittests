using System;
using System.Collections.Generic;
using System.Linq;
using Vocus.Common.Data.Nhibernate.Entities;
using Vocus.Common.Errors;

namespace Vocus.Ordering.Entities
{
    public class Order : IdentifiableEntity
    {
        public virtual string CustomerName { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual IList<OrderItem> OrderItems { get; } = new List<OrderItem>();
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual DateTime? DateCommitted { get; set; }
        public virtual DateTime? DateCompleted { get; set; }
        public virtual Decimal Shipping { get; set; } = 0;

        public virtual void AddItem(OrderItem item)
        {
            OrderItems.Add(item);
            item.Order = this;
        }

        public virtual decimal Amount()
        {
            return OrderItems.Sum(x => x.Amount());
        }

        public virtual void Commit()
        {
            // Check that the order hasn't already been committed
            if (DateCommitted.HasValue)
                throw new BusinessLogicException("Order is already committed.");

            // Check that the order has at least one item
            if (OrderItems == null || !OrderItems.Any())
                throw new BusinessLogicException("Order has no items.");

            // Set shipping amount - free shipping for orders of $100 or more, otherwise a flat rate of $10
            if (Amount() < 100)
                Shipping = 10;

            // Mark the order as having been committed
            DateCommitted = DateTime.Now;
        }
    }
}