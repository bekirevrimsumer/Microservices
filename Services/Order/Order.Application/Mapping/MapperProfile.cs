using AutoMapper;
using Order.Application.Dtos;
using Order.Domain.OrderAggregate;

namespace Order.Application.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Order.Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}