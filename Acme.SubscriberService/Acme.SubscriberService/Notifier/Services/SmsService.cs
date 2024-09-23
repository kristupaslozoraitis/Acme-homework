using Acme.SubscriberService.Models.Dtos;

namespace Acme.SubscriberService.Notifier.Services;

public class SmsService : ISubscriptionNotifier
{
    public void NotifySubscriber(SubscriptionDto subscription)
    {
        Console.WriteLine($"Sms: Subscriber's {subscription.Email} subscription expires on {subscription.ExpirationDate}");
    }
}
