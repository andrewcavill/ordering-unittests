using System;
using System.Collections.Generic;

namespace Vocus.Ordering.Api.ViewModels
{
    public class OrderVm
    {
        public int OrderId { get; set; }
        public string BrandKey { get; set; }
        public IList<OrderItemVm> Items { get; set; } = new List<OrderItemVm>();
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateCommitted { get; set; }
        public DateTime? DateCompleted { get; set; }
    }
}