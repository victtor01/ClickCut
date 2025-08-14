using ClickCut.Application.Commands;
using ClickCut.Domain.Models;

namespace ClickCut.Application.Ports.In;

public interface IUsersServicePort
{
	public Task<User> CreateUserAsync(CreateUserCommand createUserCommand);
}