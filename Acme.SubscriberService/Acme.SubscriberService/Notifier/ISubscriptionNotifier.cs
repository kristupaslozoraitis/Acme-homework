using Acme.SubscriberService.Models.Dtos;

namespace Acme.SubscriberService.Notifier;

public interface ISubscriptionNotifier
{
    void NotifySubscriber(SubscriptionDto subscription);
}
