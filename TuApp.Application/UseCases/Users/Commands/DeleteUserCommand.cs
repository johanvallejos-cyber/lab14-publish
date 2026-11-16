using MediatR;
using TuApp.Domain.Entities;
using TuApp.Domain.Repositories;

namespace TuApp.Application.UseCases.Users.Commands;

public record DeleteUserCommand(string UserId) : IRequest<bool>;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IGenericRepository<User> _repository;

    public DeleteUserCommandHandler(IGenericRepository<User> repository) => _repository = repository;

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.UserId);
        if (user is null) return false;
        await _repository.DeleteAsync(request.UserId);
        return true;
    }
}
