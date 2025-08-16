using ClickCut.Application.Commands;
using ClickCut.Application.Dtos;

namespace ClickCut.Application.Ports.In;

public interface IAuthServicePort
{
	public Task<AuthUserResponse> Auth(AuthUserCommand authUserCommand);
	public Task<AuthBusinessResponse> BusinessAuth(Guid userId, Guid businissId);
	public Task<AuthUserResponse?> RefreshSessionAsync(string refreshToken);
}