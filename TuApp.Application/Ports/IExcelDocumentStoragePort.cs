namespace TuApp.Application.Ports;

public interface IExcelDocumentStoragePort
{
    byte[]? Read(string fileName);
    byte[] Save(string fileName, byte[] content);
}
