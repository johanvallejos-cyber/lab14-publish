using MediatR;
using TuApp.Application.Dtos;
using TuApp.Application.Ports;

namespace TuApp.Application.UseCases.Reports.Queries;

public sealed record GetResponsesExcelReportQuery : IRequest<ExcelFileDto>;

internal sealed class GetResponsesExcelReportQueryHandler
    : IRequestHandler<GetResponsesExcelReportQuery, ExcelFileDto>
{
    private readonly IExcelReportServicePort _reportService;

    public GetResponsesExcelReportQueryHandler(IExcelReportServicePort reportService) => _reportService = reportService;

    public Task<ExcelFileDto> Handle(GetResponsesExcelReportQuery request, CancellationToken cancellationToken) =>
        _reportService.CreateResponsesReportAsync(cancellationToken);
}
