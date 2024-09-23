using System.Threading;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Formats.Asn1;
using System.Globalization;
using Acme.SubscriberService.Models.Dtos;
using CsvHelper;
using Acme.SubscriberService.Interfaces;
using Acme.SubscriberService.Models;
using FluentValidation;

namespace Acme.SubscriberService.Application.FileImport;

public class FileImportCommandHandler : IRequestHandler<FileImportCommand, SubscriptionResponseDto>
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IValidator<FileImportCommand> _validator;

    public FileImportCommandHandler(ISubscriptionRepository subscriptionRepository, IValidator<FileImportCommand> validator)
    {
        _subscriptionRepository = subscriptionRepository;
        _validator = validator;
    }

    public async Task<SubscriptionResponseDto> Handle(FileImportCommand request, CancellationToken cancellationToken)
    {
        var allRecords = new List<SubscriptionDto>();

        foreach (var file in request.CsvFiles)
        {
            var records = new List<SubscriptionDto>();
            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);
            allRecords.AddRange(csv.GetRecords<SubscriptionDto>().ToList());

        }

        var validSubscriptions = allRecords.Where(x => x.ExpirationDate.ToUniversalTime() > DateTime.UtcNow).Select(x => new Subscription
        {
            Id = Guid.NewGuid(),
            Email = x.Email,
            ExpirationDate = x.ExpirationDate
        }).ToList();

        var invalidSubscriptions = allRecords.Where(x => x.ExpirationDate.ToUniversalTime() <= DateTime.UtcNow).ToList();

        await _subscriptionRepository.AddValidSubscriptions(validSubscriptions);

        return new SubscriptionResponseDto
        {
            ExpiredSubscriptions = invalidSubscriptions
        };
    }
}
