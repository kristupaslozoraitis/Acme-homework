using FluentValidation;

namespace Acme.SubscriberService.Application.FileImport;

public class FileImportCommandValidator : AbstractValidator<FileImportCommand>
{
    public FileImportCommandValidator()
    {
        RuleForEach(x => x.CsvFiles).Custom((file, context) =>
        {
            if (!file.ContentType.Equals("text/csv", StringComparison.OrdinalIgnoreCase))
            {
                context.AddFailure(file.Name, $"File '{file.FileName}' type is not supported, please import CSV files.");
            }
        });
    }
}
