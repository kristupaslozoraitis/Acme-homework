using Acme.SubscriberService.Models;
using Microsoft.EntityFrameworkCore;

namespace Acme.SubscriberService.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
    public DbSet<Subscription> Subscriptions { get; set; }
}
