using TuApp.Application.Dtos;

namespace TuApp.Application.Ports;

public interface IExcelReportServicePort
{
    Task<ExcelFileDto> CreateTicketsReportAsync(CancellationToken cancellationToken);
    Task<ExcelFileDto> CreateResponsesReportAsync(CancellationToken cancellationToken);
}
