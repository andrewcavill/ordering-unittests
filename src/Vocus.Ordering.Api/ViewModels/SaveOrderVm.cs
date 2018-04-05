using System.Collections.Generic;

namespace Vocus.Ordering.Api.ViewModels
{
    public class SaveOrderVm
    {
        public string BrandKey { get; set; }
        public IList<OrderItemVm> Items { get; set; } = new List<OrderItemVm>();
    }
}