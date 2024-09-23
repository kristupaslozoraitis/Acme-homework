using FluentValidation.Results;

namespace Acme.SubscriberService.Exceptions;

public class CustomException : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; }

    public CustomException(IEnumerable<ValidationFailure> errors)
    {
        Errors = errors;
    }
}
