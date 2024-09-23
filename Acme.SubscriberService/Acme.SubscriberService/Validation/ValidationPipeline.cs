using Acme.SubscriberService.Exceptions;
using FluentValidation;
using MediatR;
using System;

namespace Acme.SubscriberService.Validation;

public sealed class ValidationPipeline<TRequest, TResponse>
: IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipeline(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures =
            _validators.Select(validator => validator.Validate(context));

        var failures = validationFailures
            .Where(result => result != null && !result.IsValid)
            .SelectMany(result => result.Errors)
            .ToList();

        if (failures.Count != 0)
        {
            throw new CustomException(failures);
        }

        return await next();
    }
}
