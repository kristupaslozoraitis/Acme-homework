using Acme.SubscriberService.Models.Dtos;
using Acme.SubscriberService.Notifier.Services;

namespace Acme.SubscriberService.Notifier;

public class MultiChannelNotifier : ISubscriptionNotifier
{
    private readonly List<ISubscriptionNotifier> notifiers =
        [
            new EmailService(),
            new SmsService(),
            new SlackService(),
        ];

    public void NotifySubscriber(SubscriptionDto subscription)
    {
        foreach (var notifier in notifiers)
        {
            notifier.NotifySubscriber(subscription);
        }
    }
}
