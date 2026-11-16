using MediatR;
using TuApp.Application.Dtos;
using TuApp.Application.Enums;
using TuApp.Application.Ports;

namespace TuApp.Application.UseCases.Excel.Commands;

public sealed record GenerateExcelExampleCommand(ExcelExample Example) : IRequest<ExcelFileDto>;

internal sealed class GenerateExcelExampleCommandHandler
    : IRequestHandler<GenerateExcelExampleCommand, ExcelFileDto>
{
    private readonly IExcelExampleServicePort _excelExampleService;

    public GenerateExcelExampleCommandHandler(IExcelExampleServicePort excelExampleService) =>
        _excelExampleService = excelExampleService;

    public Task<ExcelFileDto> Handle(GenerateExcelExampleCommand request, CancellationToken cancellationToken) =>
        _excelExampleService.GenerateAsync(request.Example, cancellationToken);
}
