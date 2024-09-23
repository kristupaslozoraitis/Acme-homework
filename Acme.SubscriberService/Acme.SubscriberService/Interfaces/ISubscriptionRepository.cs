using Acme.SubscriberService.Models;

namespace Acme.SubscriberService.Interfaces;

public interface ISubscriptionRepository
{
    public Task<List<Subscription>> GetAllSubscriptions();

    public Task AddValidSubscriptions(List<Subscription> subscriptions);
}
