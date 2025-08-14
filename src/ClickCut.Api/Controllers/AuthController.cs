using System.Runtime.CompilerServices;
using ClickCut.Api.Config;
using ClickCut.Api.Dtos.Auth;
using ClickCut.Api.Extensions;
using ClickCut.Application.Commands;
using ClickCut.Application.Dtos;
using ClickCut.Application.Ports.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickCut.Api.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController(IAuthServicePort authServicePort) : ControllerBase
{
	private readonly IAuthServicePort _authServicePort = authServicePort;

	[HttpPost]
	public async Task<IActionResult> Authenticate([FromBody] AuthRequest authRequest)
	{
		AuthUserResponse auth = await _authServicePort.Auth(new AuthUserCommand(authRequest.Email, authRequest.Password));
		HttpContext.SetHttpOnly(CookiesConfig.AccessToken, auth.Token);
		HttpContext.SetHttpOnly(CookiesConfig.RefreshToken, auth.RefreshToken);
		return Ok(new AuthResponse(Message: "Autenticado com sucesso!"));
	}

	[Authorize]
	[HttpPost("logout")]
	public IActionResult Logout()
	{
		HttpContext.ClearAllCookies();
		return Ok("Logout realizado com sucesso!");
	}
}