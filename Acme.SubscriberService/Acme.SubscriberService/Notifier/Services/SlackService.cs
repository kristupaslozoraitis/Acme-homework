using Acme.SubscriberService.Models.Dtos;

namespace Acme.SubscriberService.Notifier.Services;

public class SlackService : ISubscriptionNotifier
{
    public void NotifySubscriber(SubscriptionDto subscription)
    {
        Console.WriteLine($"Slack: Subscriber's {subscription.Email} subscription expires on {subscription.ExpirationDate}");
    }
}
