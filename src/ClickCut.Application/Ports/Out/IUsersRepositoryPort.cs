using ClickCut.Domain.Models;

namespace ClickCut.Application.Ports.Out;

public interface IUsersRepositoryPort
{
	public Task<User> CreateUserAsync(User user);
	public Task<User?> FindByEmailAsync(Email email);
	public Task<User?> FindByIdAsync(Guid Id);
}