using Acme.SubscriberService.Application.FileImport;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Acme.SubscriberService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileImportController : ControllerBase
{
    private readonly ISender _sender;

    public FileImportController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [Route("import")]
    public async Task<IActionResult> FileImport([FromForm] List<IFormFile> files)
    {
        var command = new FileImportCommand
        {
            CsvFiles = files
        };

        var result = await _sender.Send(command);

        return CreatedAtAction(nameof(FileImport), result);
    }
}
