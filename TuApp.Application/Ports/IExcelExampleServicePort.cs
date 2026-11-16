using TuApp.Application.Dtos;
using TuApp.Application.Enums;

namespace TuApp.Application.Ports;

public interface IExcelExampleServicePort
{
    Task<ExcelFileDto> GenerateAsync(ExcelExample example, CancellationToken cancellationToken);
}
