using Acme.SubscriberService.Interfaces;
using Acme.SubscriberService.Models;
using Acme.SubscriberService.Models.Dtos;
using Acme.SubscriberService.Notifier;
using AutoMapper;
using Quartz;

namespace Acme.SubscriberService.Jobs;

public class NotificationJob : IJob
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IMapper _mapper;
    private readonly ISubscriptionNotifier _subscriptionNotifier;

    public NotificationJob(ISubscriptionRepository subscriptionRepository, IMapper mapper, ISubscriptionNotifier subscriptionNotifier)
    {
        _subscriptionRepository = subscriptionRepository;
        _mapper = mapper;
        _subscriptionNotifier = subscriptionNotifier;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var subscribers = await _subscriptionRepository.GetAllSubscriptions();
        var subscriptionDtos = _mapper.Map<List<Subscription>, List<SubscriptionDto>>(subscribers);

        foreach (var subscription in subscriptionDtos)
        {
            _subscriptionNotifier.NotifySubscriber(subscription);
        }
    }
}
