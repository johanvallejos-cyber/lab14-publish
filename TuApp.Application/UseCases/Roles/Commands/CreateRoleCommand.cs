using MediatR;
using TuApp.Domain.Entities;
using TuApp.Domain.Repositories;

namespace TuApp.Application.UseCases.Roles.Commands;

public record CreateRoleCommand(string RoleName) : IRequest<string>;

internal sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, string>
{
    private readonly IGenericRepository<Role> _repository;

    public CreateRoleCommandHandler(IGenericRepository<Role> repository) => _repository = repository;

    public async Task<string> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            RoleId = Guid.NewGuid(),
            RoleName = request.RoleName
        };
        await _repository.AddAsync(role);
        return role.RoleId.ToString();
    }
}
