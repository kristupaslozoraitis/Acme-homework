using Acme.SubscriberService.Models.Dtos;

namespace Acme.SubscriberService.Notifier.Services;

public class EmailService : ISubscriptionNotifier
{
    public void NotifySubscriber(SubscriptionDto subscription)
    {
        Console.WriteLine($"Email: Subscriber's {subscription.Email} subscription expires on {subscription.ExpirationDate}");
    }
}
