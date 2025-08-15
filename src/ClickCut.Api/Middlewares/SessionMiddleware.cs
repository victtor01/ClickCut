using System.Security.Claims;
using ClickCut.Api.Config;
using ClickCut.Api.Extensions;
using ClickCut.Application.Ports.In;
using ClickCut.Application.Ports.Out;
using ClickCut.Infra.Extensions;

namespace ClickCut.Api.Middlewares;

public class SessionMiddleware(RequestDelegate next, ILogger<SessionMiddleware> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly ILogger<SessionMiddleware> _logger = logger;

	public async Task InvokeAsync(HttpContext context, IJwtServicePort jwtService, IUsersRepositoryPort userRepository)
	{
		if (context.User.Identity?.IsAuthenticated ?? false)
		{
			await _next(context);
			return;
		}

		string? accessToken = context.Request.Cookies[CookiesConfig.AccessToken];
		string? refreshToken = context.Request.Cookies[CookiesConfig.RefreshToken];

		if (accessToken is null || refreshToken is null)
		{
			_logger.LogInformation("token cookies not found!");
			await _next(context);
			return;
		}

		ClaimsPrincipal? accessTokenPrincipal = jwtService.GetPrincipalFromToken(accessToken, validateLifetime: false);
		ClaimsPrincipal? refreshTokenPrincipal = jwtService.GetPrincipalFromToken(refreshToken, validateLifetime: true);

		if (accessTokenPrincipal is null || refreshTokenPrincipal is null)
		{
			_logger.LogInformation("tokens not found!");
			await _next(context);
			return;
		}

		var userIdFromAccessToken = accessTokenPrincipal.GetUserId();
		var userIdFromRefreshToken = refreshTokenPrincipal.GetUserId();

		if (userIdFromAccessToken == Guid.Empty || userIdFromAccessToken != userIdFromRefreshToken)
		{
			_logger.LogInformation("Incompatibilidade de tokens para o usuário {UserId}", userIdFromAccessToken);
			await _next(context);
			return;
		}

		var user = await userRepository.FindByIdAsync(userIdFromAccessToken);
		if (user == null)
		{
			_logger.LogInformation("User not found with id {userId}", userIdFromAccessToken);
			await _next(context);
			return;
		}

		var newAccessToken = jwtService.Generate(user, minutes: 1);
		var newRefreshToken = jwtService.Generate(user, minutes: 60 * 24 * 7);

		context.SetHttpOnly(CookiesConfig.AccessToken, newAccessToken);
		context.SetHttpOnly(CookiesConfig.RefreshToken, newRefreshToken);

		_logger.LogInformation("Tokens renovados via refresh token para o usuário {UserId}", user.Id);

		context.User = jwtService.GetPrincipalFromToken(newAccessToken, validateLifetime: true)!;

		await _next(context);
	}

}