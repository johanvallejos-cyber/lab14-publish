using TuApp.Domain.Entities;

namespace TuApp.Domain.Repositories;

public interface IReportingRepository
{
    Task<IReadOnlyList<Ticket>> GetTicketsWithOwnersAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Response>> GetResponsesWithRelatedDataAsync(CancellationToken cancellationToken);
}
