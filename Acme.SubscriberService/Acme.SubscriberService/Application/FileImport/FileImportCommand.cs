using Acme.SubscriberService.Models.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SubscriberService.Application.FileImport;

public class FileImportCommand : IRequest<SubscriptionResponseDto>
{
    public required List<IFormFile> CsvFiles { get; set; }
}
