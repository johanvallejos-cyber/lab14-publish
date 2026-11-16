using ClosedXML.Excel;
using TuApp.Application.Dtos;
using TuApp.Application.Enums;
using TuApp.Application.Ports;

namespace TuApp.Infrastructure.Services.Implementations;

public sealed class ExcelExampleService : IExcelExampleServicePort
{
    private readonly IExcelDocumentStoragePort _storage;

    public ExcelExampleService(IExcelDocumentStoragePort storage) => _storage = storage;

    public Task<ExcelFileDto> GenerateAsync(ExcelExample example, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var file = example switch
        {
            ExcelExample.First => FirstExample(),
            ExcelExample.Second => SecondExample(),
            ExcelExample.Third => ThirdExample(),
            ExcelExample.Fourth => FourthExample(),
            _ => throw new ArgumentOutOfRangeException(nameof(example))
        };

        return Task.FromResult(file);
    }

    private ExcelFileDto FirstExample()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Hoja1");
        worksheet.Cell(1, 1).Value = "Nombre";
        worksheet.Cell(1, 2).Value = "Edad";
        worksheet.Cell(2, 1).Value = "Johan";
        worksheet.Cell(2, 2).Value = 28;
        worksheet.Columns().AdjustToContents();
        return SaveWorkbook(workbook, "archivo.xlsx");
    }

    private ExcelFileDto SecondExample()
    {
        var existingContent = _storage.Read("archivo.xlsx");
        if (existingContent is null)
            throw new FileNotFoundException("Primero genera archivo.xlsx con el ejemplo FirstExample.");

        using var stream = new MemoryStream(existingContent);
        using var workbook = new XLWorkbook(stream);
        workbook.Worksheet(1).Cell(2, 2).Value = 30;
        return SaveWorkbook(workbook, "archivo.xlsx");
    }

    private ExcelFileDto ThirdExample()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Datos");
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Nombre";
        worksheet.Cell(1, 3).Value = "Edad";
        worksheet.Cell(2, 1).Value = 1;
        worksheet.Cell(2, 2).Value = "Juan";
        worksheet.Cell(2, 3).Value = 28;
        worksheet.Cell(3, 1).Value = 2;
        worksheet.Cell(3, 2).Value = "Maria";
        worksheet.Cell(3, 3).Value = 34;
        worksheet.Range("A1:C3").CreateTable();
        worksheet.Columns().AdjustToContents();
        return SaveWorkbook(workbook, "tabla.xlsx");
    }

    private ExcelFileDto FourthExample()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Estilos");
        var headerRow = worksheet.Row(1);
        headerRow.Style.Font.Bold = true;
        headerRow.Style.Fill.BackgroundColor = XLColor.Cyan;
        headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Nombre";
        worksheet.Cell(1, 3).Value = "Edad";
        worksheet.Cell(2, 1).Value = 1;
        worksheet.Cell(2, 2).Value = "Juan";
        worksheet.Cell(2, 3).Value = 28;
        worksheet.Columns().AdjustToContents();
        return SaveWorkbook(workbook, "archivo_con_estilos.xlsx");
    }

    private ExcelFileDto SaveWorkbook(XLWorkbook workbook, string fileName)
    {
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = _storage.Save(fileName, stream.ToArray());
        return new ExcelFileDto(fileName, content);
    }
}
