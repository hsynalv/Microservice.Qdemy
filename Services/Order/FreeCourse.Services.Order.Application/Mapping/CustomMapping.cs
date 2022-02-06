using AutoMapper;
using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using _Order = FreeCourse.Services.Order.Domain.OrderAggregate.Order;

namespace FreeCourse.Services.Order.Application.Mapping
{
    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<_Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
