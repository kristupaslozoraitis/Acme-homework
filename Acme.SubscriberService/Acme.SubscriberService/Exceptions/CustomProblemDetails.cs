using Microsoft.AspNetCore.Mvc;

namespace Acme.SubscriberService.Exceptions;

public class CustomProblemDetails : ProblemDetails
{
    public List<string> Errors { get; set; } = [];
}
