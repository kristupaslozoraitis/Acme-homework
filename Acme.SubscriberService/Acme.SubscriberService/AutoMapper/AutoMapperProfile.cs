using Acme.SubscriberService.Models;
using Acme.SubscriberService.Models.Dtos;
using AutoMapper;

namespace Acme.SubscriberService.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Subscription, SubscriptionDto>();
    }
}
