using Vocus.Ordering.Api.ViewModels;
using Vocus.Ordering.Entities;

namespace Vocus.Ordering.Api.MappingProfiles
{
    public class OrdersMappingProfile : AutoMapper.Profile
    {
        public OrdersMappingProfile()
        {
            CreateMap<Order, OrderVm>()
                .ForMember(destination => destination.BrandKey, option => option.MapFrom(source => source.Brand.BrandKey))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.OrderItems));

            CreateMap<OrderItem, OrderItemVm>()
                .ForMember(d => d.ProductKey, o => o.MapFrom(s => s.Product.ProductKey));
        }
    }
}