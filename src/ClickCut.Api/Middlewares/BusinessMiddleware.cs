using System.Security.Claims;
using ClickCut.Api.Attributes;
using ClickCut.Api.Config;
using ClickCut.Api.Extensions;
using ClickCut.Application.Ports.In;
using ClickCut.Domain.Models;
using ClickCut.Shared.Utils;

namespace ClickCut.Api.Middlewares;

public class BusinessSessionMiddleware(RequestDelegate next)
{
	private readonly RequestDelegate _next = next;

	public async Task InvokeAsync(HttpContext context, IJwtServicePort jwtService)
	{
		// Pega o endpoint que a rota resolveu
		var endpoint = context.GetEndpoint();
		if (endpoint is null)
		{
			await _next(context);
			return;
		}

		// Verifica se o atributo [Business] está no método ou na classe do controller
		var attribute = endpoint.Metadata.GetMetadata<BusinessAttribute>();
		if (attribute is null)
		{
			// Se não tem o atributo, este middleware não faz nada e passa para o próximo.
			await _next(context);
			return;
		}

		// 1. Pega o cookie "business"
		if (!context.Request.Cookies.TryGetValue(CookiesConfig.BusinessToken, out var businessToken) || string.IsNullOrEmpty(businessToken))
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			await context.Response.WriteAsync("Business session cookie is missing.");
			return;
		}

		// 2. Valide o token (garantindo que a vida útil seja validada)
		var principal = jwtService.GetPrincipalFromToken(businessToken, validateLifetime: true);
		if (principal is null)
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			await context.Response.WriteAsync("Business session token is invalid or expired.");
			return;
		}

		Claim? businessIdClaim = principal.FindFirst(ClaimsKeys.BusinessId);
		string? businessId = businessIdClaim?.Value;

		if (string.IsNullOrEmpty(businessId))
		{
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			await context.Response.WriteAsync("Invalid business token: missing required claims.");
			return;
		}

		var session = new BusinessSession { BusinessId = businessId };

		context.SetBusinessSession(session);

		// 6. Pode passar para o próximo middleware
		await _next(context);
	}
}