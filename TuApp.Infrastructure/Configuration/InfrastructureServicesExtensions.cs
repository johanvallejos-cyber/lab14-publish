using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TuApp.Domain.Repositories;
using TuApp.Application.Ports;
using TuApp.Infrastructure.Adapters.Repositories;
using TuApp.Infrastructure.Adapters.Storage;
using TuApp.Infrastructure.Context;
using TuApp.Infrastructure.Services.Implementations;

namespace TuApp.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<TicketeraContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IReportingRepository, ReportingRepository>();
        services.AddScoped<IExcelReportServicePort, ExcelReportService>();
        services.AddScoped<IExcelExampleServicePort, ExcelExampleService>();
        services.AddSingleton<IExcelDocumentStoragePort, LocalExcelDocumentStorage>();

        return services;
    }
}
