using Acme.SubscriberService.Data;
using Acme.SubscriberService.Interfaces;
using Acme.SubscriberService.Models;
using Microsoft.EntityFrameworkCore;

namespace Acme.SubscriberService.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly DatabaseContext _dbContext;

    public SubscriptionRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddValidSubscriptions(List<Subscription> subscriptions)
    {
        _dbContext.Subscriptions.AddRange(subscriptions);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Subscription>> GetAllSubscriptions()
    {
        return await _dbContext.Subscriptions.ToListAsync();
    }
}
