﻿using System;
using System.Collections.Generic;
using System.Linq;
using Vocus.Common.Data.Nhibernate.Entities;

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

        public virtual bool IsCommitted()
        {
            return DateCommitted != null;
        }
    }
}