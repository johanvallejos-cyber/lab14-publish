using TuApp.Application.Ports;

namespace TuApp.Infrastructure.Adapters.Storage;

public sealed class LocalExcelDocumentStorage : IExcelDocumentStoragePort
{
    private readonly string _directory = Directory.GetCurrentDirectory();

    public byte[]? Read(string fileName)
    {
        var path = GetPath(fileName);
        return File.Exists(path) ? File.ReadAllBytes(path) : null;
    }

    public byte[] Save(string fileName, byte[] content)
    {
        File.WriteAllBytes(GetPath(fileName), content);
        return content;
    }

    private string GetPath(string fileName) => Path.Combine(_directory, Path.GetFileName(fileName));
}
