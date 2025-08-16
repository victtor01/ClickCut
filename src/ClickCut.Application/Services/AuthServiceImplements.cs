using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ClickCut.Application.Commands;
using ClickCut.Application.Dtos;
using ClickCut.Application.Ports.In;
using ClickCut.Application.Ports.Out;
using ClickCut.Application.Utils;
using ClickCut.Domain.Models;
using ClickCut.Shared.Exceptions;

namespace ClickCut.Application.Services;

public class AuthServiceImplements(IUsersRepositoryPort usersRepositoryPort,
	IPasswordServicePort passwordServicePort,
	IJwtServicePort jwtServicePort,
	IBusinessRepositoryPort businessRepositoryPort) : IAuthServicePort
{
	private readonly IJwtServicePort _jwtService = jwtServicePort;
	private readonly IUsersRepositoryPort _usersRepository = usersRepositoryPort;
	private readonly IPasswordServicePort _passwordService = passwordServicePort;
	private readonly IBusinessRepositoryPort _businessRepository = businessRepositoryPort;

	private readonly int hoursInMinutes = 6 * 60;

	public async Task<AuthUserResponse> Auth(AuthUserCommand authUserCommand)
	{
		var parsedEmail = SafeFactory.TryCreate(() => new Email(authUserCommand.Email));

		if (parsedEmail.IsFailure)
			throw new BadRequestException(parsedEmail.Error);

		User? user = await _usersRepository.FindByEmailAsync(parsedEmail.Value!);

		if (user is null || !_passwordService.Verify(authUserCommand.Password, user.Password))
			throw new BadRequestException("Email ou senha incorretos!");

		string token = _jwtService.Generate(user);

		string refreshToken = _jwtService.Generate(user, hoursInMinutes);

		return new AuthUserResponse(token, refreshToken);
	}

	public async Task<AuthBusinessResponse> BusinessAuth(Guid userId, Guid businissId)
	{
		User user = await _usersRepository.FindByIdAsync(userId)
			?? throw new BadRequestException("usuário não encontrado!");

		Business business = await _businessRepository.FindByIdAsync(businissId)
			?? throw new BadRequestException("Negócio não encontrado!");

		bool isOwnerOrMember = business.IsOwnerOrMember(user);

		if (!isOwnerOrMember)
			throw new BadRequestException("Loja não pertence ao usuário!");

		string businessSession = _jwtService.Generate(business, hoursInMinutes);

		return new AuthBusinessResponse(businessSession);
	}

	public async Task<AuthUserResponse?> RefreshSessionAsync(string refreshToken)
	{
		var principal = _jwtService.GetPrincipalFromToken(refreshToken);
		if (principal is null) return null;

		var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
		if (userId is null) return null;

		var user = await _usersRepository.FindByIdAsync(Guid.Parse(userId));

		if (user is null)
			return null;

		string newAccessToken = _jwtService.Generate(user);
		string newRefreshToken = _jwtService.Generate(user, minutes: 6 * 60);

		return new AuthUserResponse(newAccessToken, newRefreshToken);
	}
}