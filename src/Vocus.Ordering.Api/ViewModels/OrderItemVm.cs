namespace Vocus.Ordering.Api.ViewModels
{
    public class OrderItemVm
    {
        public string ProductKey { get; set; }
        public int Quantity { get; set; }
        public decimal? PriceOverride { get; set; }
    }
}