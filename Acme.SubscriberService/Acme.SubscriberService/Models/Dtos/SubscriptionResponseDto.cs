namespace Acme.SubscriberService.Models.Dtos;

public class SubscriptionResponseDto
{
    public List<SubscriptionDto> ExpiredSubscriptions { get; set; } = [];
}
