using MediatR;
using TuApp.Domain.Entities;
using TuApp.Domain.Repositories;

namespace TuApp.Application.UseCases.Tickets.Commands;

public record DeleteTicketCommand(string TicketId) : IRequest<bool>;

internal sealed class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, bool>
{
    private readonly IGenericRepository<Ticket> _repository;

    public DeleteTicketCommandHandler(IGenericRepository<Ticket> repository) => _repository = repository;

    public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _repository.GetByIdAsync(request.TicketId);
        if (ticket is null) return false;

        await _repository.DeleteAsync(request.TicketId);
        return true;
    }
}
