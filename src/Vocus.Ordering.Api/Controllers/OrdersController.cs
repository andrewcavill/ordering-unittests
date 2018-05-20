using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vocus.Ordering.Api.ViewModels;
using Vocus.Ordering.Entities;
using Vocus.Ordering.Services.Interfaces;

namespace Vocus.Ordering.Api.Controllers
{
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBrandService _brandService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrdersController(IMapper mapper, IBrandService brandService, IOrderService orderService, IProductService productService)
        {
            _brandService = brandService;
            _orderService = orderService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{orderId}")]
        public OrderVm GetById(int orderId)
        {
            return _mapper.Map<OrderVm>(_orderService.GetById(orderId));
        }

        [HttpGet]
        [Route("uncommitted")]
        public IList<OrderVm> GetUncommittedOrdersByBrandKey([FromQuery] string brandKey)
        {
            return _mapper.Map<IList<OrderVm>>(_orderService.GetUncommittedOrdersByBrandKey(brandKey));
        }

        [HttpPost]
        public int Create([FromBody] SaveOrderVm saveOrderVm)
        {
            var order = new Order();
            MapSaveOrderVmToOrder(saveOrderVm, order);
            _orderService.Create(order);
            return order.Id;
        }

        [HttpPut]
        [Route("{orderId}")]
        public void Update(int orderId, [FromBody] SaveOrderVm saveOrderVm)
        {
            var order = _orderService.GetById(orderId);
            MapSaveOrderVmToOrder(saveOrderVm, order);
        }

        [HttpPost]
        [Route("{orderId}/commit")]
        public void Commit(int orderId)
        {
            _orderService.Commit(orderId);
        }

        private void MapSaveOrderVmToOrder(SaveOrderVm saveOrderVm, Order order)
        {
            order.Brand = _brandService.GetBrandByKey(saveOrderVm.BrandKey);

            order.OrderItems.Clear();

            foreach (var itemVm in saveOrderVm.Items)
            {
                order.AddItem(new OrderItem()
                {
                    Product = _productService.GetProductByKey(itemVm.ProductKey),
                    Quantity = itemVm.Quantity,
                    PriceOverride = itemVm.PriceOverride
                });
            }
        }
    }
}