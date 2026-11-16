using MediatR;
using Microsoft.AspNetCore.Mvc;
using TuApp.Application.Enums;
using TuApp.Application.UseCases.Excel.Commands;

namespace lab11_VallejosJohan.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExcelController : ControllerBase
{
    private readonly ISender _mediator;

    public ExcelController(ISender mediator) => _mediator = mediator;

    [HttpGet("ejemplo")]
    public async Task<IActionResult> FirstExample() => await Generate(ExcelExample.First);

    [HttpPost("modificar")]
    public async Task<IActionResult> SecondExample() => await Generate(ExcelExample.Second);

    [HttpGet("tabla")]
    public async Task<IActionResult> ThirdExample() => await Generate(ExcelExample.Third);

    [HttpGet("estilos")]
    public async Task<IActionResult> FourthExample() => await Generate(ExcelExample.Fourth);

    private async Task<IActionResult> Generate(ExcelExample example)
    {
        try
        {
            var result = await _mediator.Send(new GenerateExcelExampleCommand(example));
            return File(result.Content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                result.FileName);
        }
        catch (FileNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }
}
