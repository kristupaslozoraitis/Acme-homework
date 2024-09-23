namespace Acme.SubscriberService.Models;

public class Subscription
{
    public Guid Id { get; set; }

    public required string Email { get; set; }

    public required DateTime ExpirationDate { get; set; }
}
