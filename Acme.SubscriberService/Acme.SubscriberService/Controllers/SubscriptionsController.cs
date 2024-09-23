using Acme.SubscriberService.Interfaces;
using Acme.SubscriberService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Acme.SubscriberService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionsController(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subscriptions = await _subscriptionRepository.GetAllSubscriptions();

        return Ok(subscriptions);
    }
}
