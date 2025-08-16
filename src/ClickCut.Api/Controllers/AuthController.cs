using ClickCut.Api.Attributes;
using ClickCut.Api.Config;
using ClickCut.Api.Dtos.Auth;
using ClickCut.Api.Extensions;
using ClickCut.Application.Commands;
using ClickCut.Application.Dtos;
using ClickCut.Application.Ports.In;
using ClickCut.Infra.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickCut.Api.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController(IAuthServicePort authServicePort) : ControllerBase
{
	private readonly IAuthServicePort _authServicePort = authServicePort;

	[Business]
	[HttpPost("test")]
	public IActionResult Test()
	{
		return Ok("OK");
	}

	[HttpPost]
	public async Task<IActionResult> Authenticate([FromBody] AuthRequest authRequest)
	{
		AuthUserResponse auth = await _authServicePort.Auth(new AuthUserCommand(authRequest.Email, authRequest.Password));

		HttpContext.SetHttpOnly(CookiesConfig.AccessToken, auth.Token);
		HttpContext.SetHttpOnly(CookiesConfig.RefreshToken, auth.RefreshToken);

		return Ok(new AuthResponse(Message: "Autenticado com sucesso!"));
	}

	[HttpPost("business")]
	public async Task<IActionResult> AuthenticateOnBusiness([FromBody] AuthBusinessRequest businessRequest)
	{
		Guid userId = HttpContext.User.GetUserId();
		AuthBusinessResponse authBusinessResponse = await _authServicePort.BusinessAuth(userId, businessRequest.BusinessId);

		HttpContext.SetHttpOnly(CookiesConfig.BusinessToken, authBusinessResponse.Token);

		return Ok(authBusinessResponse.Token);
	}

	[Authorize]
	[HttpPost("logout")]
	public IActionResult Logout()
	{
		HttpContext.ClearAllCookies();
		return Ok("Logout realizado com sucesso!");
	}
}