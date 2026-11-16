using Microsoft.EntityFrameworkCore;
using TuApp.Domain.Entities;
using TuApp.Domain.Repositories;
using TuApp.Infrastructure.Context;

namespace TuApp.Infrastructure.Adapters.Repositories;

public sealed class ReportingRepository : IReportingRepository
{
    private readonly TicketeraContext _context;

    public ReportingRepository(TicketeraContext context) => _context = context;

    public async Task<IReadOnlyList<Ticket>> GetTicketsWithOwnersAsync(CancellationToken cancellationToken) =>
        await _context.Tickets
            .AsNoTracking()
            .Include(ticket => ticket.User)
            .OrderBy(ticket => ticket.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Response>> GetResponsesWithRelatedDataAsync(CancellationToken cancellationToken) =>
        await _context.Responses
            .AsNoTracking()
            .Include(response => response.Ticket)
                .ThenInclude(ticket => ticket.User)
            .Include(response => response.Responder)
            .OrderBy(response => response.CreatedAt)
            .ToListAsync(cancellationToken);
}
