using MediatR;
using Microsoft.AspNetCore.Mvc;
using TuApp.Application.UseCases.Reports.Queries;

namespace lab11_VallejosJohan.Controllers;

[ApiController]
[Route("api/reportes")]
[Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
public sealed class ReportsController : ControllerBase
{
    private readonly ISender _mediator;

    public ReportsController(ISender mediator) => _mediator = mediator;

    /// <summary>Descarga el reporte de tickets con solicitante, estado y fechas.</summary>
    [HttpGet("tickets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTickets(CancellationToken cancellationToken)
    {
        var report = await _mediator.Send(new GetTicketsExcelReportQuery(), cancellationToken);
        return File(report.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report.FileName);
    }

    /// <summary>Descarga el reporte de respuestas con ticket, solicitante y agente.</summary>
    [HttpGet("respuestas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResponses(CancellationToken cancellationToken)
    {
        var report = await _mediator.Send(new GetResponsesExcelReportQuery(), cancellationToken);
        return File(report.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report.FileName);
    }
}
