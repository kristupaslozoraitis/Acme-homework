namespace Acme.SubscriberService.Models.Dtos;

public class SubscriptionDto
{
    public required string Email { get; set; }

    public required DateTime ExpirationDate { get; set; }
}
