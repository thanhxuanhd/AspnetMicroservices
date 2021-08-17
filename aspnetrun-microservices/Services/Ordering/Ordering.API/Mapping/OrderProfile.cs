
using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.Mapping;
public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
    }
}
