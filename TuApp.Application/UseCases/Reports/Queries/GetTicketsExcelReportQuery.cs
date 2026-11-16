using MediatR;
using TuApp.Application.Dtos;
using TuApp.Application.Ports;

namespace TuApp.Application.UseCases.Reports.Queries;

public sealed record GetTicketsExcelReportQuery : IRequest<ExcelFileDto>;

internal sealed class GetTicketsExcelReportQueryHandler
    : IRequestHandler<GetTicketsExcelReportQuery, ExcelFileDto>
{
    private readonly IExcelReportServicePort _reportService;

    public GetTicketsExcelReportQueryHandler(IExcelReportServicePort reportService) => _reportService = reportService;

    public Task<ExcelFileDto> Handle(GetTicketsExcelReportQuery request, CancellationToken cancellationToken) =>
        _reportService.CreateTicketsReportAsync(cancellationToken);
}
