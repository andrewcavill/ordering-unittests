using System;
using System.Collections.Generic;
using System.Linq;
using Vocus.Common.Data.Nhibernate.Entities;

namespace Vocus.Ordering.Entities
{
    public class Order : IdentifiableEntity
    {
        public virtual Brand Brand { get; set; }
        public virtual decimal Shipping { get; set; } = 0;
        public virtual IList<OrderItem> OrderItems { get; } = new List<OrderItem>();
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual DateTime? DateCommitted { get; set; }
        public virtual DateTime? DateCompleted { get; set; }

        public virtual void AddItem(OrderItem item)
        {
            OrderItems.Add(item);
            item.Order = this;
        }

        public virtual decimal SubTotal()
        {
            return OrderItems.Sum(x => x.Amount());
        }

        public virtual decimal Total()
        {
            return SubTotal() + Shipping;
        }

        public virtual bool IsCommitted()
        {
            return DateCommitted != null;
        }
    }
}