using MediatR;
using TuApp.Domain.Entities;
using TuApp.Domain.Repositories;

namespace TuApp.Application.UseCases.Tickets.Commands;

public record CreateTicketCommand(string UserId, string Title, string? Description) : IRequest<string>;

internal sealed class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, string>
{
    private readonly IGenericRepository<Ticket> _repository;

    public CreateTicketCommandHandler(IGenericRepository<Ticket> repository) => _repository = repository;

    public async Task<string> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new Ticket
        {
            TicketId = Guid.NewGuid(),
            UserId = Guid.Parse(request.UserId),
            Title = request.Title,
            Description = request.Description,
            Status = "abierto",
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(ticket);
        return ticket.TicketId.ToString();
    }
}
