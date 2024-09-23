using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Acme.SubscriberService.Exceptions;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CustomException customValidationException)
        {
            var errors = customValidationException.Errors
                .Select(error => new
                {
                    Field = error.PropertyName,
                    Error = error.ErrorMessage
                })
                .ToList();

            var problemDetails = new CustomProblemDetails
            {
                Title = "Validation failed",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
                Errors = customValidationException.Errors.Select(error => error.ErrorMessage).ToList()
            };

            context.Result = new BadRequestObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }
    }
}
