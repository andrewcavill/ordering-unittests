using System;
using System.Collections.Generic;
using Vocus.Common.Data.Nhibernate.Entities;

namespace Vocus.Ordering.Entities
{
    public class Order : IdentifiableEntity
    {
        public virtual Brand Brand { get; set; }
        public virtual IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual DateTime? DateCommitted { get; set; }
        public virtual DateTime? DateCompleted { get; set; }

        public virtual void AddItem(OrderItem item)
        {
            OrderItems.Add(item);
            item.Order = this;
        }
    }
}