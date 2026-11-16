using ClosedXML.Excel;
using TuApp.Application.Dtos;
using TuApp.Application.Ports;
using TuApp.Domain.Repositories;

namespace TuApp.Infrastructure.Services.Implementations;

public sealed class ExcelReportService : IExcelReportServicePort
{
    private readonly IReportingRepository _repository;

    public ExcelReportService(IReportingRepository repository) => _repository = repository;

    public async Task<ExcelFileDto> CreateTicketsReportAsync(CancellationToken cancellationToken)
    {
        var tickets = await _repository.GetTicketsWithOwnersAsync(cancellationToken);
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Tickets");
        var headers = new[] { "ID", "Solicitante", "Título", "Estado", "Fecha creación", "Fecha cierre" };

        WriteHeaders(worksheet, headers);
        for (var index = 0; index < tickets.Count; index++)
        {
            var ticket = tickets[index];
            var row = index + 2;
            worksheet.Cell(row, 1).Value = ticket.TicketId.ToString();
            worksheet.Cell(row, 2).Value = ticket.User.Username;
            worksheet.Cell(row, 3).Value = ticket.Title;
            worksheet.Cell(row, 4).Value = ticket.Status;
            if (ticket.CreatedAt.HasValue) worksheet.Cell(row, 5).Value = ticket.CreatedAt.Value;
            if (ticket.ClosedAt.HasValue) worksheet.Cell(row, 6).Value = ticket.ClosedAt.Value;
        }

        return CreateExcelFile(workbook, worksheet, headers.Length, tickets.Count, 5,
            "reporte_tickets.xlsx", "ReporteTickets");
    }

    public async Task<ExcelFileDto> CreateResponsesReportAsync(CancellationToken cancellationToken)
    {
        var responses = await _repository.GetResponsesWithRelatedDataAsync(cancellationToken);
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Respuestas");
        var headers = new[] { "Ticket", "Estado", "Solicitante", "Respondió", "Mensaje", "Fecha respuesta" };

        WriteHeaders(worksheet, headers);
        for (var index = 0; index < responses.Count; index++)
        {
            var response = responses[index];
            var row = index + 2;
            worksheet.Cell(row, 1).Value = response.Ticket.Title;
            worksheet.Cell(row, 2).Value = response.Ticket.Status;
            worksheet.Cell(row, 3).Value = response.Ticket.User.Username;
            worksheet.Cell(row, 4).Value = response.Responder.Username;
            worksheet.Cell(row, 5).Value = response.Message;
            if (response.CreatedAt.HasValue) worksheet.Cell(row, 6).Value = response.CreatedAt.Value;
        }

        return CreateExcelFile(workbook, worksheet, headers.Length, responses.Count, 6,
            "reporte_respuestas.xlsx", "ReporteRespuestas");
    }

    private static void WriteHeaders(IXLWorksheet worksheet, IReadOnlyList<string> headers)
    {
        for (var column = 0; column < headers.Count; column++)
            worksheet.Cell(1, column + 1).Value = headers[column];
    }

    private static ExcelFileDto CreateExcelFile(XLWorkbook workbook, IXLWorksheet worksheet, int columns,
        int rows, int firstDateColumn, string fileName, string tableName)
    {
        var header = worksheet.Range(1, 1, 1, columns);
        header.Style.Font.Bold = true;
        header.Style.Fill.BackgroundColor = XLColor.Cyan;
        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        if (rows > 0)
            worksheet.Range(2, firstDateColumn, rows + 1, columns).Style.DateFormat.Format = "yyyy-mm-dd hh:mm";

        worksheet.Range(1, 1, rows + 1, columns).CreateTable(tableName);
        worksheet.Columns().AdjustToContents();
        worksheet.SheetView.FreezeRows(1);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return new ExcelFileDto(fileName, stream.ToArray());
    }
}
